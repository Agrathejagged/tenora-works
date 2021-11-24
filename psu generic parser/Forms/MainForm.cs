using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using psu_generic_parser.FileViewers;
using System.Security.Cryptography;
using Microsoft.WindowsAPICodePack.Dialogs;
using PSULib;
using System.Drawing;
using psu_generic_parser.Forms.FileViewers;
using PSULib.FileClasses.Enemies;
using psu_generic_parser.Forms.FileViewers.Enemies;
using PSULib.FileClasses.Archives;
using PSULib.FileClasses.Missions;
using PSULib.FileClasses.Items;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Characters;
using PSULib.FileClasses.Bosses;
using PSULib.FileClasses.Textures;
using PSULib.FileClasses.Models;
using PSULib.Support;
using PSULib.FileClasses.Maps;
using psu_generic_parser.Forms;
using PSULib.FileClasses.General.Scripts;

namespace psu_generic_parser
{
    public partial class MainForm : Form
    {
        ContainerFile loadedContainer;
        PsuFile currentRight;
        MainSettings settings;
        private HexEditForm currentFileHexForm;
        public bool batchPngExport = true;
        public bool batchRecursive = true;
        public bool batchExportSubArchiveFiles = false;
        public bool compressNMLL = false;
        public bool compressTMLL = false;
        public bool exportMetaData = true;

        private class FileTreeNodeTag
        {
            public ContainerFile OwnerContainer { get; set; }
            public string FileName { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
            Text += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            setAFSEnabled(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Text = "PSU Generic Parser " + Path.GetFileName(fileDialog.FileName);
                splitContainer1.Panel2.Controls.Clear();
                openPSUArchive(fileDialog.FileName, treeView1.Nodes);
            }
        }

