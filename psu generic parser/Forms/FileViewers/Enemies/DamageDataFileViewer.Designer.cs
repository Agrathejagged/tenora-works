
namespace psu_generic_parser.Forms.FileViewers.Enemies
{
    partial class DamageDataFileViewer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.damageDataEntryListBox = new System.Windows.Forms.ListBox();
            this.topLevelListComboBox = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.eventTypeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.angleTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.addAngleButton = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventTypeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.damageDataEntryListBox);
            this.splitContainer1.Panel1.Controls.Add(this.topLevelListComboBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(552, 419);
            this.splitContainer1.SplitterDistance = 184;
            this.splitContainer1.TabIndex = 0;
            // 
            // damageDataEntryListBox
            // 
            this.damageDataEntryListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.damageDataEntryListBox.FormattingEnabled = true;
            this.damageDataEntryListBox.Location = new System.Drawing.Point(4, 32);
            this.damageDataEntryListBox.Name = "damageDataEntryListBox";
            this.damageDataEntryListBox.Size = new System.Drawing.Size(177, 381);
            this.damageDataEntryListBox.TabIndex = 1;
            this.damageDataEntryListBox.SelectedIndexChanged += new System.EventHandler(this.damageDataEntryListBox_SelectedIndexChanged);
            // 
            // topLevelListComboBox
            // 
            this.topLevelListComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topLevelListComboBox.FormattingEnabled = true;
            this.topLevelListComboBox.Location = new System.Drawing.Point(4, 4);
            this.topLevelListComboBox.Name = "topLevelListComboBox";
            this.topLevelListComboBox.Size = new System.Drawing.Size(177, 21);
            this.topLevelListComboBox.TabIndex = 0;
            this.topLevelListComboBox.SelectedIndexChanged += new System.EventHandler(this.topLevelListComboBox_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.eventTypeNumericUpDown);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(364, 419);
            this.splitContainer2.SplitterDistance = 37;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 378);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Angle Data:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Event Type:";
            // 
            // eventTypeNumericUpDown
            // 
            this.eventTypeNumericUpDown.Location = new System.Drawing.Point(77, 9);
            this.eventTypeNumericUpDown.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.eventTypeNumericUpDown.Name = "eventTypeNumericUpDown";
            this.eventTypeNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.eventTypeNumericUpDown.TabIndex = 1;
            // 
            // angleTablePanel
            // 
            this.angleTablePanel.AutoScroll = true;
            this.angleTablePanel.AutoSize = true;
            this.angleTablePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.angleTablePanel.ColumnCount = 1;
            this.angleTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.angleTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.angleTablePanel.Location = new System.Drawing.Point(0, 0);
            this.angleTablePanel.Name = "angleTablePanel";
            this.angleTablePanel.RowCount = 1;
            this.angleTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.angleTablePanel.Size = new System.Drawing.Size(358, 320);
            this.angleTablePanel.TabIndex = 0;
            // 
            // addAngleButton
            // 
            this.addAngleButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addAngleButton.Location = new System.Drawing.Point(0, 0);
            this.addAngleButton.Name = "addAngleButton";
            this.addAngleButton.Size = new System.Drawing.Size(358, 35);
            this.addAngleButton.TabIndex = 0;
            this.addAngleButton.Text = "Add New Entry";
            this.addAngleButton.UseVisualStyleBackColor = true;
            this.addAngleButton.Click += new System.EventHandler(this.addAngleButton_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(3, 16);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.angleTablePanel);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.addAngleButton);
            this.splitContainer3.Size = new System.Drawing.Size(358, 359);
            this.splitContainer3.SplitterDistance = 320;
            this.splitContainer3.TabIndex = 4;
            // 
            // DamageDataFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DamageDataFileViewer";
            this.Size = new System.Drawing.Size(552, 419);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventTypeNumericUpDown)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox damageDataEntryListBox;
        private System.Windows.Forms.ComboBox topLevelListComboBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.NumericUpDown eventTypeNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel angleTablePanel;
        private System.Windows.Forms.Button addAngleButton;
        private System.Windows.Forms.SplitContainer splitContainer3;
    }
}
