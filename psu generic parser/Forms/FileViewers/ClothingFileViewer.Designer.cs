namespace psu_generic_parser
{
    partial class ClothingFileViewer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.topPage = new System.Windows.Forms.TabPage();
            this.bottomPage = new System.Windows.Forms.TabPage();
            this.shoePage = new System.Windows.Forms.TabPage();
            this.topSetPage = new System.Windows.Forms.TabPage();
            this.bottomSetPage = new System.Windows.Forms.TabPage();
            this.fullPage = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.dataGridView6 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.topPage.SuspendLayout();
            this.bottomPage.SuspendLayout();
            this.shoePage.SuspendLayout();
            this.topSetPage.SuspendLayout();
            this.bottomSetPage.SuspendLayout();
            this.fullPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.topPage);
            this.tabControl1.Controls.Add(this.bottomPage);
            this.tabControl1.Controls.Add(this.shoePage);
            this.tabControl1.Controls.Add(this.topSetPage);
            this.tabControl1.Controls.Add(this.bottomSetPage);
            this.tabControl1.Controls.Add(this.fullPage);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 271);
            this.tabControl1.TabIndex = 0;
            // 
            // topPage
            // 
            this.topPage.Controls.Add(this.dataGridView1);
            this.topPage.Location = new System.Drawing.Point(4, 22);
            this.topPage.Name = "topPage";
            this.topPage.Padding = new System.Windows.Forms.Padding(3);
            this.topPage.Size = new System.Drawing.Size(394, 245);
            this.topPage.TabIndex = 0;
            this.topPage.Text = "Tops";
            this.topPage.UseVisualStyleBackColor = true;
            // 
            // bottomPage
            // 
            this.bottomPage.Controls.Add(this.dataGridView2);
            this.bottomPage.Location = new System.Drawing.Point(4, 22);
            this.bottomPage.Name = "bottomPage";
            this.bottomPage.Padding = new System.Windows.Forms.Padding(3);
            this.bottomPage.Size = new System.Drawing.Size(394, 245);
            this.bottomPage.TabIndex = 1;
            this.bottomPage.Text = "Bottoms";
            this.bottomPage.UseVisualStyleBackColor = true;
            // 
            // shoePage
            // 
            this.shoePage.Controls.Add(this.dataGridView3);
            this.shoePage.Location = new System.Drawing.Point(4, 22);
            this.shoePage.Name = "shoePage";
            this.shoePage.Size = new System.Drawing.Size(394, 245);
            this.shoePage.TabIndex = 2;
            this.shoePage.Text = "Shoes";
            this.shoePage.UseVisualStyleBackColor = true;
            // 
            // topSetPage
            // 
            this.topSetPage.Controls.Add(this.dataGridView4);
            this.topSetPage.Location = new System.Drawing.Point(4, 22);
            this.topSetPage.Name = "topSetPage";
            this.topSetPage.Size = new System.Drawing.Size(394, 245);
            this.topSetPage.TabIndex = 3;
            this.topSetPage.Text = "Top+Bottoms";
            this.topSetPage.UseVisualStyleBackColor = true;
            // 
            // bottomSetPage
            // 
            this.bottomSetPage.Controls.Add(this.dataGridView5);
            this.bottomSetPage.Location = new System.Drawing.Point(4, 22);
            this.bottomSetPage.Name = "bottomSetPage";
            this.bottomSetPage.Size = new System.Drawing.Size(394, 245);
            this.bottomSetPage.TabIndex = 4;
            this.bottomSetPage.Text = "Bottom+Shoes";
            this.bottomSetPage.UseVisualStyleBackColor = true;
            // 
            // fullPage
            // 
            this.fullPage.Controls.Add(this.dataGridView6);
            this.fullPage.Location = new System.Drawing.Point(4, 22);
            this.fullPage.Name = "fullPage";
            this.fullPage.Size = new System.Drawing.Size(394, 245);
            this.fullPage.TabIndex = 5;
            this.fullPage.Text = "Full Sets";
            this.fullPage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(408, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.Location = new System.Drawing.Point(408, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(388, 239);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(388, 239);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(0, 0);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(394, 245);
            this.dataGridView3.TabIndex = 0;
            this.dataGridView3.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView4.Location = new System.Drawing.Point(0, 0);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.Size = new System.Drawing.Size(394, 245);
            this.dataGridView4.TabIndex = 0;
            this.dataGridView4.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // dataGridView5
            // 
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView5.Location = new System.Drawing.Point(0, 0);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.Size = new System.Drawing.Size(394, 245);
            this.dataGridView5.TabIndex = 0;
            this.dataGridView5.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // dataGridView6
            // 
            this.dataGridView6.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView6.Location = new System.Drawing.Point(0, 0);
            this.dataGridView6.Name = "dataGridView6";
            this.dataGridView6.Size = new System.Drawing.Size(394, 245);
            this.dataGridView6.TabIndex = 0;
            this.dataGridView6.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.numberRows);
            // 
            // ClothingFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "ClothingFileViewer";
            this.Size = new System.Drawing.Size(486, 278);
            this.tabControl1.ResumeLayout(false);
            this.topPage.ResumeLayout(false);
            this.bottomPage.ResumeLayout(false);
            this.shoePage.ResumeLayout(false);
            this.topSetPage.ResumeLayout(false);
            this.bottomSetPage.ResumeLayout(false);
            this.fullPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage topPage;
        private System.Windows.Forms.TabPage bottomPage;
        private System.Windows.Forms.TabPage shoePage;
        private System.Windows.Forms.TabPage topSetPage;
        private System.Windows.Forms.TabPage bottomSetPage;
        private System.Windows.Forms.TabPage fullPage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.DataGridView dataGridView6;
    }
}
