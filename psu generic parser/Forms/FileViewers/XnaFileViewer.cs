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
    public partial class XnaFileViewer : UserControl
    {
        public XnaFile loadedFile;

        public XnaFileViewer(XnaFile xna)
        {
            InitializeComponent();
            loadedFile = xna;
            dataGridView1.DataSource = xna.boneReferences;
        }
    }
}
