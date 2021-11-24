using System;
using System.Windows.Forms;
using System.IO;
using PSULib.FileClasses.Items;

namespace psu_generic_parser
{
    public partial class ItemPriceViewer : UserControl
    {
        ItemPriceFile internalFile;
        int[] selection = { -1, -1 };

        public ItemPriceViewer()
        {
            InitializeComponent();
        }

        public ItemPriceViewer(ItemPriceFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            //dataGridView1.Columns.Add("weaponNum", "Number");
            //dataGridView1.Columns[0].DefaultCellStyle.Format = "X08";
            //dataGridView1.Columns.Add("price", "Price");
            dataGridView1.AllowUserToDeleteRows = dataGridView1.AllowUserToOrderColumns = false;
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                dataGridView1.Columns[j].MinimumWidth = 32;
            }

            for (int i = 0; i < internalFile.itemPrices.Length; i++)
            {
                TreeNode currLevel1 = new TreeNode("Top level " + i);
                currLevel1.Tag = new int[] { i, -1 };
                for (int j = 0; j < internalFile.itemPrices[i].Length; j++)
                {
                    TreeNode currLevel2 = new TreeNode("Second level " + j.ToString("X2"));
                    currLevel2.Tag = new int[] { i, j };
                    currLevel1.Nodes.Add(currLevel2);
                }
                treeView1.Nodes.Add(currLevel1);
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                
                selection = (int[])e.Node.Tag;dataGridView1.DataSource = internalFile.itemPrices[selection[0]][selection[1]];
                dataGridView1.Columns[0].DefaultCellStyle.Format = "X8";
            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    int hex = Convert.ToInt32(e.Value.ToString(), 16);
                    e.Value = hex;
                    e.ParsingApplied = true;
                }
                catch
                {
                    //Bad parse.
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter outWriter = new StreamWriter(saveFileDialog1.OpenFile());
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < internalFile.itemPrices[i].Length; j++)
                    {
                        for (int k = 0; k < internalFile.itemPrices[i][j].Count; k++)
                        {
                            outWriter.WriteLine(i + "\t" + j + "\t" + internalFile.itemPrices[i][j][k].ItemNumber.ToString("X8") + "\t" + internalFile.itemPrices[i][j][k].ItemPrice);
                        }
                    }
                }
                outWriter.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader stringReader = new StreamReader(openFileDialog1.OpenFile());
                while (!stringReader.EndOfStream)
                {
                    string currLine = stringReader.ReadLine();
                    string[] splitLine = currLine.Split('\t');
                    int topLevel = Convert.ToInt32(splitLine[0]);
                    int secondLevel = Convert.ToInt32(splitLine[1]);
                    int weaponNum = Convert.ToInt32(splitLine[2], 16);
                    int price = Convert.ToInt32(splitLine[3]);
                    if (topLevel < internalFile.itemPrices.Length && secondLevel < internalFile.itemPrices[topLevel].Length)
                    {
                        bool found = false;
                        for(int i = 0; i < internalFile.itemPrices[topLevel][secondLevel].Count; i++)
                        {
                            if(internalFile.itemPrices[topLevel][secondLevel][i].ItemNumber == weaponNum)
                            {
                                found = true;
                                internalFile.itemPrices[topLevel][secondLevel][i].ItemPrice = price;
                            }
                        }
                        if (!found)
                        {
                            ItemPriceFile.priceEntry temp = new ItemPriceFile.priceEntry();
                            temp.ItemNumber = weaponNum;
                            temp.ItemPrice = price;
                            internalFile.itemPrices[topLevel][secondLevel].Add(temp);
                        }
                    }
                }
            }
        }
    }
}
