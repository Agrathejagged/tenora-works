
namespace psu_generic_parser.Forms.FileViewers
{
    partial class EnemyParamFileViewer
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mainStatsTabPage = new System.Windows.Forms.TabPage();
            this.buffsTabPage = new System.Windows.Forms.TabPage();
            this.hitboxesTabPage = new System.Windows.Forms.TabPage();
            this.unknownTabPage = new System.Windows.Forms.TabPage();
            this.attacksTabPage = new System.Windows.Forms.TabPage();
            this.buffDataGridView = new System.Windows.Forms.DataGridView();
            this.attackDataGridView = new System.Windows.Forms.DataGridView();
            this.hitboxDataGridView = new System.Windows.Forms.DataGridView();
            this.unknownDataSplitPane = new System.Windows.Forms.SplitContainer();
            this.unknownSub1DataGridView = new System.Windows.Forms.DataGridView();
            this.unknownSub2DataGridView = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.mainStatsTabPage.SuspendLayout();
            this.buffsTabPage.SuspendLayout();
            this.hitboxesTabPage.SuspendLayout();
            this.unknownTabPage.SuspendLayout();
            this.attacksTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buffDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hitboxDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknownDataSplitPane)).BeginInit();
            this.unknownDataSplitPane.Panel1.SuspendLayout();
            this.unknownDataSplitPane.Panel2.SuspendLayout();
            this.unknownDataSplitPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unknownSub1DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknownSub2DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(501, 269);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.mainStatsTabPage);
            this.tabControl1.Controls.Add(this.buffsTabPage);
            this.tabControl1.Controls.Add(this.attacksTabPage);
            this.tabControl1.Controls.Add(this.hitboxesTabPage);
            this.tabControl1.Controls.Add(this.unknownTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(515, 301);
            this.tabControl1.TabIndex = 1;
            // 
            // mainStatsTabPage
            // 
            this.mainStatsTabPage.Controls.Add(this.propertyGrid1);
            this.mainStatsTabPage.Location = new System.Drawing.Point(4, 22);
            this.mainStatsTabPage.Name = "mainStatsTabPage";
            this.mainStatsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainStatsTabPage.Size = new System.Drawing.Size(507, 275);
            this.mainStatsTabPage.TabIndex = 0;
            this.mainStatsTabPage.Text = "Base Stats";
            this.mainStatsTabPage.UseVisualStyleBackColor = true;
            // 
            // buffsTabPage
            // 
            this.buffsTabPage.Controls.Add(this.buffDataGridView);
            this.buffsTabPage.Location = new System.Drawing.Point(4, 22);
            this.buffsTabPage.Name = "buffsTabPage";
            this.buffsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.buffsTabPage.Size = new System.Drawing.Size(507, 275);
            this.buffsTabPage.TabIndex = 1;
            this.buffsTabPage.Text = "Buffs";
            this.buffsTabPage.UseVisualStyleBackColor = true;
            // 
            // hitboxesTabPage
            // 
            this.hitboxesTabPage.Controls.Add(this.hitboxDataGridView);
            this.hitboxesTabPage.Location = new System.Drawing.Point(4, 22);
            this.hitboxesTabPage.Name = "hitboxesTabPage";
            this.hitboxesTabPage.Size = new System.Drawing.Size(507, 275);
            this.hitboxesTabPage.TabIndex = 2;
            this.hitboxesTabPage.Text = "Hitboxes";
            this.hitboxesTabPage.UseVisualStyleBackColor = true;
            // 
            // unknownTabPage
            // 
            this.unknownTabPage.Controls.Add(this.unknownDataSplitPane);
            this.unknownTabPage.Location = new System.Drawing.Point(4, 22);
            this.unknownTabPage.Name = "unknownTabPage";
            this.unknownTabPage.Size = new System.Drawing.Size(507, 275);
            this.unknownTabPage.TabIndex = 3;
            this.unknownTabPage.Text = "Unknown Data";
            this.unknownTabPage.UseVisualStyleBackColor = true;
            // 
            // attacksTabPage
            // 
            this.attacksTabPage.Controls.Add(this.attackDataGridView);
            this.attacksTabPage.Location = new System.Drawing.Point(4, 22);
            this.attacksTabPage.Name = "attacksTabPage";
            this.attacksTabPage.Size = new System.Drawing.Size(507, 275);
            this.attacksTabPage.TabIndex = 4;
            this.attacksTabPage.Text = "Attacks";
            this.attacksTabPage.UseVisualStyleBackColor = true;
            // 
            // buffDataGridView
            // 
            this.buffDataGridView.AllowUserToAddRows = false;
            this.buffDataGridView.AllowUserToDeleteRows = false;
            this.buffDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.buffDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.buffDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buffDataGridView.Location = new System.Drawing.Point(3, 3);
            this.buffDataGridView.Name = "buffDataGridView";
            this.buffDataGridView.Size = new System.Drawing.Size(501, 269);
            this.buffDataGridView.TabIndex = 0;
            // 
            // attackDataGridView
            // 
            this.attackDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attackDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attackDataGridView.Location = new System.Drawing.Point(0, 0);
            this.attackDataGridView.Name = "attackDataGridView";
            this.attackDataGridView.Size = new System.Drawing.Size(507, 275);
            this.attackDataGridView.TabIndex = 0;
            // 
            // hitboxDataGridView
            // 
            this.hitboxDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hitboxDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hitboxDataGridView.Location = new System.Drawing.Point(0, 0);
            this.hitboxDataGridView.Name = "hitboxDataGridView";
            this.hitboxDataGridView.Size = new System.Drawing.Size(507, 275);
            this.hitboxDataGridView.TabIndex = 0;
            // 
            // unknownDataSplitPane
            // 
            this.unknownDataSplitPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unknownDataSplitPane.Location = new System.Drawing.Point(0, 0);
            this.unknownDataSplitPane.Name = "unknownDataSplitPane";
            this.unknownDataSplitPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // unknownDataSplitPane.Panel1
            // 
            this.unknownDataSplitPane.Panel1.Controls.Add(this.unknownSub1DataGridView);
            // 
            // unknownDataSplitPane.Panel2
            // 
            this.unknownDataSplitPane.Panel2.Controls.Add(this.unknownSub2DataGridView);
            this.unknownDataSplitPane.Size = new System.Drawing.Size(507, 275);
            this.unknownDataSplitPane.SplitterDistance = 105;
            this.unknownDataSplitPane.TabIndex = 0;
            // 
            // unknownSub1DataGridView
            // 
            this.unknownSub1DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unknownSub1DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unknownSub1DataGridView.Location = new System.Drawing.Point(0, 0);
            this.unknownSub1DataGridView.Name = "unknownSub1DataGridView";
            this.unknownSub1DataGridView.Size = new System.Drawing.Size(507, 105);
            this.unknownSub1DataGridView.TabIndex = 0;
            // 
            // unknownSub2DataGridView
            // 
            this.unknownSub2DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unknownSub2DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unknownSub2DataGridView.Location = new System.Drawing.Point(0, 0);
            this.unknownSub2DataGridView.Name = "unknownSub2DataGridView";
            this.unknownSub2DataGridView.Size = new System.Drawing.Size(507, 166);
            this.unknownSub2DataGridView.TabIndex = 0;
            // 
            // EnemyParamFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "EnemyParamFileViewer";
            this.Size = new System.Drawing.Size(515, 301);
            this.tabControl1.ResumeLayout(false);
            this.mainStatsTabPage.ResumeLayout(false);
            this.buffsTabPage.ResumeLayout(false);
            this.hitboxesTabPage.ResumeLayout(false);
            this.unknownTabPage.ResumeLayout(false);
            this.attacksTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buffDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attackDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hitboxDataGridView)).EndInit();
            this.unknownDataSplitPane.Panel1.ResumeLayout(false);
            this.unknownDataSplitPane.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.unknownDataSplitPane)).EndInit();
            this.unknownDataSplitPane.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.unknownSub1DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unknownSub2DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage mainStatsTabPage;
        private System.Windows.Forms.TabPage buffsTabPage;
        private System.Windows.Forms.DataGridView buffDataGridView;
        private System.Windows.Forms.TabPage attacksTabPage;
        private System.Windows.Forms.TabPage hitboxesTabPage;
        private System.Windows.Forms.TabPage unknownTabPage;
        private System.Windows.Forms.DataGridView attackDataGridView;
        private System.Windows.Forms.DataGridView hitboxDataGridView;
        private System.Windows.Forms.SplitContainer unknownDataSplitPane;
        private System.Windows.Forms.DataGridView unknownSub1DataGridView;
        private System.Windows.Forms.DataGridView unknownSub2DataGridView;
    }
}
