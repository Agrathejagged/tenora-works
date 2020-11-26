using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace psu_generic_parser
{
    public partial class UnitParamViewer : UserControl
    {        
        ItemUnitParamFile internalFile;
        List<DataGridView> grids = new List<DataGridView>();
        public UnitParamViewer(ItemUnitParamFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            tabControl1.TabPages.Clear();
            string[] names = new string[] { "Head", "Arm", "Body", "Extra" };
            IList[] lists = new IList[4];
            lists[0] = internalFile.headUnits;
            lists[1] = internalFile.armUnits;
            lists[2] = internalFile.bodyUnits;
            lists[3] = internalFile.extraUnits;
            for (int i = 0; i < 4; i++)
            {
                TabPage temp;
                temp = new TabPage(names[i]);
                DataGridView tempGrid = new DataGridView(); 
                BindingSource binder = new BindingSource();
                binder.DataSource = lists[i];
                tempGrid.DataSource = binder;
                tempGrid.AutoGenerateColumns = true;
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
                tempGrid.AllowUserToAddRows = tempGrid.AllowUserToDeleteRows = true;
                tempGrid.AllowUserToOrderColumns = false;
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
