namespace psu_generic_parser.Forms.FileViewers
{
    partial class FogBankViewer
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
            this.fogSelectionListBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fogDisplayPanel = new psu_generic_parser.Forms.FileViewers.LndEffects.FogBankPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fogSelectionListBox
            // 
            this.fogSelectionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fogSelectionListBox.FormattingEnabled = true;
            this.fogSelectionListBox.Location = new System.Drawing.Point(0, 0);
            this.fogSelectionListBox.Name = "fogSelectionListBox";
            this.fogSelectionListBox.Size = new System.Drawing.Size(170, 400);
            this.fogSelectionListBox.TabIndex = 0;
            this.fogSelectionListBox.SelectedIndexChanged += new System.EventHandler(this.fogSelectionListBox_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fogSelectionListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fogDisplayPanel);
            this.splitContainer1.Size = new System.Drawing.Size(511, 400);
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 1;
            // 
            // fogDisplayPanel
            // 
            this.fogDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fogDisplayPanel.Fog = null;
            this.fogDisplayPanel.Location = new System.Drawing.Point(0, 0);
            this.fogDisplayPanel.Name = "fogDisplayPanel";
            this.fogDisplayPanel.Size = new System.Drawing.Size(337, 400);
            this.fogDisplayPanel.TabIndex = 0;
            // 
            // FogBankViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FogBankViewer";
            this.Size = new System.Drawing.Size(511, 400);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox fogSelectionListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private LndEffects.FogBankPanel fogDisplayPanel;
    }
}
