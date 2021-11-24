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
    public partial class AnimationNameHashDialog : Form
    {
        public AnimationNameHashDialog()
        {
            InitializeComponent();
        }

        private void fileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            updateHash();
        }

        public void SetFileName(string newFilename)
        {
            fileNameTextBox.Text = newFilename;
            updateHash();
        }

        private void updateHash()
        {
            int computedHash = PSULib.Support.PsuCrc32.ComputeHash(fileNameTextBox.Text, fileNameTextBox.Text.Length > 4);
            hashOutputTextBox.Text = computedHash.ToString();
        }
    }
}
