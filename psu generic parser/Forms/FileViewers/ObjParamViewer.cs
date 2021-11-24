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
    public partial class ObjParamViewer : UserControl
    {
        ObjectParamFile internalFile;

        public ObjParamViewer(ObjectParamFile paramFile)
        {
            internalFile = paramFile;
            InitializeComponent();
            foreach (var key in internalFile.ObjectDefinitions.Keys.OrderBy(x=>x)) 
            {
                listBox1.Items.Add(key);
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var objDefinition = internalFile.ObjectDefinitions[(int)listBox1.Items[listBox1.SelectedIndex]];
            dataGridView1.DataSource = objDefinition.group1Entries;
            propertyGrid1.SelectedObject = objDefinition.group2Entry;
            dataGridView3.DataSource = objDefinition.group3Entries;
            if (objDefinition.group4Entry != null)
            {
                group4Sub1Datagrid.DataSource = objDefinition.group4Entry.particleBindings;
                group4Sub2Datagrid.DataSource = objDefinition.group4Entry.soundBindings;
                group4Sub2Datagrid.Enabled = true;
                group4Sub1Datagrid.Enabled = true;
            }
            else
            {
                group4Sub1Datagrid.DataSource = null;
                group4Sub2Datagrid.DataSource = null;
                group4Sub2Datagrid.Enabled = false;
                group4Sub1Datagrid.Enabled = false;
            }
            dataGridView5.DataSource = objDefinition.group5Entries;
        }
    }
}
