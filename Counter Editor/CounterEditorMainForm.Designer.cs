namespace Counter_Editor
{
    partial class CounterEditorMainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsNBLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsAFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purgeMissingMissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.questContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeMissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceNBLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markDirtyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.categoryContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertMissionDialog = new System.Windows.Forms.OpenFileDialog();
            this.importDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.questContextMenu.SuspendLayout();
            this.categoryContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(562, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newCounterToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsNBLToolStripMenuItem,
            this.saveAsPackToolStripMenuItem,
            this.saveAsAFSToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newCounterToolStripMenuItem
            // 
            this.newCounterToolStripMenuItem.Name = "newCounterToolStripMenuItem";
            this.newCounterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newCounterToolStripMenuItem.Text = "New Counter";
            this.newCounterToolStripMenuItem.Click += new System.EventHandler(this.newCounterToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openCounterToolStripMenuItem_Click);
            // 
            // saveAsNBLToolStripMenuItem
            // 
            this.saveAsNBLToolStripMenuItem.Enabled = false;
            this.saveAsNBLToolStripMenuItem.Name = "saveAsNBLToolStripMenuItem";
            this.saveAsNBLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsNBLToolStripMenuItem.Text = "Save as NBL...";
            this.saveAsNBLToolStripMenuItem.Click += new System.EventHandler(this.saveAsNBLToolStripMenuItem_Click);
            // 
            // saveAsPackToolStripMenuItem
            // 
            this.saveAsPackToolStripMenuItem.Enabled = false;
            this.saveAsPackToolStripMenuItem.Name = "saveAsPackToolStripMenuItem";
            this.saveAsPackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsPackToolStripMenuItem.Text = "Save as pack...";
            this.saveAsPackToolStripMenuItem.Click += new System.EventHandler(this.saveAsPackToolStripMenuItem1_Click);
            // 
            // saveAsAFSToolStripMenuItem
            // 
            this.saveAsAFSToolStripMenuItem.Enabled = false;
            this.saveAsAFSToolStripMenuItem.Name = "saveAsAFSToolStripMenuItem";
            this.saveAsAFSToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsAFSToolStripMenuItem.Text = "Save as AFS...";
            this.saveAsAFSToolStripMenuItem.Click += new System.EventHandler(this.saveAsAFSToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.purgeMissingMissionsToolStripMenuItem,
            this.newCategoryToolStripMenuItem,
            this.importDirectoryToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // purgeMissingMissionsToolStripMenuItem
            // 
            this.purgeMissingMissionsToolStripMenuItem.Name = "purgeMissingMissionsToolStripMenuItem";
            this.purgeMissingMissionsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.purgeMissingMissionsToolStripMenuItem.Text = "Purge missing missions";
            this.purgeMissingMissionsToolStripMenuItem.Click += new System.EventHandler(this.purgeMissingMissionsToolStripMenuItem_Click);
            // 
            // newCategoryToolStripMenuItem
            // 
            this.newCategoryToolStripMenuItem.Name = "newCategoryToolStripMenuItem";
            this.newCategoryToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.newCategoryToolStripMenuItem.Text = "New Category";
            this.newCategoryToolStripMenuItem.Click += new System.EventHandler(this.newCategoryToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(562, 285);
            this.splitContainer1.SplitterDistance = 187;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(187, 285);
            this.treeView1.TabIndex = 0;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // questContextMenu
            // 
            this.questContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeMissionToolStripMenuItem,
            this.replaceNBLToolStripMenuItem,
            this.markDirtyToolStripMenuItem});
            this.questContextMenu.Name = "questContextMenu";
            this.questContextMenu.Size = new System.Drawing.Size(162, 70);
            // 
            // removeMissionToolStripMenuItem
            // 
            this.removeMissionToolStripMenuItem.Name = "removeMissionToolStripMenuItem";
            this.removeMissionToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.removeMissionToolStripMenuItem.Text = "Remove Mission";
            this.removeMissionToolStripMenuItem.Click += new System.EventHandler(this.removeMissionToolStripMenuItem_Click);
            // 
            // replaceNBLToolStripMenuItem
            // 
            this.replaceNBLToolStripMenuItem.Name = "replaceNBLToolStripMenuItem";
            this.replaceNBLToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.replaceNBLToolStripMenuItem.Text = "Replace NBL";
            this.replaceNBLToolStripMenuItem.Click += new System.EventHandler(this.replaceNBLToolStripMenuItem_Click);
            // 
            // markDirtyToolStripMenuItem
            // 
            this.markDirtyToolStripMenuItem.Name = "markDirtyToolStripMenuItem";
            this.markDirtyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.markDirtyToolStripMenuItem.Text = "Mark dirty";
            this.markDirtyToolStripMenuItem.Click += new System.EventHandler(this.markDirtyToolStripMenuItem_Click);
            // 
            // categoryContextMenu
            // 
            this.categoryContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMissionToolStripMenuItem,
            this.removeCategoryToolStripMenuItem});
            this.categoryContextMenu.Name = "categoryContextMenu";
            this.categoryContextMenu.Size = new System.Drawing.Size(169, 48);
            // 
            // addMissionToolStripMenuItem
            // 
            this.addMissionToolStripMenuItem.Name = "addMissionToolStripMenuItem";
            this.addMissionToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.addMissionToolStripMenuItem.Text = "Add Mission";
            this.addMissionToolStripMenuItem.Click += new System.EventHandler(this.addMissionToolStripMenuItem_Click);
            // 
            // removeCategoryToolStripMenuItem
            // 
            this.removeCategoryToolStripMenuItem.Name = "removeCategoryToolStripMenuItem";
            this.removeCategoryToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.removeCategoryToolStripMenuItem.Text = "Remove Category";
            this.removeCategoryToolStripMenuItem.Click += new System.EventHandler(this.removeCategoryToolStripMenuItem_Click);
            // 
            // insertMissionDialog
            // 
            this.insertMissionDialog.Filter = "NBL Archive|*.nbl";
            // 
            // importDirectoryToolStripMenuItem
            // 
            this.importDirectoryToolStripMenuItem.Name = "importDirectoryToolStripMenuItem";
            this.importDirectoryToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.importDirectoryToolStripMenuItem.Text = "Import Directory";
            this.importDirectoryToolStripMenuItem.Click += new System.EventHandler(this.importDirectoryToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 309);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Counter Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.questContextMenu.ResumeLayout(false);
            this.categoryContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsNBLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purgeMissingMissionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip questContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeMissionToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip categoryContextMenu;
        private System.Windows.Forms.ToolStripMenuItem newCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMissionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeCategoryToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog insertMissionDialog;
        private System.Windows.Forms.ToolStripMenuItem replaceNBLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markDirtyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsAFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDirectoryToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

