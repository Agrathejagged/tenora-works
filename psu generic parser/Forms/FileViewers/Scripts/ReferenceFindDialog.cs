using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.Scripts
{
    public partial class ReferenceFindDialog : Form
    {
        ScriptFileViewer parentControl;
        List<Tuple<string, int>> foundLocations;

        public ReferenceFindDialog(ScriptFileViewer parentControl, string subroutineName, List<Tuple<string, int>> foundLocations)
        {
            InitializeComponent();
            this.parentControl = parentControl;
            this.foundLocations = foundLocations;
            Text = $"References to {subroutineName}";
            label1.Text = $"Found the following references to {subroutineName}:";
            foreach (var foundItem in foundLocations.Select(tuple => tuple.Item1 + ": line " + tuple.Item2))
            {
                listBox1.Items.Add(foundItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            parentControl.SelectOperation(foundLocations[listBox1.SelectedIndex].Item1, foundLocations[listBox1.SelectedIndex].Item2);
        }
    }
}
