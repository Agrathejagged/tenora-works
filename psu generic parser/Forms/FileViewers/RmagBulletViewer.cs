using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PSULib.FileClasses.Items;

namespace psu_generic_parser
{
    public partial class RmagBulletViewer : UserControl
    {
        RmagBulletParamFile internalFile;
        List<DataGridView> grids = new List<DataGridView>();
        public RmagBulletViewer(RmagBulletParamFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            tabControl1.TabPages.Clear();
            for (int i = 0; i < internalFile.allBullets.Length; i++)
            {
                TabPage temp;
                string brand = "";
                switch (i)
                {
                    case 0: brand = "GRM"; break;
                    case 1: brand = "Yohmei"; break;
                    case 2: brand = "Tenora"; break;
                    case 3: brand = "Kubara"; break;
                }
                temp = new TabPage(brand);
                DataGridView tempGrid = new DataGridView();
                tempGrid.AutoGenerateColumns = true;
                BindingSource binder = new BindingSource();
                binder.DataSource = internalFile.allBullets[i];
                tempGrid.DataSource = binder;
                grids.Add(tempGrid);
                temp.Controls.Add(tempGrid);
                tempGrid.Dock = DockStyle.Fill;
                tabControl1.TabPages.Add(temp);
                for (int j = 0; j < tempGrid.ColumnCount; j++)
                {
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    int tempWidth = Math.Min(tempGrid.Columns[j].Width, 32);
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    tempGrid.Columns[j].Width = tempWidth;
                }
                tempGrid.VirtualMode = false;
                tempGrid.AllowUserToAddRows = true;
                tempGrid.AllowUserToDeleteRows = tempGrid.AllowUserToOrderColumns = false;
            }
            for (int i = 0; i < 1; i++) //Just for scoping.
            {
                TabPage temp = new TabPage("Status effects");
                DataGridView tempGrid = new DataGridView();
                tempGrid.AutoGenerateColumns = true;
                BindingSource binder = new BindingSource();
                binder.DataSource = internalFile.statusEffects;
                tempGrid.DataSource = binder;
                grids.Add(tempGrid);
                temp.Controls.Add(tempGrid);
                tempGrid.Dock = DockStyle.Fill;
                tabControl1.TabPages.Add(temp);
                for (int j = 0; j < tempGrid.ColumnCount; j++)
                {
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    int tempWidth = Math.Min(tempGrid.Columns[j].Width, 32);
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    tempGrid.Columns[j].Width = tempWidth;
                }
                tempGrid.VirtualMode = false;
                tempGrid.AllowUserToAddRows = true;
                tempGrid.AllowUserToDeleteRows = tempGrid.AllowUserToOrderColumns = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView tempGrid = grids[tabControl1.SelectedIndex];
            for (int j = 0; j < tempGrid.ColumnCount; j++)
            {
                tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                int tempWidth = Math.Min(tempGrid.Columns[j].Width, 32);
                tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                tempGrid.Columns[j].Width = tempWidth;
            }
        }
    }
}
