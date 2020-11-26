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
    public partial class WeaponListEditor : UserControl
    {
        WeaponListFile internalFile;
        public WeaponListEditor(WeaponListFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                internalFile.FromDirectory(folderBrowserDialog1.SelectedPath);
            }
        }
    }
}
