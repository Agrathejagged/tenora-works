using System;
using System.IO;

namespace psu_generic_parser
{
    partial class HexEditForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.hexEditor = new WpfHexaEditor.HexEditor();
            this.toolStripBar1 = new System.Windows.Forms.ToolStrip();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.toggleReadOnlyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.elementHost);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(868, 387);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(868, 437);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripBar1);
            // 
            // elementHost
            // 
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(868, 387);
            this.elementHost.TabIndex = 3;
            this.elementHost.Text = "elementHost";
            this.elementHost.Child = this.hexEditor;
            // 
            // toolStripBar1
            // 
            this.toolStripBar1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripBar1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveButton,
            this.toggleReadOnlyButton});
            this.toolStripBar1.Location = new System.Drawing.Point(3, 0);
            this.toolStripBar1.Name = "toolStripBar1";
            this.toolStripBar1.Size = new System.Drawing.Size(150, 25);
            this.toolStripBar1.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(35, 22);
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toggleReadOnlyButton
            // 
            this.toggleReadOnlyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toggleReadOnlyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleReadOnlyButton.Name = "toggleReadOnlyButton";
            this.toggleReadOnlyButton.Size = new System.Drawing.Size(103, 22);
            this.toggleReadOnlyButton.Text = "Toggle Read Only";
            this.toggleReadOnlyButton.Click += new System.EventHandler(this.toggleReadOnlyButton_Click);
            // 
            // HexEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 437);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "HexEditForm";
            this.Text = "Wpf HexEditor WinForm sample";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripBar1.ResumeLayout(false);
            this.toolStripBar1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private WpfHexaEditor.HexEditor hexEditor;
        private System.Windows.Forms.ToolStrip toolStripBar1;
        private System.Windows.Forms.ToolStripButton toggleReadOnlyButton;
        private System.Windows.Forms.ToolStripButton saveButton;
    }
}