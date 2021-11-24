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
    public partial class EnemySoundEffectFileViewer : UserControl
    {
        EnemySoundEffectFile internalFile;
        BindingSource bindingSource = new BindingSource();
        public EnemySoundEffectFileViewer(EnemySoundEffectFile seDataFile)
        {
            InitializeComponent();
            internalFile = seDataFile;
            soundEffectDataGrid.DataSource = bindingSource;
            bindingSource.DataSource = internalFile.SoundEffects;
            bindingSource.AllowNew = true;
        }
    }
}
