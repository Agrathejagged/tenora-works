namespace psu_generic_parser.Forms.FileViewers.SetEditorSupportClasses
{
    partial class HexMetadataEditor
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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.metadataHexEditor = new WpfHexaEditor.HexEditor();
            this.metadataLengthUD = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.metadataLengthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.metadataLengthUD)).BeginInit();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(3, 49);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(352, 158);
            this.elementHost1.TabIndex = 32;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.metadataHexEditor;
            // 
            // metadataLengthUD
            // 
            this.metadataLengthUD.Location = new System.Drawing.Point(158, 23);
            this.metadataLengthUD.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.metadataLengthUD.Name = "metadataLengthUD";
            this.metadataLengthUD.Size = new System.Drawing.Size(120, 20);
            this.metadataLengthUD.TabIndex = 35;
            this.metadataLengthUD.ValueChanged += new System.EventHandler(this.metadataLengthUD_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Current Metadata Length: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Expected Metadata Length:";
            // 
            // metadataLengthLabel
            // 
            this.metadataLengthLabel.AutoSize = true;
            this.metadataLengthLabel.Location = new System.Drawing.Point(155, 7);
            this.metadataLengthLabel.Name = "metadataLengthLabel";
            this.metadataLengthLabel.Size = new System.Drawing.Size(35, 13);
            this.metadataLengthLabel.TabIndex = 36;
            this.metadataLengthLabel.Text = "label3";
            // 
            // HexMetadataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.metadataLengthUD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.metadataLengthLabel);
            this.Name = "HexMetadataEditor";
            this.Size = new System.Drawing.Size(358, 210);
            ((System.ComponentModel.ISupportInitialize)(this.metadataLengthUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private WpfHexaEditor.HexEditor metadataHexEditor;
        private System.Windows.Forms.NumericUpDown metadataLengthUD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label metadataLengthLabel;
    }
}
