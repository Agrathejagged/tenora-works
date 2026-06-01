using Counter_Editor.Files;
using PSULib.FileClasses.Archives;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Missions;
using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Counter_Editor.Files.CounterArchive;

namespace Counter_Editor
{
    public partial class CounterEditorMainForm : Form
    {
        CounterArchive currentCounter;

        public CounterEditorMainForm()
        {
            InitializeComponent();
            PsuFiles.ExternalFromRaw = ParseFile;
        }

        private PsuFile ParseFile(string filename, byte[] rawData, byte[] inHeader, int[] ptrs, int baseAddr, bool bigEndian = false)
        {
            if (filename.Equals("table.rel"))
                return new TableFile(filename, rawData, inHeader, ptrs, baseAddr);
            return null;
        }

        private void openCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream fileStream = openFileDialog1.OpenFile())
                {
                    byte[] identifier = new byte[4];
                    fileStream.Read(identifier, 0, 4);
                    string id = ASCIIEncoding.ASCII.GetString(identifier);
                    if (id == "NMLL")
                    {
                        currentCounter = new CounterArchive(new NblLoader(fileStream));
                    }
                    else if (id == "AFS\0")
                    {
                        currentCounter = new CounterArchive(new AfsLoader(fileStream));
                    }
                    else
                    {
                        PackFile pack = new PackFile(fileStream);
                        currentCounter = new CounterArchive(pack);
                    }
                    reloadFile();
                }
            }
        }

        private void reloadFile()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Metadata");
            for (int i = 0; i < currentCounter.categories.Count; i++)
            {
                CounterArchive.CategoryInfo cat = currentCounter.categories[i];
                TreeNode node = new TreeNode("Category " + i + ": " + cat.categoryName);
                node.ContextMenuStrip = categoryContextMenu;
                treeView1.Nodes.Add(node);
                for (int j = 0; j < currentCounter.categories[i].categoryNumbersNbls.Count; j++)
                {
                    CounterArchive.QuestDefinition pair = currentCounter.categories[i].categoryNumbersNbls[j];
                    TreeNode questNode = createNode(pair);
                    node.Nodes.Add(questNode);
                }
            }
            saveAsPackToolStripMenuItem.Enabled = true;
            saveAsNBLToolStripMenuItem.Enabled = true;
            saveAsAFSToolStripMenuItem.Enabled = true;
            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
        }

        private TreeNode createNode(CounterArchive.QuestDefinition pair)
        {
            string nblStatus = "present";
            if (pair.QuestNbl == null)
                nblStatus = "absent";
            else
            {
                if (((NblChunk)pair.QuestNbl.getFileParsed(0)).getFilenames().Contains("text.bin"))
                    nblStatus = ((TextFile)((NblChunk)pair.QuestNbl.getFileParsed(0)).getFileParsed("text.bin")).strings[0][0][0];
                else
                    nblStatus = "(unnamed)";
            }
            TreeNode questNode = new TreeNode("Quest " + pair.QuestNumber + " " + nblStatus);
            questNode.ContextMenuStrip = questContextMenu;
            return questNode;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            if (e.Node.Level == 0 && e.Node.Index == 0)
            {
                CounterMetadataEditor editor = new CounterMetadataEditor(currentCounter);
                editor.Dock = DockStyle.Fill;
                editor.Parent = this;
                splitContainer1.Panel2.Controls.Add(editor);
            }
            else if (e.Node.Level == 0)
            {
                MetadataEditor editor = new MetadataEditor(currentCounter.categories[e.Node.Index - 1], treeView1.SelectedNode);
                editor.Dock = DockStyle.Fill;
                editor.Parent = this;
                splitContainer1.Panel2.Controls.Add(editor);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsPackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (currentCounter != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream outStream = saveFileDialog1.OpenFile())
                {
                    currentCounter.WriteAsPack(outStream);
                }
            }
        }

        private void purgeMissingMissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCounter != null)
            {
                currentCounter.PurgeAbsences();
                reloadFile();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void removeMissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 1) //JUST IN CASE
            {
                int categoryNumber = treeView1.SelectedNode.Parent.Index - 1;
                int questIndex = treeView1.SelectedNode.Index;
                currentCounter.categories[categoryNumber].categoryNumbersNbls.RemoveAt(questIndex);
                treeView1.SelectedNode.Parent.Nodes.RemoveAt(questIndex);
            }
        }

        private void switchCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 1) //JUST IN CASE
            {
                int categoryNumber = treeView1.SelectedNode.Parent.Index - 1;
                int questIndex = treeView1.SelectedNode.Index;
                currentCounter.categories[categoryNumber].categoryNumbersNbls.RemoveAt(questIndex);
                treeView1.SelectedNode.Parent.Nodes.RemoveAt(questIndex);
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (NewNode.Level == 1) //If it's a mission, allow reordering missions DO NOT ALLOW DROPPING INTO TOP LEVEL.
                {
                    if (DestinationNode.Level == 0)
                    {
                        //Just kinda drop it at the end of the list on the new node.
                        int categoryNumber = NewNode.Parent.Index - 1;
                        int questIndex = NewNode.Index;
                        int newCategoryNumber = DestinationNode.Index - 1;
                        Files.CounterArchive.QuestDefinition questPair = currentCounter.categories[categoryNumber].categoryNumbersNbls[questIndex];
                        currentCounter.categories[categoryNumber].categoryNumbersNbls.RemoveAt(questIndex);
                        NewNode.Remove();
                        currentCounter.categories[newCategoryNumber].categoryNumbersNbls.Add(questPair);
                        DestinationNode.Nodes.Add(NewNode);
                    }
                    else if (DestinationNode.Level == 1)
                    {
                        int categoryNumber = NewNode.Parent.Index - 1;
                        int questIndex = NewNode.Index;
                        int newQuestIndex = DestinationNode.Index;
                        Files.CounterArchive.QuestDefinition questPair = currentCounter.categories[categoryNumber].categoryNumbersNbls[questIndex];
                        currentCounter.categories[categoryNumber].categoryNumbersNbls.RemoveAt(questIndex);
                        NewNode.Remove();
                        currentCounter.categories[categoryNumber].categoryNumbersNbls.Insert(newQuestIndex, questPair);
                        DestinationNode.Parent.Nodes.Insert(newQuestIndex, NewNode);
                    }
                }
                else if (NewNode.Level == 0 && NewNode.Index != 0 && DestinationNode.Level == 0 && DestinationNode != NewNode) //Don't allow dragging it onto itself THAT'S SILLY
                {
                    int categoryNumber = NewNode.Index - 1;
                    int newCategoryNumber = DestinationNode.Index;
                    CounterArchive.CategoryInfo cat = currentCounter.categories[categoryNumber];
                    currentCounter.categories.RemoveAt(categoryNumber);
                    currentCounter.categories.Insert(newCategoryNumber - 1, cat);
                    treeView1.Nodes.Remove(NewNode);
                    treeView1.Nodes.Insert(newCategoryNumber, NewNode);
                }
            }
        }

        private void addMissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int categoryNumber = treeView1.SelectedNode.Index - 1;
            if (insertMissionDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(insertMissionDialog.FileName);
                RawFile raw = new RawFile
                {
                    fileContents = fileContents
                };
                using (Stream nblStream = new MemoryStream(fileContents))
                {
                    NblLoader newNbl = new NblLoader(nblStream);
                    NblChunk nblChunk = (NblChunk)newNbl.getFileParsed(0);
                    string questFilename = nblChunk.getFilenames().Find(filename => Regex.IsMatch(filename, "quest\\.[usxy]nr"));
                    if (questFilename != null)
                    {
                        RawFile questFile = nblChunk.getFileRaw(questFilename);
                        int questNumber = QuestDataExtractor.GetQuestNumber(questFile);
                        QuestDefinition pair = new QuestDefinition(questNumber, raw, newNbl);
                        currentCounter.categories[categoryNumber].categoryNumbersNbls.Add(pair);
                        TreeNode node = createNode(pair);
                        treeView1.SelectedNode.Nodes.Add(node);
                    }
                }
            }
        }

        private void replaceNBLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int categoryNumber = treeView1.SelectedNode.Parent.Index - 1;
            int questIndex = treeView1.SelectedNode.Index;
            if (insertMissionDialog.ShowDialog() == DialogResult.OK)
            {
                NblLoader newNbl;
                using (Stream nblStream = insertMissionDialog.OpenFile())
                {
                    newNbl = new NblLoader(nblStream);
                }
                currentCounter.categories[categoryNumber].categoryNumbersNbls[questIndex].QuestNbl = newNbl;
                TreeNode newNode = createNode(currentCounter.categories[categoryNumber].categoryNumbersNbls[questIndex]);
                treeView1.SelectedNode.Text = newNode.Text;
            }
        }

        private void removeCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int categoryNumber = treeView1.SelectedNode.Index - 1;
            currentCounter.categories.RemoveAt(categoryNumber);
            treeView1.SelectedNode.Remove();
        }

        private void newCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCounter != null)
            {
                currentCounter.categories.Add(new CounterArchive.CategoryInfo());
                TreeNode node = new TreeNode("Category " + treeView1.Nodes.Count + ": unnamed")
                {
                    ContextMenuStrip = categoryContextMenu
                };
                treeView1.Nodes.Add(node);
            }
        }

        private void markDirtyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int categoryNumber = treeView1.SelectedNode.Parent.Index - 1;
            int questIndex = treeView1.SelectedNode.Index;
            currentCounter.categories[categoryNumber].categoryNumbersNbls[questIndex].QuestNbl.dirty = true;
        }

        private void saveAsNBLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCounter != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream outStream = saveFileDialog1.OpenFile())
                {
                    currentCounter.WriteAsNbl(outStream);
                }
            }
        }

        private void saveAsAFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCounter != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream outStream = saveFileDialog1.OpenFile())
                {
                    currentCounter.WriteAsAfs(outStream);
                }
            }
        }

        private void newCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentCounter = new CounterArchive();
            reloadFile();
        }

        private void importDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Level == 0 && treeView1.SelectedNode.Index > 0 && folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] directoryFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*quest_j.nbl");
                foreach (string currFile in directoryFiles)
                {
                    addMission(currFile);
                }
            }
        }

        private void addMission(string currFile)
        {
            int categoryNumber = treeView1.SelectedNode.Index - 1;
            byte[] fileContents = File.ReadAllBytes(currFile);
            string id = ASCIIEncoding.ASCII.GetString(fileContents, 0, 4);
            if (id == "NMLL")
            {
                using (Stream nblStream = new MemoryStream(fileContents))
                {
                    RawFile raw = new RawFile
                    {
                        fileContents = fileContents
                    };
                    NblLoader newNbl = new NblLoader(nblStream);
                    NblChunk nblChunk = (NblChunk)newNbl.getFileParsed(0);
                    string questFilename = nblChunk.getFilenames().Find(filename => Regex.IsMatch(filename, "quest\\.[usxy]nr"));
                    if (questFilename != null)
                    {
                        RawFile questFile = nblChunk.getFileRaw(questFilename);
                        int questNumber = QuestDataExtractor.GetQuestNumber(questFile);
                        QuestDefinition pair = new QuestDefinition(questNumber, raw, newNbl);
                        currentCounter.categories[categoryNumber].categoryNumbersNbls.Add(pair);
                        TreeNode node = createNode(pair);
                        treeView1.SelectedNode.Nodes.Add(node);
                    }
                }
            }
        }
    }
}
