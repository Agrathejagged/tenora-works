using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSULib.FileClasses.Models;

namespace psu_generic_parser.FileViewers
{
    public partial class XntFileViewer : UserControl
    {
        public XntFile loadedFile;

        public XntFileViewer(XntFile xnt)
        {
            InitializeComponent();
            loadedFile = xnt;
            dataGridView1.DataSource = xnt.fileEntries;

            //Hacky approach to set the columns to the two enums
            DataGridViewComboBoxColumn column2 = new DataGridViewComboBoxColumn();
            column2.DataSource = Enum.GetValues(typeof(MinifyMipFilter));
            column2.DataPropertyName = "minifyMipmapFilter";
            column2.HeaderText = "Minify/Mipmap Filter";
            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.RemoveAt(2);
            dataGridView1.Columns.Insert(2, column2);

            DataGridViewComboBoxColumn column3 = new DataGridViewComboBoxColumn();
            column3.DataSource = Enum.GetValues(typeof(MagnifyFilter));
            column3.DataPropertyName = "magnifyFilter";
            column3.HeaderText = "Magnify Filter";
            column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.RemoveAt(3);
            dataGridView1.Columns.Insert(3, column3);
        }
    }
}