        private bool openPSUArchive(string fileName, TreeNodeCollection treeNodeCollection)
        {
            bool isValidArchive = false;
            byte[] formatName = new byte[4];
            Stream stream = File.Open(fileName, FileMode.Open);
            stream.Read(formatName, 0, 4);

            string identifier = Encoding.ASCII.GetString(formatName, 0, 4);
            if (identifier == "NMLL" || identifier == "NMLB")
            {
                setAFSEnabled(false);
                treeNodeCollection.Clear();
                loadedContainer = new NblLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedContainer);
                compressNMLL = loadedContainer.Compressed;
                compressTMLL = loadedContainer.getFilenames().Count > 1 && ((NblChunk)loadedContainer.getFileParsed(1)).Compressed;
                isValidArchive = true;
            } 
            else if (identifier == "AFS\0")
            {
                setAFSEnabled(true);
                treeNodeCollection.Clear();
                loadedContainer = new AfsLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedContainer);
                isValidArchive = true;
            } else if (BitConverter.ToInt16(formatName, 0) == 0x50AF)
            {
                setAFSEnabled(false);
                treeNodeCollection.Clear();
                loadedContainer = new MiniAfsLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedContainer);
                isValidArchive = true;
            }
            stream.Close();
            stream.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return isValidArchive;
        }

        private void setAFSEnabled(bool isActive)
        {
            zoneUD.Enabled = isActive;
            addZoneButton.Enabled = isActive;
            setZoneButton.Enabled = isActive;
            addFileButton.Enabled = isActive;
            setQuestButton.Enabled = isActive;
        }

        /// <summary>
        /// Adds a container file's children to a given node collection.
        /// </summary>
        /// <param name="currNode">node collection</param>
        /// <param name="toRead">container file</param>
        private void addChildFiles(TreeNodeCollection currNode, ContainerFile toRead)
        {
            List<string> filenames = toRead.getFilenames();
            for(int i = 0; i < filenames.Count; i++)
            {
                string filename = filenames[i];
                TreeNode temp = new TreeNode(filename);
                //Explicitly disallowing export/replace on raw NBL chunks--this would be very dangerous.
                if(toRead is NblLoader)
                {
                    temp.ContextMenuStrip = nblChunkContextMenuStrip;
                }
                else
                {
                    temp.ContextMenuStrip = arbitraryFileContextMenuStrip;
                }

                if (toRead is AfsLoader || toRead is NblLoader || toRead is MiniAfsLoader) //AFS still doesn't lazy-load, meaning all my performance issues are STILL HERE.
                {
                    PsuFile child = toRead.getFileParsed(i);
                    if (child != null && child is ContainerFile)
                    {
                        addChildFiles(temp.Nodes, (ContainerFile)child);
                        if(((ContainerFile)child).Compressed)
                        {
                            temp.ForeColor = Color.Green;
                        }
                    }
                }
                else //NBL chunk as parent
                {
                    //For an NBL chunk, only read parsed children if they're containers.
                    //This is sort of a mediocre variety of lazy loading...
                    RawFile raw = toRead.getFileRaw(i);
                    if (filename.EndsWith(".nbl") || raw.fileheader == "NMLL" || raw.fileheader == "TMLL")
                    {
                        addChildFiles(temp.Nodes, (ContainerFile)toRead.getFileParsed(i));
                        if (((ContainerFile)toRead.getFileParsed(i)).Compressed)
                        {
                            temp.ForeColor = Color.Green;
                        }
                    }
                }
                temp.Tag = new FileTreeNodeTag { OwnerContainer = toRead, FileName = filename };
                currNode.Add(temp);
            }
        }

        private void extractPSUArchive(string fileName, string outDirectory)
        {
            string finalDirectory = Path.Combine(outDirectory, Path.GetFileName(fileName) + "_ext");
            byte[] formatName = new byte[4];
            Stream stream = File.Open(fileName, FileMode.Open);
            stream.Read(formatName, 0, 4);

            string identifier = Encoding.ASCII.GetString(formatName, 0, 4);
            if (identifier == "NMLL" || identifier == "NMLB")
            {
                loadedContainer = new NblLoader(stream);
                exportChildFiles(loadedContainer, finalDirectory);
            }
            else if (identifier == "AFS\0")
            {
                loadedContainer = new AfsLoader(stream);
                exportChildFiles(loadedContainer, finalDirectory);
            }
            else if (BitConverter.ToInt16(formatName, 0) == 0x50AF)
            {
                loadedContainer = new MiniAfsLoader(stream);
                exportChildFiles(loadedContainer, finalDirectory);
            }
            stream.Close();
            stream.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }

        private void exportChildFiles(ContainerFile toRead, string outDirectory)
        {
            bool isArchive = false;
            Directory.CreateDirectory(outDirectory);
            List<string> filenames = toRead.getFilenames();
            List<string> writtenFiles = new List<string>();

            for (int i = 0; i < filenames.Count; i++)
            {
                string filename = filenames[i];

                if (toRead is AfsLoader || toRead is NblLoader || toRead is MiniAfsLoader) //AFS still doesn't lazy-load, meaning all my performance issues are STILL HERE.
                {
                    PsuFile child = toRead.getFileParsed(i);
                    if (child != null && child is ContainerFile)
                    {
                        isArchive = true;
                        if (filename == "NMLL chunk" || filename == "TMLL chunk")
                        {
                            exportChildFiles((ContainerFile)child, outDirectory);
                        }
                        else
                        {
                            string finalDirectory = Path.Combine(outDirectory, filename + "_ext");
                            exportChildFiles((ContainerFile)child, finalDirectory);
                        }
                    }
                }
                else //NBL chunk as parent
                {
                    RawFile raw = toRead.getFileRaw(i);
                    if (filename.EndsWith(".nbl") || raw.fileheader == "NMLL" || raw.fileheader == "TMLL")
                    {
                        isArchive = true;
                        exportChildFiles((ContainerFile)toRead.getFileParsed(i), outDirectory);
                    }
                }

                try
                {
                    if (isArchive)
                    {
                        if (batchExportSubArchiveFiles)
                        {
                            extractFile(toRead.getFileParsed(i), Path.Combine(outDirectory, filename));
                        }
                        continue;
                    }
                    else if (filename.Contains(".xvr") && batchPngExport)
                    {
                        if (toRead is AfsLoader || toRead is MiniAfsLoader)
                        {
                            filename = CheckForDupeFilenames(writtenFiles, filename);
                        }
                        filename = filename.Replace(".xvr", ".png");
                        ((ITextureFile)toRead.getFileParsed(i)).mipMaps[0].Save(Path.Combine(outDirectory, filename));
                    }
                    else
                    {
                        if (toRead is AfsLoader || toRead is MiniAfsLoader)
                        {
                            filename = CheckForDupeFilenames(writtenFiles, filename);
                        }
                        File.WriteAllBytes(Path.Combine(outDirectory, filename), toRead.getFileRaw(i).WriteToBytes(exportMetaData));
                    }
                } 
                catch
                {
                    Console.WriteLine("Unable to extract " + filename + ". The file may be in use, inaccessible, or incompatible. Skipping.");
                }
            }
        }

        private static string CheckForDupeFilenames(List<string> writtenFiles, string filename)
        {
            if (writtenFiles.Contains(filename))
            {
                int j = 0;
                while (writtenFiles.Contains(filename + $"_{j}"))
                {
                    j++;
                }
                filename = filename + $"_{j}";
                writtenFiles.Add(filename);
            }
            else
            {
                writtenFiles.Add(filename);
            }

            return filename;
        }

        private void setRightPanel(PsuFile toRead)
        {            
            splitContainer1.Panel2.Controls.Clear();
            currentRight = null;
            currentRight = toRead;
            UserControl toAdd = new UserControl();

            if (toRead is ITextureFile texFile)
            {
                toAdd = new TextureViewer(texFile);
            }
            else if (toRead is PointeredFile pointeredFile)
            {
                toAdd = new PointeredFileViewer(pointeredFile);
            }
            else if (toRead is ActDataFile actDataFile)
            {
                toAdd = new ActDataFileViewer(actDataFile);
            }
            else if (toRead is EnemySoundEffectFile seDataFile)
            {
                toAdd = new EnemySoundEffectFileViewer(seDataFile);
            }
            else if (toRead is ListFile listFile)
            {
                toAdd = new ListFileViewer(listFile);
            }
            else if (toRead is XntFile xntFile)
            {
                toAdd = new XntFileViewer(xntFile);
            }
            else if (toRead is XnaFile xnaFile)
            {
                toAdd = new XnaFileViewer(xnaFile);
            }
            else if (toRead is NomFile nomFile)
            {
                toAdd = new NomFileViewer(nomFile);
            }
            else if (toRead is EnemyLayoutFile enemyLayoutFile)
            {
                toAdd = new EnemyLayoutViewer(enemyLayoutFile);
            }
            else if (toRead is ItemTechParamFile itemTechParamFile)
            {
                toAdd = new ItemTechParamViewer(itemTechParamFile);
            }
            else if (toRead is ItemSkillParamFile itemSkillParamFile)
            {
                toAdd = new ItemSkillParamViewer(itemSkillParamFile);
            }
            else if (toRead is ItemBulletParamFile itemBulletParamFile)
            {
                toAdd = new ItemBulletParamViewer(itemBulletParamFile);
            }
            else if (toRead is RmagBulletParamFile rmagBulletParamFile)
            {
                toAdd = new RmagBulletViewer(rmagBulletParamFile);
            }
            else if (toRead is TextFile textFile)
            {
                toAdd = new TextViewer(textFile);
            }
            else if (toRead is ScriptFile scriptFile)
            {
                toAdd = new ScriptFileViewer(scriptFile);
            }
            else if (toRead is EnemyLevelParamFile enemyLevelParamFile)
            {
                toAdd = new EnemyStatEditor(enemyLevelParamFile);
            }
            else if (toRead is WeaponListFile weaponListFile)
            {
                toAdd = new WeaponListEditor(weaponListFile);
            }
            else if (toRead is PartsInfoFile partsInfoFile)
            {
                toAdd = new PartsInfoViewer(partsInfoFile);
            }
            else if (toRead is ItemPriceFile itemPriceFile)
            {
                toAdd = new ItemPriceViewer(itemPriceFile);
            }
            else if (toRead is EnemyDropFile enemyDropFile)
            {
                toAdd = new EnemyDropViewer(enemyDropFile);
            }
            else if (toRead is SetFile setFile)
            {
                toAdd = new SetFileViewer(setFile);
            }
            else if (toRead is ThinkDragonFile thinkDragonFile)
            {
                toAdd = new ThinkDragonViewer(thinkDragonFile);
            }
            else if (toRead is WeaponParamFile weaponParamFile)
            {
                toAdd = new WeaponParamViewer(weaponParamFile);
            }
            else if (toRead is ItemSuitParamFile itemSuitParamFile)
            {
                toAdd = new ClothingFileViewer(itemSuitParamFile);
            }
            else if (toRead is ItemUnitParamFile itemUnitParamFile)
            {
                toAdd = new UnitParamViewer(itemUnitParamFile);
            }
            else if (toRead is ItemCommonInfoFile itemCommonInfoFile)
            {
                toAdd = new ItemCommonInfoViewer(itemCommonInfoFile);
            }
            else if (toRead is QuestListFile questListFile)
            {
                toAdd = new QuestListViewer(questListFile);
            }
            else if (toRead is ObjectParticleInfoFile objectParticleInfoFile)
            {
                toAdd = new ObjectParticleInfoFileViewer(objectParticleInfoFile);
            }
            else if (toRead is ObjectParamFile objParamFile)
            {
                toAdd = new ObjParamViewer(objParamFile);
            }
            else if (toRead is EnemyParamFile enemyParamFile)
            {
                toAdd = new EnemyParamFileViewer(enemyParamFile);
            }
            else if (toRead is AtkDatFile atkDatFile)
            {
                toAdd = new AtkDatFileViewer(atkDatFile);
            }
            else if (toRead is DamageDataFile damageDataFile)
            {
                toAdd = new DamageDataFileViewer(damageDataFile);
            }
            else if (toRead is EnemyMotTblFile enemyMotTblFile)
            {
                toAdd = new EnemyMotTblFileViewer(enemyMotTblFile);
            }
            else if(toRead is LndCommonFile lndCommonFile)
            {
                toAdd = new LndCommonEditor(lndCommonFile);
            }
            else if (toRead is UnpointeredFile unpointeredFile)
            {
                toAdd = new UnpointeredFileViewer(unpointeredFile);
            }
            splitContainer1.Panel2.Controls.Add(toAdd);
            toAdd.Dock = DockStyle.Fill;
        }

        private void exportBlob_Click(object sender, EventArgs e)
        {
            if (loadedContainer is NblLoader)
            {
                CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
                goodOpenFileDialog.IsFolderPicker = true;
                if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ((NblLoader)loadedContainer).exportDataBlob(goodOpenFileDialog.FileName);
                }
            }
                
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedContainer != null)
            {
                saveFileDialog1.FileName = fileDialog.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    MemoryStream saveStream = new MemoryStream();
                    try
                    {
                        byte[] savedContainer = loadedContainer.ToRaw();
                        File.WriteAllBytes(saveFileDialog1.FileName, savedContainer);
                        this.Text = "PSU Generic Parser " + Path.GetFileName(saveFileDialog1.FileName);
                        fileDialog.FileName = saveFileDialog1.FileName;
                    } catch(ScriptValidationException exc)
                    {
                        string joinedErrors = String.Join("\r\n", exc.ScriptValidationErrors.Select(error =>
                        {
                            if (error.LineNumber != -1)
                            {
                                return error.FunctionName + ", line " + error.LineNumber + ": " + error.Description;
                            }
                            else
                            {
                                return error.FunctionName + ": " + error.Description;
                            }
                        }
                        ));
                        string exceptionMessage = $"Could not save archive. \r\nFile \"{exc.FileName}\" failed to validate for the following reasons: \r\n{joinedErrors}";
                        MessageBox.Show(exceptionMessage, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void setQuest_Click(object sender, EventArgs e)
        {
            if (loadedContainer is AfsLoader)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    ((AfsLoader)loadedContainer).setQuest(importDialog.OpenFile());
                }
            }
        }

        private void setZone_Click_1(object sender, EventArgs e)
        {
            if (loadedContainer is AfsLoader)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    ((AfsLoader)loadedContainer).setZone((int)zoneUD.Value, importDialog.OpenFile());
                }
            }
        }

        private void addZone_Click_1(object sender, EventArgs e)
        {
            if (loadedContainer is AfsLoader)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    ((AfsLoader)loadedContainer).addZone((int)zoneUD.Value, importDialog.OpenFile());
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            if (e.Node.Tag is FileTreeNodeTag && !(((FileTreeNodeTag)e.Node.Tag).OwnerContainer is NblLoader))
            {
                ContainerFile parent = ((FileTreeNodeTag)e.Node.Tag).OwnerContainer;
                setRightPanel(parent.getFileParsed(e.Node.Index));
            }
        }

        private void addFile_Click(object sender, EventArgs e)
        {
            if (loadedContainer is AfsLoader)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    ((AfsLoader)loadedContainer).addFile(fileDialog.SafeFileName, fileDialog.OpenFile());
                }
            }
        }

        private void createAFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderToOpen = folderBrowserDialog1.SelectedPath;
                string fileToSave = saveFileDialog1.FileName;
                AfsLoader.createFromDirectory(folderToOpen, fileToSave);
            }
        }

        private void replaceFileTreeContextItem_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null && node.Tag is FileTreeNodeTag)
            {
                var tag = ((FileTreeNodeTag)node.Tag);
                ContainerFile owningFile = tag.OwnerContainer;
                OpenFileDialog replaceDialog = new OpenFileDialog();
                replaceDialog.FileName = tag.FileName;
                if (replaceDialog.ShowDialog() == DialogResult.OK)
                {
                    RawFile file = new RawFile(replaceDialog.OpenFile(), Path.GetFileName(replaceDialog.FileName));
                    if(owningFile is FilenameAwareContainerFile awareContainerFile)
                    {
                        string filename = file.filename;
                        if (filename != tag.FileName && !awareContainerFile.ValidateFilename(filename))
                        {
                            FileRenameForm rename = new FileRenameForm(filename);
                            while (!awareContainerFile.ValidateFilename(filename))
                            {
                                if (rename.ShowDialog() == DialogResult.OK)
                                {
                                    filename = rename.FileName;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        if (filename != file.filename)
                        {
                            file.filename = filename;
                        }
                    }
                    owningFile.replaceFile(node.Index, file);
                    node.Text = file.filename;
                    tag.FileName = file.filename;
                    PsuFile parsedFile = owningFile.getFileParsed(node.Index);
                    if(parsedFile is ContainerFile)
                    {
                        node.Nodes.Clear();
                        addChildFiles(node.Nodes, (ContainerFile)parsedFile);

                    }
                    setRightPanel(parsedFile);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
        }

        private void disableScriptParsingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableScriptParsingToolStripMenuItem.Checked = !disableScriptParsingToolStripMenuItem.Checked;
            PsuFiles.parseScripts = !disableScriptParsingToolStripMenuItem.Checked;
        }

        private void exportAllWeaponsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedContainer is NblLoader && loadedContainer.getFilenames().Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    //getFilenames is relatively expensive.
                    var nmllFilenames = ((NblChunk)loadedContainer.getFileParsed(0)).getFilenames();
                    foreach (string filename in nmllFilenames)
                    {
                        if (filename.Contains("itemWeaponParam") && ((NblChunk)loadedContainer.getFileParsed(0)).getFileParsed(filename) is WeaponParamFile)
                        {
                            MemoryStream memStream = new MemoryStream();
                            ((WeaponParamFile)((NblChunk)loadedContainer.getFileParsed(0)).getFileParsed(filename)).saveTextFile(memStream);
                            File.WriteAllBytes(folderBrowserDialog1.SelectedPath + "\\" + filename + ".txt", memStream.ToArray());
                        }
                    }
                }
            }
        }

        private void importAllWeaponsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedContainer is NblLoader && loadedContainer.getFilenames().Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                    //getFilenames is relatively expensive.
                    var nmllFilenames = ((NblChunk)loadedContainer.getFileParsed(0)).getFilenames();
                    foreach (string filename in files)
                    {
                        if (filename.Contains("itemWeaponParam"))
                        {
                            //try replacing .txt with nothing (e.g itemWeaponParam_01DKSword.xnr.txt)
                            if (!tryImportWeaponTextFile((NblLoader)loadedContainer, nmllFilenames, filename, Path.GetFileName(filename).Replace(".txt", "")))
                            {
                                //try replacing .txt with .xnr (e.g itemWeaponParam_01DKSword.txt) -- parser doesn't do this, but other people may.
                                if (!tryImportWeaponTextFile((NblLoader)loadedContainer, nmllFilenames, filename, Path.GetFileName(filename).Replace(".txt", ".xnr")))
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        private bool tryImportWeaponTextFile(NblLoader nbl, List<string> nmllFilenames, string filepath, string attemptFilename)
        {
            if(nmllFilenames.Contains(attemptFilename) && (nbl.chunks[0].getFileParsed(attemptFilename) is WeaponParamFile))
            {
                WeaponParamFile paramFile = (WeaponParamFile)nbl.chunks[0].getFileParsed(attemptFilename);
                using (FileStream inStream = new FileStream(filepath, FileMode.Open))
                {
                    paramFile.loadTextFile(inStream);
                }
                return true;
            }
            return false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void insertNMLLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedContainer is NblLoader && treeView1.SelectedNode != null && treeView1.SelectedNode.Level == 1)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream inStream = fileDialog.OpenFile())
                    {
                        ((ContainerFile)loadedContainer.getFileParsed(0)).addFile(treeView1.SelectedNode.Index, new RawFile(inStream, Path.GetFileName(fileDialog.FileName)));
                    }
                    treeView1.Nodes.Clear();
                    addChildFiles(treeView1.Nodes, loadedContainer);
                }
            }
        }

        private void decryptNMLBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(fileDialog.FileName);

                MemoryStream fileStream = new MemoryStream(fileContents);
                fileStream.Seek(3, SeekOrigin.Begin);
                byte endian = (byte)fileStream.ReadByte();
                fileStream.Seek(0, SeekOrigin.Begin);
                BinaryReader fileLoader;
                bool bigEndian = false;
                if (endian == 0x42)
                {
                    fileLoader = new BigEndianBinaryReader(fileStream);
                    bigEndian = true;
                }
                else
                    fileLoader = new BinaryReader(fileStream);

                string formatName = new String(fileLoader.ReadChars(4));
                ushort fileVersion = fileLoader.ReadUInt16();
                ushort chunkFilenameLength = fileLoader.ReadUInt16();
                uint headerSize = fileLoader.ReadUInt32();
                uint nmllCount = fileLoader.ReadUInt32();
                uint uncompressedSize = fileLoader.ReadUInt32();
                uint compressedSize = fileLoader.ReadUInt32();
                uint pointerLength = fileLoader.ReadUInt32() / 4;
                uint blowfishKey = fileLoader.ReadUInt32();
                uint tmllHeaderSize = fileLoader.ReadUInt32();
                uint tmllDataSizeUncomp = fileLoader.ReadUInt32();
                uint tmllDataSizeComp = fileLoader.ReadUInt32();
                uint tmllCount = fileLoader.ReadUInt32();
                uint tmllHeaderLoc = 0;

                uint pointerLoc = 0;

                uint size = compressedSize == 0 ? uncompressedSize : compressedSize;

                uint nmllDataLoc = (uint)((headerSize + 0x7FF) & 0xFFFFF800);
                pointerLoc = (uint)(nmllDataLoc + size + 0x7FF) & 0xFFFFF800;
                if (tmllCount > 0)
                    tmllHeaderLoc = (pointerLoc + pointerLength * 4 + 0x7FF) & 0xFFFFF800;

                BlewFish fish = new BlewFish(blowfishKey, bigEndian);
                
                for(int i = 0; i < nmllCount; i++)
                {
                    int headerLoc = 0x40 + i * 0x60;
                    byte[] toDecrypt = new byte[0x30];
                    Array.Copy(fileContents, headerLoc, toDecrypt, 0, 0x30);
                    toDecrypt = fish.decryptBlock(toDecrypt);
                    Array.Copy(toDecrypt, 0, fileContents, headerLoc, 0x30);
                }

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < tmllCount; i++)
                {
                    uint headerLoc = (uint)(tmllHeaderLoc + 0x30 + i * 0x60);
                    byte[] toDecrypt = new byte[0x30];
                    Array.Copy(fileContents, headerLoc, toDecrypt, 0, 0x30);
                    toDecrypt = fish.decryptBlock(toDecrypt);
                    Array.Copy(toDecrypt, 0, fileContents, headerLoc, 0x30);

                    sb.Append(Encoding.ASCII.GetString(toDecrypt, 0, 0x20).Split('\0')[0] + "\t");
                }

                fileStream.Seek(nmllDataLoc, SeekOrigin.Begin);
                byte[] encryptedNmll = fileLoader.ReadBytes((int)size);
                byte[] decryptedNmll = fish.decryptBlock(encryptedNmll);
                byte[] decompressedNmll = compressedSize != 0 ? PrsCompDecomp.Decompress(decryptedNmll, uncompressedSize) : decryptedNmll;

                File.WriteAllText(fileDialog.FileName + ".tml.list", sb.ToString());
                File.WriteAllBytes(fileDialog.FileName + ".decrypt", fileContents);
                File.WriteAllBytes(fileDialog.FileName + ".encryptNmll", encryptedNmll);
                File.WriteAllBytes(fileDialog.FileName + ".decryptNmll", decryptedNmll);
                File.WriteAllBytes(fileDialog.FileName + ".decompressNmll", decompressedNmll);
            }
        }

        private void decryptNMLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(fileDialog.FileName);

                MemoryStream fileStream = new MemoryStream(fileContents);
                BinaryReader fileLoader = new BinaryReader(fileStream);

                string formatName = new String(fileLoader.ReadChars(4));
                ushort fileVersion = fileLoader.ReadUInt16();
                ushort chunkFilenameLength = fileLoader.ReadUInt16();
                uint headerSize = fileLoader.ReadUInt32();
                uint nmllCount = fileLoader.ReadUInt32();
                uint uncompressedSize = fileLoader.ReadUInt32();
                uint compressedSize = fileLoader.ReadUInt32();
                uint pointerLength = fileLoader.ReadUInt32() / 4;
                uint blowfishKey = fileLoader.ReadUInt32();
                uint tmllHeaderSize = fileLoader.ReadUInt32();
                uint tmllDataSizeUncomp = fileLoader.ReadUInt32();
                uint tmllDataSizeComp = fileLoader.ReadUInt32();
                uint tmllCount = fileLoader.ReadUInt32();
                uint tmllHeaderLoc = 0;

                uint pointerLoc = 0;

                uint size = compressedSize == 0 ? uncompressedSize : compressedSize;

                uint nmllDataLoc = (uint)((headerSize + 0x7FF) & 0xFFFFF800);
                pointerLoc = (uint)(nmllDataLoc + size + 0x7FF) & 0xFFFFF800;
                if (tmllCount > 0)
                    tmllHeaderLoc = (pointerLoc + pointerLength * 4 + 0x7FF) & 0xFFFFF800;

                BlewFish fish = new BlewFish(blowfishKey);

                for (int i = 0; i < nmllCount; i++)
                {
                    int headerLoc = 0x40 + i * 0x60;
                    byte[] toDecrypt = new byte[0x30];
                    Array.Copy(fileContents, headerLoc, toDecrypt, 0, 0x30);
                    toDecrypt = fish.decryptBlock(toDecrypt);
                    Array.Copy(toDecrypt, 0, fileContents, headerLoc, 0x30);
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < tmllCount; i++)
                {
                    uint headerLoc = (uint)(tmllHeaderLoc + 0x30 + i * 0x60);
                    byte[] toDecrypt = new byte[0x30];
                    Array.Copy(fileContents, headerLoc, toDecrypt, 0, 0x30);
                    toDecrypt = fish.decryptBlock(toDecrypt);
                    Array.Copy(toDecrypt, 0, fileContents, headerLoc, 0x30);

                    sb.Append(Encoding.ASCII.GetString(toDecrypt, 0, 0x20).Split('\0')[0] + "\t");
                    sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4C)) + "\t");
                    sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4E)) + "\n");
                }
                File.WriteAllText(fileDialog.FileName + ".tml.list", sb.ToString());
                File.WriteAllBytes(fileDialog.FileName + ".decrypt", fileContents);
            }
        }

        private class TextureEntry
        {
            public RawFile fileContents;
            public List<string> containingFiles = new List<string>();
        }

        private void textureCatalogueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, Dictionary<string, TextureEntry>> textureEntries = new Dictionary<string, Dictionary<string, TextureEntry>>();
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath))
                {
                    MD5 md5 = MD5.Create();
                    using (Stream s = new FileStream(file, FileMode.Open))
                    {
                        byte[] identifier = new byte[4];
                        s.Read(identifier, 0, 4);
                        s.Seek(0, SeekOrigin.Begin);
                        if (identifier.SequenceEqual(new byte[] { 0x4E, 0x4D, 0x4C, 0x4C }))
                        {
                            NblLoader nbl = new NblLoader(s);
                            if (nbl.chunks.Count > 1)
                            {
                                //This means there's a TMLL...
                                foreach (RawFile raw in nbl.chunks[1].fileContents)
                                {
                                    byte[] fileMd5 = md5.ComputeHash(raw.fileContents);
                                    string md5String = BitConverter.ToString(fileMd5).Replace("-", "");
                                    if (!textureEntries.ContainsKey(raw.filename))
                                    {
                                        textureEntries[raw.filename] = new Dictionary<string, TextureEntry>();
                                    }
                                    if (!textureEntries[raw.filename].ContainsKey(md5String))
                                    {
                                        TextureEntry entry = new TextureEntry();
                                        entry.fileContents = raw;
                                        textureEntries[raw.filename][md5String] = entry;
                                    }
                                    if (!textureEntries[raw.filename][md5String].containingFiles.Contains(Path.GetFileName(file)))
                                    {
                                        textureEntries[raw.filename][md5String].containingFiles.Add(Path.GetFileName(file));
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var ent in textureEntries)
                {
                    if (ent.Value.Values.Count > 1)
                    {
                        Console.Out.WriteLine("Texture: " + ent.Key);
                        foreach (var val in ent.Value)
                        {
                            Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\categorized\\conflicted\\" + ent.Key + "\\" + val.Key);
                            using (Stream outStream = new FileStream(folderBrowserDialog1.SelectedPath + "\\categorized\\conflicted\\" + ent.Key + "\\" + val.Key + "\\" + val.Value.fileContents.filename, FileMode.Create))
                            {
                                val.Value.fileContents.WriteToStream(outStream);
                            }
                            XvrTextureFile xvr = new XvrTextureFile(val.Value.fileContents.subHeader, val.Value.fileContents.fileContents, val.Value.fileContents.filename);
                            xvr.mipMaps[0].Save(folderBrowserDialog1.SelectedPath + "\\categorized\\conflicted\\" + ent.Key + "\\" + val.Key + "\\" + val.Value.fileContents.filename.Replace(".xvr", ".png"));
                            Console.Out.WriteLine("\t" + val.Key + ": " + string.Join(", ", val.Value.containingFiles));
                        }
                        Console.Out.WriteLine();
                    }
                    else
                    {

                        string hash = ent.Value.Keys.First();
                        RawFile raw = ent.Value[hash].fileContents;
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\categorized\\" + ent.Key);
                        using (Stream outStream = new FileStream(folderBrowserDialog1.SelectedPath + "\\categorized\\" + ent.Key + "\\" + raw.filename, FileMode.Create))
                        {
                            raw.WriteToStream(outStream);
                        }
                        XvrTextureFile xvr = new XvrTextureFile(raw.subHeader, raw.fileContents, raw.filename);
                        xvr.mipMaps[0].Save(folderBrowserDialog1.SelectedPath + "\\categorized\\" + ent.Key + "\\" + raw.filename.Replace(".xvr", ".png"));
                    }
                }
            }
        }

        private void exportSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportSelected();
        }

        private void extractFileTreeContextItem_Click(object sender, EventArgs e)
        {
            exportSelected();
        }

        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //It is a sin to use the standard folder dialog
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;

            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                exportAll(treeView1.Nodes, goodOpenFileDialog.FileName);
            }
        }

        private void exportAll(TreeNodeCollection treeNodes, string folderName)
        {
            Directory.CreateDirectory(folderName);
            foreach (TreeNode node in treeNodes)
            {
                exportNode(node, folderName);
            }
        }

        private void exportSelected()
        {
            SaveFileDialog exportFileDialog = new SaveFileDialog();

            if (currentRight != null)
            {
                exportFileDialog.FileName = currentRight.filename;

                if (currentRight is ITextureFile)
                {
                    exportFileDialog.FileName = exportFileDialog.FileName.Replace(".xvr", ".png"); // Treat .png as default
                    exportFileDialog.Filter = "Portable Network Graphics (*.png)|*.png|Xbox PowerVR Texture (*.xvr)|*.xvr";
                } else if (currentRight is TextFile)
                {
                    exportFileDialog.FileName = exportFileDialog.FileName.Replace(".bin", ".txt"); // Treat .txt as default
                    exportFileDialog.Filter = "Text (*.txt)|*.txt|Binary File (*.bin)|*.bin";
                }
                
                if (exportFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (currentRight is ITextureFile && Path.GetExtension(exportFileDialog.FileName).Equals(".png"))
                    {
                        ((ITextureFile)currentRight).mipMaps[0].Save(exportFileDialog.FileName);
                    } else if (currentRight is TextFile && Path.GetExtension(exportFileDialog.FileName).Equals(".txt"))
                    {
                        ((TextFile)currentRight).saveToTextFile(exportFileDialog.OpenFile());
                    }
                    else
                    {
                        extractFile(currentRight, exportFileDialog.FileName);
                    }
                }
            }

        }

        private void exportNode(TreeNode node, string fileDirectory)
        {
            if (node.Tag is FileTreeNodeTag)
            {
                var tag = (FileTreeNodeTag)node.Tag;
                ContainerFile parent = tag.OwnerContainer;
                string originalFilename = tag.FileName;
                int fileIndex = node.Index;
                List<string> parentFilenames = parent.getFilenames();
                PsuFile file = parent.getFileParsed(fileIndex);
                //NBLs only have "NML(B/L)" or "TML(B/L)" chunks as children.
                if (!(parent is NblLoader))
                {

                    if (file is ITextureFile && batchPngExport)
                    {
                        string filename = Path.Combine(fileDirectory, Path.GetFileName(originalFilename + ".png"));
                        ((ITextureFile)file).mipMaps[0].Save(filename);
                    }
                    else
                    {
                        if (batchExportSubArchiveFiles || !(file is ContainerFile))
                        {
                            string filename = Path.Combine(fileDirectory, getUniqueFilename(originalFilename, fileIndex, parentFilenames));
                            extractFile(file, filename);
                        }
                    }
                }

                //Handle it as a new archive within the current one
                if (file is ContainerFile)
                {
                    string newFolder = fileDirectory + @"\" + getUniqueFilename(originalFilename, fileIndex, parentFilenames) + "_ext";
                    exportAll(node.Nodes, newFolder);
                }
                else
                {
                    foreach (TreeNode nodeChild in node.Nodes)
                    {
                        exportNode(nodeChild, fileDirectory);
                    }
                }
            }
        }

        private string getUniqueFilename(string originalFilename, int fileIndex, List<string> parentFilenames)
        {
            string usedFilename;
            //If the same file exists multiple times, append the index to it.
            if (parentFilenames.Count(filename => filename == originalFilename) > 1)
            {
                usedFilename = Path.GetFileName(originalFilename) + "_" + (fileIndex - parentFilenames.FindIndex(name => name == originalFilename)) + Path.GetExtension(originalFilename);
            }
            else
            {
                usedFilename = originalFilename;
            }
            return usedFilename;
        }

        private void extractFile(PsuFile psuFile, string filename)
        {
            RawFile file = psuFile.ToRawFile(0);
            byte[] bytes = file.WriteToBytes(exportMetaData);
            try
            {
                File.WriteAllBytes(filename, bytes);
            }
            catch
            {
                MessageBox.Show("Unable to extract " + filename + ". The file may be in use or otherwise inaccessible. Skipping.");
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(settings == null || settings.IsDisposed)
            {
                settings = new MainSettings(this);
            }
            settings.Show();
            settings.BringToFront();
        }

        private void extractAllInFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //It is a sin to use the standard folder dialog
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;

            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] fileNames = Directory.GetFiles(goodOpenFileDialog.FileName, "*.*", SearchOption.AllDirectories);
                if(batchRecursive)
                {
                    fileNames = Directory.GetFiles(goodOpenFileDialog.FileName, "*.*", SearchOption.AllDirectories);
                } else
                {
                    fileNames = Directory.GetFiles(goodOpenFileDialog.FileName);
                }
                actionProgressBar.Maximum = fileNames.Length;
                progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                progressStatusLabel.Refresh();

                foreach (string fileName in fileNames)
                {
                    Console.WriteLine(fileName);
                    string newFolder = Path.GetDirectoryName(fileName);

                    extractPSUArchive(fileName, newFolder);

                    actionProgressBar.Value++;
                    progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                    progressStatusLabel.Refresh();
                }
            }
            actionProgressBar.Value = 0;
            progressStatusLabel.Text = "Progress: Done!";
        }

        private void viewInHexButton_Click(object sender, EventArgs e)
        {
            if (currentRight != null)
            {
                PointeredFile pointeredFile = null;
                byte[] file = currentRight.ToRaw();
                if (currentRight.calculatedPointers != null)
                {
                    if (currentRight is PointeredFile)
                    {
                        pointeredFile = (PointeredFile)currentRight;
                    } 
                    else
                    {
                        //For now, Big Endian files don't really need to be considered here since they'd be a PointeredFile already. Possibly add in further support if added elsewhere later
                        pointeredFile = new PointeredFile(currentRight.filename, file, currentRight.header, currentRight.calculatedPointers, 0, false);
                    }
                    pointeredFile.ToRaw();
                }

                string headingText = $"Selected File: {currentRight.filename}";

                if(currentFileHexForm != null)
                {
                    currentFileHexForm.Close();
                }
                if(pointeredFile != null)
                {
                    currentFileHexForm = new HexEditForm(file, headingText, true,
                        pointeredFile);
                } else
                {
                    currentFileHexForm = new HexEditForm(file, headingText, true,
                        null);
                }

                currentFileHexForm.Show();
            }
        }

        private void compressChunkToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            FileTreeNodeTag tag = node.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblLoader)
            {
                ContainerFile parent = tag.OwnerContainer;
                ((NblChunk)parent.getFileParsed(treeView1.SelectedNode.Index)).Compressed = compressChunkToolStripMenuItem.Checked;
                if(compressChunkToolStripMenuItem.Checked)
                {
                    treeView1.SelectedNode.ForeColor = Color.Green;
                }
                else
                {
                    treeView1.SelectedNode.ForeColor = Color.Black;
                }
                if (node.Parent != null)
                {
                    if(parent.Compressed)
                    {
                        node.Parent.ForeColor = Color.Green;
                    }
                    else
                    {
                        node.Parent.ForeColor = Color.Black;
                    }
                }
            }
        }

        public void setNmllCompressOverride(NblLoader.CompressionOverride settings)
        {
            NblLoader.NmllCompressionOverride = settings;
        }

        public void setTmllCompressOverride(NblLoader.CompressionOverride settings)
        {
            NblLoader.TmllCompressionOverride = settings;
        }

        private void nblChunkContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileTreeNodeTag tag = treeView1.SelectedNode.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblLoader)
            {
                ContainerFile parent = tag.OwnerContainer;
                compressChunkToolStripMenuItem.Checked = ((NblChunk)parent.getFileParsed(treeView1.SelectedNode.Index)).Compressed;
            }
        }

        private void listAllObjparamsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;

            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] fileNames = Directory.GetFiles(goodOpenFileDialog.FileName);
                actionProgressBar.Maximum = fileNames.Length;
                progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                progressStatusLabel.Refresh();
                Dictionary<int, Tuple<string, ObjectParamFile.ObjectEntry>> objects = new Dictionary<int, Tuple<string, ObjectParamFile.ObjectEntry>>();

                foreach (string fileName in fileNames)
                {
                    Console.WriteLine(fileName);
                    string newFolder = Path.GetDirectoryName(fileName);
                    byte[] formatName = new byte[4];
                    try
                    {
                        using (Stream stream = File.Open(fileName, FileMode.Open))
                        {
                            stream.Read(formatName, 0, 4);

                            string identifier = Encoding.ASCII.GetString(formatName, 0, 4);
                            if (identifier == "NMLL")
                            {
                                NblLoader nbl = new NblLoader(stream);
                                if (((NblChunk)nbl.getFileParsed(0)).doesFileExist("obj_param.xnr"))
                                {
                                    ObjectParamFile paramFile = (ObjectParamFile)((NblChunk)nbl.getFileParsed(0)).getFileParsed("obj_param.xnr");
                                    foreach (int objectId in paramFile.ObjectDefinitions.Keys)
                                    {
                                        if (objects.ContainsKey(objectId) && !objects[objectId].Item2.group2Entry.Equals(paramFile.ObjectDefinitions[objectId].group2Entry))
                                        {
                                            Console.WriteLine("Mismatched object, ID = " + objectId + " compared to " + objects[objectId].Item1);
                                        }
                                        else
                                        {
                                            objects[objectId] = new Tuple<string, ObjectParamFile.ObjectEntry>(fileName, paramFile.ObjectDefinitions[objectId]);
                                        }
                                    }
                                }
                            }
                        }
                    } catch (Exception exception)
                    {
                        Console.WriteLine("Error reading file");
                        //just ignore
                    }

                    actionProgressBar.Value++;
                    progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                    progressStatusLabel.Refresh();
                }

                foreach(int i in objects.Keys.OrderBy(a=>a))
                {
                    var hitbox = objects[i].Item2.group2Entry;
                    Console.WriteLine("Object " + i + ", first found in " + objects[i].Item1 + ": group 0 = " + hitbox.hitboxShape + "; {" + hitbox.unknownFloat2 + ", " + hitbox.unknownFloat3 + ", " + hitbox.unknownFloat3 + "}; id 1 = " + hitbox.unknownInt5 + "; isolated float = " + hitbox.unknownFloat6 + "; last value = " + hitbox.unknownInt9);
                }
            }
        }

        private void listAllMonsterLayoutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;

            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string outputFileName = Path.Combine(goodOpenFileDialog.FileName, "report2.txt");
                StreamWriter writer = new StreamWriter(outputFileName);
                string[] fileNames = Directory.GetFiles(goodOpenFileDialog.FileName);
                actionProgressBar.Maximum = fileNames.Length;
                progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                progressStatusLabel.Refresh();
                Dictionary<int, Tuple<string, ObjectParamFile.ObjectEntry>> objects = new Dictionary<int, Tuple<string, ObjectParamFile.ObjectEntry>>();

                foreach (string fileName in fileNames)
                {
                    string newFolder = Path.GetDirectoryName(fileName);
                    byte[] formatName = new byte[4];
                    try
                    {
                        using (Stream stream = File.Open(fileName, FileMode.Open))
                        {
                            stream.Read(formatName, 0, 4);

                            string identifier = Encoding.ASCII.GetString(formatName, 0, 3);
                            if (identifier == "AFS")
                            {
                                writer.WriteLine(fileName);
                                AfsLoader afs = new AfsLoader(stream);
                                foreach(var file in afs.afsList)
                                {
                                    if(file.fileName.StartsWith("zone") && file.fileName.EndsWith("_ae.nbl"))
                                    {
                                        NblLoader nbl = (NblLoader)file.fileContents;
                                        foreach(var nblFile in ((ContainerFile)nbl.getFileParsed(0)).getFilenames())
                                        {
                                            if(nblFile.StartsWith("enemy") && nblFile.EndsWith(".xnr"))
                                            {
                                                EnemyLayoutFile layoutFile = (EnemyLayoutFile)((ContainerFile)nbl.getFileParsed(0)).getFileParsed(nblFile);
                                                writer.WriteLine("\t" + nblFile + ":");
                                                for (int i = 0; i < layoutFile.spawns.Length; i++)
                                                {
                                                        writer.WriteLine($"\t\tSpawn {i}:");
                                                        writer.WriteLine($"\t\tMonsters:");
                                                        for (int j = 0; j < layoutFile.spawns[i].monsters.Length; j++)
                                                        {
                                                            writer.WriteLine($"\t\t\tGroup {j}:");
                                                            for (int k = 0; k < layoutFile.spawns[i].monsters[j].Length; k++)
                                                            {
                                                                writer.WriteLine("\t\t\t\t" + layoutFile.spawns[i].monsters[j][k].ToString());
                                                            }
                                                        }
                                                        writer.WriteLine($"\t\tArrangements:");
                                                        for (int j = 0; j < layoutFile.spawns[i].arrangements.Length; j++)
                                                        {
                                                            writer.WriteLine("\t\t\t" + layoutFile.spawns[i].arrangements[j].ToString());
                                                        }
                                                        writer.WriteLine($"\t\tSpawn Data:");
                                                        for (int j = 0; j < layoutFile.spawns[i].spawnData.Length; j++)
                                                        {
                                                            writer.WriteLine("\t\t\t" + layoutFile.spawns[i].spawnData[j].ToString());
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error reading file");
                        //just ignore
                    }

                    actionProgressBar.Value++;
                    progressStatusLabel.Text = $"Progress: {actionProgressBar.Value}/{actionProgressBar.Maximum} Files. Please wait, this can take time.";
                    progressStatusLabel.Refresh();
                }
                /*
                foreach (int i in objects.Keys.OrderBy(a => a))
                {
                    var hitbox = objects[i].Item2.group2Entry;
                    Console.WriteLine("Object " + i + ", first found in " + objects[i].Item1 + ": group 0 = " + hitbox.hitboxShape + "; {" + hitbox.unknownFloat2 + ", " + hitbox.unknownFloat3 + ", " + hitbox.unknownFloat3 + "}; id 1 = " + hitbox.unknownInt5 + "; isolated float = " + hitbox.unknownFloat6 + "; last value = " + hitbox.unknownInt9);
                }*/
            }
        }

        //TODO: This should be in a different program.
        private string convertDamageResists(int rawResists)
        {
            StringBuilder sb = new StringBuilder(3);

            switch (rawResists & 0x3)
            {
                default: break;
                case 1: sb.Append("s"); break;
                case 2: case 3: sb.Append("S"); break;
            }
            switch (rawResists & 0xC)
            {
                default: break;
                case 4: sb.Append("r"); break;
                case 8: case 0xC: sb.Append("R"); ; break;
            }
            switch (rawResists & 0x30)
            {
                default: break;
                case 4: sb.Append("t"); break;
                case 8: case 0xC: sb.Append("T"); break;
            }
            return sb.ToString();
        }

        private void catalogueEnemyparamToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                Dictionary<string, EnemyParamFile> paramFileMap = new Dictionary<string, EnemyParamFile>();
                Dictionary<string, ActDataFile> actDataFileMap = new Dictionary<string, ActDataFile>();
                Dictionary<string, DamageDataFile> damageDataFileMap = new Dictionary<string, DamageDataFile>();
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog1.SelectedPath))
                {
                    using (Stream s = new FileStream(file, FileMode.Open))
                    {
                        byte[] identifier = new byte[4];
                        s.Read(identifier, 0, 4);
                        s.Seek(0, SeekOrigin.Begin);
                        if (identifier.SequenceEqual(new byte[] { 0x4E, 0x4D, 0x4C, 0x4C }))
                        {
                            NblLoader nbl = new NblLoader(s);
                            if (nbl.chunks.Count > 0)
                            {
                                foreach (RawFile raw in nbl.chunks[0].fileContents)
                                {
                                    if(raw.filename.StartsWith("Param") && !raw.filename.Contains("ColtobaShare"))
                                    {
                                        paramFileMap[raw.filename] = (EnemyParamFile)nbl.chunks[0].getFileParsed(raw.filename);
                                    }
                                    else if(raw.filename.StartsWith("ActData") && !raw.filename.Contains("Quadruped_a"))
                                    {
                                        actDataFileMap[raw.filename] = (ActDataFile)nbl.chunks[0].getFileParsed(raw.filename);
                                    }
                                    else if (raw.filename.StartsWith("DamageData") && !raw.filename.Contains("Quadruped_a"))
                                    {
                                        damageDataFileMap[raw.filename] = (DamageDataFile)nbl.chunks[0].getFileParsed(raw.filename);
                                    }
                                }
                            }
                        }
                    }
                }

                /*
                foreach (var entry in paramFileMap.OrderBy(x => x.Key))
                {
                    EnemyParamFile file = entry.Value;
                    Console.Out.WriteLine(entry.Key);
                    
                    Console.Out.WriteLine("Base Stats:");
                    Console.Out.WriteLine("\tHpModifier: " + file.baseParams.HpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tAtpModifier: " + file.baseParams.AtpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tDfpModifier: " + file.baseParams.DfpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tAtaModifier: " + file.baseParams.AtaModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tEvpModifier: " + file.baseParams.EvpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tStaModifier: " + file.baseParams.StaModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tLckModifier: " + file.baseParams.LckModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tTpModifier: " + file.baseParams.TpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tMstModifier: " + file.baseParams.MstModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tElementModifier: " + file.baseParams.ElementModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tExpModifier: " + file.baseParams.ExpModifier.ToString("0.00##"));
                    Console.Out.WriteLine("\tUnknownValue1: " + file.baseParams.UnknownValue1);
                    Console.Out.WriteLine("\tUnknownValue2: " + file.baseParams.UnknownValue2);
                    Console.Out.WriteLine("\tUnknownValue3: " + file.baseParams.UnknownValue3);
                    Console.Out.WriteLine("\tStatusResists: " + file.baseParams.StatusResists.ToString("X"));
                    Console.Out.WriteLine("\tDamageResists: " + convertDamageResists(file.baseParams.DamageResists));
                    Console.Out.WriteLine("\tUnknownModifier3: " + file.baseParams.UnknownModifier3.ToString("0.00##"));
                    Console.Out.WriteLine("\tUnknownModifier4: " + file.baseParams.UnknownModifier4.ToString("0.00##"));
                    Console.Out.WriteLine("\tUnknownValue4: " + file.baseParams.UnknownValue4);
                    Console.Out.WriteLine("\tUnknownValue5: " + file.baseParams.UnknownValue5);
                    Console.Out.WriteLine("\tUnknownModifier5: " + file.baseParams.UnknownModifier5.ToString("0.00##"));
                    Console.Out.WriteLine("\tUnknownModifier6: " + file.baseParams.UnknownModifier6.ToString("0.00##"));
                    Console.Out.WriteLine("\tUnknownModifier7: " + file.baseParams.UnknownModifier7.ToString("0.00##"));
                    string element = "UNKNOWN";
                    switch(file.baseParams.MonsterElement)
                    {
                        case 0: element = "Neutral"; break;
                        case 1: element = "Fire"; break;
                        case 2:
                            element = "Lightning"; break;
                        case 4:
                            element = "Light"; break;
                        case 9:
                            element = "Ice"; break;
                        case 10:
                            element = "Ground"; break;
                        case 12:
                            element = "Dark"; break;
                        default: break;
                    }
                    Console.Out.WriteLine("\tMonsterElement: " + element);
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Buffs:");
                    Console.Out.WriteLine("\t?\t??\t???\t????\t?????\tATP\tDFP\tATA\tEVP\tSTA\tLCK\tTP\tMST\tEXP\tSERes\tDmgRes");
                    //Console.Out.WriteLine("\t?\t??\t???\t????\t?????\tATP\tDFP\tATA\tEVP\tSTA\tLCK\tTP\tMST\tEXP\tUnused\tUnused\tUnused\tUnused\tSERes\tDmgRes");
                    foreach (var buff in file.buffParams)
                    {
                        Console.Out.Write("\t"); 
                        Console.Out.Write(buff.UnknownValue1 + "\t");
                        Console.Out.Write(buff.UnknownValue2 + "\t");
                        Console.Out.Write(buff.UnknownValue3 + "\t");
                        Console.Out.Write(buff.UnknownValue4 + "\t");
                        Console.Out.Write(buff.UnusedIntValue1 + "\t");
                        Console.Out.Write(buff.AtpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.DfpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.AtaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.EvpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.StaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.LckModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.TpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.MstModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(buff.ExpModifier.ToString("0.00##") + "\t");
                        
                        //Console.Out.Write(buff.UnusedIntValue2 + "\t");
                        //Console.Out.Write(buff.UnusedModifier1 + "\t");
                        //Console.Out.Write(buff.UnusedModifier2 + "\t");
                        //Console.Out.Write(buff.UnusedModifier3 + "\t");
                        Console.Out.Write(buff.StatusResists.ToString("X") + "\t");
                        Console.Out.Write(convertDamageResists(buff.DamageResists));
                        //Console.Out.Write(buff.DamageResists.ToString("X"));
                        Console.Out.WriteLine();
                    }
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Attacks:");
                    //Console.Out.WriteLine("\tBone          \t?\t??\t???\t????\t?????\tOnhit\tSE(s)\tLevel\t??\t???\tHP\tATP\tDFP\tATA\tEVP\tSTA\tLCK\tTP\tMST\tELE%\tEXP\tUnused\tUnused\tUnused");
                    Console.Out.WriteLine("\tBone          \tX\tY\tZ\tWidth\tHeight\tOnhit\tSE(s)\tLevel\t??\t???\tHP\tATP\tDFP\tATA\tEVP\tSTA\tLCK\tTP\tMST\tELE%\tEXP");
                    foreach (var attack in file.attackParams)
                    {
                        Console.Out.Write("\t");
                        Console.Out.Write(attack.BoneName.PadRight(14) + "\t");
                        Console.Out.Write(attack.OffsetX.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.OffsetY.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.OffsetZ.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.BoundCylinderWidth.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.BoundCylinderHeight.ToString("0.00##") + "\t");

                        Console.Out.Write(attack.OnHitEffect.ToString("X4") + "\t");
                        Console.Out.Write(attack.StatusEffect.ToString("X4") + "\t");
                        Console.Out.Write(attack.UnknownSubgroup2Int3 + "\t");
                        Console.Out.Write(attack.UnknownSubgroup2Int4 + "\t");
                        Console.Out.Write(attack.UnknownSubgroup2Int5.ToString("X4") + "\t");

                        Console.Out.Write(attack.HpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.AtpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.DfpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.AtaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.EvpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.StaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.LckModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.TpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.MstModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.ElementModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(attack.ExpModifier.ToString("0.00##"));
                        
                        //Console.Out.Write(attack.ExpModifier + "\t");
                        //Console.Out.Write(attack.UnusedModifier1 + "\t");
                        //Console.Out.Write(attack.UnusedModifier2 + "\t");
                        //Console.Out.Write(attack.UnusedModifier3);
                        Console.Out.WriteLine();
                    }
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Hitboxes:");
                    Console.Out.WriteLine("\tCanHit\tBone          \tX\tY\tZ\tWidth\tHeight\tHP\tATP\tDFP\tATA\tEVP\tSTA\tLCK\tTP\tMST\tELE%\tEXP\tUnused\tUnused\tUnused");
                    foreach (var hitbox in file.hitboxParams)
                    {
                        Console.Out.Write("\t");
                        Console.Out.Write(hitbox.Targetable + "\t");
                        Console.Out.Write((hitbox.BoneName != null ? hitbox.BoneName : "").PadRight(14) + "\t");
                        Console.Out.Write(hitbox.OffsetX.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.OffsetY.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.OffsetZ.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.BoundCylinderWidth.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.BoundCylinderHeight.ToString("0.00##") + "\t");

                        Console.Out.Write(hitbox.HpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.AtpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.DfpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.AtaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.EvpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.StaModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.LckModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.TpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.MstModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.ElementModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.ExpModifier.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.UnusedModifier1.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.UnusedModifier2.ToString("0.00##") + "\t");
                        Console.Out.Write(hitbox.UnusedModifier3.ToString("0.00##"));
                        Console.Out.WriteLine();
                    }
                    
                    Console.Out.WriteLine("\tGroup 2:");
                    foreach (var subentry1 in file.unknownSubEntry1List)
                    {
                        Console.Out.Write("\t");
                        Console.Out.Write("\t" + subentry1.UnknownInt1);
                        Console.Out.Write("\t" + subentry1.OffsetX.ToString("0.00##"));
                        Console.Out.Write("\t" + subentry1.OffsetY.ToString("0.00##"));
                        Console.Out.Write("\t" + subentry1.OffsetZ.ToString("0.00##"));
                        Console.Out.Write("\t" + subentry1.Scale1.ToString("0.00##"));
                        Console.Out.WriteLine("\t" + subentry1.Scale2.ToString("0.00##"));
                    }
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("\tGroup 2:");
                    foreach (var subentry2 in file.unknownSubEntry2List)
                    {
                        Console.Out.Write("\t");
                        Console.Out.Write("\t" + subentry2.UnknownInt1);
                        Console.Out.Write("\t" + subentry2.UnknownInt2);
                        Console.Out.Write("\t" + subentry2.UnknownFloat1.ToString("0.00##"));
                        Console.Out.Write("\t" + subentry2.UnknownInt3);
                        Console.Out.Write("\t" + subentry2.UnknownInt4);
                        Console.Out.Write("\t" + subentry2.UnknownInt5);
                        Console.Out.Write("\t" + subentry2.UnknownInt6);
                        Console.Out.Write("\t" + subentry2.UnknownInt7);
                        Console.Out.WriteLine("\t" + subentry2.UnknownInt8);
                    }
                    Console.Out.WriteLine();
                    Console.Out.WriteLine();
                }
                */
                /*
                foreach(var entry in actDataFileMap)
                {
                    ActDataFile actDataFile = entry.Value;
                    Console.Out.WriteLine(entry.Key);
                    Console.Out.WriteLine("whatever");
                    for(int i = 0; i < actDataFile.Actions.Count; i++)
                    {
                        Console.Out.WriteLine("Action " + i);
                        
                        foreach (var action in actDataFile.Actions[i].ActionEntries)
                        {
                            Console.Out.Write("\t" + action.UnknownInt1);
                            Console.Out.Write("\t" + action.MotTblID);
                            Console.Out.Write("\t" + action.UnknownFloatAt3);
                            Console.Out.Write("\t" + action.VerticalExaggeration);
                            Console.Out.Write("\t" + action.MotionFloat1);
                            Console.Out.Write("\t" + action.MotionFloat2);
                            Console.Out.Write("\t" + action.HorizontalUnknown);
                            Console.Out.Write("\t" + action.UnknownFloatAt8);
                            Console.Out.Write("\t" + action.UnknownFloatAt9);
                            Console.Out.Write("\t" + action.UnknownAngleDegrees1);
                            Console.Out.Write("\t" + action.UnknownIntAt11);
                            Console.Out.Write("\t" + action.UnknownAngleDegrees2);
                            Console.Out.Write("\t" + action.UnknownAngleDegrees3);
                            Console.Out.Write("\t" + action.UnknownStateValue);
                            Console.Out.Write("\t" + action.UnknownStateModifier1);
                            Console.Out.Write("\t" + action.UnknownStateModifier2);
                            Console.Out.Write("\t" + action.AttackID);
                            Console.Out.Write("\t" + action.UnknownInt15);
                            Console.Out.Write("\t" + action.UnknownFloat6);
                            Console.Out.Write("\t" + action.UnknownInt16);
                            Console.Out.Write("\t" + action.UnknownFloat7);
                            Console.Out.Write("\t" + action.DamageDataList);
                            Console.Out.Write("\t" + action.UnknownInt18);
                            Console.Out.Write("\t" + action.UnknownInt19);
                            Console.Out.Write("\t" + action.UnknownInt20);
                            Console.Out.Write("\t" + action.UnknownFloatAt21);
                            Console.Out.Write("\t" + action.UnusedInt22);
                            Console.Out.Write("\t" + action.UnusedInt23);
                            Console.Out.Write("\t" + action.UnusedInt24);
                            Console.Out.Write("\t" + action.UnusedInt25);
                            Console.Out.WriteLine();
                        }
                        */
                        /*
                        for(int j = 0; j < actDataFile.Actions[i].ActionEntries.Count; j++)
                        {
                            if (actDataFile.Actions[i].ActionEntries[j].SubEntryList1.Count > 0 || actDataFile.Actions[i].ActionEntries[j].SubEntryList2.Count > 0)
                            {
                                Console.Out.WriteLine("\tSubaction " + j);
                                if (actDataFile.Actions[i].ActionEntries[j].SubEntryList1.Count > 0)
                                {
                                    Console.Out.WriteLine("\tSublist 1:");
                                    for (int k = 0; k < actDataFile.Actions[i].ActionEntries[j].SubEntryList1.Count; k++)
                                    {
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList1[k].UnknownInt1);
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList1[k].UnknownFloat.ToString("0.00##"));
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList1[k].UnknownInt2);
                                        Console.Out.WriteLine();
                                    }
                                }
                                if (actDataFile.Actions[i].ActionEntries[j].SubEntryList2.Count > 0)
                                {
                                    Console.Out.WriteLine("\tSublist 2:");
                                    for (int k = 0; k < actDataFile.Actions[i].ActionEntries[j].SubEntryList2.Count; k++)
                                    {
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList2[k].UnknownInt1);
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList2[k].UnknownFloat.ToString("0.00##"));
                                        Console.Out.Write("\t\t" + actDataFile.Actions[i].ActionEntries[j].SubEntryList2[k].UnknownInt2);
                                        Console.Out.WriteLine();
                                    }
                                }
                                Console.Out.WriteLine();
                            }
                        }*/
                        /*
                        Console.Out.WriteLine();
                    }
                    Console.Out.WriteLine();
                }
                */
                foreach (var entry in damageDataFileMap)
                {
                    DamageDataFile damageDataFile = entry.Value;
                    Console.Out.WriteLine(entry.Key);
                    for (int i = 0; i < damageDataFile.DamageTypeEntries.Count; i++)
                    {
                        Console.Out.WriteLine("Damage lookup " + i);
                        for(int j = 0; j < damageDataFile.DamageTypeEntries[i].Count; j++)
                        {
                            Console.Out.WriteLine("\tDamage index " + j + ", Type: " + damageDataFile.DamageTypeEntries[i][j].DamageType + ", Angle count: " + damageDataFile.DamageTypeEntries[i][j].Angles.Count);
                            foreach (var angleEntry in damageDataFile.DamageTypeEntries[i][j].Angles)
                            {
                                Console.Out.Write("\t\t" + angleEntry.UnknownInt1);
                                Console.Out.Write("\t" + angleEntry.UnknownInt2);
                                Console.Out.Write("\tActions: " + string.Join(", ", angleEntry.ActionList));
                                Console.Out.WriteLine();
                            }
                            Console.Out.WriteLine();
                        }
                        Console.Out.WriteLine();
                    }
                    Console.Out.WriteLine();
                }
            }
        }

        AnimationNameHashDialog dialog = new AnimationNameHashDialog();

        private void calculateAnimationNameHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dialog.IsDisposed)
            {
                dialog = new AnimationNameHashDialog();
            }
            if (currentRight != null && !(currentRight is NblChunk))
            {
                dialog.SetFileName(currentRight.filename);
            }
            if (!dialog.Visible)
            {
                dialog.Show();
            }
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            FileTreeNodeTag tag = node.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblLoader)
            {
                ContainerFile parent = tag.OwnerContainer;
                OpenFileDialog replaceDialog = new OpenFileDialog();
                NblChunk chunk = ((NblChunk)parent.getFileParsed(treeView1.SelectedNode.Index));

                if (replaceDialog.ShowDialog() == DialogResult.OK)
                {
                    RawFile file = new RawFile(replaceDialog.OpenFile(), Path.GetFileName(replaceDialog.FileName));
                    string filename = file.filename;
                    if(!chunk.ValidateFilename(filename))
                    {
                        FileRenameForm rename = new FileRenameForm(filename);
                        while (!chunk.ValidateFilename(filename))
                        {
                            if (rename.ShowDialog() == DialogResult.OK)
                            {
                                filename = rename.FileName;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    if(filename != file.filename)
                    {
                        file.filename = filename;
                    }

                    chunk.addFile(file);

                    TreeNode newNode = new TreeNode(file.filename);
                    FileTreeNodeTag newTag = new FileTreeNodeTag();
                    newTag.OwnerContainer = chunk;
                    newTag.FileName = file.filename;
                    newNode.Tag = newTag;
                    newNode.ContextMenuStrip = arbitraryFileContextMenuStrip;
                    node.Nodes.Add(newNode);

                    if(file.fileheader == "NMLL" || file.fileheader == "NMLB")
                    {
                        addChildFiles(newNode.Nodes, (ContainerFile)chunk.getFileParsed(newNode.Index));
                    }
                    treeView1.SelectedNode = newNode;
                }
            }
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            FileTreeNodeTag tag = node.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblChunk chunk)
            {
                chunk.removeFile(node.Index);
                node.Parent.Nodes.Remove(node);
            }
        }

        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            node.BeginEdit();
        }

        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = treeView1.SelectedNode;
            FileTreeNodeTag tag = node.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblLoader)
            {
                e.CancelEdit = true;
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = treeView1.SelectedNode;
            FileTreeNodeTag tag = node.Tag as FileTreeNodeTag;
            if (tag != null && tag.OwnerContainer is NblLoader)
            {
                e.CancelEdit = true;
            }
            else if(tag != null && e.Label != null)
            {
                if(!(tag.OwnerContainer is FilenameAwareContainerFile facf) || facf.ValidateFilename(e.Label))
                {
                    tag.OwnerContainer.renameFile(node.Index, e.Label);
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }
    }
}
