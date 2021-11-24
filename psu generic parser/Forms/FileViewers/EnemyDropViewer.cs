using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using PSULib.FileClasses.Items;

namespace psu_generic_parser
{
    public partial class EnemyDropViewer : UserControl
    {
        EnemyDropFile internalFile;

        public EnemyDropViewer(EnemyDropFile theDrops)
        {
            InitializeComponent();

            internalFile = theDrops;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = internalFile.monsterDrops;
            dataGridView1.AllowUserToAddRows = dataGridView1.AllowUserToDeleteRows = dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.VirtualMode = false;
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                dataGridView1.Columns[j].MinimumWidth = 32;
                if (dataGridView1.Columns[j].Name.Contains("Drop") && !dataGridView1.Columns[j].Name.StartsWith("Drop"))
                    dataGridView1.Columns[j].DefaultCellStyle.Format = "X8";
                if(internalFile.isV1File && Regex.IsMatch(dataGridView1.Columns[j].Name, "Tier[2-5]"))
                {
                    dataGridView1.Columns[j].Visible = false;
                }
            }
            dataGridView1.Columns[0].DefaultCellStyle.Format = "X2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream outStream = saveFileDialog1.OpenFile();
                StreamWriter outWriter = new StreamWriter(outStream);
                outWriter.WriteLine("//ID\tDNR\tarea\tmesRt\tmesMin\tmesMax\tspec1   \tspec1Rt\tspec2   \tspec2Rt\tspec3   \tdrop    \trate\tetc");
                for (int i = 0; i < internalFile.monsterDrops.Length; i++)
                {
                    outWriter.Write(internalFile.monsterDrops[i].EnemyNum + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].DropNothing + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].AreaProb + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].MesetaProb + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].MesetaMin + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].MesetaMax + "\t");
                    
                    outWriter.Write(internalFile.monsterDrops[i].SpecDrop1.ToString("X8") + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].SpecProb1 + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].SpecDrop2.ToString("X8") + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].SpecProb2 + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].SpecDrop3.ToString("X8") + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].Tier1Drop.ToString("X8") + "\t");
                    outWriter.Write(internalFile.monsterDrops[i].Tier1Prob + "\t");
                    if (!internalFile.isV1File)
                    {
                        outWriter.Write(internalFile.monsterDrops[i].Tier2Drop.ToString("X8") + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier2Prob + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier3Drop.ToString("X8") + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier3Prob + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier4Drop.ToString("X8") + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier4Prob + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier5Drop.ToString("X8") + "\t");
                        outWriter.Write(internalFile.monsterDrops[i].Tier5Prob + "\t");
                    }
                    outWriter.WriteLine();
                }
                outWriter.Close();
            }
        }
    }
}
