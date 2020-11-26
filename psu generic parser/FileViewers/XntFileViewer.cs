using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }
    }
}
