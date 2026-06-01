
namespace psu_generic_parser.Forms.FileViewers.LndEffects
{
    partial class LightEditorPanel
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
            this.positionGroupBox = new System.Windows.Forms.GroupBox();
            this.yUpDown = new System.Windows.Forms.NumericUpDown();
            this.zUpDown = new System.Windows.Forms.NumericUpDown();
            this.xUpDown = new System.Windows.Forms.NumericUpDown();
            this.colorGroupBox = new System.Windows.Forms.GroupBox();
            this.colorEditor3 = new psu_generic_parser.Forms.FileViewers.Common.ColorEditor();
            this.positionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xUpDown)).BeginInit();
            this.colorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // positionGroupBox
            // 
            this.positionGroupBox.Controls.Add(this.yUpDown);
            this.positionGroupBox.Controls.Add(this.zUpDown);
            this.positionGroupBox.Controls.Add(this.xUpDown);
            this.positionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.positionGroupBox.Name = "positionGroupBox";
            this.positionGroupBox.Size = new System.Drawing.Size(291, 51);
            this.positionGroupBox.TabIndex = 0;
            this.positionGroupBox.TabStop = false;
            this.positionGroupBox.Text = "Location";
            // 
            // yUpDown
            // 
            this.yUpDown.DecimalPlaces = 2;
            this.yUpDown.Location = new System.Drawing.Point(101, 19);
            this.yUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.yUpDown.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.yUpDown.Name = "yUpDown";
            this.yUpDown.Size = new System.Drawing.Size(89, 20);
            this.yUpDown.TabIndex = 1;
            this.yUpDown.ValueChanged += new System.EventHandler(this.yUpDown_ValueChanged);
            // 
            // zUpDown
            // 
            this.zUpDown.DecimalPlaces = 2;
            this.zUpDown.Location = new System.Drawing.Point(196, 19);
            this.zUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.zUpDown.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.zUpDown.Name = "zUpDown";
            this.zUpDown.Size = new System.Drawing.Size(89, 20);
            this.zUpDown.TabIndex = 2;
            this.zUpDown.ValueChanged += new System.EventHandler(this.zUpDown_ValueChanged);
            // 
            // xUpDown
            // 
            this.xUpDown.DecimalPlaces = 2;
            this.xUpDown.Location = new System.Drawing.Point(6, 19);
            this.xUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.xUpDown.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.xUpDown.Name = "xUpDown";
            this.xUpDown.Size = new System.Drawing.Size(89, 20);
            this.xUpDown.TabIndex = 0;
            this.xUpDown.ValueChanged += new System.EventHandler(this.xUpDown_ValueChanged);
            // 
            // colorGroupBox
            // 
            this.colorGroupBox.Controls.Add(this.colorEditor3);
            this.colorGroupBox.Location = new System.Drawing.Point(300, 3);
            this.colorGroupBox.Name = "colorGroupBox";
            this.colorGroupBox.Size = new System.Drawing.Size(230, 113);
            this.colorGroupBox.TabIndex = 1;
            this.colorGroupBox.TabStop = false;
            this.colorGroupBox.Text = "Color";
            // 
            // colorEditor3
            // 
            this.colorEditor3.Location = new System.Drawing.Point(7, 14);
            this.colorEditor3.Name = "colorEditor3";
            this.colorEditor3.Size = new System.Drawing.Size(219, 93);
            this.colorEditor3.TabIndex = 3;
            // 
            // LightEditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colorGroupBox);
            this.Controls.Add(this.positionGroupBox);
            this.Name = "LightEditorPanel";
            this.Size = new System.Drawing.Size(538, 124);
            this.positionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xUpDown)).EndInit();
            this.colorGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox positionGroupBox;
        private System.Windows.Forms.NumericUpDown yUpDown;
        private System.Windows.Forms.NumericUpDown zUpDown;
        private System.Windows.Forms.NumericUpDown xUpDown;
        private System.Windows.Forms.GroupBox colorGroupBox;
        private Common.ColorEditor colorEditor3;
    }
}
