namespace psu_generic_parser
{
    partial class MainForm
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
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.standardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelectedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listAllObjparamsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBlobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableScriptParsingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllWeaponsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importAllWeaponsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNMLLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptNMLBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptNMLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureCatalogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.importDialog = new System.Windows.Forms.OpenFileDialog();
            this.setQuestButton = new System.Windows.Forms.Button();
            this.setZoneButton = new System.Windows.Forms.Button();
            this.addZoneButton = new System.Windows.Forms.Button();
            this.addFileButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.arbitraryFileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceFileTreeContextItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractSelectedTreeContextItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionProgressBar = new System.Windows.Forms.ProgressBar();
            this.progressStatusLabel = new System.Windows.Forms.Label();
            this.viewInHexButton = new System.Windows.Forms.Button();
            this.zoneUD = new System.Windows.Forms.NumericUpDown();
            this.AFSQuestItemsLabel = new System.Windows.Forms.Label();
            this.zoneLabel = new System.Windows.Forms.Label();
            this.afsNblFileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nblChunkContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.compressChunkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listAllMonsterLayoutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.arbitraryFileContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoneUD)).BeginInit();
            this.nblChunkContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardToolStripMenuItem,
            this.batchToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(827, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // standardToolStripMenuItem
            // 
            this.standardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.standardToolStripMenuItem.Name = "standardToolStripMenuItem";
            this.standardToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.standardToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSelectedFileToolStripMenuItem,
            this.exportAllToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportSelectedFileToolStripMenuItem
            // 
            this.exportSelectedFileToolStripMenuItem.Name = "exportSelectedFileToolStripMenuItem";
            this.exportSelectedFileToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exportSelectedFileToolStripMenuItem.Text = "Export Selected";
            this.exportSelectedFileToolStripMenuItem.Click += new System.EventHandler(this.exportSelectedToolStripMenuItem_Click);
            // 
            // exportAllToolStripMenuItem
            // 
            this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exportAllToolStripMenuItem.Text = "Export All";
            this.exportAllToolStripMenuItem.Click += new System.EventHandler(this.exportAllToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // batchToolStripMenuItem
            // 
            this.batchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractAllInFolderToolStripMenuItem,
            this.listAllObjparamsToolStripMenuItem,
            this.listAllMonsterLayoutsToolStripMenuItem});
            this.batchToolStripMenuItem.Name = "batchToolStripMenuItem";
            this.batchToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.batchToolStripMenuItem.Text = "Batch";
            // 
            // extractAllInFolderToolStripMenuItem
            // 
            this.extractAllInFolderToolStripMenuItem.Name = "extractAllInFolderToolStripMenuItem";
            this.extractAllInFolderToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.extractAllInFolderToolStripMenuItem.Text = "Extract All In Folder";
            this.extractAllInFolderToolStripMenuItem.Click += new System.EventHandler(this.extractAllInFolderToolStripMenuItem_Click);
            // 
            // listAllObjparamsToolStripMenuItem
            // 
            this.listAllObjparamsToolStripMenuItem.Name = "listAllObjparamsToolStripMenuItem";
            this.listAllObjparamsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.listAllObjparamsToolStripMenuItem.Text = "List all objparams";
            this.listAllObjparamsToolStripMenuItem.Click += new System.EventHandler(this.listAllObjparamsToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportBlobToolStripMenuItem,
            this.createAFSToolStripMenuItem,
            this.disableScriptParsingToolStripMenuItem,
            this.exportAllWeaponsToolStripMenuItem,
            this.importAllWeaponsToolStripMenuItem,
            this.insertNMLLFileToolStripMenuItem,
            this.decryptNMLBToolStripMenuItem,
            this.decryptNMLLToolStripMenuItem,
            this.textureCatalogueToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // exportBlobToolStripMenuItem
            // 
            this.exportBlobToolStripMenuItem.Name = "exportBlobToolStripMenuItem";
            this.exportBlobToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exportBlobToolStripMenuItem.Text = "Export blob";
            this.exportBlobToolStripMenuItem.Click += new System.EventHandler(this.exportBlob_Click);
            // 
            // createAFSToolStripMenuItem
            // 
            this.createAFSToolStripMenuItem.Name = "createAFSToolStripMenuItem";
            this.createAFSToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.createAFSToolStripMenuItem.Text = "Create AFS";
            this.createAFSToolStripMenuItem.Click += new System.EventHandler(this.createAFSToolStripMenuItem_Click);
            // 
            // disableScriptParsingToolStripMenuItem
            // 
            this.disableScriptParsingToolStripMenuItem.Name = "disableScriptParsingToolStripMenuItem";
            this.disableScriptParsingToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.disableScriptParsingToolStripMenuItem.Text = "Disable Script Parsing";
            this.disableScriptParsingToolStripMenuItem.Click += new System.EventHandler(this.disableScriptParsingToolStripMenuItem_Click);
            // 
            // exportAllWeaponsToolStripMenuItem
            // 
            this.exportAllWeaponsToolStripMenuItem.Name = "exportAllWeaponsToolStripMenuItem";
            this.exportAllWeaponsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exportAllWeaponsToolStripMenuItem.Text = "Export all weapons";
            this.exportAllWeaponsToolStripMenuItem.Click += new System.EventHandler(this.exportAllWeaponsToolStripMenuItem_Click);
            // 
            // importAllWeaponsToolStripMenuItem
            // 
            this.importAllWeaponsToolStripMenuItem.Name = "importAllWeaponsToolStripMenuItem";
            this.importAllWeaponsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.importAllWeaponsToolStripMenuItem.Text = "Import all weapons";
            this.importAllWeaponsToolStripMenuItem.Click += new System.EventHandler(this.importAllWeaponsToolStripMenuItem_Click);
            // 
            // insertNMLLFileToolStripMenuItem
            // 
            this.insertNMLLFileToolStripMenuItem.Name = "insertNMLLFileToolStripMenuItem";
            this.insertNMLLFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.insertNMLLFileToolStripMenuItem.Text = "Insert NMLL file";
            this.insertNMLLFileToolStripMenuItem.Click += new System.EventHandler(this.insertNMLLFileToolStripMenuItem_Click);
            // 
            // decryptNMLBToolStripMenuItem
            // 
            this.decryptNMLBToolStripMenuItem.Name = "decryptNMLBToolStripMenuItem";
            this.decryptNMLBToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.decryptNMLBToolStripMenuItem.Text = "Decrypt NMLB";
            this.decryptNMLBToolStripMenuItem.Click += new System.EventHandler(this.decryptNMLBToolStripMenuItem_Click);
            // 
            // decryptNMLLToolStripMenuItem
            // 
            this.decryptNMLLToolStripMenuItem.Name = "decryptNMLLToolStripMenuItem";
            this.decryptNMLLToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.decryptNMLLToolStripMenuItem.Text = "Decrypt NMLL";
            this.decryptNMLLToolStripMenuItem.Click += new System.EventHandler(this.decryptNMLLToolStripMenuItem_Click);
            // 
            // textureCatalogueToolStripMenuItem
            // 
            this.textureCatalogueToolStripMenuItem.Name = "textureCatalogueToolStripMenuItem";
            this.textureCatalogueToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.textureCatalogueToolStripMenuItem.Text = "Texture catalogue";
            this.textureCatalogueToolStripMenuItem.Click += new System.EventHandler(this.textureCatalogueToolStripMenuItem_Click);
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
            this.splitContainer1.Size = new System.Drawing.Size(802, 395);
            this.splitContainer1.SplitterDistance = 265;
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
            this.treeView1.Size = new System.Drawing.Size(262, 392);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // importDialog
            // 
            this.importDialog.FileName = "openFileDialog2";
            // 
            // setQuestButton
            // 
            this.setQuestButton.Location = new System.Drawing.Point(93, 25);
            this.setQuestButton.Name = "setQuestButton";
            this.setQuestButton.Size = new System.Drawing.Size(75, 23);
            this.setQuestButton.TabIndex = 16;
            this.setQuestButton.Text = "Set Quest";
            this.setQuestButton.UseVisualStyleBackColor = true;
            this.setQuestButton.Click += new System.EventHandler(this.setQuest_Click);
            // 
            // setZoneButton
            // 
            this.setZoneButton.Location = new System.Drawing.Point(93, 49);
            this.setZoneButton.Name = "setZoneButton";
            this.setZoneButton.Size = new System.Drawing.Size(75, 23);
            this.setZoneButton.TabIndex = 17;
            this.setZoneButton.Text = "Set Zone";
            this.setZoneButton.UseVisualStyleBackColor = true;
            this.setZoneButton.Click += new System.EventHandler(this.setZone_Click_1);
            // 
            // addZoneButton
            // 
            this.addZoneButton.Location = new System.Drawing.Point(174, 49);
            this.addZoneButton.Name = "addZoneButton";
            this.addZoneButton.Size = new System.Drawing.Size(75, 23);
            this.addZoneButton.TabIndex = 18;
            this.addZoneButton.Text = "Add Zone";
            this.addZoneButton.UseVisualStyleBackColor = true;
            this.addZoneButton.Click += new System.EventHandler(this.addZone_Click_1);
            // 
            // addFileButton
            // 
            this.addFileButton.Location = new System.Drawing.Point(174, 25);
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(75, 23);
            this.addFileButton.TabIndex = 21;
            this.addFileButton.Text = "Add File";
            this.addFileButton.UseVisualStyleBackColor = true;
            this.addFileButton.Click += new System.EventHandler(this.addFile_Click);
            // 
            // arbitraryFileContextMenuStrip
            // 
            this.arbitraryFileContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceFileTreeContextItem,
            this.extractSelectedTreeContextItem});
            this.arbitraryFileContextMenuStrip.Name = "treeViewContextMenu";
            this.arbitraryFileContextMenuStrip.ShowImageMargin = false;
            this.arbitraryFileContextMenuStrip.Size = new System.Drawing.Size(112, 48);
            // 
            // replaceFileTreeContextItem
            // 
            this.replaceFileTreeContextItem.Name = "replaceFileTreeContextItem";
            this.replaceFileTreeContextItem.Size = new System.Drawing.Size(111, 22);
            this.replaceFileTreeContextItem.Text = "Replace File";
            this.replaceFileTreeContextItem.Click += new System.EventHandler(this.replaceFileTreeContextItem_Click);
            // 
            // extractSelectedTreeContextItem
            // 
            this.extractSelectedTreeContextItem.Name = "extractSelectedTreeContextItem";
            this.extractSelectedTreeContextItem.Size = new System.Drawing.Size(111, 22);
            this.extractSelectedTreeContextItem.Text = "Extract File";
            this.extractSelectedTreeContextItem.Click += new System.EventHandler(this.extractFileTreeContextItem_Click);
            // 
            // actionProgressBar
            // 
            this.actionProgressBar.Location = new System.Drawing.Point(282, 53);
            this.actionProgressBar.Name = "actionProgressBar";
            this.actionProgressBar.Size = new System.Drawing.Size(289, 17);
            this.actionProgressBar.TabIndex = 22;
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.AutoSize = true;
            this.progressStatusLabel.Location = new System.Drawing.Point(280, 30);
            this.progressStatusLabel.Name = "progressStatusLabel";
            this.progressStatusLabel.Size = new System.Drawing.Size(54, 13);
            this.progressStatusLabel.TabIndex = 23;
            this.progressStatusLabel.Text = "Progress: ";
            // 
            // viewInHexButton
            // 
            this.viewInHexButton.Location = new System.Drawing.Point(575, 28);
            this.viewInHexButton.Name = "viewInHexButton";
            this.viewInHexButton.Size = new System.Drawing.Size(127, 23);
            this.viewInHexButton.TabIndex = 24;
            this.viewInHexButton.Text = "View Current File in Hex";
            this.viewInHexButton.UseVisualStyleBackColor = true;
            this.viewInHexButton.Click += new System.EventHandler(this.viewInHexButton_Click);
            // 
            // zoneUD
            // 
            this.zoneUD.Location = new System.Drawing.Point(50, 50);
            this.zoneUD.Name = "zoneUD";
            this.zoneUD.Size = new System.Drawing.Size(37, 20);
            this.zoneUD.TabIndex = 19;
            // 
            // AFSQuestItemsLabel
            // 
            this.AFSQuestItemsLabel.AutoSize = true;
            this.AFSQuestItemsLabel.Location = new System.Drawing.Point(5, 29);
            this.AFSQuestItemsLabel.Name = "AFSQuestItemsLabel";
            this.AFSQuestItemsLabel.Size = new System.Drawing.Size(85, 13);
            this.AFSQuestItemsLabel.TabIndex = 25;
            this.AFSQuestItemsLabel.Text = "AFS/quest items";
            // 
            // zoneLabel
            // 
            this.zoneLabel.AutoSize = true;
            this.zoneLabel.Location = new System.Drawing.Point(12, 52);
            this.zoneLabel.Name = "zoneLabel";
            this.zoneLabel.Size = new System.Drawing.Size(35, 13);
            this.zoneLabel.TabIndex = 26;
            this.zoneLabel.Text = "Zone:";
            // 
            // afsNblFileContextMenuStrip
            // 
            this.afsNblFileContextMenuStrip.Name = "afsNblFileContextMenuStrip";
            this.afsNblFileContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // nblChunkContextMenuStrip
            // 
            this.nblChunkContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressChunkToolStripMenuItem});
            this.nblChunkContextMenuStrip.Name = "nblChunkContextMenuStrip";
            this.nblChunkContextMenuStrip.Size = new System.Drawing.Size(166, 26);
            this.nblChunkContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.nblChunkContextMenuStrip_Opening);
            // 
            // compressChunkToolStripMenuItem
            // 
            this.compressChunkToolStripMenuItem.CheckOnClick = true;
            this.compressChunkToolStripMenuItem.Name = "compressChunkToolStripMenuItem";
            this.compressChunkToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.compressChunkToolStripMenuItem.Text = "Compress Chunk";
            this.compressChunkToolStripMenuItem.CheckedChanged += new System.EventHandler(this.compressChunkToolStripMenuItem_CheckedChanged);
            // 
            // listAllMonsterLayoutsToolStripMenuItem
            // 
            this.listAllMonsterLayoutsToolStripMenuItem.Name = "listAllMonsterLayoutsToolStripMenuItem";
            this.listAllMonsterLayoutsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.listAllMonsterLayoutsToolStripMenuItem.Text = "List all monster layouts";
            this.listAllMonsterLayoutsToolStripMenuItem.Click += new System.EventHandler(this.listAllMonsterLayoutsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 480);
            this.Controls.Add(this.zoneLabel);
            this.Controls.Add(this.AFSQuestItemsLabel);
            this.Controls.Add(this.viewInHexButton);
            this.Controls.Add(this.progressStatusLabel);
            this.Controls.Add(this.actionProgressBar);
            this.Controls.Add(this.addFileButton);
            this.Controls.Add(this.zoneUD);
            this.Controls.Add(this.addZoneButton);
            this.Controls.Add(this.setZoneButton);
            this.Controls.Add(this.setQuestButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "PSU Generic Parser build ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.arbitraryFileContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zoneUD)).EndInit();
            this.nblChunkContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem standardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBlobToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.OpenFileDialog importDialog;
        private System.Windows.Forms.Button setQuestButton;
        private System.Windows.Forms.Button setZoneButton;
        private System.Windows.Forms.Button addZoneButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button addFileButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem createAFSToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip arbitraryFileContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem replaceFileTreeContextItem;
        private System.Windows.Forms.ToolStripMenuItem extractSelectedTreeContextItem;
        private System.Windows.Forms.ToolStripMenuItem disableScriptParsingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllWeaponsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importAllWeaponsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertNMLLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptNMLBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptNMLLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureCatalogueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelectedFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractAllInFolderToolStripMenuItem;
        private System.Windows.Forms.ProgressBar actionProgressBar;
        private System.Windows.Forms.Label progressStatusLabel;
        private System.Windows.Forms.Button viewInHexButton;
        private System.Windows.Forms.NumericUpDown zoneUD;
        private System.Windows.Forms.Label AFSQuestItemsLabel;
        private System.Windows.Forms.Label zoneLabel;
        private System.Windows.Forms.ContextMenuStrip afsNblFileContextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip nblChunkContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem compressChunkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listAllObjparamsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listAllMonsterLayoutsToolStripMenuItem;
    }
}

