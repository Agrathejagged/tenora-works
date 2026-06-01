using psu_generic_parser.Forms.FileViewers.LndEffects;
using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class FogBankViewer : UserControl
    {

        private FogBankFile loadedFile;

        public FogBankViewer(FogBankFile fogBankFile)
        {
            loadedFile = fogBankFile;
            InitializeComponent();
            loadValues();
        }

        private void loadValues()
        {
            for(int i = 0; i< loadedFile.FogEntries.Count; i++)
            {
                fogSelectionListBox.Items.Add("Fog " + i);
            }
            fogSelectionListBox.SelectedIndex = 0;
        }

        private void fogSelectionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fogDisplayPanel.Fog = loadedFile.FogEntries[fogSelectionListBox.SelectedIndex];
        }
    }
}
