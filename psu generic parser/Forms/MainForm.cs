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
using System.Drawing;
using PSULib.FileClasses;
using psu_generic_parser.Forms.FileViewers;

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
            else if (toRead is XnaFile)
            {
                toAdd = new XnaFileViewer((XnaFile)toRead);
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
            else if (toRead is ObjectParticleInfoFile)
            {
                toAdd = new ObjectParticleInfoFileViewer((ObjectParticleInfoFile)toRead);
            }
            else if (toRead is ObjectParamFile)
            {
                toAdd = new ObjParamViewer((ObjectParamFile)toRead);
            }
            else if (toRead is UnpointeredFile)
            {
                toAdd = new UnpointeredFileViewer((UnpointeredFile)toRead);
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
                    loadedContainer.saveFile(saveFileDialog1.OpenFile());
                    this.Text = "PSU Generic Parser " + Path.GetFileName(saveFileDialog1.FileName);
                    fileDialog.FileName = saveFileDialog1.FileName;
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

                    if (file is TextureFile && batchPngExport)
                    {
                        string filename = Path.Combine(fileDirectory, Path.GetFileName(originalFilename + ".png"));
                        ((TextureFile)file).mipMaps[0].Save(filename);
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
    }
}
