namespace psu_generic_parser
{
    partial class MainSettings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExportPNGCheckBox = new System.Windows.Forms.CheckBox();
            this.CompressNMLLCheckButton = new System.Windows.Forms.CheckBox();
            this.CompressTMLLCheckButton = new System.Windows.Forms.CheckBox();
            this.exportMetaDataCheckBox = new System.Windows.Forms.CheckBox();
            this.BatchExportSubContainersCheckBox = new System.Windows.Forms.CheckBox();
            this.BatchExportSubDirectoriesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ExportPNGCheckBox
            // 
            this.ExportPNGCheckBox.AutoSize = true;
            this.ExportPNGCheckBox.Checked = true;
            this.ExportPNGCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExportPNGCheckBox.Location = new System.Drawing.Point(12, 12);
            this.ExportPNGCheckBox.Name = "ExportPNGCheckBox";
            this.ExportPNGCheckBox.Size = new System.Drawing.Size(171, 17);
            this.ExportPNGCheckBox.TabIndex = 2;
            this.ExportPNGCheckBox.Text = "Batch Export Textures To .png";
            this.ExportPNGCheckBox.UseVisualStyleBackColor = true;
            this.ExportPNGCheckBox.CheckedChanged += new System.EventHandler(this.ExportPNGCheckBox_CheckedChanged);
            // 
            // CompressNMLLCheckButton
            // 
            this.CompressNMLLCheckButton.AutoSize = true;
            this.CompressNMLLCheckButton.Location = new System.Drawing.Point(12, 100);
            this.CompressNMLLCheckButton.Name = "CompressNMLLCheckButton";
            this.CompressNMLLCheckButton.Size = new System.Drawing.Size(104, 17);
            this.CompressNMLLCheckButton.TabIndex = 3;
            this.CompressNMLLCheckButton.Text = "Compress NMLL";
            this.CompressNMLLCheckButton.UseVisualStyleBackColor = true;
            this.CompressNMLLCheckButton.CheckedChanged += new System.EventHandler(this.CompressNMLLCheckButton_CheckedChanged);
            // 
            // CompressTMLLCheckButton
            // 
            this.CompressTMLLCheckButton.AutoSize = true;
            this.CompressTMLLCheckButton.Location = new System.Drawing.Point(12, 123);
            this.CompressTMLLCheckButton.Name = "CompressTMLLCheckButton";
            this.CompressTMLLCheckButton.Size = new System.Drawing.Size(103, 17);
            this.CompressTMLLCheckButton.TabIndex = 4;
            this.CompressTMLLCheckButton.Text = "Compress TMLL";
            this.CompressTMLLCheckButton.UseVisualStyleBackColor = true;
            this.CompressTMLLCheckButton.CheckedChanged += new System.EventHandler(this.CompressTMLLCheckButton_CheckedChanged);
            // 
            // exportMetaDataCheckBox
            // 
            this.exportMetaDataCheckBox.AutoSize = true;
            this.exportMetaDataCheckBox.Location = new System.Drawing.Point(12, 77);
            this.exportMetaDataCheckBox.Name = "exportMetaDataCheckBox";
            this.exportMetaDataCheckBox.Size = new System.Drawing.Size(109, 17);
            this.exportMetaDataCheckBox.TabIndex = 5;
            this.exportMetaDataCheckBox.Text = "Export Meta Data";
            this.exportMetaDataCheckBox.UseVisualStyleBackColor = true;
            this.exportMetaDataCheckBox.CheckedChanged += new System.EventHandler(this.exportMetaDataCheckBox_CheckedChanged);
            // 
            // BatchExportSubContainersCheckBox
            // 
            this.BatchExportSubContainersCheckBox.AutoSize = true;
            this.BatchExportSubContainersCheckBox.Location = new System.Drawing.Point(12, 35);
            this.BatchExportSubContainersCheckBox.Name = "BatchExportSubContainersCheckBox";
            this.BatchExportSubContainersCheckBox.Size = new System.Drawing.Size(198, 17);
            this.BatchExportSubContainersCheckBox.TabIndex = 6;
            this.BatchExportSubContainersCheckBox.Text = "Batch Export SubArchive Containers";
            this.BatchExportSubContainersCheckBox.UseVisualStyleBackColor = true;
            this.BatchExportSubContainersCheckBox.CheckedChanged += new System.EventHandler(this.BatchExportSubContainersCheckBox_CheckedChanged);
            // 
            // BatchExportSubDirectoriesCheckBox
            // 
            this.BatchExportSubDirectoriesCheckBox.AutoSize = true;
            this.BatchExportSubDirectoriesCheckBox.Checked = true;
            this.BatchExportSubDirectoriesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BatchExportSubDirectoriesCheckBox.Location = new System.Drawing.Point(12, 56);
            this.BatchExportSubDirectoriesCheckBox.Name = "BatchExportSubDirectoriesCheckBox";
            this.BatchExportSubDirectoriesCheckBox.Size = new System.Drawing.Size(173, 17);
            this.BatchExportSubDirectoriesCheckBox.TabIndex = 7;
            this.BatchExportSubDirectoriesCheckBox.Text = "Batch Export Subdirectory Files";
            this.BatchExportSubDirectoriesCheckBox.UseVisualStyleBackColor = true;
            this.BatchExportSubDirectoriesCheckBox.CheckedChanged += new System.EventHandler(this.BatchExportSubDirectories_CheckedChanged);
            // 
            // MainSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 149);
            this.Controls.Add(this.BatchExportSubDirectoriesCheckBox);
            this.Controls.Add(this.BatchExportSubContainersCheckBox);
            this.Controls.Add(this.exportMetaDataCheckBox);
            this.Controls.Add(this.CompressTMLLCheckButton);
            this.Controls.Add(this.CompressNMLLCheckButton);
            this.Controls.Add(this.ExportPNGCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainSettings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ExportPNGCheckBox;
        private System.Windows.Forms.CheckBox CompressNMLLCheckButton;
        private System.Windows.Forms.CheckBox CompressTMLLCheckButton;
        private System.Windows.Forms.CheckBox exportMetaDataCheckBox;
        private System.Windows.Forms.CheckBox BatchExportSubContainersCheckBox;
        private System.Windows.Forms.CheckBox BatchExportSubDirectoriesCheckBox;
    }
}