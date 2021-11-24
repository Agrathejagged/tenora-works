using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using psu_generic_parser.Forms.FileViewers.Scripts;
using PSULib.FileClasses.General;

namespace psu_generic_parser
{
    public partial class ScriptFileViewer : UserControl
    {
        readonly ScriptFile internalFile;
        bool changing = true;
        int prevIndex = -1;
        bool forceChange = false;
        BindingSource binder = new BindingSource();
        private List<ReferenceFindDialog> children = new List<ReferenceFindDialog>();

        public ScriptFileViewer(ScriptFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            for (int i = 0; i < internalFile.Subroutines.Count; i++)
            {
                subroutineListBox.Items.Add((internalFile.Subroutines[i]).SubroutineName);
            }
            List<string> opcodeNames = new List<string>();
            foreach (string opcode in ScriptFile.opcodes)
            {
                if (opcode != "")
                {
                    opcodeNames.Add(opcode);
                }
            }
            OpcodeColumn.Items.AddRange(opcodeNames.ToArray());
            LabelColumn.DataPropertyName = "Label";
            ArgColumn.DataPropertyName = "OperandText";
            OpcodeColumn.DataPropertyName = "OpCodeName";
            if (subroutineListBox.Items.Count > 0)
                subroutineListBox.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subroutineListBox.SelectedIndex != -1 && (subroutineListBox.SelectedIndex != prevIndex || forceChange))
            {
                forceChange = false;
                prevIndex = subroutineListBox.SelectedIndex;
                changing = true;
                ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
                textBox1.Text = currentSub.SubroutineName;
                bufferLengthUpDown.Value = currentSub.BufferLength;
                UpdateUIElements();
                dataGridView2.AutoGenerateColumns = false;
                binder.DataSource = currentSub.Operations;
                dataGridView2.DataSource = binder;
                changing = false;
            }
        }

        private void UpdateUIElements()
        {
            ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
            textBox1.Text = currentSub.SubroutineName;
            if (currentSub.SubType == 0x4C)
            {
                comboBox1.SelectedIndex = 2;
                bufferLengthLabel.Visible = false;
                bufferLengthUpDown.Visible = false;
                dataGridView2.Enabled = true;
            }
            else if(currentSub.SubType == 0x49)
            {
                comboBox1.SelectedIndex = 1;
                bufferLengthLabel.Visible = true;
                bufferLengthUpDown.Visible = true;
                dataGridView2.Enabled = false;
            }
            else if(currentSub.SubType == 0x3C)
            {
                comboBox1.SelectedIndex = 0;
                bufferLengthLabel.Visible = false;
                bufferLengthUpDown.Visible = false;
                dataGridView2.Enabled = false;
            }
            else
            {
                throw new Exception("Received unexpected subroutine type: " + currentSub.SubType.ToString("X2"));
            }
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
                internalFile.Subroutines[subroutineListBox.SelectedIndex].SubroutineName = textBox1.Text;
                subroutineListBox.Items[subroutineListBox.SelectedIndex] = textBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete " + internalFile.Subroutines[subroutineListBox.SelectedIndex].SubroutineName + "?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int oldIndex = subroutineListBox.SelectedIndex;
                subroutineListBox.Items.RemoveAt(oldIndex);
                internalFile.Subroutines.RemoveAt(oldIndex);
                forceChange = true;
                subroutineListBox.SelectedIndex = Math.Min(oldIndex, subroutineListBox.Items.Count -1);
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
            comboBox.Items.AddRange(subroutineListBox.Items.Cast<string>().ToArray());
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
                ScriptFile.Subroutine sub = new ScriptFile.Subroutine();
                sub.SubType = 0x4C;
                internalFile.Subroutines.Insert(comboBox.SelectedIndex, sub);
                subroutineListBox.Items.Insert(comboBox.SelectedIndex, internalFile.Subroutines[comboBox.SelectedIndex].SubroutineName);
                forceChange = true;
                subroutineListBox.SelectedIndex = comboBox.SelectedIndex;
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
            checkList.Items.AddRange(subroutineListBox.Items.Cast<string>().ToArray());
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

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempDia = new OpenFileDialog();
            if (tempDia.ShowDialog() == DialogResult.OK)
            {
                byte[] impScript = System.IO.File.ReadAllBytes(tempDia.FileName);
                ScriptFile tempScript = new ScriptFile("dummy.bin", impScript);
                internalFile.importScript(tempScript);
                foreach (ScriptFile.Subroutine sub in tempScript.Subroutines)
                {
                    if (!subroutineListBox.Items.Contains(sub.SubroutineName))
                        subroutineListBox.Items.Add(sub.SubroutineName);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                changing = true;
                switch(comboBox1.SelectedIndex)
                {
                    //Numeric variable
                    case 0:
                        internalFile.Subroutines[subroutineListBox.SelectedIndex].SubType = 0x3C;
                        break;
                    //String variable
                    case 1:
                        internalFile.Subroutines[subroutineListBox.SelectedIndex].SubType = 0x49;
                        break;
                    //Subroutine
                    case 2:
                        internalFile.Subroutines[subroutineListBox.SelectedIndex].SubType = 0x4C;
                        break;
                }
                UpdateUIElements();
                changing = false;
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView2.HitTest(e.X, e.Y);
                if (hti.RowIndex > -1 && hti.RowIndex < dataGridView2.Rows.Count)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[hti.RowIndex].Selected = true;
                }
            }
        }

        private void insertRowMenuItem_Click(object sender, EventArgs e)
        {
            ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
            int rowToInsert = dataGridView2.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            dataGridView2.ClearSelection();
            ScriptFile.Operation newOp = new ScriptFile.Operation();
            newOp.OpCode = 1;
            currentSub.Operations.Insert(rowToInsert, newOp);
            binder.ResetBindings(false);
            dataGridView2.Refresh();
        }

