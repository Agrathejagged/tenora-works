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
            dataGridView1.DataSource = objDefinition.GroupOneEntries;
            propertyGrid1.SelectedObject = objDefinition.Hitbox;
            dataGridView3.DataSource = objDefinition.Animations;
            if (objDefinition.ParticleSoundReferences != null)
            {
                group4Sub1Datagrid.DataSource = objDefinition.ParticleSoundReferences.particleBindings;
                group4Sub2Datagrid.DataSource = objDefinition.ParticleSoundReferences.soundBindings;
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
            dataGridView5.DataSource = objDefinition.Models;
        }
    }
}
