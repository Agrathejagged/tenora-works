
namespace psu_generic_parser.Forms.FileViewers.LndEffects
{
    partial class FogBankPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nearPlaneUpDown = new System.Windows.Forms.NumericUpDown();
            this.farPlaneUpDown = new System.Windows.Forms.NumericUpDown();
            this.initialIntensityUpDown = new System.Windows.Forms.NumericUpDown();
            this.rampSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.colorEditor1 = new psu_generic_parser.Forms.FileViewers.Common.ColorEditor();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nearPlaneUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.farPlaneUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialIntensityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rampSpeedUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Near Plane:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Far Plane:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Initial Intensity:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ramp Up Speed:";
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSize = true;
            this.groupBox4.Controls.Add(this.colorEditor1);
            this.groupBox4.Location = new System.Drawing.Point(7, 111);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(235, 130);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Color";
            // 
            // nearPlaneUpDown
            // 
            this.nearPlaneUpDown.Location = new System.Drawing.Point(99, 4);
            this.nearPlaneUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nearPlaneUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nearPlaneUpDown.Name = "nearPlaneUpDown";
            this.nearPlaneUpDown.Size = new System.Drawing.Size(120, 20);
            this.nearPlaneUpDown.TabIndex = 0;
            this.nearPlaneUpDown.ValueChanged += new System.EventHandler(this.nearPlaneUpDown_ValueChanged);
            // 
            // farPlaneUpDown
            // 
            this.farPlaneUpDown.Location = new System.Drawing.Point(99, 31);
            this.farPlaneUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.farPlaneUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.farPlaneUpDown.Name = "farPlaneUpDown";
            this.farPlaneUpDown.Size = new System.Drawing.Size(120, 20);
            this.farPlaneUpDown.TabIndex = 1;
            this.farPlaneUpDown.ValueChanged += new System.EventHandler(this.farPlaneUpDown_ValueChanged);
            // 
            // initialIntensityUpDown
            // 
            this.initialIntensityUpDown.Location = new System.Drawing.Point(99, 58);
            this.initialIntensityUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.initialIntensityUpDown.Name = "initialIntensityUpDown";
            this.initialIntensityUpDown.Size = new System.Drawing.Size(120, 20);
            this.initialIntensityUpDown.TabIndex = 2;
            this.initialIntensityUpDown.ValueChanged += new System.EventHandler(this.initialIntensityUpDown_ValueChanged);
            // 
            // rampSpeedUpDown
            // 
            this.rampSpeedUpDown.Location = new System.Drawing.Point(99, 85);
            this.rampSpeedUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rampSpeedUpDown.Name = "rampSpeedUpDown";
            this.rampSpeedUpDown.Size = new System.Drawing.Size(120, 20);
            this.rampSpeedUpDown.TabIndex = 3;
            this.rampSpeedUpDown.ValueChanged += new System.EventHandler(this.rampSpeedUpDown_ValueChanged);
            // 
            // colorEditor1
            // 
            this.colorEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorEditor1.Location = new System.Drawing.Point(3, 16);
            this.colorEditor1.Name = "colorEditor1";
            this.colorEditor1.Size = new System.Drawing.Size(229, 111);
            this.colorEditor1.TabIndex = 13;
            // 
            // FogBankPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rampSpeedUpDown);
            this.Controls.Add(this.initialIntensityUpDown);
            this.Controls.Add(this.farPlaneUpDown);
            this.Controls.Add(this.nearPlaneUpDown);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FogBankPanel";
            this.Size = new System.Drawing.Size(436, 370);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nearPlaneUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.farPlaneUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialIntensityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rampSpeedUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private Common.ColorEditor colorEditor1;
        private System.Windows.Forms.NumericUpDown nearPlaneUpDown;
        private System.Windows.Forms.NumericUpDown farPlaneUpDown;
        private System.Windows.Forms.NumericUpDown initialIntensityUpDown;
        private System.Windows.Forms.NumericUpDown rampSpeedUpDown;
    }
}
