using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser
{
    public partial class MissionBuilder : Form
    {
        MD5 hasher = MD5.Create();
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        RawFile questNbl = null;
        Dictionary<int, RawFile> zones = new Dictionary<int, RawFile>();
        List<int> zoneList = new List<int>();

        public MissionBuilder()
        {
            InitializeComponent();
            textBox1.Text = "blank";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                questNbl = new RawFile { filename = "quest.nbl", fileContents = File.ReadAllBytes(openFileDialog1.FileName) };
                if (listBox1.Items.Count == 0)
                    listBox1.Items.Add("quest.nbl: " + openFileDialog1.FileName);
                else if (((string)listBox1.Items[0]).StartsWith("quest.nbl"))
                { 
                    listBox1.Items.RemoveAt(0);
                    listBox1.Items.Insert(0, "quest.nbl: " + openFileDialog1.FileName);
                }
                else
                    listBox1.Items.Insert(0, "quest.nbl: " + openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int zoneNumber = Decimal.ToInt32(numericUpDown1.Value);
                RawFile zone = new RawFile { filename = "zone" + zoneNumber.ToString("D2") + ".nbl", fileContents = File.ReadAllBytes(openFileDialog1.FileName) };
                string zoneText = "zone" + zoneNumber.ToString("D2") + ": " + openFileDialog1.FileName;
                if (zones.ContainsKey(zoneNumber))
                    zones.Remove(zoneNumber);
                zones.Add(zoneNumber, zone);

                int index = zoneList.FindIndex(i => i > zoneNumber);
                int questOffset = questNbl != null ? 1 : 0;
                if (zoneList.Contains(zoneNumber))
                {
                    listBox1.Items.RemoveAt(zoneList.IndexOf(zoneNumber) + questOffset);
                    listBox1.Items.Insert(zoneList.IndexOf(zoneNumber) + questOffset, zoneText);
                }
                else if(index != -1)
                {
                    listBox1.Items.Insert(index + questOffset, zoneText);
                    zoneList.Insert(index, zoneNumber);
                }
                else
                {
                    listBox1.Items.Add(zoneText);
                    zoneList.Add(zoneNumber);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int zoneNumber = Decimal.ToInt32(numericUpDown2.Value);
            if (zones.ContainsKey(zoneNumber))
                zones.Remove(zoneNumber);

            int questOffset = questNbl != null ? 1 : 0;
            if (zoneList.Contains(zoneNumber))
            {
                listBox1.Items.RemoveAt(zoneList.IndexOf(zoneNumber) + questOffset);
                zoneList.Remove(zoneNumber);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filename = "xf_qe_" + textBox1.Text + "aafs";
            byte[] fileMD5 = hasher.ComputeHash(encoding.GetBytes(filename));
            string MD5string = "";
            for (int k = 0; k < fileMD5.Length; k++)
                MD5string += fileMD5[k].ToString("x2");
            textBox2.Text = MD5string;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (questNbl != null)
            {
                saveFileDialog1.FileName = textBox2.Text;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    AfsLoader mission = AfsLoader.CreateMissionAfs(questNbl, zones, checkBox1.Checked);
                    using (var outStream = saveFileDialog1.OpenFile())
                    {
                        mission.saveFile(outStream);
                    }
                }
            }
        }
    }
}
