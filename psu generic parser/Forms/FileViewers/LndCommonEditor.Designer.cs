
namespace psu_generic_parser.Forms.FileViewers
{
    partial class LndCommonEditor
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
            this.nblTextBox = new System.Windows.Forms.TextBox();
            this.xnt1TextBox = new System.Windows.Forms.TextBox();
            this.xnt2TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.unknownValueUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.unknownValueUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // nblTextBox
            // 
            this.nblTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nblTextBox.Location = new System.Drawing.Point(95, 7);
            this.nblTextBox.Name = "nblTextBox";
            this.nblTextBox.Size = new System.Drawing.Size(192, 20);
            this.nblTextBox.TabIndex = 0;
            this.nblTextBox.TextChanged += new System.EventHandler(this.nblTextBox_TextChanged);
            // 
            // xnt1TextBox
            // 
            this.xnt1TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xnt1TextBox.Location = new System.Drawing.Point(95, 33);
            this.xnt1TextBox.Name = "xnt1TextBox";
            this.xnt1TextBox.Size = new System.Drawing.Size(192, 20);
            this.xnt1TextBox.TabIndex = 1;
            this.xnt1TextBox.TextChanged += new System.EventHandler(this.xnt1TextBox_TextChanged);
            // 
            // xnt2TextBox
            // 
            this.xnt2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xnt2TextBox.Location = new System.Drawing.Point(95, 59);
            this.xnt2TextBox.Name = "xnt2TextBox";
            this.xnt2TextBox.Size = new System.Drawing.Size(192, 20);
            this.xnt2TextBox.TabIndex = 2;
            this.xnt2TextBox.TextChanged += new System.EventHandler(this.xnt2TextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "NBL Filename:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "XNT Filename 1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "XNT Filename 2:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(293, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = ".xnt";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = ".xnt";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(293, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = ".nbl";
            // 
            // unknownValueUpDown
            // 
            this.unknownValueUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unknownValueUpDown.DecimalPlaces = 2;
            this.unknownValueUpDown.Location = new System.Drawing.Point(95, 86);
            this.unknownValueUpDown.Name = "unknownValueUpDown";
            this.unknownValueUpDown.Size = new System.Drawing.Size(192, 20);
            this.unknownValueUpDown.TabIndex = 9;
            this.unknownValueUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Float value:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(255, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "WARNING: DO NOT INCLUDE FILE EXTENSIONS";
            // 
            // LndCommonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.unknownValueUpDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xnt2TextBox);
            this.Controls.Add(this.xnt1TextBox);
            this.Controls.Add(this.nblTextBox);
            this.Name = "LndCommonEditor";
            this.Size = new System.Drawing.Size(334, 147);
            ((System.ComponentModel.ISupportInitialize)(this.unknownValueUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nblTextBox;
        private System.Windows.Forms.TextBox xnt1TextBox;
        private System.Windows.Forms.TextBox xnt2TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown unknownValueUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}
