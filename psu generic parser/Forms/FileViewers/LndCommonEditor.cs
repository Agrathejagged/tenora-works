using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSULib.FileClasses.Maps;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class LndCommonEditor : UserControl
    {
        private LndCommonFile internalFile;
        public LndCommonEditor(LndCommonFile internalFile)
        {
            InitializeComponent();
            this.internalFile = internalFile;
            nblTextBox.Text = internalFile.NblFilenameFragment;
            xnt1TextBox.Text = internalFile.XntFilenameFragment1;
            xnt2TextBox.Text = internalFile.XntFilenameFragment2;
            unknownValueUpDown.Value = (decimal)internalFile.UnknownFloat;
        }

        private void nblTextBox_TextChanged(object sender, EventArgs e)
        {
            internalFile.NblFilenameFragment = nblTextBox.Text;
        }

        private void xnt1TextBox_TextChanged(object sender, EventArgs e)
        {
            internalFile.XntFilenameFragment1 = xnt1TextBox.Text;
        }

        private void xnt2TextBox_TextChanged(object sender, EventArgs e)
        {
            internalFile.XntFilenameFragment2 = xnt2TextBox.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            internalFile.UnknownFloat = (float)unknownValueUpDown.Value;
        }
    }
}
