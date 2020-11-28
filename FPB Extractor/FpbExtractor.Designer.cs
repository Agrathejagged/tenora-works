namespace FPB_Extractor
{
    partial class FpbExtractForm
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
            this.psp1FileNaRadioButton = new System.Windows.Forms.RadioButton();
            this.infinityFileRadioButton = new System.Windows.Forms.RadioButton();
            this.infinityMediaRadioButton = new System.Windows.Forms.RadioButton();
            this.customFpbRadioButton = new System.Windows.Forms.RadioButton();
            this.extractFpbButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.psp1FileJRadioButton = new System.Windows.Forms.RadioButton();
            this.psp1FileEurRadioButton = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.psp2MediaJRadioButton = new System.Windows.Forms.RadioButton();
            this.psp2MediaEurRadioButton = new System.Windows.Forms.RadioButton();
            this.psp2MediaNaRadioButton = new System.Windows.Forms.RadioButton();
            this.psp2FileJRadioButton = new System.Windows.Forms.RadioButton();
            this.psp2FileEurRadioButton = new System.Windows.Forms.RadioButton();
            this.psp2FileNaRadioButton = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.offsetExplanation = new System.Windows.Forms.Label();
            this.browseListFileButton = new System.Windows.Forms.Button();
            this.listFileLabel = new System.Windows.Forms.Label();
            this.baseOffsetLabel = new System.Windows.Forms.Label();
            this.listFileBox = new System.Windows.Forms.TextBox();
            this.baseOffsetUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.destinationTextBox = new System.Windows.Forms.TextBox();
            this.addExtensionCheckbox = new System.Windows.Forms.CheckBox();
            this.browseDestinationButton = new System.Windows.Forms.Button();
            this.useCrc32FilenameCheckbox = new System.Windows.Forms.CheckBox();
            this.destinationFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.fpbFileBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.listFileBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.bruteRipButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseOffsetUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // psp1FileNaRadioButton
            // 
            this.psp1FileNaRadioButton.AutoSize = true;
            this.psp1FileNaRadioButton.Checked = true;
            this.psp1FileNaRadioButton.Location = new System.Drawing.Point(19, 36);
            this.psp1FileNaRadioButton.Name = "psp1FileNaRadioButton";
            this.psp1FileNaRadioButton.Size = new System.Drawing.Size(80, 17);
            this.psp1FileNaRadioButton.TabIndex = 0;
            this.psp1FileNaRadioButton.TabStop = true;
            this.psp1FileNaRadioButton.Text = "file.fpb (NA)";
            this.psp1FileNaRadioButton.UseVisualStyleBackColor = true;
            // 
            // infinityFileRadioButton
            // 
            this.infinityFileRadioButton.AutoSize = true;
            this.infinityFileRadioButton.Location = new System.Drawing.Point(19, 157);
            this.infinityFileRadioButton.Name = "infinityFileRadioButton";
            this.infinityFileRadioButton.Size = new System.Drawing.Size(56, 17);
            this.infinityFileRadioButton.TabIndex = 1;
            this.infinityFileRadioButton.Text = "file.fpb";
            this.infinityFileRadioButton.UseVisualStyleBackColor = true;
            // 
            // infinityMediaRadioButton
            // 
            this.infinityMediaRadioButton.AutoSize = true;
            this.infinityMediaRadioButton.Location = new System.Drawing.Point(120, 157);
            this.infinityMediaRadioButton.Name = "infinityMediaRadioButton";
            this.infinityMediaRadioButton.Size = new System.Drawing.Size(71, 17);
            this.infinityMediaRadioButton.TabIndex = 2;
            this.infinityMediaRadioButton.Text = "media.fpb";
            this.infinityMediaRadioButton.UseVisualStyleBackColor = true;
            // 
            // customFpbRadioButton
            // 
            this.customFpbRadioButton.AutoSize = true;
            this.customFpbRadioButton.Location = new System.Drawing.Point(19, 211);
            this.customFpbRadioButton.Name = "customFpbRadioButton";
            this.customFpbRadioButton.Size = new System.Drawing.Size(94, 17);
            this.customFpbRadioButton.TabIndex = 3;
            this.customFpbRadioButton.Text = "CUSTOM FPB";
            this.customFpbRadioButton.UseVisualStyleBackColor = true;
            this.customFpbRadioButton.CheckedChanged += new System.EventHandler(this.customFpbRadioButton_CheckedChanged);
            // 
            // extractFpbButton
            // 
            this.extractFpbButton.Location = new System.Drawing.Point(13, 13);
            this.extractFpbButton.Name = "extractFpbButton";
            this.extractFpbButton.Size = new System.Drawing.Size(131, 23);
            this.extractFpbButton.TabIndex = 4;
            this.extractFpbButton.Text = "Extract FPB using list";
            this.extractFpbButton.UseVisualStyleBackColor = true;
            this.extractFpbButton.Click += new System.EventHandler(this.extractFpbButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.psp1FileJRadioButton);
            this.groupBox1.Controls.Add(this.psp1FileEurRadioButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.psp2MediaJRadioButton);
            this.groupBox1.Controls.Add(this.psp2MediaEurRadioButton);
            this.groupBox1.Controls.Add(this.psp2MediaNaRadioButton);
            this.groupBox1.Controls.Add(this.psp2FileJRadioButton);
            this.groupBox1.Controls.Add(this.psp2FileEurRadioButton);
            this.groupBox1.Controls.Add(this.psp2FileNaRadioButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.offsetExplanation);
            this.groupBox1.Controls.Add(this.browseListFileButton);
            this.groupBox1.Controls.Add(this.listFileLabel);
            this.groupBox1.Controls.Add(this.baseOffsetLabel);
            this.groupBox1.Controls.Add(this.listFileBox);
            this.groupBox1.Controls.Add(this.baseOffsetUpDown);
            this.groupBox1.Controls.Add(this.infinityMediaRadioButton);
            this.groupBox1.Controls.Add(this.psp1FileNaRadioButton);
            this.groupBox1.Controls.Add(this.customFpbRadioButton);
            this.groupBox1.Controls.Add(this.infinityFileRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(555, 293);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List File Settings";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Custom:";
            // 
            // psp1FileJRadioButton
            // 
            this.psp1FileJRadioButton.AutoSize = true;
            this.psp1FileJRadioButton.Location = new System.Drawing.Point(229, 36);
            this.psp1FileJRadioButton.Name = "psp1FileJRadioButton";
            this.psp1FileJRadioButton.Size = new System.Drawing.Size(70, 17);
            this.psp1FileJRadioButton.TabIndex = 20;
            this.psp1FileJRadioButton.Text = "file.fpb (J)";
            this.psp1FileJRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp1FileEurRadioButton
            // 
            this.psp1FileEurRadioButton.AutoSize = true;
            this.psp1FileEurRadioButton.Location = new System.Drawing.Point(120, 36);
            this.psp1FileEurRadioButton.Name = "psp1FileEurRadioButton";
            this.psp1FileEurRadioButton.Size = new System.Drawing.Size(88, 17);
            this.psp1FileEurRadioButton.TabIndex = 19;
            this.psp1FileEurRadioButton.Text = "file.fpb (EUR)";
            this.psp1FileEurRadioButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "PSP2i final:";
            // 
            // psp2MediaJRadioButton
            // 
            this.psp2MediaJRadioButton.AutoSize = true;
            this.psp2MediaJRadioButton.Location = new System.Drawing.Point(229, 108);
            this.psp2MediaJRadioButton.Name = "psp2MediaJRadioButton";
            this.psp2MediaJRadioButton.Size = new System.Drawing.Size(85, 17);
            this.psp2MediaJRadioButton.TabIndex = 17;
            this.psp2MediaJRadioButton.Text = "media.fpb (J)";
            this.psp2MediaJRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2MediaJRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp2MediaEurRadioButton
            // 
            this.psp2MediaEurRadioButton.AutoSize = true;
            this.psp2MediaEurRadioButton.Location = new System.Drawing.Point(120, 108);
            this.psp2MediaEurRadioButton.Name = "psp2MediaEurRadioButton";
            this.psp2MediaEurRadioButton.Size = new System.Drawing.Size(103, 17);
            this.psp2MediaEurRadioButton.TabIndex = 16;
            this.psp2MediaEurRadioButton.Text = "media.fpb (EUR)";
            this.psp2MediaEurRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2MediaEurRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp2MediaNaRadioButton
            // 
            this.psp2MediaNaRadioButton.AutoSize = true;
            this.psp2MediaNaRadioButton.Location = new System.Drawing.Point(19, 108);
            this.psp2MediaNaRadioButton.Name = "psp2MediaNaRadioButton";
            this.psp2MediaNaRadioButton.Size = new System.Drawing.Size(95, 17);
            this.psp2MediaNaRadioButton.TabIndex = 15;
            this.psp2MediaNaRadioButton.Text = "media.fpb (NA)";
            this.psp2MediaNaRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2MediaNaRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp2FileJRadioButton
            // 
            this.psp2FileJRadioButton.AutoSize = true;
            this.psp2FileJRadioButton.Location = new System.Drawing.Point(229, 85);
            this.psp2FileJRadioButton.Name = "psp2FileJRadioButton";
            this.psp2FileJRadioButton.Size = new System.Drawing.Size(70, 17);
            this.psp2FileJRadioButton.TabIndex = 14;
            this.psp2FileJRadioButton.Text = "file.fpb (J)";
            this.psp2FileJRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2FileJRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp2FileEurRadioButton
            // 
            this.psp2FileEurRadioButton.AutoSize = true;
            this.psp2FileEurRadioButton.Location = new System.Drawing.Point(120, 85);
            this.psp2FileEurRadioButton.Name = "psp2FileEurRadioButton";
            this.psp2FileEurRadioButton.Size = new System.Drawing.Size(88, 17);
            this.psp2FileEurRadioButton.TabIndex = 13;
            this.psp2FileEurRadioButton.Text = "file.fpb (EUR)";
            this.psp2FileEurRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2FileEurRadioButton.UseVisualStyleBackColor = true;
            // 
            // psp2FileNaRadioButton
            // 
            this.psp2FileNaRadioButton.AutoSize = true;
            this.psp2FileNaRadioButton.Location = new System.Drawing.Point(19, 85);
            this.psp2FileNaRadioButton.Name = "psp2FileNaRadioButton";
            this.psp2FileNaRadioButton.Size = new System.Drawing.Size(80, 17);
            this.psp2FileNaRadioButton.TabIndex = 12;
            this.psp2FileNaRadioButton.Text = "file.fpb (NA)";
            this.psp2FileNaRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.psp2FileNaRadioButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "PSP2:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "PSP1:";
            // 
            // offsetExplanation
            // 
            this.offsetExplanation.AutoSize = true;
            this.offsetExplanation.Location = new System.Drawing.Point(224, 236);
            this.offsetExplanation.Name = "offsetExplanation";
            this.offsetExplanation.Size = new System.Drawing.Size(304, 13);
            this.offsetExplanation.TabIndex = 9;
            this.offsetExplanation.Text = "NOTE: This is probably 0 for file.fpb, or 80000000 for media.fpb";
            this.offsetExplanation.Visible = false;
            // 
            // browseListFileButton
            // 
            this.browseListFileButton.Enabled = false;
            this.browseListFileButton.Location = new System.Drawing.Point(477, 258);
            this.browseListFileButton.Name = "browseListFileButton";
            this.browseListFileButton.Size = new System.Drawing.Size(75, 23);
            this.browseListFileButton.TabIndex = 8;
            this.browseListFileButton.Text = "Browse";
            this.browseListFileButton.UseVisualStyleBackColor = true;
            this.browseListFileButton.Click += new System.EventHandler(this.browseListFileButton_Click);
            // 
            // listFileLabel
            // 
            this.listFileLabel.AutoSize = true;
            this.listFileLabel.Enabled = false;
            this.listFileLabel.Location = new System.Drawing.Point(29, 263);
            this.listFileLabel.Name = "listFileLabel";
            this.listFileLabel.Size = new System.Drawing.Size(45, 13);
            this.listFileLabel.TabIndex = 7;
            this.listFileLabel.Text = "List File:";
            // 
            // baseOffsetLabel
            // 
            this.baseOffsetLabel.AutoSize = true;
            this.baseOffsetLabel.Enabled = false;
            this.baseOffsetLabel.Location = new System.Drawing.Point(29, 236);
            this.baseOffsetLabel.Name = "baseOffsetLabel";
            this.baseOffsetLabel.Size = new System.Drawing.Size(63, 13);
            this.baseOffsetLabel.TabIndex = 6;
            this.baseOffsetLabel.Text = "Base offset:";
            // 
            // listFileBox
            // 
            this.listFileBox.Enabled = false;
            this.listFileBox.Location = new System.Drawing.Point(98, 260);
            this.listFileBox.Name = "listFileBox";
            this.listFileBox.Size = new System.Drawing.Size(373, 20);
            this.listFileBox.TabIndex = 5;
            // 
            // baseOffsetUpDown
            // 
            this.baseOffsetUpDown.Enabled = false;
            this.baseOffsetUpDown.Hexadecimal = true;
            this.baseOffsetUpDown.Location = new System.Drawing.Point(98, 234);
            this.baseOffsetUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.baseOffsetUpDown.Name = "baseOffsetUpDown";
            this.baseOffsetUpDown.Size = new System.Drawing.Size(120, 20);
            this.baseOffsetUpDown.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Destination: ";
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Location = new System.Drawing.Point(85, 40);
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.Size = new System.Drawing.Size(396, 20);
            this.destinationTextBox.TabIndex = 8;
            this.destinationTextBox.Text = "extracted";
            // 
            // addExtensionCheckbox
            // 
            this.addExtensionCheckbox.AutoSize = true;
            this.addExtensionCheckbox.Checked = true;
            this.addExtensionCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addExtensionCheckbox.Location = new System.Drawing.Point(13, 66);
            this.addExtensionCheckbox.Name = "addExtensionCheckbox";
            this.addExtensionCheckbox.Size = new System.Drawing.Size(156, 17);
            this.addExtensionCheckbox.TabIndex = 9;
            this.addExtensionCheckbox.Text = "Include predicted extension";
            this.addExtensionCheckbox.UseVisualStyleBackColor = true;
            // 
            // browseDestinationButton
            // 
            this.browseDestinationButton.Location = new System.Drawing.Point(487, 38);
            this.browseDestinationButton.Name = "browseDestinationButton";
            this.browseDestinationButton.Size = new System.Drawing.Size(75, 23);
            this.browseDestinationButton.TabIndex = 10;
            this.browseDestinationButton.Text = "Browse";
            this.browseDestinationButton.UseVisualStyleBackColor = true;
            this.browseDestinationButton.Click += new System.EventHandler(this.browseDestinationButton_Click);
            // 
            // useCrc32FilenameCheckbox
            // 
            this.useCrc32FilenameCheckbox.AutoSize = true;
            this.useCrc32FilenameCheckbox.Checked = true;
            this.useCrc32FilenameCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useCrc32FilenameCheckbox.Location = new System.Drawing.Point(13, 90);
            this.useCrc32FilenameCheckbox.Name = "useCrc32FilenameCheckbox";
            this.useCrc32FilenameCheckbox.Size = new System.Drawing.Size(131, 17);
            this.useCrc32FilenameCheckbox.TabIndex = 11;
            this.useCrc32FilenameCheckbox.Text = "Use crc32 as filename";
            this.useCrc32FilenameCheckbox.UseVisualStyleBackColor = true;
            // 
            // fpbFileBrowserDialog
            // 
            this.fpbFileBrowserDialog.FileName = "openFileDialog1";
            this.fpbFileBrowserDialog.Filter = "FPB Files|*.fpb|All Files|*.*";
            // 
            // listFileBrowserDialog
            // 
            this.listFileBrowserDialog.FileName = "openFileDialog2";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(105, 412);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(454, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 417);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Extract Progress";
            // 
            // bruteRipButton
            // 
            this.bruteRipButton.Location = new System.Drawing.Point(150, 13);
            this.bruteRipButton.Name = "bruteRipButton";
            this.bruteRipButton.Size = new System.Drawing.Size(95, 23);
            this.bruteRipButton.TabIndex = 14;
            this.bruteRipButton.Text = "Rip Files (no list)";
            this.bruteRipButton.UseVisualStyleBackColor = true;
            this.bruteRipButton.Click += new System.EventHandler(this.ripFpbButton_Click);
            // 
            // FpbExtractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 439);
            this.Controls.Add(this.bruteRipButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.useCrc32FilenameCheckbox);
            this.Controls.Add(this.browseDestinationButton);
            this.Controls.Add(this.addExtensionCheckbox);
            this.Controls.Add(this.destinationTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.extractFpbButton);
            this.Name = "FpbExtractForm";
            this.Text = "FPB Extractor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseOffsetUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton psp1FileNaRadioButton;
        private System.Windows.Forms.RadioButton infinityFileRadioButton;
        private System.Windows.Forms.RadioButton infinityMediaRadioButton;
        private System.Windows.Forms.RadioButton customFpbRadioButton;
        private System.Windows.Forms.Button extractFpbButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button browseListFileButton;
        private System.Windows.Forms.Label listFileLabel;
        private System.Windows.Forms.Label baseOffsetLabel;
        private System.Windows.Forms.TextBox listFileBox;
        private System.Windows.Forms.NumericUpDown baseOffsetUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox destinationTextBox;
        private System.Windows.Forms.CheckBox addExtensionCheckbox;
        private System.Windows.Forms.Button browseDestinationButton;
        private System.Windows.Forms.CheckBox useCrc32FilenameCheckbox;
        private System.Windows.Forms.FolderBrowserDialog destinationFolderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog fpbFileBrowserDialog;
        private System.Windows.Forms.OpenFileDialog listFileBrowserDialog;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label offsetExplanation;
        private System.Windows.Forms.Button bruteRipButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton psp2MediaJRadioButton;
        private System.Windows.Forms.RadioButton psp2MediaEurRadioButton;
        private System.Windows.Forms.RadioButton psp2MediaNaRadioButton;
        private System.Windows.Forms.RadioButton psp2FileJRadioButton;
        private System.Windows.Forms.RadioButton psp2FileEurRadioButton;
        private System.Windows.Forms.RadioButton psp2FileNaRadioButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton psp1FileJRadioButton;
        private System.Windows.Forms.RadioButton psp1FileEurRadioButton;
    }
}

