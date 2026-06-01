namespace psu_generic_parser.Forms.FileViewers.Bosses
{
    partial class BossServerParamViewer
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
            this.hitboxDataGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.attackDataGrid = new System.Windows.Forms.DataGridView();
            this.hitboxSplitContainer = new System.Windows.Forms.SplitContainer();
            this.overallSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mysteryStatDataGrid = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.elementNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.unknown1UpDown = new System.Windows.Forms.NumericUpDown();
            this.unknown2UpDown = new System.Windows.Forms.NumericUpDown();
            this.baseStatPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.testSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.hitboxDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hitboxSplitContainer)).BeginInit();
            this.hitboxSplitContainer.Panel1.SuspendLayout();
            this.hitboxSplitContainer.Panel2.SuspendLayout();
            this.hitboxSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overallSplitContainer)).BeginInit();
            this.overallSplitContainer.Panel1.SuspendLayout();
            this.overallSplitContainer.Panel2.SuspendLayout();
            this.overallSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mysteryStatDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testSplitContainer)).BeginInit();
            this.testSplitContainer.Panel1.SuspendLayout();
            this.testSplitContainer.Panel2.SuspendLayout();
            this.testSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // hitboxDataGrid
            // 
            this.hitboxDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hitboxDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hitboxDataGrid.Location = new System.Drawing.Point(3, 22);
            this.hitboxDataGrid.Name = "hitboxDataGrid";
            this.hitboxDataGrid.Size = new System.Drawing.Size(319, 212);
            this.hitboxDataGrid.TabIndex = 0;
            this.hitboxDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.hitboxDataGrid_CellValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hitboxes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Attacks";
            // 
            // attackDataGrid
            // 
            this.attackDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.attackDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attackDataGrid.Location = new System.Drawing.Point(3, 16);
            this.attackDataGrid.Name = "attackDataGrid";
            this.attackDataGrid.Size = new System.Drawing.Size(320, 231);
            this.attackDataGrid.TabIndex = 3;
            this.attackDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.attackDataGrid_CellValueChanged);
            // 
            // hitboxSplitContainer
            // 
            this.hitboxSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hitboxSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.hitboxSplitContainer.Name = "hitboxSplitContainer";
            this.hitboxSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // hitboxSplitContainer.Panel1
            // 
            this.hitboxSplitContainer.Panel1.Controls.Add(this.hitboxDataGrid);
            this.hitboxSplitContainer.Panel1.Controls.Add(this.label1);
            // 
            // hitboxSplitContainer.Panel2
            // 
            this.hitboxSplitContainer.Panel2.Controls.Add(this.attackDataGrid);
            this.hitboxSplitContainer.Panel2.Controls.Add(this.label2);
            this.hitboxSplitContainer.Size = new System.Drawing.Size(326, 491);
            this.hitboxSplitContainer.SplitterDistance = 237;
            this.hitboxSplitContainer.TabIndex = 4;
            // 
            // overallSplitContainer
            // 
            this.overallSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overallSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.overallSplitContainer.Name = "overallSplitContainer";
            // 
            // overallSplitContainer.Panel1
            // 
            this.overallSplitContainer.Panel1.Controls.Add(this.testSplitContainer);
            this.overallSplitContainer.Panel1.Controls.Add(this.unknown2UpDown);
            this.overallSplitContainer.Panel1.Controls.Add(this.unknown1UpDown);
            this.overallSplitContainer.Panel1.Controls.Add(this.elementNumericUpDown);
            this.overallSplitContainer.Panel1.Controls.Add(this.label5);
            this.overallSplitContainer.Panel1.Controls.Add(this.label4);
            this.overallSplitContainer.Panel1.Controls.Add(this.label3);
            // 
            // overallSplitContainer.Panel2
            // 
            this.overallSplitContainer.Panel2.Controls.Add(this.hitboxSplitContainer);
            this.overallSplitContainer.Size = new System.Drawing.Size(544, 491);
            this.overallSplitContainer.SplitterDistance = 214;
            this.overallSplitContainer.TabIndex = 2;
            // 
            // mysteryStatDataGrid
            // 
            this.mysteryStatDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mysteryStatDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mysteryStatDataGrid.Location = new System.Drawing.Point(0, 0);
            this.mysteryStatDataGrid.Name = "mysteryStatDataGrid";
            this.mysteryStatDataGrid.Size = new System.Drawing.Size(204, 199);
            this.mysteryStatDataGrid.TabIndex = 0;
            this.mysteryStatDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Element:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Unknown Short 1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Unknown Short 2:";
            // 
            // elementNumericUpDown
            // 
            this.elementNumericUpDown.Hexadecimal = true;
            this.elementNumericUpDown.Location = new System.Drawing.Point(103, 4);
            this.elementNumericUpDown.Maximum = new decimal(new int[] {
            4000000,
            0,
            0,
            0});
            this.elementNumericUpDown.Minimum = new decimal(new int[] {
            4000000,
            0,
            0,
            -2147483648});
            this.elementNumericUpDown.Name = "elementNumericUpDown";
            this.elementNumericUpDown.Size = new System.Drawing.Size(75, 20);
            this.elementNumericUpDown.TabIndex = 4;
            this.elementNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // unknown1UpDown
            // 
            this.unknown1UpDown.Hexadecimal = true;
            this.unknown1UpDown.Location = new System.Drawing.Point(102, 30);
            this.unknown1UpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.unknown1UpDown.Name = "unknown1UpDown";
            this.unknown1UpDown.Size = new System.Drawing.Size(75, 20);
            this.unknown1UpDown.TabIndex = 5;
            this.unknown1UpDown.ValueChanged += new System.EventHandler(this.unknown1UpDown_ValueChanged);
            // 
            // unknown2UpDown
            // 
            this.unknown2UpDown.Hexadecimal = true;
            this.unknown2UpDown.Location = new System.Drawing.Point(102, 56);
            this.unknown2UpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.unknown2UpDown.Name = "unknown2UpDown";
            this.unknown2UpDown.Size = new System.Drawing.Size(75, 20);
            this.unknown2UpDown.TabIndex = 6;
            this.unknown2UpDown.ValueChanged += new System.EventHandler(this.unknown2UpDown_ValueChanged);
            // 
            // baseStatPropertyGrid
            // 
            this.baseStatPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseStatPropertyGrid.HelpVisible = false;
            this.baseStatPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.baseStatPropertyGrid.Name = "baseStatPropertyGrid";
            this.baseStatPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.baseStatPropertyGrid.Size = new System.Drawing.Size(204, 203);
            this.baseStatPropertyGrid.TabIndex = 7;
            // 
            // testSplitContainer
            // 
            this.testSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testSplitContainer.Location = new System.Drawing.Point(7, 82);
            this.testSplitContainer.Name = "testSplitContainer";
            this.testSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // testSplitContainer.Panel1
            // 
            this.testSplitContainer.Panel1.Controls.Add(this.baseStatPropertyGrid);
            // 
            // testSplitContainer.Panel2
            // 
            this.testSplitContainer.Panel2.Controls.Add(this.mysteryStatDataGrid);
            this.testSplitContainer.Size = new System.Drawing.Size(204, 406);
            this.testSplitContainer.SplitterDistance = 203;
            this.testSplitContainer.TabIndex = 8;
            // 
            // BossServerParamViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.overallSplitContainer);
            this.Name = "BossServerParamViewer";
            this.Size = new System.Drawing.Size(544, 491);
            ((System.ComponentModel.ISupportInitialize)(this.hitboxDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackDataGrid)).EndInit();
            this.hitboxSplitContainer.Panel1.ResumeLayout(false);
            this.hitboxSplitContainer.Panel1.PerformLayout();
            this.hitboxSplitContainer.Panel2.ResumeLayout(false);
            this.hitboxSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hitboxSplitContainer)).EndInit();
            this.hitboxSplitContainer.ResumeLayout(false);
            this.overallSplitContainer.Panel1.ResumeLayout(false);
            this.overallSplitContainer.Panel1.PerformLayout();
            this.overallSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.overallSplitContainer)).EndInit();
            this.overallSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mysteryStatDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.elementNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown1UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknown2UpDown)).EndInit();
            this.testSplitContainer.Panel1.ResumeLayout(false);
            this.testSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testSplitContainer)).EndInit();
            this.testSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView hitboxDataGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView attackDataGrid;
        private System.Windows.Forms.SplitContainer hitboxSplitContainer;
        private System.Windows.Forms.SplitContainer overallSplitContainer;
        private System.Windows.Forms.DataGridView mysteryStatDataGrid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown unknown2UpDown;
        private System.Windows.Forms.NumericUpDown unknown1UpDown;
        private System.Windows.Forms.NumericUpDown elementNumericUpDown;
        private System.Windows.Forms.PropertyGrid baseStatPropertyGrid;
        private System.Windows.Forms.SplitContainer testSplitContainer;
    }
}
