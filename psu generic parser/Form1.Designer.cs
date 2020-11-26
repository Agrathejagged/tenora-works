namespace psu_generic_parser
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.standardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createMissionAFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBlobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTMLLBlobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTMLLFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testCompressorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCurrentFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableScriptParsingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllWeaponsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importAllWeaponsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNMLLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptNMLBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptNMLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableNMLLLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.importDialog = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.treeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureCatalogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.treeViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(434, 27);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Import test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 27);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "Compress NMLL";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(575, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // standardToolStripMenuItem
            // 
            this.standardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.createMissionAFSToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.standardToolStripMenuItem.Name = "standardToolStripMenuItem";
            this.standardToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.standardToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // createMissionAFSToolStripMenuItem
            // 
            this.createMissionAFSToolStripMenuItem.Name = "createMissionAFSToolStripMenuItem";
            this.createMissionAFSToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.createMissionAFSToolStripMenuItem.Text = "Create Mission AFS";
            this.createMissionAFSToolStripMenuItem.Click += new System.EventHandler(this.createMissionAFSToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportBlobToolStripMenuItem,
            this.exportTMLLBlobToolStripMenuItem,
            this.exportTMLLFilesToolStripMenuItem,
            this.testCompressorToolStripMenuItem,
            this.exportCurrentFileToolStripMenuItem,
            this.createAFSToolStripMenuItem,
            this.disableScriptParsingToolStripMenuItem,
            this.exportAllWeaponsToolStripMenuItem,
            this.importAllWeaponsToolStripMenuItem,
            this.insertNMLLFileToolStripMenuItem,
            this.decryptNMLBToolStripMenuItem,
            this.decryptNMLLToolStripMenuItem,
            this.enableNMLLLoggingToolStripMenuItem,
            this.textureCatalogueToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // exportBlobToolStripMenuItem
            // 
            this.exportBlobToolStripMenuItem.Name = "exportBlobToolStripMenuItem";
            this.exportBlobToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportBlobToolStripMenuItem.Text = "Export blob";
            this.exportBlobToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // exportTMLLBlobToolStripMenuItem
            // 
            this.exportTMLLBlobToolStripMenuItem.Name = "exportTMLLBlobToolStripMenuItem";
            this.exportTMLLBlobToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportTMLLBlobToolStripMenuItem.Text = "Export TMLL blob";
            this.exportTMLLBlobToolStripMenuItem.Click += new System.EventHandler(this.exportTMLLBlobToolStripMenuItem_Click);
            // 
            // exportTMLLFilesToolStripMenuItem
            // 
            this.exportTMLLFilesToolStripMenuItem.Name = "exportTMLLFilesToolStripMenuItem";
            this.exportTMLLFilesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportTMLLFilesToolStripMenuItem.Text = "Export TMLL files";
            this.exportTMLLFilesToolStripMenuItem.Click += new System.EventHandler(this.button5_Click);
            // 
            // testCompressorToolStripMenuItem
            // 
            this.testCompressorToolStripMenuItem.Name = "testCompressorToolStripMenuItem";
            this.testCompressorToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.testCompressorToolStripMenuItem.Text = "Test compressor";
            this.testCompressorToolStripMenuItem.Click += new System.EventHandler(this.button6_Click);
            // 
            // exportCurrentFileToolStripMenuItem
            // 
            this.exportCurrentFileToolStripMenuItem.Name = "exportCurrentFileToolStripMenuItem";
            this.exportCurrentFileToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportCurrentFileToolStripMenuItem.Text = "Export current file";
            this.exportCurrentFileToolStripMenuItem.Click += new System.EventHandler(this.exportCurrentFileToolStripMenuItem_Click);
            // 
            // createAFSToolStripMenuItem
            // 
            this.createAFSToolStripMenuItem.Name = "createAFSToolStripMenuItem";
            this.createAFSToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.createAFSToolStripMenuItem.Text = "Create AFS";
            this.createAFSToolStripMenuItem.Click += new System.EventHandler(this.createAFSToolStripMenuItem_Click);
            // 
            // disableScriptParsingToolStripMenuItem
            // 
            this.disableScriptParsingToolStripMenuItem.Name = "disableScriptParsingToolStripMenuItem";
            this.disableScriptParsingToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.disableScriptParsingToolStripMenuItem.Text = "Disable Script Parsing";
            this.disableScriptParsingToolStripMenuItem.Click += new System.EventHandler(this.disableScriptParsingToolStripMenuItem_Click);
            // 
            // exportAllWeaponsToolStripMenuItem
            // 
            this.exportAllWeaponsToolStripMenuItem.Name = "exportAllWeaponsToolStripMenuItem";
            this.exportAllWeaponsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportAllWeaponsToolStripMenuItem.Text = "Export all weapons";
            this.exportAllWeaponsToolStripMenuItem.Click += new System.EventHandler(this.exportAllWeaponsToolStripMenuItem_Click);
            // 
            // importAllWeaponsToolStripMenuItem
            // 
            this.importAllWeaponsToolStripMenuItem.Name = "importAllWeaponsToolStripMenuItem";
            this.importAllWeaponsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.importAllWeaponsToolStripMenuItem.Text = "Import all weapons";
            this.importAllWeaponsToolStripMenuItem.Click += new System.EventHandler(this.importAllWeaponsToolStripMenuItem_Click);
            // 
            // insertNMLLFileToolStripMenuItem
            // 
            this.insertNMLLFileToolStripMenuItem.Name = "insertNMLLFileToolStripMenuItem";
            this.insertNMLLFileToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.insertNMLLFileToolStripMenuItem.Text = "Insert NMLL file";
            this.insertNMLLFileToolStripMenuItem.Click += new System.EventHandler(this.insertNMLLFileToolStripMenuItem_Click);
            // 
            // decryptNMLBToolStripMenuItem
            // 
            this.decryptNMLBToolStripMenuItem.Name = "decryptNMLBToolStripMenuItem";
            this.decryptNMLBToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.decryptNMLBToolStripMenuItem.Text = "Decrypt NMLB";
            this.decryptNMLBToolStripMenuItem.Click += new System.EventHandler(this.decryptNMLBToolStripMenuItem_Click);
            // 
            // decryptNMLLToolStripMenuItem
            // 
            this.decryptNMLLToolStripMenuItem.Name = "decryptNMLLToolStripMenuItem";
            this.decryptNMLLToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.decryptNMLLToolStripMenuItem.Text = "Decrypt NMLL";
            this.decryptNMLLToolStripMenuItem.Click += new System.EventHandler(this.decryptNMLLToolStripMenuItem_Click);
            // 
            // enableNMLLLoggingToolStripMenuItem
            // 
            this.enableNMLLLoggingToolStripMenuItem.CheckOnClick = true;
            this.enableNMLLLoggingToolStripMenuItem.Name = "enableNMLLLoggingToolStripMenuItem";
            this.enableNMLLLoggingToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.enableNMLLLoggingToolStripMenuItem.Text = "Enable NMLL logging";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 50);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(103, 17);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Compress TMLL";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 73);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(550, 395);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 14;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(183, 392);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(181, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Replace File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // importDialog
            // 
            this.importDialog.FileName = "openFileDialog2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(263, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Set Quest";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(263, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "Set Zone";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(345, 46);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 18;
            this.button5.Text = "Add Zone";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(220, 47);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown1.TabIndex = 19;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(434, 45);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 20;
            this.button6.Text = "To text";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(345, 17);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 21;
            this.button7.Text = "Add File";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // treeViewContextMenu
            // 
            this.treeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceFileToolStripMenuItem});
            this.treeViewContextMenu.Name = "treeViewContextMenu";
            this.treeViewContextMenu.ShowImageMargin = false;
            this.treeViewContextMenu.Size = new System.Drawing.Size(112, 26);
            // 
            // replaceFileToolStripMenuItem
            // 
            this.replaceFileToolStripMenuItem.Name = "replaceFileToolStripMenuItem";
            this.replaceFileToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.replaceFileToolStripMenuItem.Text = "Replace File";
            this.replaceFileToolStripMenuItem.Click += new System.EventHandler(this.replaceFileToolStripMenuItem_Click);
            // 
            // textureCatalogueToolStripMenuItem
            // 
            this.textureCatalogueToolStripMenuItem.Name = "textureCatalogueToolStripMenuItem";
            this.textureCatalogueToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.textureCatalogueToolStripMenuItem.Text = "Texture catalogue";
            this.textureCatalogueToolStripMenuItem.Click += new System.EventHandler(this.textureCatalogueToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 480);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "PSU Generic Parser build ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.treeViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem standardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBlobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportTMLLFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testCompressorToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem exportTMLLBlobToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog importDialog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ToolStripMenuItem exportCurrentFileToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem createAFSToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip treeViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem replaceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableScriptParsingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllWeaponsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importAllWeaponsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createMissionAFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertNMLLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptNMLBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptNMLLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableNMLLLoggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureCatalogueToolStripMenuItem;
    }
}

