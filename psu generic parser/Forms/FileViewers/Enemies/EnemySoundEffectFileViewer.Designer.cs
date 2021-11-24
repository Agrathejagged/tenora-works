
namespace psu_generic_parser.Forms.FileViewers.Enemies
{
    partial class EnemySoundEffectFileViewer
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
            this.soundEffectDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.soundEffectDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.soundEffectDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.soundEffectDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soundEffectDataGrid.Location = new System.Drawing.Point(0, 0);
            this.soundEffectDataGrid.Name = "dataGridView1";
            this.soundEffectDataGrid.Size = new System.Drawing.Size(476, 323);
            this.soundEffectDataGrid.TabIndex = 0;
            // 
            // SeDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.soundEffectDataGrid);
            this.Name = "SeDataViewer";
            this.Size = new System.Drawing.Size(476, 323);
            ((System.ComponentModel.ISupportInitialize)(this.soundEffectDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView soundEffectDataGrid;
    }
}
