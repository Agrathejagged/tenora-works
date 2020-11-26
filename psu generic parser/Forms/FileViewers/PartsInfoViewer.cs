using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class PartsInfoViewer : UserControl
    {
        PartsInfoFile internalFile;
        public PartsInfoViewer(PartsInfoFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            dataGridView1.Columns[0].DefaultCellStyle.Format = "X08";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "X08";
            dataGridView1.Rows.Add(internalFile.parts.Length);
            for (int i = 0; i < internalFile.parts.Length; i++)
            {
                byte[] temp = BitConverter.GetBytes(internalFile.parts[i].partNumber);
                Array.Reverse(temp);
                dataGridView1[0, i].Value = BitConverter.ToInt32(temp, 0);
                dataGridView1[1, i].Value = internalFile.parts[i].fileNumber;
                dataGridView1[2, i].Value = internalFile.parts[i].unknownFlags;
                dataGridView1[3, i].Value = internalFile.parts[i].fileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                internalFile.importFile(openFileDialog1.OpenFile());
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(internalFile.parts.Length);
                for (int i = 0; i < internalFile.parts.Length; i++)
                {
                    byte[] temp = BitConverter.GetBytes(internalFile.parts[i].partNumber);
                    Array.Reverse(temp);
                    dataGridView1[0, i].Value = BitConverter.ToInt32(temp, 0);
                    dataGridView1[1, i].Value = internalFile.parts[i].fileNumber;
                    dataGridView1[2, i].Value = internalFile.parts[i].unknownFlags;
                    dataGridView1[3, i].Value = internalFile.parts[i].fileName;
                }
            }
        }
    }
}
