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
    public partial class ItemTechParamViewer : UserControl
    {
        ItemTechParamFile internalFile;
        public ItemTechParamViewer(ItemTechParamFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            tabControl1.TabPages.Clear();
            for (int i = 0; i < internalFile.techInfo.Length; i++)
            {
                TabPage temp;
                if (internalFile.techIndexes.Length > 0)
                    temp = new TabPage("Tech " + Array.IndexOf(internalFile.techIndexes, (byte)i));
                else
                    temp = new TabPage("Tech " + i);
                DataGridView tempGrid = new DataGridView();
                tempGrid.AutoGenerateColumns = true;
                tempGrid.DataSource = internalFile.allTechs[i];
                temp.Controls.Add(tempGrid);
                tempGrid.Dock = DockStyle.Fill;
                tabControl1.TabPages.Add(temp);
                for (int j = 0; j < tempGrid.ColumnCount; j++)
                {
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    int tempWidth = tempGrid.Columns[j].Width;
                    tempGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    tempGrid.Columns[j].Width = tempWidth;
                }
                tempGrid.VirtualMode = false;
                tempGrid.AllowUserToAddRows = tempGrid.AllowUserToDeleteRows = tempGrid.AllowUserToOrderColumns = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
