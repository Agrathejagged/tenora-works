using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class TextViewer : UserControl
    {
        int selectedTop = -1;
        int selectedSecond = -1;
        int selectedThird = -1;
        TextFile fileToRead;

        public TextViewer(TextFile toSet)
        {
            InitializeComponent();
            fileToRead = toSet;
            List<string>[][] internalStrings = fileToRead.stringArray;
            for (int i = 0; i < fileToRead.stringArray.Length; i++)
            {
                TreeNode currLevel1 = new TreeNode("Top level " + i);
                for (int j = 0; j < fileToRead.stringArray[i].Length; j++)
                {
                    TreeNode currLevel2 = new TreeNode("Second level " + j.ToString("X2"));
                    for (int k = 0; k < fileToRead.stringArray[i][j].Count; k++)
                    {
                        TreeNode currLevel3 = new TreeNode(k.ToString("X2") + " " + fileToRead.stringArray[i][j][k]);
                        currLevel2.Nodes.Add(currLevel3);
                    }
                    currLevel2.Nodes.Add("Add new...");
                    currLevel2.Expand();
                    currLevel1.Nodes.Add(currLevel2);
                }
                currLevel1.Expand();
                treeView1.Nodes.Add(currLevel1);
            }
        }

        private void storeButton_Click(object sender, EventArgs e)
        {
            if (selectedTop != -1 && selectedSecond != -1 && selectedThird != -1)
            {
                if (treeView1.SelectedNode.Text == "Add new...")
                {
                    fileToRead.stringArray[selectedTop][selectedSecond].Add("");
                    treeView1.SelectedNode.Parent.Nodes.Add("Add new...");
                }
                fileToRead.stringArray[selectedTop][selectedSecond][selectedThird] = textBox1.Text;
                treeView1.Nodes[selectedTop].Nodes[selectedSecond].Nodes[selectedThird].Text = selectedThird.ToString("X2") + " " + textBox1.Text;
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 2)
            {
                selectedThird = e.Node.Index;
                selectedSecond = e.Node.Parent.Index;
                selectedTop = e.Node.Parent.Parent.Index;
                storeButton.Enabled = true;
                textBox1.Enabled = true;
                if (e.Node.Text != "Add new...")
                    textBox1.Text = fileToRead.stringArray[selectedTop][selectedSecond][selectedThird];
                else
                    textBox1.Clear();
            }
            else
            {
                textBox1.Clear();
                textBox1.Enabled = false;
                storeButton.Enabled = false;
                selectedThird = -1;
                selectedSecond = -1;
                selectedTop = -1;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedTop != -1 && selectedSecond != -1 && selectedThird != -1)
            {
                if ((e.KeyCode & (Keys.Enter)) == Keys.Enter && e.Control == true)
                {
                    if (treeView1.SelectedNode.Text == "Add new...")
                    {
                        fileToRead.stringArray[selectedTop][selectedSecond].Add("");
                        treeView1.SelectedNode.Parent.Nodes.Add("Add new...");
                    }
                    fileToRead.stringArray[selectedTop][selectedSecond][selectedThird] = textBox1.Text;
                    treeView1.Nodes[selectedTop].Nodes[selectedSecond].Nodes[selectedThird].Text = selectedThird.ToString("X2") + " " + textBox1.Text;
                    if (selectedThird + 1 >= treeView1.Nodes[selectedTop].Nodes[selectedSecond].Nodes.Count)
                    {
                        if (selectedSecond + 1 < treeView1.Nodes[selectedTop].Nodes.Count)
                            treeView1.SelectedNode = treeView1.Nodes[selectedTop].Nodes[selectedSecond + 1];
                    }
                    else
                        treeView1.SelectedNode = treeView1.Nodes[selectedTop].Nodes[selectedSecond].Nodes[++selectedThird];
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
