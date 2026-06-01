
namespace psu_generic_parser.Forms.FileViewers.Common
{
    partial class ColorEditor
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
            this.greenUpDown = new System.Windows.Forms.NumericUpDown();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.redUpDown = new System.Windows.Forms.NumericUpDown();
            this.blueUpDown = new System.Windows.Forms.NumericUpDown();
            this.colorChooser = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.greenUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // greenUpDown
            // 
            this.greenUpDown.DecimalPlaces = 2;
            this.greenUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.greenUpDown.Location = new System.Drawing.Point(99, 3);
            this.greenUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.greenUpDown.Name = "greenUpDown";
            this.greenUpDown.Size = new System.Drawing.Size(42, 20);
            this.greenUpDown.TabIndex = 1;
            this.greenUpDown.ValueChanged += new System.EventHandler(this.greenUpDown_ValueChanged);
            // 
            // previewBox
            // 
            this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewBox.Location = new System.Drawing.Point(157, 34);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(55, 50);
            this.previewBox.TabIndex = 17;
            this.previewBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Choose";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // redUpDown
            // 
            this.redUpDown.DecimalPlaces = 2;
            this.redUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.redUpDown.Location = new System.Drawing.Point(27, 3);
            this.redUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.redUpDown.Name = "redUpDown";
            this.redUpDown.Size = new System.Drawing.Size(42, 20);
            this.redUpDown.TabIndex = 0;
            this.redUpDown.ValueChanged += new System.EventHandler(this.redUpDown_ValueChanged);
            // 
            // blueUpDown
            // 
            this.blueUpDown.DecimalPlaces = 2;
            this.blueUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.blueUpDown.Location = new System.Drawing.Point(170, 3);
            this.blueUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.blueUpDown.Name = "blueUpDown";
            this.blueUpDown.Size = new System.Drawing.Size(42, 20);
            this.blueUpDown.TabIndex = 2;
            this.blueUpDown.ValueChanged += new System.EventHandler(this.blueUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "R:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "G:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "B:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Preview:";
            // 
            // ColorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.greenUpDown);
            this.Controls.Add(this.previewBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.redUpDown);
            this.Controls.Add(this.blueUpDown);
            this.Name = "ColorEditor";
            this.Size = new System.Drawing.Size(219, 93);
            ((System.ComponentModel.ISupportInitialize)(this.greenUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown greenUpDown;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown redUpDown;
        private System.Windows.Forms.NumericUpDown blueUpDown;
        private System.Windows.Forms.ColorDialog colorChooser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
