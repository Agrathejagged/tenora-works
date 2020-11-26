using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class ItemBulletParamViewer : UserControl
    {
        ItemBulletParamFile internalFile;
        List<DataGridView> grids = new List<DataGridView>();
        public ItemBulletParamViewer(ItemBulletParamFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            tabControl1.TabPages.Clear();
            for (int i = 0; i < internalFile.allBullets.Length; i++)
            {
                TabPage temp;
                if(i == 0)
                    temp = new TabPage("Default");
                else
                    temp = new TabPage("Bullet " + (i - 1));
                DataGridView tempGrid = new DataGridView();
                tempGrid.AutoGenerateColumns = true;
                tempGrid.DataSource = internalFile.allBullets[i];
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
                tempGrid.AllowUserToAddRows = tempGrid.AllowUserToDeleteRows = tempGrid.AllowUserToOrderColumns = false;
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
