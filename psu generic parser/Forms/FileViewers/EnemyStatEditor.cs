using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PSULib.FileClasses.Enemies;

namespace psu_generic_parser
{
    public partial class EnemyStatEditor : UserControl
    {
        EnemyLevelParamFile internalRep;
        bool loaded = false;
        public EnemyStatEditor(EnemyLevelParamFile fileToEdit)
        {
            InitializeComponent();
            internalRep = fileToEdit;
            int levelCount = internalRep.HPperLevel.Length;
            dataGridView1.Rows.Add(levelCount);
            dataGridView1.Columns[0].ValueType = typeof(int);
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].ValueType = typeof(short);
            }

            for (int i = 0; i < levelCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
                dataGridView1.Rows[i].Cells[0].Value = internalRep.HPperLevel[i];
                for(int j = 0; j < 10; j++)
                    dataGridView1.Rows[i].Cells[j + 1].Value = internalRep.allOtherStats[i, j];
            }
            loaded = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (loaded)
            {
                if (e.ColumnIndex == 0)
                    internalRep.HPperLevel[e.RowIndex] = (int)dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                else
                    internalRep.allOtherStats[e.RowIndex, e.ColumnIndex - 1] = (short)dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            }
        }
    }
}
