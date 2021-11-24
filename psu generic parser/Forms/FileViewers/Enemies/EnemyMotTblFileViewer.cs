using PSULib.FileClasses.Enemies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.Enemies
{
    public partial class EnemyMotTblFileViewer : UserControl
    {
        private EnemyMotTblFile internalFile;
        private BindingSource binding = new BindingSource();
        public EnemyMotTblFileViewer(EnemyMotTblFile inFile)
        {
            internalFile = inFile;
            InitializeComponent();
            binding.DataSource = internalFile.MotTblEntries;
            dataGridView1.DataSource = binding;
        }

        private void setAnimationByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
