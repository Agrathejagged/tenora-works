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
            this.exportMetaDataCheckBox = new System.Windows.Forms.CheckBox();
            this.BatchExportSubContainersCheckBox = new System.Windows.Forms.CheckBox();
            this.BatchExportSubDirectoriesCheckBox = new System.Windows.Forms.CheckBox();
            this.nblSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.tmllChunkGroupBox = new System.Windows.Forms.GroupBox();
            this.alwaysCompressTmllRadioButton = new System.Windows.Forms.RadioButton();
            this.alwaysDecompressTmllRadioButton = new System.Windows.Forms.RadioButton();
            this.useOriginalTmllCompressionRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.alwaysCompressNmllRadioButton = new System.Windows.Forms.RadioButton();
            this.alwaysDecompressNmllRadioButton = new System.Windows.Forms.RadioButton();
            this.useOriginalNmllCompressionRadioButton = new System.Windows.Forms.RadioButton();
            this.nblSettingsGroupBox.SuspendLayout();
            this.tmllChunkGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // nblSettingsGroupBox
            // 
            this.nblSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nblSettingsGroupBox.Controls.Add(this.tmllChunkGroupBox);
            this.nblSettingsGroupBox.Controls.Add(this.groupBox1);
            this.nblSettingsGroupBox.Location = new System.Drawing.Point(12, 100);
            this.nblSettingsGroupBox.Name = "nblSettingsGroupBox";
            this.nblSettingsGroupBox.Size = new System.Drawing.Size(214, 216);
            this.nblSettingsGroupBox.TabIndex = 9;
            this.nblSettingsGroupBox.TabStop = false;
            this.nblSettingsGroupBox.Text = "NBL Settings";
            // 
            // tmllChunkGroupBox
            // 
            this.tmllChunkGroupBox.Controls.Add(this.alwaysCompressTmllRadioButton);
            this.tmllChunkGroupBox.Controls.Add(this.alwaysDecompressTmllRadioButton);
            this.tmllChunkGroupBox.Controls.Add(this.useOriginalTmllCompressionRadioButton);
            this.tmllChunkGroupBox.Location = new System.Drawing.Point(11, 116);
            this.tmllChunkGroupBox.Name = "tmllChunkGroupBox";
            this.tmllChunkGroupBox.Size = new System.Drawing.Size(187, 91);
            this.tmllChunkGroupBox.TabIndex = 14;
            this.tmllChunkGroupBox.TabStop = false;
            this.tmllChunkGroupBox.Text = "TMLL Chunks";
            // 
            // alwaysCompressTmllRadioButton
            // 
            this.alwaysCompressTmllRadioButton.AutoSize = true;
            this.alwaysCompressTmllRadioButton.Location = new System.Drawing.Point(9, 19);
            this.alwaysCompressTmllRadioButton.Name = "alwaysCompressTmllRadioButton";
            this.alwaysCompressTmllRadioButton.Size = new System.Drawing.Size(107, 17);
            this.alwaysCompressTmllRadioButton.TabIndex = 12;
            this.alwaysCompressTmllRadioButton.Text = "Always Compress";
            this.alwaysCompressTmllRadioButton.UseVisualStyleBackColor = true;
            this.alwaysCompressTmllRadioButton.CheckedChanged += new System.EventHandler(this.tmllChunkOverrideOptions_CheckedChanged);
            // 
            // alwaysDecompressTmllRadioButton
            // 
            this.alwaysDecompressTmllRadioButton.AutoSize = true;
            this.alwaysDecompressTmllRadioButton.Location = new System.Drawing.Point(9, 42);
            this.alwaysDecompressTmllRadioButton.Name = "alwaysDecompressTmllRadioButton";
            this.alwaysDecompressTmllRadioButton.Size = new System.Drawing.Size(120, 17);
            this.alwaysDecompressTmllRadioButton.TabIndex = 10;
            this.alwaysDecompressTmllRadioButton.Text = "Always Decompress";
            this.alwaysDecompressTmllRadioButton.UseVisualStyleBackColor = true;
            this.alwaysDecompressTmllRadioButton.CheckedChanged += new System.EventHandler(this.tmllChunkOverrideOptions_CheckedChanged);
            // 
            // useOriginalTmllCompressionRadioButton
            // 
            this.useOriginalTmllCompressionRadioButton.AutoSize = true;
            this.useOriginalTmllCompressionRadioButton.Location = new System.Drawing.Point(9, 65);
            this.useOriginalTmllCompressionRadioButton.Name = "useOriginalTmllCompressionRadioButton";
            this.useOriginalTmllCompressionRadioButton.Size = new System.Drawing.Size(156, 17);
            this.useOriginalTmllCompressionRadioButton.TabIndex = 11;
            this.useOriginalTmllCompressionRadioButton.Text = "Preserve Individual Settings";
            this.useOriginalTmllCompressionRadioButton.UseVisualStyleBackColor = true;
            this.useOriginalTmllCompressionRadioButton.CheckedChanged += new System.EventHandler(this.tmllChunkOverrideOptions_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.alwaysCompressNmllRadioButton);
            this.groupBox1.Controls.Add(this.alwaysDecompressNmllRadioButton);
            this.groupBox1.Controls.Add(this.useOriginalNmllCompressionRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(11, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(187, 91);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NMLL Chunks";
            // 
            // alwaysCompressNmllRadioButton
            // 
            this.alwaysCompressNmllRadioButton.AutoSize = true;
            this.alwaysCompressNmllRadioButton.Location = new System.Drawing.Point(9, 19);
            this.alwaysCompressNmllRadioButton.Name = "alwaysCompressNmllRadioButton";
            this.alwaysCompressNmllRadioButton.Size = new System.Drawing.Size(107, 17);
            this.alwaysCompressNmllRadioButton.TabIndex = 12;
            this.alwaysCompressNmllRadioButton.Text = "Always Compress";
            this.alwaysCompressNmllRadioButton.UseVisualStyleBackColor = true;
            this.alwaysCompressNmllRadioButton.CheckedChanged += new System.EventHandler(this.nmllChunkOverrideOptions_CheckedChanged);
            // 
            // alwaysDecompressNmllRadioButton
            // 
            this.alwaysDecompressNmllRadioButton.AutoSize = true;
            this.alwaysDecompressNmllRadioButton.Location = new System.Drawing.Point(9, 42);
            this.alwaysDecompressNmllRadioButton.Name = "alwaysDecompressNmllRadioButton";
            this.alwaysDecompressNmllRadioButton.Size = new System.Drawing.Size(120, 17);
            this.alwaysDecompressNmllRadioButton.TabIndex = 10;
            this.alwaysDecompressNmllRadioButton.Text = "Always Decompress";
            this.alwaysDecompressNmllRadioButton.UseVisualStyleBackColor = true;
            this.alwaysDecompressNmllRadioButton.CheckedChanged += new System.EventHandler(this.nmllChunkOverrideOptions_CheckedChanged);
            // 
            // useOriginalNmllCompressionRadioButton
            // 
            this.useOriginalNmllCompressionRadioButton.AutoSize = true;
            this.useOriginalNmllCompressionRadioButton.Location = new System.Drawing.Point(9, 65);
            this.useOriginalNmllCompressionRadioButton.Name = "useOriginalNmllCompressionRadioButton";
            this.useOriginalNmllCompressionRadioButton.Size = new System.Drawing.Size(156, 17);
            this.useOriginalNmllCompressionRadioButton.TabIndex = 11;
            this.useOriginalNmllCompressionRadioButton.Text = "Preserve Individual Settings";
            this.useOriginalNmllCompressionRadioButton.UseVisualStyleBackColor = true;
            this.useOriginalNmllCompressionRadioButton.CheckedChanged += new System.EventHandler(this.nmllChunkOverrideOptions_CheckedChanged);
            // 
            // MainSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 323);
            this.Controls.Add(this.nblSettingsGroupBox);
            this.Controls.Add(this.BatchExportSubDirectoriesCheckBox);
            this.Controls.Add(this.BatchExportSubContainersCheckBox);
            this.Controls.Add(this.exportMetaDataCheckBox);
            this.Controls.Add(this.ExportPNGCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainSettings";
            this.Text = "Settings";
            this.nblSettingsGroupBox.ResumeLayout(false);
            this.tmllChunkGroupBox.ResumeLayout(false);
            this.tmllChunkGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ExportPNGCheckBox;
        private System.Windows.Forms.CheckBox exportMetaDataCheckBox;
        private System.Windows.Forms.CheckBox BatchExportSubContainersCheckBox;
        private System.Windows.Forms.CheckBox BatchExportSubDirectoriesCheckBox;
        private System.Windows.Forms.GroupBox nblSettingsGroupBox;
        private System.Windows.Forms.GroupBox tmllChunkGroupBox;
        private System.Windows.Forms.RadioButton alwaysCompressTmllRadioButton;
        private System.Windows.Forms.RadioButton alwaysDecompressTmllRadioButton;
        private System.Windows.Forms.RadioButton useOriginalTmllCompressionRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton alwaysCompressNmllRadioButton;
        private System.Windows.Forms.RadioButton alwaysDecompressNmllRadioButton;
        private System.Windows.Forms.RadioButton useOriginalNmllCompressionRadioButton;
    }
}