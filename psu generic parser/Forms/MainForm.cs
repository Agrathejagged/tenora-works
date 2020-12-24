using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using psu_generic_parser.FileViewers;
using psu_generic_parser.FileClasses;
using System.Security.Cryptography;
using Microsoft.WindowsAPICodePack.Dialogs;
using PSULib;

namespace psu_generic_parser
{
    public partial class MainForm : Form
    {
        NblLoader loadedNbl;
        AfsLoader loadedAfs;
        MiniAfsLoader loadedMiniAfs;
        PsuFile currentRight;
        MainSettings settings;
        private HexEditForm currentFileHexForm;
        public bool batchPngExport = true;
        public bool batchRecursive = true;
        public bool batchExportSubArchiveFiles = false;
        public bool compressNMLL = false;
        public bool compressTMLL = false;
        public bool exportMetaData = false;

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
                loadedAfs = null;
                loadedMiniAfs = null;
                loadedNbl = new NblLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedNbl);
                compressNMLL = loadedNbl.isCompressed;
                compressTMLL = loadedNbl.chunks.Count > 1 && loadedNbl.chunks[1].compressed;
                isValidArchive = true;
            } 
            else if (identifier == "AFS\0")
            {
                setAFSEnabled(true);
                treeNodeCollection.Clear();
                loadedNbl = null;
                loadedMiniAfs = null;
                loadedAfs = new AfsLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedAfs);
                isValidArchive = true;
            } else if (BitConverter.ToInt16(formatName, 0) == 0x50AF)
            {
                setAFSEnabled(false);
                treeNodeCollection.Clear();
                loadedAfs = null;
                loadedNbl = null;
                loadedMiniAfs = new MiniAfsLoader(stream);
                splitContainer1.Panel2.Controls.Clear();
                addChildFiles(treeNodeCollection, loadedMiniAfs);
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
                temp.ContextMenuStrip = treeViewContextMenu;

                if (toRead is AfsLoader || toRead is NblLoader || toRead is MiniAfsLoader) //AFS still doesn't lazy-load, meaning all my performance issues are STILL HERE.
                {
                    PsuFile child = toRead.getFileParsed(i);
                    if (child != null && child is ContainerFile)
                    {
                        addChildFiles(temp.Nodes, (ContainerFile)child);
                    }
                }
                else //NBL chunk as parent
                {
                    RawFile raw = toRead.getFileRaw(i);
                    if (filename.EndsWith(".nbl") || raw.fileheader == "NMLL" || raw.fileheader == "TMLL")
                    {
                        addChildFiles(temp.Nodes, (ContainerFile)toRead.getFileParsed(i));
                    }
                }
                temp.Tag = new object[] { toRead, filename };
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
                loadedAfs = null;
                loadedMiniAfs = null;
                loadedNbl = new NblLoader(stream);
                exportChildFiles(loadedNbl, finalDirectory);
            }
            else if (identifier == "AFS\0")
            {
                loadedNbl = null;
                loadedMiniAfs = null;
                loadedAfs = new AfsLoader(stream);
                exportChildFiles(loadedAfs, finalDirectory);
            }
            else if (BitConverter.ToInt16(formatName, 0) == 0x50AF)
            {
                loadedAfs = null;
                loadedNbl = null;
                loadedMiniAfs = new MiniAfsLoader(stream);
                exportChildFiles(loadedMiniAfs, finalDirectory);
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
                        ((TextureFile)toRead.getFileParsed(i)).mipMaps[0].Save(Path.Combine(outDirectory, filename));
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

        /*
private void addAfsFiles(TreeNodeCollection currNode, AfsLoader toRead)
{
   for (int i = 0; i < toRead.fileCount; i++)
   {
       TreeNode temp = new TreeNode(toRead.afsList[i].fileName);
       if (toRead.subPaths[i] != null)
       {
           addAfsFiles(temp.Nodes, toRead.subPaths[i]);
       }
       else if (toRead.nblContents[i] != null)
       {
           temp.Tag = toRead.nblContents[i];
           addNblFiles(temp.Nodes, toRead.nblContents[i]);
       }
       currNode.Add(temp);
   }
}

private void addNblFiles(TreeNodeCollection currNode, NblLoader toRead)
{
   foreach (NblChunk content in toRead.chunks)
   {
       currNode.Add(content.chunkID + " chunk");
       foreach (RawFile currFile in content.fileContents)
       {
           TreeNode temp = new TreeNode(currFile.filename);
           temp.Tag = new object[]{content, currFile.filename};//currFile.toRead.nmllFiles[i].actualFile;
           temp.ContextMenuStrip = treeViewContextMenu;
           if (temp.Text.Contains(".nbl") || currFile.fileheader == "NMLL")
           {
               addNblFiles(temp.Nodes, (NblLoader)content.getFileParsed(currFile.filename));
           }
           currNode[0].Nodes.Add(temp);
       }
   }
   /*
   currNode.Add("NMLL chunk");
   for (int i = 0; i < toRead.nmllFiles.Length; i++)
   {
       TreeNode temp = new TreeNode(toRead.nmllFiles[i].fileName);
       temp.Tag = toRead.nmllFiles[i].actualFile;
       temp.ContextMenuStrip = treeViewContextMenu;
       if (temp.Text.Contains(".nbl") || toRead.nmllFiles[i].actualFile.GetType() == typeof(NblLoader))
       {
           addNblFiles(temp.Nodes, (NblLoader)toRead.nmllFiles[i].actualFile);
       }
       currNode[0].Nodes.Add(temp);
   }
   if (toRead.tmllFiles != null && toRead.tmllFiles.Length > 0)
   {
       currNode.Add("TMLL chunk");
       for (int i = 0; i < toRead.tmllFiles.Length; i++)
       {
           TreeNode temp = new TreeNode(toRead.tmllFiles[i].fileName);
           temp.Tag = toRead.tmllFiles[i].actualFile;
           currNode[1].Nodes.Add(temp);
       }
   }
}*/


        private void setRightPanel(PsuFile toRead)
        {
            splitContainer1.Panel2.Controls.Clear();
            currentRight = null;
            currentRight = toRead;
            UserControl toAdd = new UserControl();

            if (toRead is TextureFile)
            {
                toAdd = new XvrViewer((TextureFile)toRead);
            }
            else if (toRead is PointeredFile)
            {
                toAdd = new PointeredFileViewer((PointeredFile)toRead);
            }
            else if (toRead is ListFile)
            {
                toAdd = new ListFileViewer((ListFile)toRead);
            }
            else if (toRead is XntFile)
            {
                toAdd = new XntFileViewer((XntFile)toRead);
            }
            else if (toRead is NomFile)
            {
                toAdd = new NomFileViewer((NomFile)toRead);
            }
            else if (toRead is EnemyLayoutFile)
            {
                toAdd = new EnemyLayoutViewer((EnemyLayoutFile)toRead);
            }
            else if (toRead is ItemTechParamFile)
            {
                toAdd = new ItemTechParamViewer((ItemTechParamFile)toRead);
            }
            else if (toRead is ItemSkillParamFile)
            {
                toAdd = new ItemSkillParamViewer((ItemSkillParamFile)toRead);
            }
            else if (toRead is ItemBulletParamFile)
            {
                toAdd = new ItemBulletParamViewer((ItemBulletParamFile)toRead);
            }
            else if (toRead is RmagBulletParamFile)
            {
                toAdd = new RmagBulletViewer((RmagBulletParamFile)toRead);
            }
            else if (toRead is TextFile)
            {
                toAdd = new TextViewer((TextFile)toRead);
            }
            else if (toRead is ScriptFile)
            {
                toAdd = new ScriptFileViewer((ScriptFile)toRead);
            }
            else if (toRead is EnemyLevelParamFile)
            {
                toAdd = new EnemyStatEditor((EnemyLevelParamFile)toRead);
            }
            else if (toRead is WeaponListFile)
            {
                toAdd = new WeaponListEditor((WeaponListFile)toRead);
            }
            else if (toRead is PartsInfoFile)
            {
                toAdd = new PartsInfoViewer((PartsInfoFile)toRead);
            }
            else if (toRead is ItemPriceFile)
            {
                toAdd = new ItemPriceViewer((ItemPriceFile)toRead);
            }
            else if (toRead is EnemyDropFile)
            {
                toAdd = new EnemyDropViewer((EnemyDropFile)toRead);
            }
            else if (toRead is SetFile)
            {
                toAdd = new SetFileViewer((SetFile)toRead);
            }
            else if(toRead is ThinkDragonFile)
            {
                toAdd = new ThinkDragonViewer((ThinkDragonFile)toRead);
            }
            else if (toRead is WeaponParamFile)
            {
                toAdd = new WeaponParamViewer((WeaponParamFile)toRead);
            }
            else if (toRead is ItemSuitParamFile)
            {
                toAdd = new ClothingFileViewer((ItemSuitParamFile)toRead);
            }
            else if (toRead is ItemUnitParamFile)
            {
                toAdd = new UnitParamViewer((ItemUnitParamFile)toRead);
            }
            else if (toRead is EnemyLevelParamFile)
            {
                toAdd = new EnemyStatEditor((EnemyLevelParamFile)toRead);
            }
            else if (toRead is CommonInfoFile)
            {
                toAdd = new ItemCommonInfoViewer((CommonInfoFile)toRead);
            }
            else if(toRead is QuestListFile)
            {
                toAdd = new QuestListViewer((QuestListFile)toRead);
            }
            else if (toRead is UnpointeredFile)
            {
                toAdd = new UnpointeredFileViewer((UnpointeredFile)toRead);
            }
            splitContainer1.Panel2.Controls.Add(toAdd);
            toAdd.Dock = DockStyle.Fill;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (loadedNbl != null && listBox1.SelectedIndex > -1)
            {
                splitContainer1.Panel2.Controls.Clear();
                if (listBox1.SelectedIndex < loadedNbl.nmllFiles.Length)
                {
                    string fileIdentifier = new string(Encoding.ASCII.GetChars(loadedNbl.splitFiles[listBox1.SelectedIndex], 0, 4));
                    if (fileIdentifier == "RIPC") // THIS SECTION NEEDS FIXED
                    {
                        loadedTex = new TextureViewer(loadedNbl.nmllFiles[listBox1.SelectedIndex].subHeader, loadedNbl.splitFiles[listBox1.SelectedIndex], (string)listBox1.SelectedItem);
                        splitContainer1.Panel2.Controls.Add(loadedTex);
                        //loadedTex.Location = new Point(0, 0);
                        //loadedTex.Size = new Size(300, 300);
                        //loadedTex.Show();
                    }
                    else if (loadedNbl.filenames[listBox1.SelectedIndex].Contains("filelist"))
                    {
                        loadedList = new ListViewer(loadedNbl.splitFiles[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition, loadedNbl.splitPointers[listBox1.SelectedIndex], listTypes.filelist);
                        splitContainer1.Panel2.Controls.Add(loadedList);
                    }
                    else if (loadedNbl.filenames[listBox1.SelectedIndex].Contains(".xna"))
                    {
                        loadedList = new ListViewer(loadedNbl.splitFiles[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition, loadedNbl.splitPointers[listBox1.SelectedIndex], listTypes.XNA);
                        splitContainer1.Panel2.Controls.Add(loadedList);
                    }
                    else if (loadedNbl.filenames[listBox1.SelectedIndex].Contains(".xnt"))    //Ugh, should probably do SOMETHING else here, not sure what.
                    {
                        loadedList = new ListViewer(loadedNbl.splitFiles[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition, loadedNbl.splitPointers[listBox1.SelectedIndex], listTypes.XNT);
                        splitContainer1.Panel2.Controls.Add(loadedList);
                    }
                    else if (loadedNbl.filenames[listBox1.SelectedIndex].Contains("particle_info"))
                    {
                        loadedList = new ListViewer(loadedNbl.splitFiles[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition, loadedNbl.splitPointers[listBox1.SelectedIndex], listTypes.particle);
                        splitContainer1.Panel2.Controls.Add(loadedList);
                    }
                    else
                    {
                        if (loadedNbl.filenames[listBox1.SelectedIndex].Contains(".bin"))
                        {
                            PsuFile.FromRaw(loadedNbl.filenames[listBox1.SelectedIndex], loadedNbl.splitFiles[listBox1.SelectedIndex], loadedNbl.nmllFiles[listBox1.SelectedIndex].subHeader).ToRaw();

                        }
                        if (loadedNbl.splitFiles[listBox1.SelectedIndex][0] < 0x30 || listBox1.SelectedIndex >= loadedNbl.splitPointers.Length)
                            loadedBin = new BinaryViewer(loadedNbl.splitFiles[listBox1.SelectedIndex]);
                        else
                        {
                            //PsuFile.FromRaw(loadedNbl.filenames[listBox1.SelectedIndex], loadedNbl.splitFiles[listBox1.SelectedIndex], loadedNbl.nmllFiles[listBox1.SelectedIndex].subHeader, loadedNbl.splitPointers[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition);
                            loadedBin = new BinaryViewer(loadedNbl.splitFiles[listBox1.SelectedIndex], (int)loadedNbl.nmllFiles[listBox1.SelectedIndex].filePosition, loadedNbl.splitPointers[listBox1.SelectedIndex]);
                        }
                        //panel1.Controls.Clear();
                        //if(!panel1.Controls.Contains(loadedBin))
                        splitContainer1.Panel2.Controls.Add(loadedBin);
                        //loadedBin.Location = new Point(0, 0);
                        //loadedBin.Dock = DockStyle.Fill;
                    }
                }
                else
                {
                    loadedTex = new TextureViewer(loadedNbl.tmllFiles[listBox1.SelectedIndex - loadedNbl.nmllFiles.Length].subHeader, loadedNbl.splitTmll[listBox1.SelectedIndex - loadedNbl.nmllFiles.Length], (string)listBox1.SelectedItem);
                    //.Controls.Clear();
                    splitContainer1.Panel2.Controls.Add(loadedTex);
                    //loadedTex.Location = new Point(0, 0);
                    //loadedTex.Dock = DockStyle.Fill;
                }
            }*/
        }

        private void exportBlob_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
            {
                CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
                goodOpenFileDialog.IsFolderPicker = true;
                if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    loadedNbl.exportDataBlob(goodOpenFileDialog.FileName);
                }
            }
                
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
            {
                saveFileDialog1.FileName = fileDialog.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedNbl.saveFile(saveFileDialog1.OpenFile(), compressNMLL, compressTMLL, false);
                    this.Text = "PSU Generic Parser " + Path.GetFileName(saveFileDialog1.FileName);
                }
            }
            else if (loadedAfs != null)
            {
                saveFileDialog1.FileName = fileDialog.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.saveFile(saveFileDialog1.OpenFile());
                    this.Text = "PSU Generic Parser " + Path.GetFileName(saveFileDialog1.FileName);
                }
            }
        }

        private void setQuest_Click(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.setQuest(importDialog.OpenFile());
                }
            }
        }

        private void setZone_Click_1(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.setZone((int)zoneUD.Value, importDialog.OpenFile());
                }
            }
        }

        private void addZone_Click_1(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.addZone((int)zoneUD.Value, importDialog.OpenFile());
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            if (e.Node.Text != "NMLL chunk" && e.Node.Text != "TMLL chunk")
            {
                if (e.Node.Tag != null && e.Node.Tag is object[])
                {
                    ContainerFile parent = (ContainerFile)((object[])e.Node.Tag)[0];
                    string filename = (string)((object[])e.Node.Tag)[1];
                    setRightPanel((PsuFile)parent.getFileParsed(filename));
                }
            }
        }

        private void addFile_Click(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.addFile(fileDialog.SafeFileName, fileDialog.OpenFile());
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
            ContextMenuStrip temp = (ContextMenuStrip)((ToolStripMenuItem)sender).Owner;
            TreeView tempTree = (TreeView)temp.SourceControl;
            TreeNode selectedNode = tempTree.SelectedNode;// = (TreeNode)((MenuItem)sender).GetContextMenu().SourceControl;
            ContainerFile owningFile;
            owningFile = (ContainerFile)((object[])selectedNode.Tag)[0];
            OpenFileDialog replaceDialog = new OpenFileDialog();
            replaceDialog.FileName = selectedNode.Text;
            if (replaceDialog.ShowDialog() == DialogResult.OK)
            {
                RawFile file = new RawFile(replaceDialog.OpenFile());
                owningFile.replaceFile(selectedNode.Text, file);
                selectedNode.Text = file.filename;
                ((object[])selectedNode.Tag)[1] = file.filename;
                /*
                owningFile.replaceFile(selectedNode.Text, replaceDialog.OpenFile());
                if(selectedNode.Parent.Index == 0)
                    selectedNode.Tag = owningFile.nmllFiles[selectedNode.Index].actualFile;
                else
                    selectedNode.Tag = owningFile.tmllFiles[selectedNode.Index].actualFile;*/
                setRightPanel(owningFile.getFileParsed(selectedNode.Text));
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
            /*
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && loadedNbl != null)
            {
                foreach (NblLoader.FileHeader currFile in loadedNbl.nmllFiles)
                {
                    if (currFile.actualFile.GetType() == typeof(WeaponParamFile))
                    {
                        FileStream outStream = new FileStream(folderBrowserDialog1.SelectedPath + "\\" + currFile.fileName + ".txt", FileMode.Create);
                        ((WeaponParamFile)currFile.actualFile).saveTextFile(outStream);
                        outStream.Close();
                    }
                }
            }*/
        }

        private void importAllWeaponsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && loadedNbl != null)
            {
                foreach (NblLoader.FileHeader currFile in loadedNbl.nmllFiles)
                {
                    if (currFile.actualFile.GetType() == typeof(WeaponParamFile) && File.Exists(folderBrowserDialog1.SelectedPath + "\\" + currFile.fileName + ".txt"))
                    {
                        FileStream inStream = new FileStream(folderBrowserDialog1.SelectedPath + "\\" + currFile.fileName + ".txt", FileMode.Open);
                        ((WeaponParamFile)currFile.actualFile).loadTextFile(inStream);
                        inStream.Close();
                    }
                }
            }
             */
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void insertNMLLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null && treeView1.SelectedNode != null && treeView1.SelectedNode.Level == 1)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream inStream = fileDialog.OpenFile();
                    loadedNbl.chunks[0].addFile(treeView1.SelectedNode.Index, new RawFile(inStream));
                    inStream.Close();
                    treeView1.Nodes.Clear();

                    addChildFiles(treeView1.Nodes, loadedNbl);
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
                    //sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4C)) + "\t");
                    //sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4E)) + "\n");
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

        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
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

                if (currentRight is TextureFile)
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
                    if (currentRight is TextureFile && Path.GetExtension(exportFileDialog.FileName).Equals(".png"))
                    {
                        ((TextureFile)currentRight).mipMaps[0].Save(exportFileDialog.FileName);
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
            string extension = Path.GetExtension(node.Text);
            if (node.Text != "NMLL chunk" && node.Text != "TMLL chunk" && node.Text != "NMLB chunk" && node.Text != "TMLB chunk")
            {
                if (node.Tag != null && node.Tag is object[])
                {
                    ContainerFile parent = (ContainerFile)((object[])node.Tag)[0];
                    string filename = (string)((object[])node.Tag)[1];
                    PsuFile file = parent.getFileParsed(filename);
                    filename = Path.Combine(fileDirectory, filename);

                    if (file is TextureFile && batchPngExport)
                    {
                        filename = filename.Replace(".xvr", ".png");
                        ((TextureFile)file).mipMaps[0].Save(filename);
                    }
                    else
                    {
                        if (extension != ".nbl" || extension != ".afs")
                        {
                            extractFile(file, filename);
                        }
                        else if (batchExportSubArchiveFiles)
                        {
                            extractFile(file, filename);
                        }
                    }
                }
            }

            //Handle it as a new archive within the current one
            if(extension == ".nbl" || extension == ".afs")
            {
                string newFolder = fileDirectory + @"\" + node.Text + "_ext";
                exportAll(node.Nodes, newFolder);
            } else
            {
                foreach (TreeNode nodeChild in node.Nodes)
                {
                    exportNode(nodeChild, fileDirectory);
                }
            }
            
        }

        private void extractFile(PsuFile psuFile, string filename)
        {
            RawFile file = psuFile.ToRawFile(0);
            byte[] bytes = file.WriteToBytes(exportMetaData);
            /*

            List<byte> output = new List<byte>();
            byte[] toSave = psuFile.ToRaw();
            byte[] subHeader = psuFile.header;
            int[] pointers = psuFile.calculatedPointers;
            string fileNameSansPath = Path.GetFileName(filename);

            if (exportMetaData == true)
            {
                if (pointers == null)
                {
                    output.AddRange(Encoding.ASCII.GetBytes("STD\0"));
                }
                else
                {
                    output.AddRange(Encoding.ASCII.GetBytes(fileNameSansPath.ToUpper().ToCharArray(fileNameSansPath.Length - 3, 3)));
                    output.Add(0);
                }

                output.AddRange(new byte[0xC]);
                output.AddRange(ContainerUtilities.encodePaddedSjisString(fileNameSansPath, 0x20));
                output.AddRange(new byte[0x4]);
                output.AddRange(BitConverter.GetBytes(toSave.Length));
                output.AddRange(new byte[0x4]);

                if (pointers != null)
                {
                    output.AddRange(BitConverter.GetBytes(pointers.Length));
                }
                else
                {
                    output.AddRange(new byte[0x4]);
                }
                if (subHeader != null)
                {
                    output.AddRange(subHeader);
                }
                else
                {
                    output.AddRange(new byte[0x20]);
                }
            }
            output.AddRange(toSave);

            if (exportMetaData == true)
            {
                if (pointers != null)
                {
                    //Calc padding to get to pointer start
                    long pointerPadding = ((output.Count + 0x7F) & 0xFFFFFF80) - output.Count;
                    output.AddRange(new byte[pointerPadding]);
                    for (int i = 0; i < pointers.Length; i++)
                    {
                        output.AddRange(BitConverter.GetBytes(pointers[i]));
                    }
                }
            }*/
            try
            {
                //File.WriteAllBytes(filename, output.ToArray());
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
            //currentFileHexForm.setBytesDelegate = new SetBytesDelegate(this.setFileBytes);
        }

        /*
        private void setFileBytes(byte[] fileBytes)
        {

        }*/

        private void enableNMLLLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
