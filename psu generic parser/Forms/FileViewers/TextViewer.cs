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
        TextFile fileToRead;

        public TextViewer(TextFile toSet)
        {
            InitializeComponent();
            fileToRead = toSet;
            List<string>[][] internalStrings = fileToRead.stringArray;

            //We assume based on observation that there are NOT ever branchings on the trees despite the setup for them. Therefore, read + display as list
            textLB.BeginUpdate();
            for(int i = 0; i < fileToRead.stringArray[0][0].Count; i++)
            {
                //We'll access the array directly when pulling to the textbox so these are mainly reference.
                textLB.Items.Add(i.ToString("X2") + " " + fileToRead.stringArray[0][0][i]);
            }
            textLB.EndUpdate();
            textLB.SelectedIndex = 0;
            textBox1.Text = fileToRead.stringArray[0][0][textLB.SelectedIndex];
        }

        //Save the textbox to the appropriate text entry in the file
        private void storeButton_Click(object sender, EventArgs e)
        {
            fileToRead.stringArray[0][0][textLB.SelectedIndex] = textBox1.Text;
            textLB.Items[textLB.SelectedIndex] = textLB.SelectedIndex.ToString("X2") + " " + textBox1.Text;
        }

        //Insert and increment selection by 1
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode & (Keys.Enter)) == Keys.Enter && e.Control == true)
            {
                InsertTextEntry();
                textLB.SelectedIndex++;
                e.SuppressKeyPress = true;
            }
        }

        private void textLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(textLB.SelectedIndex == -1)
            {
                textLB.SelectedIndex = 0;
            }
            storeButton.Enabled = true;
            textBox1.Enabled = true;
            textBox1.Text = fileToRead.stringArray[0][0][textLB.SelectedIndex];
        }

        //Add entry to the end
        private void addButton_Click(object sender, EventArgs e)
        {
            fileToRead.stringArray[0][0].Add("");
            textBox1.Text = "";
            textLB.BeginUpdate();
            textLB.Items.Add((textLB.Items.Count).ToString("X2") + " ");
            textLB.EndUpdate();
            textLB.SelectedIndex = textLB.Items.Count - 1;
        }

        //Remove entry from selected index
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (fileToRead.stringArray[0][0].Count > 1)
            {
                fileToRead.stringArray[0][0].RemoveAt(textLB.SelectedIndex);
                int temp = textLB.SelectedIndex;
                textLB.BeginUpdate();
                if (textLB.SelectedIndex > 0)
                {
                    textLB.SelectedIndex = textLB.SelectedIndex - 1;
                }
                else
                {
                    textLB.SelectedIndex = 0;
                }
                textBox1.Text = fileToRead.stringArray[0][0][textLB.SelectedIndex];
                textLB.Items.RemoveAt(temp);
                for (int i = 0; i < textLB.Items.Count; i++)
                {
                    textLB.Items[i] = i.ToString("X2") + " " + fileToRead.stringArray[0][0][i];
                }
                textLB.EndUpdate();
            }
            else
            {
                MessageBox.Show("You cannot remove the last text string!");
            }
        }

        //Insert entry and select new entry
        private void insertButton_Click(object sender, EventArgs e)
        {
            InsertTextEntry();
        }

        private void InsertTextEntry()
        {
            fileToRead.stringArray[0][0].Insert(textLB.SelectedIndex, "");
            textBox1.Text = "";
            textLB.BeginUpdate();
            textLB.Items.Insert(textLB.SelectedIndex, textLB.SelectedIndex.ToString("X2") + " ");
            for (int i = textLB.SelectedIndex; i < textLB.Items.Count; i++)
            {
                textLB.Items[i] = i.ToString("X2") + " " + fileToRead.stringArray[0][0][i];
            }
            textLB.EndUpdate();
            if (textLB.SelectedIndex > 0)
            {
                textLB.SelectedIndex = textLB.SelectedIndex - 1;
            }
            else
            {
                textLB.SelectedIndex = 0;
            }
        }
    }
}
