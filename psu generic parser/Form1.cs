using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using psu_generic_parser.FileViewers;
using psu_generic_parser.FileClasses;
using System.Security.Cryptography;

namespace psu_generic_parser
{
    public partial class Form1 : Form
    {
        //int baseAddr = 0x9EC60; //Dummy from when this program was VERY different.
        //FileStream addrList;// = new FileStream("C:\\SEGA\\PHANTASY STAR UNIVERSE Illuminus\\DATA\\sb6800\\0201ptrs.bin", FileMode.Open);
        //FileStream currFile;// = new FileStream("C:\\SEGA\\PHANTASY STAR UNIVERSE Illuminus\\DATA\\sb6800\\nmll\\0201.xnj", FileMode.Open);
        NblLoader loadedNbl;
        AfsLoader loadedAfs;
        MiniAfsLoader loadedMiniAfs;
        TextureViewer loadedTex;
        BinaryViewer loadedBin;
        ListViewer loadedList;
        PsuFile currentRight;
        //PsuFile currentOwner;

        public Form1()
        {
            InitializeComponent();
            Text += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //textBox1.Text = baseAddr.ToString();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] formatName = new byte[4];
                using (Stream stream = openFileDialog1.OpenFile())
                {
                    stream.Read(formatName, 0, 4);
                }
                splitContainer1.Panel2.Controls.Clear();
                string identifier = new string(Encoding.ASCII.GetChars(formatName, 0, 4));
                if (identifier == "NMLL" || identifier == "NMLB")
                {
                    treeView1.Nodes.Clear();
                    loadedAfs = null;
                    loadedMiniAfs = null;
                    using (Stream stream = openFileDialog1.OpenFile())
                    {
                        loadedNbl = new NblLoader(stream);
                        //listBox1.Items.Clear();
                        //for (int i = 0; i < loadedNbl.filenames.Length; i++)
                        //{
                        //listBox1.Items.Add(loadedNbl.filenames[i]);
                        //}
                        addChildFiles(treeView1.Nodes, loadedNbl);
                        /*
                        treeView1.Nodes.Add("NMLL chunk");
                        for (int i = 0; i < loadedNbl.nmllFiles.Length; i++)
                        {
                            TreeNode temp = new TreeNode(loadedNbl.nmllFiles[i].fileName);
                            if (temp.Text.Contains(".nbl"))
                                addNblFiles(temp, (NblLoader)loadedNbl.nmllFiles[i].actualFile);
                            treeView1.Nodes[0].Nodes.Add(temp);

                        }
                        if (loadedNbl.tmllFiles != null && loadedNbl.tmllFiles.Length > 0)
                        {
                            treeView1.Nodes.Add("TMLL chunk");
                            for (int i = 0; i < loadedNbl.tmllFiles.Length; i++)
                            {
                                treeView1.Nodes[1].Nodes.Add(loadedNbl.tmllFiles[i].fileName);
                            }
                        }*/
                        checkBox1.Checked = loadedNbl.isCompressed;
                    }
                }
                if (new string(Encoding.ASCII.GetChars(formatName, 0, 4)) == "AFS\0")
                {
                    treeView1.Nodes.Clear();
                    loadedNbl = null;
                    loadedMiniAfs = null;
                    using (Stream stream = openFileDialog1.OpenFile())
                    {
                        loadedAfs = new AfsLoader(stream);
                        splitContainer1.Panel2.Controls.Clear();
                        //listBox1.Items.Clear();
                        /*TextBox txtBox = new TextBox();
                        splitContainer1.Panel2.Controls.Add(txtBox);
                        txtBox.Dock = DockStyle.Fill;
                        txtBox.Multiline = true;
                        txtBox.ScrollBars = ScrollBars.Vertical;*/

                        //for (int i = 0; i < loadedAfs.fileCount; i++)
                        //{
                        /*txtBox.Text += "\r\n" + loadedAfs.afsList[i].fileName + "\t\t" + loadedAfs.afsList[i].location.ToString("X8") + " " + loadedAfs.afsList[i].fileSize.ToString("X8") + "\t\t" + loadedAfs.afsList[i].year + "-" + loadedAfs.afsList[i].month + "-" + loadedAfs.afsList[i].day + " " 
                            + loadedAfs.afsList[i].hour.ToString("D2") + ":" + loadedAfs.afsList[i].minute.ToString("D2") + ":" + loadedAfs.afsList[i].second.ToString("D2") + "\t" + loadedAfs.afsList[i].garbageInt.ToString("X8");*/
                        //  listBox1.Items.Add(loadedAfs.afsList[i].fileName);
                        /*TreeNode temp = new TreeNode(loadedAfs.afsList[i].fileName);
                        if (loadedAfs.subPaths[i] != null)
                            addAfsFiles(temp, loadedAfs.subPaths[i]);
                        else if (loadedAfs.nblContents[i] != null)
                            addNblFiles(temp, loadedAfs.nblContents[i]);
                        treeView1.Nodes.Add(temp);*/
                        //}

                        addChildFiles(treeView1.Nodes, loadedAfs);
                    }
                }
                if (BitConverter.ToInt16(formatName, 0) == 0x50AF)
                {
                    treeView1.Nodes.Clear();
                    loadedAfs = null;
                    loadedNbl = null;
                    using (Stream stream = openFileDialog1.OpenFile())
                    {
                        loadedMiniAfs = new MiniAfsLoader(stream);
                        splitContainer1.Panel2.Controls.Clear();
                        //listBox1.Items.Clear();
                        /*TextBox txtBox = new TextBox();
                        splitContainer1.Panel2.Controls.Add(txtBox);
                        txtBox.Dock = DockStyle.Fill;
                        txtBox.Multiline = true;
                        txtBox.ScrollBars = ScrollBars.Vertical;*/

                        //for (int i = 0; i < loadedAfs.fileCount; i++)
                        //{
                        /*txtBox.Text += "\r\n" + loadedAfs.afsList[i].fileName + "\t\t" + loadedAfs.afsList[i].location.ToString("X8") + " " + loadedAfs.afsList[i].fileSize.ToString("X8") + "\t\t" + loadedAfs.afsList[i].year + "-" + loadedAfs.afsList[i].month + "-" + loadedAfs.afsList[i].day + " " 
                            + loadedAfs.afsList[i].hour.ToString("D2") + ":" + loadedAfs.afsList[i].minute.ToString("D2") + ":" + loadedAfs.afsList[i].second.ToString("D2") + "\t" + loadedAfs.afsList[i].garbageInt.ToString("X8");*/
                        //  listBox1.Items.Add(loadedAfs.afsList[i].fileName);
                        /*TreeNode temp = new TreeNode(loadedAfs.afsList[i].fileName);
                        if (loadedAfs.subPaths[i] != null)
                            addAfsFiles(temp, loadedAfs.subPaths[i]);
                        else if (loadedAfs.nblContents[i] != null)
                            addNblFiles(temp, loadedAfs.nblContents[i]);
                        treeView1.Nodes.Add(temp);*/
                        //}

                        addChildFiles(treeView1.Nodes, loadedMiniAfs);
                    }
                }
            }
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
            currentRight = toRead;
            UserControl toAdd = new UserControl();
            if (toRead is TextureFile)
            {
                toAdd = new XvrViewer((TextureFile)toRead);
            }
            else if (toRead.GetType() == typeof(PointeredFile))
            {
                toAdd = new PointeredFileViewer((PointeredFile)toRead);
            }
            else if (toRead.GetType() == typeof(PointeredFile360))
            {
                toAdd = new PointeredFileViewer((PointeredFile360)toRead);
            }
            else if (toRead.GetType() == typeof(ListFile))
            {
                toAdd = new ListFileViewer((ListFile)toRead);
            }
            else if (toRead.GetType() == typeof(XntFile))
            {
                toAdd = new XntFileViewer((XntFile)toRead);
            }
            else if (toRead.GetType() == typeof(NomFile))
            {
                toAdd = new NomFileViewer((NomFile)toRead);
            }
            else if (toRead.GetType() == typeof(EnemyLayoutFile))
            {
                toAdd = new EnemyLayoutViewer((EnemyLayoutFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemTechParamFile))
            {
                toAdd = new ItemTechParamViewer((ItemTechParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemSkillParamFile))
            {
                toAdd = new ItemSkillParamViewer((ItemSkillParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemBulletParamFile))
            {
                toAdd = new ItemBulletParamViewer((ItemBulletParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(RmagBulletParamFile))
            {
                toAdd = new RmagBulletViewer((RmagBulletParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(TextFile))
            {
                toAdd = new TextViewer((TextFile)toRead);
            }
            else if (toRead.GetType() == typeof(ScriptFile))
            {
                toAdd = new ScriptFileViewer((ScriptFile)toRead);
            }
            else if (toRead.GetType() == typeof(EnemyLevelParamFile))
            {
                toAdd = new EnemyStatEditor((EnemyLevelParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(WeaponListFile))
            {
                toAdd = new WeaponListEditor((WeaponListFile)toRead);
            }
            else if (toRead.GetType() == typeof(PartsInfoFile))
            {
                toAdd = new PartsInfoViewer((PartsInfoFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemPriceFile))
            {
                toAdd = new ItemPriceViewer((ItemPriceFile)toRead);
            }
            else if (toRead.GetType() == typeof(EnemyDropFile))
            {
                toAdd = new EnemyDropViewer((EnemyDropFile)toRead);
            }
            else if (toRead.GetType() == typeof(SetFile))
            {
                toAdd = new SetFileViewer((SetFile)toRead);
            }
            else if(toRead.GetType() == typeof(ThinkDragonFile))
            {
                toAdd = new ThinkDragonViewer((ThinkDragonFile)toRead);
            }
            else if (toRead.GetType() == typeof(WeaponParamFile))
            {
                toAdd = new WeaponParamViewer((WeaponParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemSuitParamFile))
            {
                toAdd = new ClothingFileViewer((ItemSuitParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(ItemUnitParamFile))
            {
                toAdd = new UnitParamViewer((ItemUnitParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(EnemyLevelParamFile))
            {
                toAdd = new EnemyStatEditor((EnemyLevelParamFile)toRead);
            }
            else if (toRead.GetType() == typeof(CommonInfoFile))
            {
                toAdd = new ItemCommonInfoViewer((CommonInfoFile)toRead);
            }
            else if(toRead.GetType() == typeof(QuestListFile))
            {
                toAdd = new QuestListViewer((QuestListFile)toRead);
            }
            else if (toRead.GetType() == typeof(UnpointeredFile))
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
                loadedNbl.exportAll(Path.GetDirectoryName(openFileDialog1.FileName));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
                loadedNbl.exportTmll(Path.GetDirectoryName(openFileDialog1.FileName));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            loadedNbl.compressTest();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
            {
                saveFileDialog1.FileName = openFileDialog1.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedNbl.saveFile(saveFileDialog1.OpenFile(), checkBox1.Checked, checkBox2.Checked, false);
                }
            }
            else if (loadedAfs != null)
            {
                saveFileDialog1.FileName = openFileDialog1.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.saveFile(saveFileDialog1.OpenFile());
                }
            }
        }

        private void exportTMLLBlobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
            {
                saveFileDialog1.FileName = openFileDialog1.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedNbl.exportTmllAll(saveFileDialog1.OpenFile());
                }
            }
        }

        private void saveNBLunmodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null)
            {
                saveFileDialog1.FileName = openFileDialog1.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //loadedNbl.exportTest(saveFileDialog1.OpenFile());
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //if (loadedAfs != null)
            //{
                //if (importDialog.ShowDialog() == DialogResult.OK)
                //{
                    //loadedAfs.replaceFile(listBox1.SelectedIndex, importDialog.OpenFile());
                //}
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.setQuest(importDialog.OpenFile());
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.setZone((int)numericUpDown1.Value, importDialog.OpenFile());
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (importDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.addZone((int)numericUpDown1.Value, importDialog.OpenFile());
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

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            if (currentRight != null && currentRight.GetType() == typeof(TextFile))
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ((TextFile)currentRight).importBinNoReplace(openFileDialog1.OpenFile());
                    setRightPanel(currentRight);
                }
            }*/
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (currentRight.GetType() == typeof(TextFile))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ((TextFile)currentRight).saveToTextFile(saveFileDialog1.OpenFile());
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (loadedAfs != null)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    loadedAfs.addFile(openFileDialog1.SafeFileName, openFileDialog1.OpenFile());
                }
            }
        }

        private void exportCurrentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] toSave = currentRight.ToRaw();
            int[] pointers = currentRight.calculatedPointers;
            byte[] subHeader = currentRight.header;
            saveFileDialog1.FileName = currentRight.filename;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream outFile = (FileStream)saveFileDialog1.OpenFile();
                BinaryWriter outWriter = new BinaryWriter(outFile);
                if(pointers == null)
                    outWriter.Write(ASCIIEncoding.ASCII.GetBytes("STD\0"));
                else
                    outWriter.Write(ASCIIEncoding.ASCII.GetBytes(currentRight.filename.ToUpper().ToCharArray(currentRight.filename.Length - 3, 3)));
                outFile.Seek(0x10, SeekOrigin.Begin);
                outWriter.Write(ASCIIEncoding.ASCII.GetBytes(currentRight.filename.PadRight(0x20, '\0')));
                outWriter.Write((int)0);
                outWriter.Write(toSave.Length);
                outWriter.Write((int)0);
                if(pointers != null)
                    outWriter.Write((int)pointers.Length);
                else
                    outWriter.Write((int)0);
                if (subHeader != null)
                    outWriter.Write(subHeader);
                else
                    outFile.Seek(0x60, SeekOrigin.Begin);
                outWriter.Write(toSave);
                outFile.Seek((outFile.Position + 0x7F) & 0xFFFFFF80, SeekOrigin.Begin);
                if (pointers != null)
                {
                    for (int i = 0; i < pointers.Length; i++)
                        outWriter.Write(pointers[i]);
                }
                outWriter.Close();
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

        private void replaceFileToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void createMissionAFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MissionBuilder().Show(this);
        }

        private void insertNMLLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loadedNbl != null && treeView1.SelectedNode != null && treeView1.SelectedNode.Level == 1)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Stream inStream = openFileDialog1.OpenFile();
                    loadedNbl.chunks[0].addFile(treeView1.SelectedNode.Index, new RawFile(inStream));
                    inStream.Close();
                    treeView1.Nodes.Clear();

                    addChildFiles(treeView1.Nodes, loadedNbl);
                }
            }
        }

