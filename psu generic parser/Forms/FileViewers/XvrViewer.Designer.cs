namespace psu_generic_parser
{
    partial class TextureViewer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mipLabel = new System.Windows.Forms.Label();
            this.buttonDownMip = new System.Windows.Forms.Button();
            this.buttonUpMip = new System.Windows.Forms.Button();
            this.buttonReplaceMip = new System.Windows.Forms.Button();
            this.loadMipDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMipDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonImport = new System.Windows.Forms.Button();
            this.rebuildMipsCheckbox = new System.Windows.Forms.CheckBox();
            this.buttonSaveMip = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.importTextureDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportTextureDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pixelFormatDropDown = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Lime;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(366, 277);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mipmaps:";
            // 
            // mipLabel
            // 
            this.mipLabel.AutoSize = true;
            this.mipLabel.Location = new System.Drawing.Point(44, 25);
            this.mipLabel.Name = "mipLabel";
            this.mipLabel.Size = new System.Drawing.Size(13, 13);
            this.mipLabel.TabIndex = 2;
            this.mipLabel.Text = "0";
            // 
            // buttonDownMip
            // 
            this.buttonDownMip.Enabled = false;
            this.buttonDownMip.Location = new System.Drawing.Point(7, 19);
            this.buttonDownMip.Name = "buttonDownMip";
            this.buttonDownMip.Size = new System.Drawing.Size(31, 23);
            this.buttonDownMip.TabIndex = 3;
            this.buttonDownMip.Text = "<<";
            this.buttonDownMip.UseVisualStyleBackColor = true;
            this.buttonDownMip.Click += new System.EventHandler(this.buttonDownMip_Click);
            // 
            // buttonUpMip
            // 
            this.buttonUpMip.Enabled = false;
            this.buttonUpMip.Location = new System.Drawing.Point(74, 20);
            this.buttonUpMip.Name = "buttonUpMip";
            this.buttonUpMip.Size = new System.Drawing.Size(33, 23);
            this.buttonUpMip.TabIndex = 4;
            this.buttonUpMip.Text = ">>";
            this.buttonUpMip.UseVisualStyleBackColor = true;
            this.buttonUpMip.Click += new System.EventHandler(this.buttonUpMip_Click);
            // 
            // buttonReplaceMip
            // 
            this.buttonReplaceMip.Location = new System.Drawing.Point(113, 20);
            this.buttonReplaceMip.Name = "buttonReplaceMip";
            this.buttonReplaceMip.Size = new System.Drawing.Size(75, 23);
            this.buttonReplaceMip.TabIndex = 5;
            this.buttonReplaceMip.Text = "Replace";
            this.buttonReplaceMip.UseVisualStyleBackColor = true;
            this.buttonReplaceMip.Click += new System.EventHandler(this.replaceMipButton_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(406, 48);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 6;
            this.buttonImport.Text = "Import...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.importImageButton_Click);
            // 
            // rebuildMipsCheckbox
            // 
            this.rebuildMipsCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rebuildMipsCheckbox.AutoSize = true;
            this.rebuildMipsCheckbox.Checked = true;
            this.rebuildMipsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rebuildMipsCheckbox.Location = new System.Drawing.Point(400, 155);
            this.rebuildMipsCheckbox.Name = "rebuildMipsCheckbox";
            this.rebuildMipsCheckbox.Size = new System.Drawing.Size(93, 17);
            this.rebuildMipsCheckbox.TabIndex = 7;
            this.rebuildMipsCheckbox.Text = "Rebuild Mips?";
            this.rebuildMipsCheckbox.UseVisualStyleBackColor = true;
            // 
            // buttonSaveMip
            // 
            this.buttonSaveMip.Location = new System.Drawing.Point(194, 20);
            this.buttonSaveMip.Name = "buttonSaveMip";
            this.buttonSaveMip.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveMip.TabIndex = 8;
            this.buttonSaveMip.Text = "Save";
            this.buttonSaveMip.UseVisualStyleBackColor = true;
            this.buttonSaveMip.Click += new System.EventHandler(this.buttonSaveMip_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(406, 76);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 9;
            this.buttonExport.Text = "Export...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // importTextureDialog
            // 
            this.importTextureDialog.FileName = "openFileDialog1";
            this.importTextureDialog.Filter = "PNG Image|*.png|PSU XVR Files|*.xvr|PVRTexTool PVR|*.pvr";
            // 
            // exportTextureDialog
            // 
            this.exportTextureDialog.Filter = "PNG Image|*.png|PSU XVR Files|*.xvr|PVRTexTool PVR|*.pvr";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(7, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 308);
            this.panel1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Pixel Format:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(357, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "pfText";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(275, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Texture format: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(357, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "rfText";
            // 
            // pixelFormatDropDown
            // 
            this.pixelFormatDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelFormatDropDown.FormattingEnabled = true;
            this.pixelFormatDropDown.Location = new System.Drawing.Point(400, 204);
            this.pixelFormatDropDown.Name = "pixelFormatDropDown";
            this.pixelFormatDropDown.Size = new System.Drawing.Size(121, 21);
            this.pixelFormatDropDown.TabIndex = 15;
            this.pixelFormatDropDown.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Save Pixel Format:";
            // 
            // XvrViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pixelFormatDropDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonSaveMip);
            this.Controls.Add(this.rebuildMipsCheckbox);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonReplaceMip);
            this.Controls.Add(this.buttonUpMip);
            this.Controls.Add(this.buttonDownMip);
            this.Controls.Add(this.mipLabel);
            this.Controls.Add(this.label1);
            this.Name = "XvrViewer";
            this.Size = new System.Drawing.Size(524, 360);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label mipLabel;
        private System.Windows.Forms.Button buttonDownMip;
        private System.Windows.Forms.Button buttonUpMip;
        private System.Windows.Forms.Button buttonReplaceMip;
        private System.Windows.Forms.OpenFileDialog loadMipDialog;
        private System.Windows.Forms.SaveFileDialog saveMipDialog;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.CheckBox rebuildMipsCheckbox;
        private System.Windows.Forms.Button buttonSaveMip;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.OpenFileDialog importTextureDialog;
        private System.Windows.Forms.SaveFileDialog exportTextureDialog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox pixelFormatDropDown;
        private System.Windows.Forms.Label label6;
    }
}
