using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PSULib.FileClasses.Items;

namespace psu_generic_parser
{
    public partial class ClothingFileViewer : UserControl
    {
        ItemSuitParamFile internalFile;
        DataGridView[] clothesViews = new DataGridView[6];
        bool updatingTable = false;

        public ClothingFileViewer(ItemSuitParamFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            clothesViews[0] = dataGridView1;
            clothesViews[1] = dataGridView2;
            clothesViews[2] = dataGridView3;
            clothesViews[3] = dataGridView4;
            clothesViews[4] = dataGridView5;
            clothesViews[5] = dataGridView6;
            for (int i = 0; i < 6; i++)
            {
                //weaponsViews[i] = new DataGridView();
                clothesViews[i].AutoGenerateColumns = true;
                clothesViews[i].DataSource = internalFile.clothes[i];
                clothesViews[i].AllowUserToAddRows = clothesViews[i].AllowUserToDeleteRows = clothesViews[i].AllowUserToOrderColumns = false;
                clothesViews[i].VirtualMode = false;
                for (int j = 0; j < clothesViews[i].Columns.Count; j++)
                {
                    clothesViews[i].Columns[j].MinimumWidth = 32;
                }
                clothesViews[i].CellValueChanged += new DataGridViewCellEventHandler(weaponGrid_CellValueChanged);
            }
        }

        private void weaponGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!updatingTable)
            {
                DataGridView temp = (DataGridView)sender;
                if (e.ColumnIndex != -1 && temp.Columns[e.ColumnIndex].DataPropertyName == "IsValid")
                {
                    if ((bool)(((DataGridViewCheckBoxCell)temp[e.ColumnIndex, e.RowIndex]).Value) == true)
                    {
                        temp["WeaponModel", e.RowIndex].Value = (short)e.RowIndex;
                        temp["MinATP", e.RowIndex].Value = 1;
                        temp["MaxATP", e.RowIndex].Value = 2;
                        temp["Ata", e.RowIndex].Value = 3;
                        temp["MaxTargets", e.RowIndex].Value = 3;
                        temp["AvailableRaces", e.RowIndex].Value = 63;
                        temp["ReqATP", e.RowIndex].Value = 5;
                        temp["Rank", e.RowIndex].Value = 3;
                        temp["BasePP", e.RowIndex].Value = 1;
                        temp["RegenPP", e.RowIndex].Value = 1;
                    }
                }
            }
        }

        private void numberRows(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;
            if (gridView == dataGridView5)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                    gridView.Rows[r.Index].HeaderCell.Value = (r.Index).ToString();
            }
            else if(gridView != null)
                foreach (DataGridViewRow r in gridView.Rows)
                    gridView.Rows[r.Index].HeaderCell.Value = (r.Index).ToString("X2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempDialog = new SaveFileDialog();
            if (tempDialog.ShowDialog() == DialogResult.OK)
            {
                Stream outStream = tempDialog.OpenFile();
                internalFile.saveTextFile(outStream);
                outStream.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempDialog = new OpenFileDialog();
            if (tempDialog.ShowDialog() == DialogResult.OK)
            {
                Stream inStream = tempDialog.OpenFile();
                internalFile.loadTextFile(inStream);
                inStream.Close();
                Refresh();
            }
        }
    }
}
