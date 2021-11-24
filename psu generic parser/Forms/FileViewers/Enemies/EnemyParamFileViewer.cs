using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSULib.FileClasses.Enemies;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class EnemyParamFileViewer : UserControl
    {
        private EnemyParamFile loadedFile;
        public EnemyParamFileViewer(EnemyParamFile internalFile)
        {
            loadedFile = internalFile;
            InitializeComponent();
            propertyGrid1.SelectedObject = loadedFile.baseParams;
            buffDataGridView.DataSource = loadedFile.buffParams;
            attackDataGridView.DataSource = loadedFile.attackParams;
            hitboxDataGridView.DataSource = loadedFile.hitboxParams;
            unknownSub1DataGridView.DataSource = loadedFile.unknownSubEntry1List;
            unknownSub2DataGridView.DataSource = loadedFile.unknownSubEntry2List;
        }
    }
}
