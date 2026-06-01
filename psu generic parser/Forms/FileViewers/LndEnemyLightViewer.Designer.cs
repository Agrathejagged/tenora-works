
namespace psu_generic_parser.Forms.FileViewers
{
    partial class LndEnemyLightViewer
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
            this.lightGroupBox3 = new System.Windows.Forms.GroupBox();
            this.lightEditorPanel3 = new psu_generic_parser.Forms.FileViewers.LndEffects.LightEditorPanel();
            this.lightGroupBox2 = new System.Windows.Forms.GroupBox();
            this.lightEditorPanel2 = new psu_generic_parser.Forms.FileViewers.LndEffects.LightEditorPanel();
            this.lightGroupBox1 = new System.Windows.Forms.GroupBox();
            this.lightEditorPanel1 = new psu_generic_parser.Forms.FileViewers.LndEffects.LightEditorPanel();
            this.lightGroupBox3.SuspendLayout();
            this.lightGroupBox2.SuspendLayout();
            this.lightGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lightGroupBox3
            // 
            this.lightGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lightGroupBox3.Controls.Add(this.lightEditorPanel3);
            this.lightGroupBox3.Location = new System.Drawing.Point(2, 301);
            this.lightGroupBox3.Name = "lightGroupBox3";
            this.lightGroupBox3.Size = new System.Drawing.Size(544, 143);
            this.lightGroupBox3.TabIndex = 2;
            this.lightGroupBox3.TabStop = false;
            this.lightGroupBox3.Text = "Light 3 (Ambient)";
            // 
            // lightEditorPanel3
            // 
            this.lightEditorPanel3.Light = null;
            this.lightEditorPanel3.Location = new System.Drawing.Point(3, 16);
            this.lightEditorPanel3.Name = "lightEditorPanel3";
            this.lightEditorPanel3.Size = new System.Drawing.Size(538, 124);
            this.lightEditorPanel3.TabIndex = 2;
            // 
            // lightGroupBox2
            // 
            this.lightGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lightGroupBox2.Controls.Add(this.lightEditorPanel2);
            this.lightGroupBox2.Location = new System.Drawing.Point(3, 152);
            this.lightGroupBox2.Name = "lightGroupBox2";
            this.lightGroupBox2.Size = new System.Drawing.Size(543, 143);
            this.lightGroupBox2.TabIndex = 1;
            this.lightGroupBox2.TabStop = false;
            this.lightGroupBox2.Text = "Light 2";
            // 
            // lightEditorPanel2
            // 
            this.lightEditorPanel2.Light = null;
            this.lightEditorPanel2.Location = new System.Drawing.Point(3, 16);
            this.lightEditorPanel2.Name = "lightEditorPanel2";
            this.lightEditorPanel2.Size = new System.Drawing.Size(537, 124);
            this.lightEditorPanel2.TabIndex = 1;
            // 
            // lightGroupBox1
            // 
            this.lightGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lightGroupBox1.Controls.Add(this.lightEditorPanel1);
            this.lightGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.lightGroupBox1.Name = "lightGroupBox1";
            this.lightGroupBox1.Size = new System.Drawing.Size(543, 143);
            this.lightGroupBox1.TabIndex = 0;
            this.lightGroupBox1.TabStop = false;
            this.lightGroupBox1.Text = "Light 1";
            // 
            // lightEditorPanel1
            // 
            this.lightEditorPanel1.Light = null;
            this.lightEditorPanel1.Location = new System.Drawing.Point(3, 16);
            this.lightEditorPanel1.Name = "lightEditorPanel1";
            this.lightEditorPanel1.Size = new System.Drawing.Size(537, 124);
            this.lightEditorPanel1.TabIndex = 0;
            // 
            // LndEnemyLightViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lightGroupBox3);
            this.Controls.Add(this.lightGroupBox2);
            this.Controls.Add(this.lightGroupBox1);
            this.Name = "LndEnemyLightViewer";
            this.Size = new System.Drawing.Size(549, 447);
            this.lightGroupBox3.ResumeLayout(false);
            this.lightGroupBox2.ResumeLayout(false);
            this.lightGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox lightGroupBox3;
        private LndEffects.LightEditorPanel lightEditorPanel3;
        private System.Windows.Forms.GroupBox lightGroupBox2;
        private LndEffects.LightEditorPanel lightEditorPanel2;
        private System.Windows.Forms.GroupBox lightGroupBox1;
        private LndEffects.LightEditorPanel lightEditorPanel1;
    }
}
