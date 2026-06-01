namespace psu_generic_parser.Forms.FileViewers
{
    partial class SoundFileViewer
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.playSoundButton = new System.Windows.Forms.Button();
            this.exportSelectedButton = new System.Windows.Forms.Button();
            this.exportAllButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(299, 316);
            this.listBox1.TabIndex = 0;
            // 
            // playSoundButton
            // 
            this.playSoundButton.Location = new System.Drawing.Point(310, 4);
            this.playSoundButton.Name = "playSoundButton";
            this.playSoundButton.Size = new System.Drawing.Size(92, 23);
            this.playSoundButton.TabIndex = 1;
            this.playSoundButton.Text = "Play Selected";
            this.playSoundButton.UseVisualStyleBackColor = true;
            this.playSoundButton.Click += new System.EventHandler(this.playSoundButton_Click);
            // 
            // exportSelectedButton
            // 
            this.exportSelectedButton.Location = new System.Drawing.Point(310, 33);
            this.exportSelectedButton.Name = "exportSelectedButton";
            this.exportSelectedButton.Size = new System.Drawing.Size(92, 23);
            this.exportSelectedButton.TabIndex = 2;
            this.exportSelectedButton.Text = "Export Selected";
            this.exportSelectedButton.UseVisualStyleBackColor = true;
            this.exportSelectedButton.Click += new System.EventHandler(this.exportSoundButton_Click);
            // 
            // exportAllButton
            // 
            this.exportAllButton.Location = new System.Drawing.Point(310, 62);
            this.exportAllButton.Name = "exportAllButton";
            this.exportAllButton.Size = new System.Drawing.Size(92, 23);
            this.exportAllButton.TabIndex = 3;
            this.exportAllButton.Text = "Export All";
            this.exportAllButton.UseVisualStyleBackColor = true;
            this.exportAllButton.Click += new System.EventHandler(this.exportAllButton_Click);
            // 
            // SoundFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exportAllButton);
            this.Controls.Add(this.exportSelectedButton);
            this.Controls.Add(this.playSoundButton);
            this.Controls.Add(this.listBox1);
            this.Name = "SoundFileViewer";
            this.Size = new System.Drawing.Size(433, 332);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button playSoundButton;
        private System.Windows.Forms.Button exportSelectedButton;
        private System.Windows.Forms.Button exportAllButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
