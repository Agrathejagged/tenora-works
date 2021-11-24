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
    public partial class ActDataFileViewer : UserControl
    {
        private ActDataFile internalFile;
        private bool updating = false;
        public ActDataFileViewer(ActDataFile actDataFile)
        {
            InitializeComponent();
            internalFile = actDataFile;
            listBox1.BeginUpdate();
            for(int i = 0; i < actDataFile.Actions.Count; i++)
            {
                listBox1.Items.Add("Action " + i + " (" + actDataFile.Actions[i].ActionEntries.Count + ")");
            }
            listBox1.EndUpdate();
            listBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!updating)
            {
                updating = true;
                propertyGrid1.SelectedObject = internalFile.Actions[listBox1.SelectedIndex].ActionEntries[comboBox1.SelectedIndex];
                dataGridView1.DataSource = internalFile.Actions[listBox1.SelectedIndex].ActionEntries[comboBox1.SelectedIndex].SubEntryList1;
                dataGridView2.DataSource = internalFile.Actions[listBox1.SelectedIndex].ActionEntries[comboBox1.SelectedIndex].SubEntryList2;
                updating = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updating = true;
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            for (int i = 0; i < internalFile.Actions[listBox1.SelectedIndex].ActionEntries.Count; i++)
            {
                comboBox1.Items.Add("Subaction " + i);
            }
            comboBox1.EndUpdate();
            updating = false;
            comboBox1.SelectedIndex = 0;
        }
    }
}
