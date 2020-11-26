using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace psu_generic_parser
{
    public enum listTypes { XNT, XNA, filelist, particle };
    public partial class ListViewer : UserControl
    {
        int numEntries = 0;
        int startLoc;
        byte[] data;
        int[] ptrs;
        int[] indexes;
        listTypes fileType;
        string[] strings;

        public ListViewer()
        {
            Dock = DockStyle.Fill;
            InitializeComponent();
        }

        public ListViewer(byte[] fileData, int startAddress, int[] pointers, listTypes theType)
            : this()
        {
            startLoc = startAddress;
            MemoryStream alpha = new MemoryStream(fileData);
            BinaryReader beta = new BinaryReader(alpha);
            fileType = theType;
            alpha.Seek(8, SeekOrigin.Current);
            int headerLoc = beta.ReadInt32();
            int listLoc = 0;
            alpha.Seek(headerLoc, SeekOrigin.Begin);
            switch (fileType)
            {
                case listTypes.XNA:
                    beta.ReadInt32();
                    numEntries = beta.ReadInt32();
                    listLoc = beta.ReadInt32() - startAddress;
                    indexes = new int[numEntries];
                    strings = new string[numEntries];
                    alpha.Seek(listLoc, SeekOrigin.Begin);
                    int[] stringLocs = new int[numEntries];
                    for (int i = 0; i < numEntries; i++)
                    {
                        indexes[i] = beta.ReadInt32();
                        stringLocs[i] = beta.ReadInt32() - startLoc;
                    }
                    for (int i = 0; i < numEntries; i++)
                    {
                        alpha.Seek(stringLocs[i], SeekOrigin.Begin);
                        if (i < numEntries - 1)
                            strings[i] = new string(beta.ReadChars(stringLocs[i + 1] - stringLocs[i]));
                        else
                            strings[i] = new string(beta.ReadChars(Array.IndexOf(fileData, (byte)0, stringLocs[i]) - stringLocs[i]));
                    }
                    dataGridView1.Columns.Add("indexCol", "Index");
                    dataGridView1.Columns.Add("stringCol", "Bone name");
                    dataGridView1.Columns[0].Width = 32;
                    dataGridView1.Columns[1].Width = 128;
                    dataGridView1.Rows.Add(numEntries);
                    for (int i = 0; i < numEntries; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = indexes[i];
                        dataGridView1.Rows[i].Cells[1].Value = strings[i];
                    }
                    break;
                case listTypes.XNT:
                    numEntries = beta.ReadInt32();
                    listLoc = beta.ReadInt32() - startAddress;
                    indexes = new int[numEntries];
                    strings = new string[numEntries];
                    short[] unknowns1 = new short[numEntries];
                    short[] unknowns2 = new short[numEntries];
                    int[] unknowns3 = new int[numEntries];
                    int[] unknowns4 = new int[numEntries];
                    stringLocs = new int[numEntries];

                    alpha.Seek(listLoc, SeekOrigin.Begin);
                    for (int i = 0; i < numEntries; i++)
                    {
                        indexes[i] = beta.ReadInt32();
                        stringLocs[i] = beta.ReadInt32() - startLoc;
                        unknowns1[i] = beta.ReadInt16();
                        unknowns2[i] = beta.ReadInt16();
                        unknowns3[i] = beta.ReadInt32();
                        unknowns4[i] = beta.ReadInt32();
                    }
                    for (int i = 0; i < numEntries; i++)
                    {
                        alpha.Seek(stringLocs[i], SeekOrigin.Begin);
                        //if (i < numEntries - 1)
                        //    strings[i] = new string(beta.ReadChars(stringLocs[i + 1] - stringLocs[i]));
                        //else
                            strings[i] = new string(beta.ReadChars(Array.IndexOf(fileData, (byte)0, stringLocs[i]) - stringLocs[i]));
                    }
                    dataGridView1.Columns.Add("indexCol", "Index");
                    dataGridView1.Columns.Add("stringCol", "Filename");
                    dataGridView1.Columns.Add("unk1Col", "?");
                    dataGridView1.Columns.Add("unk2Col", "?");
                    dataGridView1.Columns.Add("unk3Col", "?");
                    dataGridView1.Columns.Add("unk4Col", "?");
                    dataGridView1.Columns[0].Width = 32;
                    dataGridView1.Columns[1].Width = 128;
                    dataGridView1.Columns[2].Width = 32;
                    dataGridView1.Columns[3].Width = 32;
                    dataGridView1.Columns[4].Width = 32;
                    dataGridView1.Columns[5].Width = 32;
                    dataGridView1.Rows.Add(Math.Max(numEntries, 1));
                    for (int i = 0; i < numEntries; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = indexes[i];
                        dataGridView1.Rows[i].Cells[1].Value = strings[i];
                        dataGridView1.Rows[i].Cells[2].Value = unknowns1[i];
                        dataGridView1.Rows[i].Cells[3].Value = unknowns2[i];
                        dataGridView1.Rows[i].Cells[4].Value = unknowns3[i];
                        dataGridView1.Rows[i].Cells[5].Value = unknowns4[i];
                    }
                    break;
                case listTypes.filelist:

                    break;
                case listTypes.particle:
                    break;
            }

        }
    }
}
