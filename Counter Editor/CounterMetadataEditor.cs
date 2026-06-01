using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Counter_Editor
{
    public partial class CounterMetadataEditor : UserControl
    {
        Files.CounterArchive counterFile;
        bool loaded = false;

        public CounterMetadataEditor(Files.CounterArchive counter)
        {
            InitializeComponent();
            counterFile = counter;
            propertyGrid1.SelectedObject = counter;
            textBox1.Text = counterFile.counterDescription;
            nameTextBox.Text = counterFile.counterName;
            loaded = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                counterFile.counterDescription = textBox1.Text;
                this.Parent.Refresh();
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                counterFile.counterName = nameTextBox.Text;
                this.Parent.Refresh();
            }
        }
    }
}
