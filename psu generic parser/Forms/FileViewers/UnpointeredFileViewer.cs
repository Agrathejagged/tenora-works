using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PSULib.FileClasses.General;

namespace psu_generic_parser.FileViewers
{
    public partial class UnpointeredFileViewer : UserControl
    {
        UnpointeredFile internalFile;

        public UnpointeredFileViewer(UnpointeredFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            StringWriter beta = new StringWriter();
            int lineLength = 0;
            for (int j = 0; j < internalFile.theData.Length; j++)
            {
                    beta.Write(BitConverter.ToString(internalFile.theData, j, 1));
                    lineLength++;
                    if (lineLength % 4 == 0)
                        beta.Write(" ");
                    if (lineLength == 16)
                    {
                        beta.WriteLine();
                        lineLength = 0;
                    }
            }
            richTextBox1.Text = beta.ToString();
        }
    }
}
