using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class TextViewer : UserControl
    {
        private int selectedList = -1;
        private int selectedEntry = -1;
        private TextFile fileToRead;
        private static readonly string STRING_NODE_TAG_TEXT = "stringNode";

        public TextViewer(TextFile toSet)
        {
            InitializeComponent();
            fileToRead = toSet;
            //Going to assume the top level only ever has one entry--I've never seen more than one, and as far as I'm aware
            //the game uses a 2-int index rather than a 3 (e.g list 6 entry 5, not list 0 sublist 6 entry 5)
            for (int j = 0; j < fileToRead.strings[0].Count; j++)
            {
                TreeNode currLevel2 = new TreeNode("List " + j.ToString("X2"));
                for (int k = 0; k < fileToRead.strings[0][j].Count; k++)
                {
                    TreeNode currLevel3 = new TreeNode(k.ToString("X2") + " " + fileToRead.strings[0][j][k]);
                    currLevel3.Tag = STRING_NODE_TAG_TEXT;
                    currLevel2.Nodes.Add(currLevel3);
                }
                currLevel2.Nodes.Add("Add new...");
                currLevel2.Expand();
                treeView1.Nodes.Add(currLevel2);
            }
        }

        private void storeButton_Click(object sender, EventArgs e)
        {
            if (selectedList != -1 && selectedEntry != -1)
            {
                if (treeView1.SelectedNode.Tag == null)
                {
                    treeView1.SelectedNode.Tag = STRING_NODE_TAG_TEXT;
                    fileToRead.strings[0][selectedList].Add("");
                    treeView1.SelectedNode.Parent.Nodes.Add("Add new...");
                }
                fileToRead.strings[0][selectedList][selectedEntry] = textBox1.Text;
                treeView1.Nodes[selectedList].Nodes[selectedEntry].Text = selectedEntry.ToString("X2") + " " + textBox1.Text;
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                selectedEntry = e.Node.Index;
                selectedList = e.Node.Parent.Index;
                storeButton.Enabled = true;
                textBox1.Enabled = true;
                if (e.Node.Tag != null)
                {
                    textBox1.Text = fileToRead.strings[0][selectedList][selectedEntry];
                }
                else
                {
                    textBox1.Clear();
                }
            }
            else
            {
                textBox1.Clear();
                textBox1.Enabled = false;
                storeButton.Enabled = false;
                selectedEntry = -1;
                selectedList = -1;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedList != -1 && selectedEntry != -1)
            {
                if ((e.KeyCode & (Keys.Enter)) == Keys.Enter && e.Control == true)
                {
                    if (treeView1.SelectedNode.Tag == null)
                    {
                        fileToRead.strings[0][selectedList].Add("");
                        treeView1.SelectedNode.Tag = STRING_NODE_TAG_TEXT;
                        treeView1.SelectedNode.Parent.Nodes.Add("Add new...");
                    }
                    fileToRead.strings[0][selectedList][selectedEntry] = textBox1.Text;
                    treeView1.Nodes[selectedList].Nodes[selectedEntry].Text = selectedEntry.ToString("X2") + " " + fileToRead.strings[0][selectedList][selectedEntry];
                    if (selectedEntry + 1 >= treeView1.Nodes[selectedList].Nodes.Count)
                    {
                        if (selectedList + 1 < treeView1.Nodes.Count)
                        {
                            treeView1.SelectedNode = treeView1.Nodes[selectedList + 1];
                        }
                    }
                    else
                    {
                        treeView1.SelectedNode = treeView1.Nodes[selectedList].Nodes[++selectedEntry];
                    }

                    e.SuppressKeyPress = true;
                }
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            //level 0 = new string list
            if (node == null || node.Level == 0)
            {
                int selectedEntry = node != null ? node.Index : 0;
                var newNode = new TreeNode("List " + selectedEntry.ToString("X2"));
                treeView1.Nodes.Insert(selectedEntry, newNode);
                newNode.Nodes.Add("Add new...");
                fileToRead.strings[0].Insert(selectedEntry, new List<string>());
                refreshRoots(selectedEntry);
            }
            //level 1 = new string
            if (node != null && node.Level == 1)
            {
                int insertIndex = node.Index;
                int parentIndex = node.Parent.Index;
                fileToRead.strings[0][parentIndex].Insert(insertIndex, "");
                var newNode = new TreeNode(insertIndex.ToString("X2") + " " + fileToRead.strings[0][parentIndex][insertIndex]);
                newNode.Tag = "stringNode";
                node.Parent.Nodes.Insert(insertIndex, newNode);
                refreshTree();
            }
        }

        private void refreshTree()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                var rootNode = treeView1.Nodes[i];
                rootNode.Text = "List " + i.ToString("X2");
                for (int j = 0; j < rootNode.Nodes.Count; j++)
                {
                    if (rootNode.Nodes[j].Tag != null)
                    {
                        rootNode.Nodes[j].Text = j.ToString("X2") + " " + fileToRead.strings[0][i][j];
                    }
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node == null) // can't delete "add new..." nodes
            {
                return;
            }
            //level 0 = delete string list
            if (node.Level == 0)
            {
                int deleteIndex = node.Index;
                treeView1.Nodes.RemoveAt(deleteIndex);
                fileToRead.strings[0].RemoveAt(deleteIndex);
                refreshRoots(deleteIndex);
            }
            //level 1 = delete string
            if (node.Level == 1 && node.Tag != null)
            {
                int deleteIndex = node.Index;
                int parentIndex = node.Parent.Index;
                fileToRead.strings[0][parentIndex].RemoveAt(deleteIndex);
                node.Parent.Nodes.RemoveAt(deleteIndex);
                refreshSubtree(parentIndex, deleteIndex);
            }
        }

        private void refreshRoots(int selectedEntry)
        {
            for (int i = selectedEntry; i < fileToRead.strings[0].Count; i++)
            {
                var rootNode = treeView1.Nodes[i];
                rootNode.Text = "List " + i.ToString("X2");
            }
        }

        private void refreshSubtree(int parentIndex, int deleteIndex)
        {
            List<string> currentList = fileToRead.strings[0][parentIndex];
            var rootNode = treeView1.Nodes[parentIndex];
            for (int i = deleteIndex; i < currentList.Count; i++)
            {
                rootNode.Nodes[i].Text = i.ToString("X2") + " " + fileToRead.strings[0][parentIndex][i];
            }
        }
    }
}
