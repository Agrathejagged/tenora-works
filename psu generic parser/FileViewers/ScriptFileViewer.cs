using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class ScriptFileViewer : UserControl
    {
        ScriptFile internalFile;
        bool changing = true;
        int prevIndex = -1;
        bool forceChange = false;

        public ScriptFileViewer(ScriptFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            dataGridView1.Columns[1].ValueType = typeof(int);
            for (int i = 0; i < internalFile.subroutines.Count; i++)
            {
                listBox1.Items.Add(((ScriptFile.subroutine)internalFile.subroutines[i]).name);
            }
            if(listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && (listBox1.SelectedIndex != prevIndex || forceChange))
            {
                forceChange = false;
                prevIndex = listBox1.SelectedIndex;
                ScriptFile.subroutine currentSub = (ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex];
                changing = true;
                textBox1.Text = currentSub.name;
                if (currentSub.subType == 0x4C)
                {
                    comboBox1.SelectedIndex = 1;
                    dataGridView2.Enabled = true;
                }
                else
                {
                    comboBox1.SelectedIndex = 0;
                    dataGridView2.Enabled = false;
                }
                dataGridView2.AutoGenerateColumns = false;
                BindingSource binder = new BindingSource();
                binder.DataSource = currentSub.opcodes;
                dataGridView2.DataSource = binder;
                IntColumn.DataPropertyName = "intArg";
                StringColumn.DataPropertyName = "strArg";
                FloatColumn.DataPropertyName = "floatArg";
                OpcodeColumn.DataPropertyName = "opcodeName";
                /*
                 * dataGridView1.Rows.Clear();
            
                if (currentSub.opcodes.Count > 0)
                {
                    dataGridView1.Rows.Add(currentSub.opcodes.Count);
                    for (int i = 0; i < currentSub.opcodes.Count; i++)
                    {
                        dataGridView1[0, i].Value = internalFile.opcodes[((ScriptFile.operation)currentSub.opcodes[i]).opcode];
                        if (internalFile.opcodeTypes[((ScriptFile.operation)currentSub.opcodes[i]).opcode] == 1) // 1 = no args
                        {
                            dataGridView1[1, i].ReadOnly = true;
                            dataGridView1[2, i].ReadOnly = true;
                            dataGridView1[3, i].ReadOnly = true;
                        }
                        else if (internalFile.opcodeTypes[((ScriptFile.operation)currentSub.opcodes[i]).opcode] == 2) // 2 = int
                        {
                            dataGridView1[1, i].ReadOnly = true;
                            dataGridView1[2, i].ReadOnly = false;
                            dataGridView1[3, i].ReadOnly = true;
                            dataGridView1[2, i].Value = ((ScriptFile.operation)currentSub.opcodes[i]).intArg;
                        }
                        else if (internalFile.opcodeTypes[((ScriptFile.operation)currentSub.opcodes[i]).opcode] == 3) // 3 = float
                        {
                            dataGridView1[1, i].ReadOnly = false;
                            dataGridView1[2, i].ReadOnly = true;
                            dataGridView1[3, i].ReadOnly = true;
                            dataGridView1[1, i].Value = ((ScriptFile.operation)currentSub.opcodes[i]).floatArg;
                        }
                        else // 4/99 = string
                        {
                            dataGridView1[1, i].ReadOnly = true;
                            dataGridView1[2, i].ReadOnly = true;
                            dataGridView1[3, i].ReadOnly = false;
                            dataGridView1[3, i].Value = ((ScriptFile.operation)currentSub.opcodes[i]).strArg;
                        }
                    }
                }
                 */
                changing = false;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (!changing)
            {
                if (e.RowIndex >= ((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes.Count)
                {
                    ScriptFile.operation temp = new ScriptFile.operation();
                    temp.opcode = 1;
                    ((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes.Add(temp);
                    changing = true;
                    if (e.ColumnIndex != 0)
                    {
                        dataGridView1[0, e.RowIndex].Value = "NOP";
                        dataGridView1[1, e.RowIndex].ReadOnly = true;
                        dataGridView1[2, e.RowIndex].ReadOnly = true;
                        dataGridView1[3, e.RowIndex].ReadOnly = true;
                    }
                    changing = false;
                }
                if (e.ColumnIndex == 0)
                {
                    int newOpcode = Array.IndexOf(internalFile.opcodes, dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                    if(internalFile.opcodeTypes[newOpcode] == 2)
                    {
                        dataGridView1[1, e.RowIndex].ReadOnly = true;
                        dataGridView1[2, e.RowIndex].ReadOnly = false;
                        dataGridView1[3, e.RowIndex].ReadOnly = true;
                    }
                    else if (internalFile.opcodeTypes[newOpcode] == 3)
                    {
                        dataGridView1[1, e.RowIndex].ReadOnly = false;
                        dataGridView1[2, e.RowIndex].ReadOnly = true;
                        dataGridView1[3, e.RowIndex].ReadOnly = true;
                    }
                    else if (internalFile.opcodeTypes[newOpcode] == 1)
                    {
                        dataGridView1[1, e.RowIndex].ReadOnly = true;
                        dataGridView1[2, e.RowIndex].ReadOnly = true;
                        dataGridView1[3, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dataGridView1[1, e.RowIndex].ReadOnly = true;
                        dataGridView1[2, e.RowIndex].ReadOnly = true;
                        dataGridView1[3, e.RowIndex].ReadOnly = false;
                    }
                    ((ScriptFile.operation)((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes[e.RowIndex]).opcode = 
                        newOpcode;
                }
                else if (e.ColumnIndex == 1)
                {
                    ((ScriptFile.operation)((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes[e.RowIndex]).floatArg =
                        Convert.ToSingle((string)dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                }
                else if (e.ColumnIndex == 2)
                {
                    ((ScriptFile.operation)((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes[e.RowIndex]).intArg =
                        Convert.ToInt32((string)dataGridView1[e.ColumnIndex, e.RowIndex].Value);
                }
                else if (e.ColumnIndex == 3)
                {
                    ((ScriptFile.operation)((ScriptFile.subroutine)internalFile.subroutines[listBox1.SelectedIndex]).opcodes[e.RowIndex]).strArg =
                        (string)dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                }
            }
             */
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;
            foreach (DataGridViewRow r in gridView.Rows)
                gridView.Rows[r.Index].HeaderCell.Value = (r.Index).ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                internalFile.subroutines[listBox1.SelectedIndex].name = textBox1.Text;
                listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete " + internalFile.subroutines[listBox1.SelectedIndex].name + "?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int oldIndex = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(oldIndex);
                internalFile.subroutines.RemoveAt(oldIndex);
                forceChange = true;
                listBox1.SelectedIndex = Math.Min(oldIndex, listBox1.Items.Count -1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool accept = false;
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;
            prompt.ShowInTaskbar = false;
            prompt.Text = "Select location";
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Select where to insert subroutine." };
            textLabel.AutoSize = true;
            ComboBox comboBox = new ComboBox() { Left = 50, Top = 50, Width = 400 };
            comboBox.Items.AddRange(listBox1.Items.Cast<string>().ToArray());
            comboBox.Items.Add("End of file");
            comboBox.SelectedIndex = 0;
            
            NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "OK", Left = 125, Width = 100, Top = 85 };
            confirmation.Click += (a, b) => { prompt.Close(); accept = true; };
            Button cancel = new Button() { Text = "Cancel", Left = 250, Width = 100, Top = 85 };
            cancel.Click += (a, b) => { prompt.Close();};
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(comboBox);
            prompt.Controls.Add(cancel);
            prompt.ShowDialog();
            if (accept && comboBox.SelectedIndex != -1)
            {
                ScriptFile.subroutine sub = new ScriptFile.subroutine();
                sub.subType = 0x4C;
                internalFile.subroutines.Insert(comboBox.SelectedIndex, sub);
                listBox1.Items.Insert(comboBox.SelectedIndex, internalFile.subroutines[comboBox.SelectedIndex].name);
                forceChange = true;
                listBox1.SelectedIndex = comboBox.SelectedIndex;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool accept = false;
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 350;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;
            prompt.ShowInTaskbar = false;
            prompt.Text = "Select subroutines";
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Select subroutines to export." };
            textLabel.AutoSize = true;
            CheckedListBox checkList = new CheckedListBox() { Left = 50, Top = 50, Width = 400, Height = 200 };
            checkList.Items.AddRange(listBox1.Items.Cast<string>().ToArray());
            Button confirmation = new Button() { Text = "OK", Left = 125, Width = 100, Top = 260 };
            confirmation.Click += (a, b) => { prompt.Close(); accept = true; };
            Button cancel = new Button() { Text = "Cancel", Left = 250, Width = 100, Top = 260 };
            cancel.Click += (a, b) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(checkList);
            prompt.Controls.Add(cancel);
            prompt.ShowDialog();
            if (accept && checkList.CheckedItems.Count > 0)
            {
                string[] selectedFuncs = new string[checkList.CheckedItems.Count];
                for (int i = 0; i < selectedFuncs.Length; i++)
                {
                    selectedFuncs[i] = (string)checkList.CheckedItems[i];

                }
                SaveFileDialog tempDialog = new SaveFileDialog();
                if (tempDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] scriptOut = internalFile.dumpSubroutine(selectedFuncs);
                    System.IO.File.WriteAllBytes(tempDialog.FileName, scriptOut);
                }
            }
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempDia = new OpenFileDialog();
            if (tempDia.ShowDialog() == DialogResult.OK)
            {
                byte[] impScript = System.IO.File.ReadAllBytes(tempDia.FileName);
                ScriptFile tempScript = new ScriptFile("dummy.bin", impScript);
                internalFile.importScript(tempScript);
                foreach (ScriptFile.subroutine sub in tempScript.subroutines)
                {
                    if (!listBox1.Items.Contains(sub.name))
                        listBox1.Items.Add(sub.name);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    internalFile.subroutines[listBox1.SelectedIndex].subType = 0x3C;
                    dataGridView2.Enabled = false;
                }
                else
                {
                    internalFile.subroutines[listBox1.SelectedIndex].subType = 0x4C;
                    dataGridView2.Enabled = true;
                }
            }
        }
    }
}
