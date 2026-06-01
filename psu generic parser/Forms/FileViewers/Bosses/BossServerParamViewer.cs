using PSULib.FileClasses.Bosses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.Bosses
{
    public partial class BossServerParamViewer : UserControl
    {
        private ParamServerFile paramServerFile;
        public BossServerParamViewer(ParamServerFile paramServerFile)
        {
            this.paramServerFile = paramServerFile;
            InitializeComponent();
            attackDataGrid.DataSource = paramServerFile.AttackModifiers;
            attackDataGrid.Columns[0].DefaultCellStyle.Format = "X";
            attackDataGrid.Columns[1].DefaultCellStyle.Format = "X";
            attackDataGrid.Columns[2].DefaultCellStyle.Format = "X";
            hitboxDataGrid.DataSource = paramServerFile.HitboxModifiers;
            hitboxDataGrid.Columns[0].DefaultCellStyle.Format = "X";
            elementNumericUpDown.Value = paramServerFile.Element;
            unknown1UpDown.Value = paramServerFile.UnknownShort1;
            unknown2UpDown.Value = paramServerFile.UnknownShort2;
            baseStatPropertyGrid.SelectedObject = paramServerFile.StatModifiers;
            mysteryStatDataGrid.Columns.Add("valueColumn", "Values");
            mysteryStatDataGrid.RowCount = paramServerFile.MysteryFloats.Length;
            for (int i = 0; i < paramServerFile.MysteryFloats.Length; i++)
            {
                mysteryStatDataGrid.Rows[i].Cells[0].Value = paramServerFile.MysteryFloats[i];
            }
        }

        private void attackDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            attackDataGrid.InvalidateRow(e.RowIndex);
        }

        private void hitboxDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            hitboxDataGrid.InvalidateRow(e.RowIndex);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            paramServerFile.Element = (int)elementNumericUpDown.Value;
        }

        private void unknown1UpDown_ValueChanged(object sender, EventArgs e)
        {
            paramServerFile.UnknownShort1 = (ushort)unknown1UpDown.Value;
        }

        private void unknown2UpDown_ValueChanged(object sender, EventArgs e)
        {
            paramServerFile.UnknownShort2 = (ushort)unknown2UpDown.Value;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            paramServerFile.MysteryFloats[e.RowIndex] = (float)mysteryStatDataGrid.Rows[e.RowIndex].Cells[0].Value;
        }
    }
}