        private void decryptNMLBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(openFileDialog1.FileName);

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

                    sb.Append(ASCIIEncoding.ASCII.GetString(toDecrypt, 0, 0x20).Split('\0')[0] + "\t");
                    //sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4C)) + "\t");
                    //sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4E)) + "\n");
                }

                fileStream.Seek(nmllDataLoc, SeekOrigin.Begin);
                byte[] encryptedNmll = fileLoader.ReadBytes((int)size);
                byte[] decryptedNmll = fish.decryptBlock(encryptedNmll);
                byte[] decompressedNmll = compressedSize != 0 ? PrsCompDecomp.Decompress(decryptedNmll, uncompressedSize) : decryptedNmll;

                File.WriteAllText(openFileDialog1.FileName + ".tml.list", sb.ToString());
                File.WriteAllBytes(openFileDialog1.FileName + ".decrypt", fileContents);
                File.WriteAllBytes(openFileDialog1.FileName + ".encryptNmll", encryptedNmll);
                File.WriteAllBytes(openFileDialog1.FileName + ".decryptNmll", decryptedNmll);
                File.WriteAllBytes(openFileDialog1.FileName + ".decompressNmll", decompressedNmll);
            }
        }

        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        private void decryptNMLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(openFileDialog1.FileName);

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

                    sb.Append(ASCIIEncoding.ASCII.GetString(toDecrypt, 0, 0x20).Split('\0')[0] + "\t");
                    sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4C)) + "\t");
                    sb.Append(BitConverter.ToUInt16(fileContents, (int)(headerLoc + 0x4E)) + "\n");
                }
                File.WriteAllText(openFileDialog1.FileName + ".tml.list", sb.ToString());
                File.WriteAllBytes(openFileDialog1.FileName + ".decrypt", fileContents);
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
    }
}
