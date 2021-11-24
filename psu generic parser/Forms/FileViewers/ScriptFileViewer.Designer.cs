namespace psu_generic_parser
{
    partial class ScriptFileViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.deleteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.subroutineListBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.subroutineSearchBox = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.bufferLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.bufferLengthLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.LabelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpcodeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ArgColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operationsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.insertRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subroutineListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToReferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bufferLengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.operationsContextMenuStrip.SuspendLayout();
            this.subroutineListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // deleteRowMenuItem
            // 
            this.deleteRowMenuItem.Name = "deleteRowMenuItem";
            this.deleteRowMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteRowMenuItem.Text = "Delete Row";
            this.deleteRowMenuItem.Click += new System.EventHandler(this.deleteRowMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(54, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type: ";
            // 
            // subroutineListBox
            // 
            this.subroutineListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subroutineListBox.ContextMenuStrip = this.subroutineListContextMenuStrip;
            this.subroutineListBox.FormattingEnabled = true;
            this.subroutineListBox.Location = new System.Drawing.Point(0, 26);
            this.subroutineListBox.Name = "subroutineListBox";
            this.subroutineListBox.Size = new System.Drawing.Size(187, 355);
            this.subroutineListBox.TabIndex = 8;
            this.subroutineListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.subroutineListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.subroutineListBox_MouseDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.subroutineSearchBox);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.subroutineListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bufferLengthUpDown);
            this.splitContainer1.Panel2.Controls.Add(this.bufferLengthLabel);
            this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(562, 444);
            this.splitContainer1.SplitterDistance = 187;
            this.splitContainer1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Find:";
            // 
            // subroutineSearchBox
            // 
            this.subroutineSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subroutineSearchBox.Location = new System.Drawing.Point(40, 3);
            this.subroutineSearchBox.Name = "subroutineSearchBox";
            this.subroutineSearchBox.Size = new System.Drawing.Size(141, 20);
            this.subroutineSearchBox.TabIndex = 15;
            this.subroutineSearchBox.TextChanged += new System.EventHandler(this.subroutineSearch_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 382);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button3);
            this.splitContainer2.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button4);
            this.splitContainer2.Size = new System.Drawing.Size(184, 59);
            this.splitContainer2.SplitterDistance = 87;
            this.splitContainer2.TabIndex = 12;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(3, 33);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Export";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(3, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Add Sub";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(3, 33);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Import";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // bufferLengthUpDown
            // 
            this.bufferLengthUpDown.Location = new System.Drawing.Point(222, 32);
            this.bufferLengthUpDown.Name = "bufferLengthUpDown";
            this.bufferLengthUpDown.Size = new System.Drawing.Size(120, 20);
            this.bufferLengthUpDown.TabIndex = 14;
            this.bufferLengthUpDown.Visible = false;
            this.bufferLengthUpDown.ValueChanged += new System.EventHandler(this.bufferLengthUpDown_ValueChanged);
            // 
            // bufferLengthLabel
            // 
            this.bufferLengthLabel.AutoSize = true;
            this.bufferLengthLabel.Location = new System.Drawing.Point(141, 34);
            this.bufferLengthLabel.Name = "bufferLengthLabel";
            this.bufferLengthLabel.Size = new System.Drawing.Size(74, 13);
            this.bufferLengthLabel.TabIndex = 13;
            this.bufferLengthLabel.Text = "Buffer Length:";
            this.bufferLengthLabel.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Numeric Variable",
            "String Variable",
            "Function"});
            this.comboBox1.Location = new System.Drawing.Point(221, 3);
            this.comboBox1.MaxDropDownItems = 2;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Delete subroutine";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LabelColumn,
            this.OpcodeColumn,
            this.ArgColumn});
            this.dataGridView2.ContextMenuStrip = this.operationsContextMenuStrip;
            this.dataGridView2.Location = new System.Drawing.Point(2, 73);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(366, 365);
            this.dataGridView2.TabIndex = 10;
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView2_DataBindingComplete);
            this.dataGridView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDown);
            // 
            // LabelColumn
            // 
            this.LabelColumn.HeaderText = "Label";
            this.LabelColumn.Name = "LabelColumn";
            // 
            // OpcodeColumn
            // 
            this.OpcodeColumn.HeaderText = "Opcode";
            this.OpcodeColumn.Name = "OpcodeColumn";
            this.OpcodeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OpcodeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ArgColumn
            // 
            this.ArgColumn.HeaderText = "Argument";
            this.ArgColumn.Name = "ArgColumn";
            // 
            // operationsContextMenuStrip
            // 
            this.operationsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertRowMenuItem,
            this.deleteRowMenuItem,
            this.goToReferenceToolStripMenuItem});
            this.operationsContextMenuStrip.Name = "contextMenuStrip1";
            this.operationsContextMenuStrip.Size = new System.Drawing.Size(139, 70);
            this.operationsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.operationsContextMenuStrip_Opening);
            // 
            // insertRowMenuItem
            // 
            this.insertRowMenuItem.Name = "insertRowMenuItem";
            this.insertRowMenuItem.Size = new System.Drawing.Size(180, 22);
            this.insertRowMenuItem.Text = "Insert Row";
            this.insertRowMenuItem.Click += new System.EventHandler(this.insertRowMenuItem_Click);
            // 
            // subroutineListContextMenuStrip
            // 
            this.subroutineListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findReferencesToolStripMenuItem});
            this.subroutineListContextMenuStrip.Name = "subroutineListContextMenuStrip";
            this.subroutineListContextMenuStrip.Size = new System.Drawing.Size(158, 26);
            // 
            // goToReferenceToolStripMenuItem
            // 
            this.goToReferenceToolStripMenuItem.Name = "goToReferenceToolStripMenuItem";
            this.goToReferenceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.goToReferenceToolStripMenuItem.Text = "Go to Target";
            this.goToReferenceToolStripMenuItem.Click += new System.EventHandler(this.goToReferenceToolStripMenuItem_Click);
            // 
            // findReferencesToolStripMenuItem
            // 
            this.findReferencesToolStripMenuItem.Name = "findReferencesToolStripMenuItem";
            this.findReferencesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.findReferencesToolStripMenuItem.Text = "Find References";
            this.findReferencesToolStripMenuItem.Click += new System.EventHandler(this.findReferencesToolStripMenuItem_Click);
            // 
            // ScriptFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScriptFileViewer";
            this.Size = new System.Drawing.Size(562, 444);
            this.ParentChanged += new System.EventHandler(this.ScriptFileViewer_ParentChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bufferLengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.operationsContextMenuStrip.ResumeLayout(false);
            this.subroutineListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox subroutineListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ContextMenuStrip operationsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem insertRowMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabelColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn OpcodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgColumn;
        private System.Windows.Forms.NumericUpDown bufferLengthUpDown;
        private System.Windows.Forms.Label bufferLengthLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox subroutineSearchBox;
        private System.Windows.Forms.ToolStripMenuItem goToReferenceToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip subroutineListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findReferencesToolStripMenuItem;
    }
}
