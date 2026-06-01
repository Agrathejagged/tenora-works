namespace Set_Object_Manager
{
    partial class MapEditorMainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            importObjectIntoMapToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            buildDatabaseToolStripMenuItem = new ToolStripMenuItem();
            compressResultingMissionsToolStripMenuItem = new ToolStripMenuItem();
            folderBrowserDialog1 = new FolderBrowserDialog();
            mapFileOpenDialog = new OpenFileDialog();
            objectFileOpenDialog = new OpenFileDialog();
            databaseFolderDialog = new FolderBrowserDialog();
            progressBar1 = new ProgressBar();
            statusPanel = new Panel();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            statusPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importObjectIntoMapToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // importObjectIntoMapToolStripMenuItem
            // 
            importObjectIntoMapToolStripMenuItem.Name = "importObjectIntoMapToolStripMenuItem";
            importObjectIntoMapToolStripMenuItem.Size = new Size(199, 22);
            importObjectIntoMapToolStripMenuItem.Text = "Import Object Into Map";
            importObjectIntoMapToolStripMenuItem.Click += importObjectIntoMapToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { buildDatabaseToolStripMenuItem, compressResultingMissionsToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // buildDatabaseToolStripMenuItem
            // 
            buildDatabaseToolStripMenuItem.Name = "buildDatabaseToolStripMenuItem";
            buildDatabaseToolStripMenuItem.Size = new Size(228, 22);
            buildDatabaseToolStripMenuItem.Text = "Build Database";
            buildDatabaseToolStripMenuItem.Click += buildDatabaseToolStripMenuItem_Click;
            // 
            // compressResultingMissionsToolStripMenuItem
            // 
            compressResultingMissionsToolStripMenuItem.Checked = true;
            compressResultingMissionsToolStripMenuItem.CheckOnClick = true;
            compressResultingMissionsToolStripMenuItem.CheckState = CheckState.Checked;
            compressResultingMissionsToolStripMenuItem.Name = "compressResultingMissionsToolStripMenuItem";
            compressResultingMissionsToolStripMenuItem.Size = new Size(228, 22);
            compressResultingMissionsToolStripMenuItem.Text = "Compress Resulting Missions";
            // 
            // mapFileOpenDialog
            // 
            mapFileOpenDialog.Title = "Select Map File";
            // 
            // objectFileOpenDialog
            // 
            objectFileOpenDialog.Multiselect = true;
            objectFileOpenDialog.Title = "Select Object NBL(s)";
            // 
            // databaseFolderDialog
            // 
            databaseFolderDialog.Description = "Select Destination Directory";
            databaseFolderDialog.UseDescriptionForTitle = true;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Right;
            progressBar1.Location = new Point(551, 0);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(249, 31);
            progressBar1.TabIndex = 1;
            // 
            // statusPanel
            // 
            statusPanel.Controls.Add(label1);
            statusPanel.Controls.Add(progressBar1);
            statusPanel.Dock = DockStyle.Bottom;
            statusPanel.Location = new Point(0, 419);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new Size(800, 31);
            statusPanel.TabIndex = 2;
            statusPanel.Visible = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(412, 7);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 2;
            label1.Text = "Processing...";
            // 
            // MapEditorMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusPanel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MapEditorMainForm";
            Text = "Set Object Model Manager";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusPanel.ResumeLayout(false);
            statusPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem buildDatabaseToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private ToolStripMenuItem importObjectIntoMapToolStripMenuItem;
        private OpenFileDialog mapFileOpenDialog;
        private OpenFileDialog objectFileOpenDialog;
        private ToolStripMenuItem compressResultingMissionsToolStripMenuItem;
        private FolderBrowserDialog databaseFolderDialog;
        private ProgressBar progressBar1;
        private Panel statusPanel;
        private Label label1;
    }
}