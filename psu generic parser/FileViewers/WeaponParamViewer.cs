using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace psu_generic_parser
{
    public partial class WeaponParamViewer : UserControl
    {
        WeaponParamFile internalFile;
        DataGridView[] weaponsViews = new DataGridView[4];
        bool updatingTable = false;

        public WeaponParamViewer(WeaponParamFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            weaponsViews[0] = dataGridView1;
            weaponsViews[1] = dataGridView2;
            weaponsViews[2] = dataGridView3;
            weaponsViews[3] = dataGridView4;
            for (int i = 0; i < 4; i++)
            {
                //weaponsViews[i] = new DataGridView();
                weaponsViews[i].AutoGenerateColumns = true;
                weaponsViews[i].DataSource = internalFile.parsedWeapons[i];
                weaponsViews[i].AllowUserToAddRows = weaponsViews[i].AllowUserToDeleteRows = weaponsViews[i].AllowUserToOrderColumns = false;
                weaponsViews[i].VirtualMode = false;
                for (int j = 0; j < weaponsViews[i].Columns.Count; j++)
                {
                    weaponsViews[i].Columns[j].MinimumWidth = 32;
                }
                weaponsViews[i].CellValueChanged += new DataGridViewCellEventHandler(weaponGrid_CellValueChanged);
            }

            dataGridView5.AutoGenerateColumns = true;
            dataGridView5.DataSource = internalFile.parsedPackets;
            dataGridView5.AllowUserToDeleteRows = dataGridView5.AllowUserToOrderColumns = false;
            if (internalFile.parsedPackets.Count == 0)
                dataGridView5.AllowUserToAddRows = false;
            dataGridView5.VirtualMode = false;
            for (int j = 0; j < dataGridView5.Columns.Count; j++)
            {
                dataGridView5.Columns[j].MinimumWidth = 32;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (internalFile.dataType != 2)
            {
                internalFile.expandFile();
                updatingTable = true;
                /*for (int i = 14; i < internalFile.parsedWeapons[0].Length; i++)
                {
                    for (int j = 0; j < internalFile.parsedWeapons[i].Length; j++)
                    {
                        weaponsViews[i].Rows[j].HeaderCell.Value = j.ToString("X2");
                        weaponsViews[i].Rows[j].Cells[0].Value = internalFile.parsedWeapons[i][j].ShootEffect;
                        weaponsViews[i].Rows[j].Cells[1].Value = internalFile.parsedWeapons[i][j].WeaponModel;
                        weaponsViews[i].Rows[j].Cells[2].Value = internalFile.parsedWeapons[i][j].MinATP;
                        weaponsViews[i].Rows[j].Cells[3].Value = internalFile.parsedWeapons[i][j].MaxATP;
                        weaponsViews[i].Rows[j].Cells[4].Value = internalFile.parsedWeapons[i][j].Ata;
                        weaponsViews[i].Rows[j].Cells[5].Value = internalFile.parsedWeapons[i][j].StatusEffect;
                        weaponsViews[i].Rows[j].Cells[6].Value = internalFile.parsedWeapons[i][j].StatusLevel;
                        weaponsViews[i].Rows[j].Cells[7].Value = internalFile.parsedWeapons[i][j].MaxTargets;
                        weaponsViews[i].Rows[j].Cells[8].Value = internalFile.parsedWeapons[i][j].AvailableRaces;
                        weaponsViews[i].Rows[j].Cells[9].Value = internalFile.parsedWeapons[i][j].ReqATP;
                        weaponsViews[i].Rows[j].Cells[10].Value = internalFile.parsedWeapons[i][j].Rank;
                        weaponsViews[i].Rows[j].Cells[11].Value = internalFile.parsedWeapons[i][j].DropElement;
                        weaponsViews[i].Rows[j].Cells[12].Value = internalFile.parsedWeapons[i][j].DropPercent;
                        weaponsViews[i].Rows[j].Cells[13].Value = internalFile.parsedWeapons[i][j].SoundEffect;
                        weaponsViews[i].Rows[j].Cells[14].Value = internalFile.parsedWeapons[i][j].BasePP;
                        weaponsViews[i].Rows[j].Cells[15].Value = internalFile.parsedWeapons[i][j].RegenPP;
                        weaponsViews[i].Rows[j].Cells[16].Value = internalFile.parsedWeapons[i][j].ModifyATP;
                        weaponsViews[i].Rows[j].Cells[17].Value = internalFile.parsedWeapons[i][j].Unknown1;
                        weaponsViews[i].Rows[j].Cells[18].Value = internalFile.parsedWeapons[i][j].Unknown2;
                        weaponsViews[i].Rows[j].Cells[19].Value = internalFile.parsedWeapons[i][j].Unknown3;
                        weaponsViews[i].Rows[j].Cells[20].Value = internalFile.parsedWeapons[i][j].Unknown4;
                        weaponsViews[i].Rows[j].Cells[21].Value = internalFile.parsedWeapons[i][j].Unknown5;
                        weaponsViews[i].Rows[j].Cells[22].Value = internalFile.parsedWeapons[i][j].Unknown6;
                    }
                }*/
                weaponsViews[0].DataSource = internalFile.parsedWeapons[0];
                weaponsViews[1].DataSource = internalFile.parsedWeapons[1];
                weaponsViews[2].DataSource = internalFile.parsedWeapons[2];
                weaponsViews[3].DataSource = internalFile.parsedWeapons[3];
                updatingTable = false;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //((DataGridView)sender)//.Update();
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

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempDialog = new SaveFileDialog();
            if (tempDialog.ShowDialog() == DialogResult.OK)
            {
                Stream outStream = tempDialog.OpenFile();
                internalFile.saveTextFile(outStream);
                outStream.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
