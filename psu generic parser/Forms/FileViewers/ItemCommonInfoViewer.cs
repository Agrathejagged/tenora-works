using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PSULib.FileClasses.Items;

namespace psu_generic_parser
{

    public partial class ItemCommonInfoViewer : UserControl
    {
        ItemCommonInfoFile internalFile;
        public ItemCommonInfoViewer(ItemCommonInfoFile inFile)
        {
            InitializeComponent();
            internalFile = inFile;
            for (int i = 0; i < internalFile.entries.Length; i++)
            {
                TreeNode topNode = new TreeNode((i + 1).ToString("X2") + ":yy:zz:xx");
                for (int j = 0; j < internalFile.entries[i].Length; j++)
                {
                    TreeNode secondLevel = new TreeNode((i + 1).ToString("X2") + ":yy:zz:" + j.ToString("X2"));

                    for (int k = 0; k < internalFile.entries[i][j].Length; k++)
                    {
                        TreeNode thirdLevel = new TreeNode((i + 1).ToString("X2") + ":" + (k + 1).ToString("X2") + ":zz:" + j.ToString("X2"));
                        thirdLevel.Tag = new int[] { i, j, k };
                        secondLevel.Nodes.Add(thirdLevel);
                    }
                    TreeNode thirdLevelAddNew = new TreeNode("Add new...");
                    thirdLevelAddNew.Tag = new int[] { i, j, -1 };
                    secondLevel.Nodes.Add(thirdLevelAddNew);

                    int[] secondCoords = new int[2];
                    secondCoords[0] = i;
                    secondCoords[1] = j;
                    secondLevel.Tag = secondCoords;
                    topNode.Nodes.Add(secondLevel);
                }

                topNode.Tag = i;
                treeView1.Nodes.Add(topNode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag.GetType() == typeof(int[]) && ((int[])e.Node.Tag).Length == 3)
            {
                int[] coordinate = (int[])(e.Node.Tag);
                if (coordinate[2] == -1)
                {
                    coordinate[2] = internalFile.entries[coordinate[0]][coordinate[1]].Length;
                    Array.Resize(ref internalFile.entries[coordinate[0]][coordinate[1]], internalFile.entries[coordinate[0]][coordinate[1]].Length + 1);
                    internalFile.entries[coordinate[0]][coordinate[1]][coordinate[2]] = new List<ItemCommonInfoFile.CommonEntry>();
                    e.Node.Text = coordinate[0].ToString("X2") + ":" + coordinate[2].ToString("X2") + ":zz:" + coordinate[1].ToString("X2");
                    TreeNode tempNode = new TreeNode("Add new...");
                    tempNode.Tag = new int[] { coordinate[0], coordinate[1], -1 };
                    e.Node.Parent.Nodes.Add(tempNode);
                }
                BindingSource binder = new BindingSource();
                binder.DataSource = internalFile.entries[coordinate[0]][coordinate[1]][coordinate[2]];
                dataGridView1.DataSource = binder;
                dataGridView1.Columns[0].Width = 64;
                dataGridView1.Columns[1].Width = 64;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = dataGridView1.AllowUserToOrderColumns = false;
            }
            else
                dataGridView1.DataSource = null;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();
        }
    }
}
