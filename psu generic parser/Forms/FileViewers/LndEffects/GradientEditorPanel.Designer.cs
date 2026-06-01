
namespace psu_generic_parser.Forms.FileViewers
{
    partial class GradientEditorPanel
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
            this.unknownValueUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chooseEndColorButton = new System.Windows.Forms.Button();
            this.endOpacityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.endBlueUpDown = new System.Windows.Forms.NumericUpDown();
            this.endRedUpDown = new System.Windows.Forms.NumericUpDown();
            this.endGreenUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.endHeightUpDown = new System.Windows.Forms.NumericUpDown();
            this.topGradientStartPanel = new System.Windows.Forms.GroupBox();
            this.chooseStartColorButton = new System.Windows.Forms.Button();
            this.startOpacityUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.startBlueUpDown = new System.Windows.Forms.NumericUpDown();
            this.startRedUpDown = new System.Windows.Forms.NumericUpDown();
            this.startGreenUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.startHeightUpDown = new System.Windows.Forms.NumericUpDown();
            this.thicknessUpDown = new System.Windows.Forms.NumericUpDown();
            this.gradientColorChooseDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.unknownValueUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endOpacityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endBlueUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endRedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endGreenUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endHeightUpDown)).BeginInit();
            this.topGradientStartPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startOpacityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBlueUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startRedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startGreenUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startHeightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // unknownValueUpDown
            // 
            this.unknownValueUpDown.Location = new System.Drawing.Point(325, 3);
            this.unknownValueUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.unknownValueUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.unknownValueUpDown.Name = "unknownValueUpDown";
            this.unknownValueUpDown.Size = new System.Drawing.Size(74, 20);
            this.unknownValueUpDown.TabIndex = 1;
            this.unknownValueUpDown.ValueChanged += new System.EventHandler(this.destinationMultiplierUpDown_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Opacity Multiplier:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(213, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Background Multiplier:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chooseEndColorButton);
            this.groupBox4.Controls.Add(this.endOpacityUpDown);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.endBlueUpDown);
            this.groupBox4.Controls.Add(this.endRedUpDown);
            this.groupBox4.Controls.Add(this.endGreenUpDown);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.endHeightUpDown);
            this.groupBox4.Location = new System.Drawing.Point(216, 29);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 122);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "End";
            // 
            // chooseEndColorButton
            // 
            this.chooseEndColorButton.Location = new System.Drawing.Point(59, 67);
            this.chooseEndColorButton.Name = "chooseEndColorButton";
            this.chooseEndColorButton.Size = new System.Drawing.Size(80, 23);
            this.chooseEndColorButton.TabIndex = 4;
            this.chooseEndColorButton.Text = "Choose Color";
            this.chooseEndColorButton.UseVisualStyleBackColor = true;
            this.chooseEndColorButton.Click += new System.EventHandler(this.chooseEndColorButton_Click);
            // 
            // endOpacityUpDown
            // 
            this.endOpacityUpDown.DecimalPlaces = 2;
            this.endOpacityUpDown.Location = new System.Drawing.Point(59, 96);
            this.endOpacityUpDown.Name = "endOpacityUpDown";
            this.endOpacityUpDown.Size = new System.Drawing.Size(44, 20);
            this.endOpacityUpDown.TabIndex = 5;
            this.endOpacityUpDown.ValueChanged += new System.EventHandler(this.endOpacityUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Opacity: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Color:";
            // 
            // endBlueUpDown
            // 
            this.endBlueUpDown.DecimalPlaces = 2;
            this.endBlueUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.endBlueUpDown.Location = new System.Drawing.Point(159, 42);
            this.endBlueUpDown.Name = "endBlueUpDown";
            this.endBlueUpDown.Size = new System.Drawing.Size(44, 20);
            this.endBlueUpDown.TabIndex = 3;
            this.endBlueUpDown.ValueChanged += new System.EventHandler(this.endBlueUpDown_ValueChanged);
            // 
            // endRedUpDown
            // 
            this.endRedUpDown.DecimalPlaces = 2;
            this.endRedUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.endRedUpDown.Location = new System.Drawing.Point(59, 42);
            this.endRedUpDown.Name = "endRedUpDown";
            this.endRedUpDown.Size = new System.Drawing.Size(44, 20);
            this.endRedUpDown.TabIndex = 1;
            this.endRedUpDown.ValueChanged += new System.EventHandler(this.endRedUpDown_ValueChanged);
            // 
            // endGreenUpDown
            // 
            this.endGreenUpDown.DecimalPlaces = 2;
            this.endGreenUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.endGreenUpDown.Location = new System.Drawing.Point(109, 42);
            this.endGreenUpDown.Name = "endGreenUpDown";
            this.endGreenUpDown.Size = new System.Drawing.Size(44, 20);
            this.endGreenUpDown.TabIndex = 2;
            this.endGreenUpDown.ValueChanged += new System.EventHandler(this.endGreenUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Height:";
            // 
            // endHeightUpDown
            // 
            this.endHeightUpDown.DecimalPlaces = 2;
            this.endHeightUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.endHeightUpDown.Location = new System.Drawing.Point(59, 16);
            this.endHeightUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.endHeightUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.endHeightUpDown.Name = "endHeightUpDown";
            this.endHeightUpDown.Size = new System.Drawing.Size(80, 20);
            this.endHeightUpDown.TabIndex = 0;
            this.endHeightUpDown.ValueChanged += new System.EventHandler(this.endHeightUpDown_ValueChanged);
            // 
            // topGradientStartPanel
            // 
            this.topGradientStartPanel.Controls.Add(this.chooseStartColorButton);
            this.topGradientStartPanel.Controls.Add(this.startOpacityUpDown);
            this.topGradientStartPanel.Controls.Add(this.label10);
            this.topGradientStartPanel.Controls.Add(this.label9);
            this.topGradientStartPanel.Controls.Add(this.startBlueUpDown);
            this.topGradientStartPanel.Controls.Add(this.startRedUpDown);
            this.topGradientStartPanel.Controls.Add(this.startGreenUpDown);
            this.topGradientStartPanel.Controls.Add(this.label1);
            this.topGradientStartPanel.Controls.Add(this.startHeightUpDown);
            this.topGradientStartPanel.Location = new System.Drawing.Point(5, 29);
            this.topGradientStartPanel.Name = "topGradientStartPanel";
            this.topGradientStartPanel.Size = new System.Drawing.Size(205, 122);
            this.topGradientStartPanel.TabIndex = 2;
            this.topGradientStartPanel.TabStop = false;
            this.topGradientStartPanel.Text = "Start";
            // 
            // chooseStartColorButton
            // 
            this.chooseStartColorButton.Location = new System.Drawing.Point(53, 67);
            this.chooseStartColorButton.Name = "chooseStartColorButton";
            this.chooseStartColorButton.Size = new System.Drawing.Size(80, 23);
            this.chooseStartColorButton.TabIndex = 4;
            this.chooseStartColorButton.Text = "Choose Color";
            this.chooseStartColorButton.UseVisualStyleBackColor = true;
            this.chooseStartColorButton.Click += new System.EventHandler(this.chooseStartColorButton_Click);
            // 
            // startOpacityUpDown
            // 
            this.startOpacityUpDown.DecimalPlaces = 2;
            this.startOpacityUpDown.Location = new System.Drawing.Point(53, 96);
            this.startOpacityUpDown.Name = "startOpacityUpDown";
            this.startOpacityUpDown.Size = new System.Drawing.Size(44, 20);
            this.startOpacityUpDown.TabIndex = 5;
            this.startOpacityUpDown.ValueChanged += new System.EventHandler(this.startOpacityUpDown_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Opacity: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Color:";
            // 
            // startBlueUpDown
            // 
            this.startBlueUpDown.DecimalPlaces = 2;
            this.startBlueUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.startBlueUpDown.Location = new System.Drawing.Point(153, 42);
            this.startBlueUpDown.Name = "startBlueUpDown";
            this.startBlueUpDown.Size = new System.Drawing.Size(44, 20);
            this.startBlueUpDown.TabIndex = 3;
            this.startBlueUpDown.ValueChanged += new System.EventHandler(this.startBlueUpDown_ValueChanged);
            // 
            // startRedUpDown
            // 
            this.startRedUpDown.DecimalPlaces = 2;
            this.startRedUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.startRedUpDown.Location = new System.Drawing.Point(53, 42);
            this.startRedUpDown.Name = "startRedUpDown";
            this.startRedUpDown.Size = new System.Drawing.Size(44, 20);
            this.startRedUpDown.TabIndex = 1;
            this.startRedUpDown.ValueChanged += new System.EventHandler(this.startRedUpDown_ValueChanged);
            // 
            // startGreenUpDown
            // 
            this.startGreenUpDown.DecimalPlaces = 2;
            this.startGreenUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.startGreenUpDown.Location = new System.Drawing.Point(103, 42);
            this.startGreenUpDown.Name = "startGreenUpDown";
            this.startGreenUpDown.Size = new System.Drawing.Size(44, 20);
            this.startGreenUpDown.TabIndex = 2;
            this.startGreenUpDown.ValueChanged += new System.EventHandler(this.startGreenUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Height:";
            // 
            // startHeightUpDown
            // 
            this.startHeightUpDown.DecimalPlaces = 2;
            this.startHeightUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.startHeightUpDown.Location = new System.Drawing.Point(53, 16);
            this.startHeightUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.startHeightUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.startHeightUpDown.Name = "startHeightUpDown";
            this.startHeightUpDown.Size = new System.Drawing.Size(80, 20);
            this.startHeightUpDown.TabIndex = 0;
            this.startHeightUpDown.ValueChanged += new System.EventHandler(this.startHeightUpDown_ValueChanged);
            // 
            // thicknessUpDown
            // 
            this.thicknessUpDown.Location = new System.Drawing.Point(108, 3);
            this.thicknessUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.thicknessUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.thicknessUpDown.Name = "thicknessUpDown";
            this.thicknessUpDown.Size = new System.Drawing.Size(69, 20);
            this.thicknessUpDown.TabIndex = 0;
            this.thicknessUpDown.ValueChanged += new System.EventHandler(this.thicknessUpDown_ValueChanged);
            // 
            // GradientEditorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.unknownValueUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.topGradientStartPanel);
            this.Controls.Add(this.thicknessUpDown);
            this.MinimumSize = new System.Drawing.Size(432, 160);
            this.Name = "GradientEditorPanel";
            this.Size = new System.Drawing.Size(432, 160);
            ((System.ComponentModel.ISupportInitialize)(this.unknownValueUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endOpacityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endBlueUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endRedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endGreenUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endHeightUpDown)).EndInit();
            this.topGradientStartPanel.ResumeLayout(false);
            this.topGradientStartPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startOpacityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBlueUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startRedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startGreenUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startHeightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown unknownValueUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button chooseEndColorButton;
        private System.Windows.Forms.NumericUpDown endOpacityUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown endBlueUpDown;
        private System.Windows.Forms.NumericUpDown endRedUpDown;
        private System.Windows.Forms.NumericUpDown endGreenUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown endHeightUpDown;
        private System.Windows.Forms.GroupBox topGradientStartPanel;
        private System.Windows.Forms.Button chooseStartColorButton;
        private System.Windows.Forms.NumericUpDown startOpacityUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown startBlueUpDown;
        private System.Windows.Forms.NumericUpDown startRedUpDown;
        private System.Windows.Forms.NumericUpDown startGreenUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown startHeightUpDown;
        private System.Windows.Forms.NumericUpDown thicknessUpDown;
        private System.Windows.Forms.ColorDialog gradientColorChooseDialog;
    }
}
