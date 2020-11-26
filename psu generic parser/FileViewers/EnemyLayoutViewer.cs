using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;

namespace psu_generic_parser
{
    public partial class EnemyLayoutViewer : UserControl
    {
        EnemyLayoutFile internalFile;
        DataContractJsonSerializer tempJson = new DataContractJsonSerializer(typeof(EnemyLayoutFile));
        public EnemyLayoutViewer(EnemyLayoutFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream tempStream = saveFileDialog1.OpenFile();
                tempJson.WriteObject(tempStream, internalFile);
                tempStream.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream tempStream = openFileDialog1.OpenFile();
                EnemyLayoutFile tempLayout = (EnemyLayoutFile)tempJson.ReadObject(tempStream);
                tempStream.Close();
                internalFile.spawns = tempLayout.spawns;
            }
        }
    }
}
