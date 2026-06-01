using PSULib.FileClasses.Archives;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Maps;
using PSULib.FileClasses.Models;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace Set_Object_Manager
{
    public partial class MapEditorMainForm : Form
    {
        private const string FILE_LIST_NAME = "nbl_obj_unit_filelist.rel";

        [GeneratedRegex("obj_param\\..nr")]
        private static partial Regex ObjectParamRegex();

        public MapEditorMainForm()
        {
            InitializeComponent();
            //Because psulib is .net framework, it uses shift-jis directly.
            //We need to shim this here for .net 7.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private async void buildDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && databaseFolderDialog.ShowDialog() == DialogResult.OK)
            {
                var progress = new Progress<int>(v =>
                {
                    // This lambda is executed in context of UI thread,
                    // so it can safely update form controls
                    progressBar1.Value = v;
                });
                var gameFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                setUiEnabled(false);
                statusPanel.Visible = true;
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Maximum = gameFiles.Length;
                progressBar1.Value = 0;
                label1.Text = "Building Database";
                await Task.Run(() => exportObjects(folderBrowserDialog1.SelectedPath, databaseFolderDialog.SelectedPath, progress));
                setUiEnabled(true);
                statusPanel.Visible = false;
            }
        }

        private void exportObjects(string sourceDirectory, string outputDirectory, IProgress<int> progress)
        {
            var gameFiles = Directory.GetFiles(sourceDirectory);
            int processed = 0;
            HashSet<int> knownObjects = new();
            foreach (var gameFile in gameFiles)
            {
                NblLoader? nbl = getNbl(gameFile);
                if (nbl != null)
                {
                    NblChunk nmllChunk = (NblChunk)nbl.getFileParsed(0);
                    NblChunk? tmllChunk = nbl.chunks.Count > 1 ? (NblChunk)nbl.getFileParsed(1) : null;
                    //if we don't have both an object file list and an object parameter file, we're lost.
                    string? paramFileName = nmllChunk.getFilenames().Find(filename => ObjectParamRegex().IsMatch(filename));
                    if (nmllChunk.getFilenames().Contains(FILE_LIST_NAME) && paramFileName != null)
                    {
                        ObjectParamFile paramFile = (ObjectParamFile)nmllChunk.getFileParsed(paramFileName);
                        ListFile listFile = (ListFile)nmllChunk.getFileParsed(FILE_LIST_NAME);
                        foreach (var objectId in paramFile.ObjectDefinitions.Keys)
                        {
                            List<List<string>> fileNames = new();
                            for (int i = 0; i < 16; i++)
                            {
                                fileNames.Add(new List<string>());
                            }
                            fileNames[7].Add(paramFileName);
                            if (!knownObjects.Contains(objectId))
                            {
                                List<RawFile> nmllFiles = new();
                                List<RawFile> tmllFiles = new();
                                var objectDefinition = paramFile.ObjectDefinitions[objectId];
                                //find all the used files for it
                                //put into a new param file
                                //copy all related files into NBL
                                //save into database
                                ObjectParamFile outParamFile = new(paramFileName, new Dictionary<int, ObjectParamFile.ObjectEntry> { { objectId, objectDefinition } });
                                HashSet<string> textureAnimations = new();
                                HashSet<string> boneAnimations = new();
                                //texture lists aren't included separately; I think they may be forced to share this name?
                                HashSet<string> modelNames = objectDefinition.Models.Select(model => model.fileName).Where(model => !string.IsNullOrEmpty(model)).ToHashSet();
                                HashSet<string> particleNames = new(); //this one has an extension--the others do not.
                                                                       //Can't justify using two queries here, so the foreach is how it goes.
                                objectDefinition.Animations.ForEach(anim =>
                                {
                                    if (!string.IsNullOrEmpty(anim.boneAnimName))
                                    {
                                        boneAnimations.Add(anim.boneAnimName);
                                    }
                                    if (!string.IsNullOrEmpty(anim.texAnimName))
                                    {
                                        textureAnimations.Add(anim.texAnimName);
                                    }
                                });
                                if (objectDefinition.ParticleSoundReferences != null)
                                {
                                    particleNames.UnionWith(objectDefinition.ParticleSoundReferences.particleBindings.Select(binding => binding.particleName).Where(name => !string.IsNullOrEmpty(name)));
                                }
                                nmllFiles.Add(outParamFile.ToRawFile(0));
                                HashSet<string> alreadyCopiedFiles = new();
                                foreach (string particle in particleNames)
                                {
                                    RawFile unparsedParticle = nmllChunk.getFileRaw(particle);
                                    if (unparsedParticle != null && unparsedParticle.fileContents.Length > 4 && Encoding.UTF8.GetString(unparsedParticle.fileContents, 0, 4) == "YPD0")
                                    {
                                        nmllFiles.Add(unparsedParticle);
                                        PartialEffectFile.ParticleMetadata parsedParticle = PartialEffectFile.readParticleEntries(unparsedParticle);
                                        if (parsedParticle != null)
                                        {
                                            //Files referenced in the particle file are not included in the main file list.
                                            var particleStrings = parsedParticle.filenames;
                                            foreach (var particleRef in particleStrings)
                                            {
                                                var nmllFile = nmllChunk.getFileRaw(particleRef);
                                                var tmllFile = tmllChunk?.getFileRaw(particleRef);
                                                if (nmllFile != null)
                                                {
                                                    nmllFiles.Add(nmllFile);
                                                }
                                                else if (tmllFile != null)
                                                {
                                                    tmllFiles.Add(tmllFile);
                                                }
                                                alreadyCopiedFiles.Add(particleRef);
                                            }
                                        }
                                    }
                                }
                                int currentIndex = nmllFiles.Count;

                                HashSet<string> knownTextures = new();

                                //Now we go through each file in the NBL...
                                foreach (string file in nmllChunk.getFilenames())
                                {
                                    if (Path.GetExtension(file).EndsWith("nv") && textureAnimations.Contains(Path.GetFileNameWithoutExtension(file)))
                                    {
                                        if (!alreadyCopiedFiles.Contains(file))
                                        {
                                            RawFile rawXnv = nmllChunk.getFileRaw(file);
                                            nmllFiles.Add(rawXnv);
                                            alreadyCopiedFiles.Add(file);
                                        }
                                        fileNames[11].Add(file);
                                    }
                                    else if (Path.GetExtension(file).EndsWith("nm") && boneAnimations.Contains(Path.GetFileNameWithoutExtension(file)))
                                    {
                                        if (!alreadyCopiedFiles.Contains(file))
                                        {
                                            RawFile rawXnm = nmllChunk.getFileRaw(file);
                                            nmllFiles.Add(rawXnm);
                                            alreadyCopiedFiles.Add(file);
                                        }
                                        fileNames[4].Add(file);
                                    }
                                    //I don't think xnj/xnt/xna have separate listings in here, but they both show up.
                                    else if (Path.GetExtension(file).EndsWith("nj") && modelNames.Contains(Path.GetFileNameWithoutExtension(file)))
                                    {
                                        if (!alreadyCopiedFiles.Contains(file))
                                        {
                                            RawFile rawXnj = nmllChunk.getFileRaw(file);
                                            nmllFiles.Add(rawXnj);
                                            alreadyCopiedFiles.Add(file);
                                        }
                                        fileNames[0].Add(file);
                                    }
                                    else if (Path.GetExtension(file).EndsWith("na") && modelNames.Contains(Path.GetFileNameWithoutExtension(file)))
                                    {
                                        if (!alreadyCopiedFiles.Contains(file))
                                        {
                                            RawFile rawXna = nmllChunk.getFileRaw(file);
                                            nmllFiles.Add(rawXna);
                                            alreadyCopiedFiles.Add(file);
                                        }
                                        fileNames[3].Add(file);
                                    }
                                    else if (Path.GetExtension(file).EndsWith("nt") && modelNames.Contains(Path.GetFileNameWithoutExtension(file)))
                                    {
                                        //texture lists reference textures, the textures need to be copied over too...
                                        XntFile xnt = (XntFile)nmllChunk.getFileParsed(file);
                                        foreach (var texture in xnt.fileEntries)
                                        {
                                            knownTextures.Add(texture.filename);
                                        }
                                        if (!alreadyCopiedFiles.Contains(file))
                                        {
                                            RawFile rawXnt = nmllChunk.getFileRaw(file);
                                            nmllFiles.Add(rawXnt);
                                            alreadyCopiedFiles.Add(file);
                                        }
                                        fileNames[1].Add(file);
                                    }
                                }
                                if (tmllChunk != null)
                                {
                                    foreach (var texture in knownTextures.Where(texName => !alreadyCopiedFiles.Contains(texName)))
                                    {
                                        var rawTex = tmllChunk.getFileRaw(texture);
                                        if (rawTex != null)
                                        {
                                            tmllFiles.Add(rawTex);
                                        }
                                    }
                                }
                                nmllFiles.Insert(currentIndex, new ListFile(FILE_LIST_NAME, fileNames).ToRawFile(0));
                                NblLoader outNbl = new(nmllFiles, tmllFiles);
                                using (Stream outStream = new FileStream(Path.Combine(outputDirectory, "object_" + objectId.ToString("D4") + ".nbl"), FileMode.Create))
                                {
                                    outNbl.saveFile(outStream);
                                }
                                knownObjects.Add(objectId);
                            }
                        }
                    }
                }
                processed++;
                progress.Report(processed);
            }

        }

        private NblLoader? getNbl(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                if (fileStream.Length > 0x800)
                {
                    byte[] magicNumbers = new byte[4];
                    fileStream.Read(magicNumbers, 0, 4);
                    string magicWord = ASCIIEncoding.ASCII.GetString(magicNumbers);
                    //we only care about maps, maps are always NBLs.
                    if (magicWord == "NMLL" || magicWord == "NMBL")
                    {
                        return new NblLoader(fileStream);
                    }
                }
            }
            return null;
        }

        private async void importObjectIntoMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapFileOpenDialog.ShowDialog() == DialogResult.OK)
            {
                NblLoader? mapNbl = getNbl(mapFileOpenDialog.FileName);
                if (mapNbl != null)
                {
                    NblChunk mapNmll = (NblChunk)mapNbl.getFileParsed(0);
                    NblChunk mapTmll = (NblChunk)mapNbl.getFileParsed(1);
                    string? paramFileName = mapNmll.getFilenames().Find(filename => ObjectParamRegex().IsMatch(filename));
                    if (mapNmll.getFilenames().Contains(FILE_LIST_NAME) && paramFileName != null)
                    {
                        if (objectFileOpenDialog.ShowDialog() == DialogResult.OK)
                        {
                            bool success = false;
                            foreach (string objectFilename in objectFileOpenDialog.FileNames)
                            {
                                NblLoader? objectNbl = getNbl(objectFilename);
                                if (objectNbl != null)
                                {
                                    NblChunk objectNmll = (NblChunk)objectNbl.getFileParsed(0);
                                    string? objectParamFileName = objectNmll.getFilenames().Find(filename => ObjectParamRegex().IsMatch(filename));
                                    if (objectNmll.getFilenames().Contains(FILE_LIST_NAME) && objectParamFileName != null)
                                    {
                                        ObjectParamFile mapParamFile = (ObjectParamFile)mapNmll.getFileParsed(paramFileName);
                                        ListFile mapListFile = (ListFile)mapNmll.getFileParsed(FILE_LIST_NAME);
                                        ObjectParamFile importParamFile = (ObjectParamFile)objectNmll.getFileParsed(objectParamFileName);
                                        ListFile importListFile = (ListFile)objectNmll.getFileParsed(FILE_LIST_NAME);

                                        mergeNblChunks(mapNmll, objectNmll);
                                        if (objectNbl.doesFileExist("TMLL chunk")) { 
                                            NblChunk objectTmll = (NblChunk)objectNbl.getFileParsed(1);
                                            mergeNblChunks(mapTmll, objectTmll);
                                        }

                                        mergeListFiles(mapListFile, importListFile);
                                        mergeObjectParameterFiles(mapParamFile, importParamFile);
                                        success = true;
                                    }
                                }
                            }
                            if (success)
                            {
                                setUiEnabled(false);
                                statusPanel.Visible = true;
                                label1.Text = "Saving NBL";
                                progressBar1.Style = ProgressBarStyle.Marquee;
                                var timer = Stopwatch.StartNew();
                                await Task.Run(() => saveNblFile(mapNbl));
                                setUiEnabled(true);
                                statusPanel.Visible = false;
                                MessageBox.Show("Save complete; time = " + timer.Elapsed.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Could not find any objects in selected files. Please select another file or files.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This does not appear to be a map file. Choose another file.");
                    }
                }
            }
        }

        private void setUiEnabled(bool enableState)
        {
            menuStrip1.Enabled = enableState;
        }

        private void saveNblFile(NblLoader mapNbl)
        {
            mapNbl.chunks.ForEach(chunk => chunk.compressed = compressResultingMissionsToolStripMenuItem.Checked);
            byte[] savedNbl = mapNbl.ToRaw();
            File.WriteAllBytes(mapFileOpenDialog.FileName + "_updated", savedNbl);
        }

        private void mergeListFiles(ListFile destination, ListFile source)
        {
            for (int i = 0; i < destination.filenames.Count; i++)
            {
                foreach (var filename in source.filenames[i])
                {
                    if (!destination.filenames[i].Any(reference => reference.Filename == filename.Filename))
                    {
                        destination.filenames[i].Add(filename);
                    }
                }
            }
        }

        private void mergeObjectParameterFiles(ObjectParamFile destination, ObjectParamFile source)
        {
            foreach (var objectId in source.ObjectDefinitions.Keys.Where(key => !destination.ObjectDefinitions.ContainsKey(key)))
            {
                destination.ObjectDefinitions[objectId] = source.ObjectDefinitions[objectId];
            }
        }

        private void mergeNblChunks(NblChunk destination, NblChunk source)
        {
            List<string> destinationFilenames = destination.getFilenames();
            List<string> sourceFilenames = source.getFilenames();
            foreach (string importFileName in sourceFilenames)
            {
                if (!destinationFilenames.Contains(importFileName))
                {
                    destination.addFile(source.getFileRaw(importFileName));
                }
            }
        }
    }
}