        private void deleteRowMenuItem_Click(object sender, EventArgs e)
        {
            int rowToDelete = dataGridView2.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
            dataGridView2.ClearSelection();
            currentSub.Operations.RemoveAt(rowToDelete);
            binder.ResetBindings(false);
            dataGridView2.Refresh();
        }

        private void bufferLengthUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(!changing)
            {
                ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
                currentSub.BufferLength = (int)bufferLengthUpDown.Value;
            }
        }

        private void subroutineSearch_TextChanged(object sender, EventArgs e)
        {
            int result = subroutineListBox.FindString(subroutineSearchBox.Text);
            if(result != -1)
            {
                subroutineListBox.SelectedIndex = result;
                subroutineSearchBox.ForeColor = SystemColors.WindowText;
            }
            else
            {
                subroutineSearchBox.ForeColor = Color.Red;
            }
        }

        private void goToReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowToRead = dataGridView2.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
            ScriptFile.Operation currentOp = currentSub.Operations[rowToRead];
            if (currentOp.OpCodeType == ScriptFile.OpCodeOperandTypes.BranchTarget)
            {
                int selectedIndex = currentSub.Operations.FindIndex(op => op.Label == currentOp.OperandText);
                if (selectedIndex != -1)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.CurrentCell = dataGridView2.Rows[selectedIndex].Cells[0];
                }
                else
                {
                    MessageBox.Show("Could not find label: " + currentOp.OperandText);
                }
            }
            else if(currentOp.OpCodeType == ScriptFile.OpCodeOperandTypes.FunctionName || currentOp.OpCodeType == ScriptFile.OpCodeOperandTypes.NumericVariableName || currentOp.OpCodeType == ScriptFile.OpCodeOperandTypes.StringVariableName)
            {
                int selectedIndex = internalFile.Subroutines.FindIndex(op => op.SubroutineName == currentOp.OperandText);
                if (selectedIndex != -1)
                {
                    subroutineListBox.SelectedIndex = selectedIndex;
                }
                else
                {
                    MessageBox.Show("Could not find function/variable: " + currentOp.OperandText);
                }
            }
        }

        private void operationsContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                e.Cancel = true;
            }
            else
            {
                int rowToRead = dataGridView2.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                ScriptFile.Subroutine currentSub = internalFile.Subroutines[subroutineListBox.SelectedIndex];
                if (rowToRead < currentSub.Operations.Count)
                {
                    ScriptFile.Operation currentOp = currentSub.Operations[rowToRead];
                    goToReferenceToolStripMenuItem.Visible = hasDestination(currentOp.OpCodeType);
                    deleteRowMenuItem.Enabled = true;
                }
                else
                {
                    deleteRowMenuItem.Enabled = false;
                    goToReferenceToolStripMenuItem.Visible = false;
                }
            }
        }

        private bool hasDestination(ScriptFile.OpCodeOperandTypes testType)
        {
            switch (testType)
            {
                case ScriptFile.OpCodeOperandTypes.FunctionName: case ScriptFile.OpCodeOperandTypes.BranchTarget: case ScriptFile.OpCodeOperandTypes.NumericVariableName: case ScriptFile.OpCodeOperandTypes.StringVariableName: return true;
            }
            return false;
        }

        private bool hasSubroutineDestination(ScriptFile.OpCodeOperandTypes testType)
        {
            return testType == ScriptFile.OpCodeOperandTypes.FunctionName || testType == ScriptFile.OpCodeOperandTypes.NumericVariableName || testType == ScriptFile.OpCodeOperandTypes.StringVariableName;
        }

        public void SelectOperation(string subroutineName, int lineNumber)
        {
            int functionIndex = internalFile.Subroutines.FindIndex(sub => sub.SubroutineName == subroutineName);
            if(functionIndex != -1)
            {
                subroutineListBox.SelectedIndex = functionIndex;
                if(internalFile.Subroutines[functionIndex].Operations.Count > lineNumber)
                {
                    dataGridView2.CurrentCell = dataGridView2.Rows[lineNumber].Cells[0];
                }
            }
        }

        private void findReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string subroutineName = (string)subroutineListBox.SelectedItem;
            var findResults = new List<Tuple<string, int>>();
            foreach(var subroutine in internalFile.Subroutines)
            {
                for(int i = 0; i < subroutine.Operations.Count; i++)
                {
                    if(hasSubroutineDestination(subroutine.Operations[i].OpCodeType) && subroutine.Operations[i].OperandText == subroutineName)
                    {
                        findResults.Add(new Tuple<string, int>(subroutine.SubroutineName, i));
                    }
                }
            }

            if(findResults.Count > 0)
            {
                ReferenceFindDialog dlg = new ReferenceFindDialog(this, subroutineName, findResults);
                children.Add(dlg);
                dlg.Show();
            }
            else
            {
                MessageBox.Show("No references found to subroutine/variable " + subroutineName, "No Results Found");
            }
        }

        private void subroutineListBox_MouseDown(object sender, MouseEventArgs e)
        {
            subroutineListBox.SelectedIndex = subroutineListBox.IndexFromPoint(e.Location);
            /*
            if (e.Button == MouseButtons.Right)
            {
                var hti = subroutineListBox..HitTest(e.X, e.Y);
                if (hti.RowIndex > -1 && hti.RowIndex < dataGridView2.Rows.Count)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[hti.RowIndex].Selected = true;
                }
            }*/
        }

        private void ScriptFileViewer_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent == null)
            {
                foreach (var child in children)
                {
                    child.Hide();
                }
            }
        }
    }
}
