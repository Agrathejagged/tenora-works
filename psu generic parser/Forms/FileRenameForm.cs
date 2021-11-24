using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms
{
    public partial class FileRenameForm : Form
    {
        public FileRenameForm(string initialFilename)
        {
            InitializeComponent();
            filenameTextBox.Text = initialFilename;
        }

        public string FileName { get { return filenameTextBox.Text; } }
    }
}
