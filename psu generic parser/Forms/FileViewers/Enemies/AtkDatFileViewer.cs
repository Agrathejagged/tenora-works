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
    public partial class AtkDatFileViewer : UserControl
    {
        private AtkDatFile internalFile;
        private BindingSource bindingSource = new BindingSource();

        public AtkDatFileViewer(AtkDatFile inFile)
        {
            internalFile = inFile;
            InitializeComponent();
            for(int i = 0; i < inFile.Attacks.Count; i++)
            {
                listBox1.Items.Add(i);
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex < internalFile.Attacks.Count)
            {
                bindingSource.DataSource = internalFile.Attacks[listBox1.SelectedIndex].AttackEntries;
                dataGridView1.DataSource = bindingSource;
                bindingSource.AllowNew = true;
                dataGridView1.AllowUserToAddRows = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            internalFile.Attacks.Add(new AtkDatFile.Attack());
            listBox1.Items.Add(listBox1.Items.Count);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            internalFile.Attacks.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }
}
