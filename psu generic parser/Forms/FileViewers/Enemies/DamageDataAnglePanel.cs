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
    public partial class DamageDataAnglePanel : UserControl
    {
        private DamageDataFileViewer parentViewer;
        public readonly DamageDataFile.DamageAngleEntry angleEntry;
        BindingSource bindingSource = new BindingSource();
        public DamageDataAnglePanel(DamageDataFileViewer parentViewer, DamageDataFile.DamageAngleEntry angleEntry)
        {
            this.parentViewer = parentViewer;
            InitializeComponent();
            this.angleEntry = angleEntry;
            firstValueNumericUpDown.Value = angleEntry.UnknownInt1;
            secondValueNumericUpDown.Value = angleEntry.UnknownInt2;
            textBox1.Text = string.Join(Environment.NewLine, angleEntry.ActionList);
            bindingSource.DataSource = angleEntry.ActionList;
            bindingSource.AllowNew = true;
        }

        private void firstValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            angleEntry.UnknownInt1 = (int)firstValueNumericUpDown.Value;
        }

        private void secondValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            angleEntry.UnknownInt2 = (int)secondValueNumericUpDown.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parentViewer.RemoveAngle(angleEntry, this);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string textbox = textBox1.Text;
            string[] strings = textbox.Split(null);
            List<int> oldInts = angleEntry.ActionList;
            List<int> newInts = new List<int>();
            foreach(var number in strings)
            {
                int numeric;
                bool success = int.TryParse(number, out numeric);
                if (success)
                {
                    newInts.Add(numeric);
                }
            }
            angleEntry.ActionList.Clear();
            angleEntry.ActionList.AddRange(newInts);
        }
    }
}
