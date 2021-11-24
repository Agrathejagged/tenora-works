using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSULib.FileClasses.Maps;

namespace psu_generic_parser.FileViewers
{
    public partial class ObjectParticleInfoFileViewer : UserControl
    {
        public ObjectParticleInfoFile loadedFile;

        public ObjectParticleInfoFileViewer(ObjectParticleInfoFile theFile)
        {
            InitializeComponent();
            loadedFile = theFile;
            dataGridView1.DataSource = loadedFile.particleFileEntries;
        }
    }
}
