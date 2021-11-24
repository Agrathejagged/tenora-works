using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PSULib.FileClasses.Missions;

namespace psu_generic_parser.FileViewers
{
    public partial class QuestListViewer : UserControl
    {
        QuestListFile theList;
        public QuestListViewer(QuestListFile file)
        {
            theList = file;
            InitializeComponent();
            dataGridView1.DataSource = theList.questList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                theList.LoadTextFile(lines);
                Refresh();
            }
        }
    }
}
