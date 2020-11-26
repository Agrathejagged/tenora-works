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
    public partial class PointeredFileViewer : UserControl
    {
        PointeredFile internalFile;
        PointeredFile360 alternateFile;

        public PointeredFileViewer(PointeredFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            richTextBox1.Text = "";
            if (internalFile.splitData != null)
            {
                List<int> condensedPointers = new List<int>();
                for(int i = 0; i < internalFile.sortedPtrs.Length; i++)
                {
                    if(!condensedPointers.Contains(internalFile.sortedPtrs[i]))
                    {
                        condensedPointers.Add(internalFile.sortedPtrs[i]);
                    }
                }
                StringWriter beta = new StringWriter();
                beta.WriteLine("Header:");
                beta.WriteLine(ASCIIEncoding.ASCII.GetString(internalFile.splitData[0], 0, 4).Replace("\0", ""));
                for (int i = 4; i < internalFile.splitData[0].Length; i++)
                {
                    if (internalFile.sortedPtrLocs.Contains(i))
                    {
                        if (i != 4)
                            beta.WriteLine();
                        beta.Write(BitConverter.ToString(internalFile.splitData[0], i, 4).Replace("-", ""));
                        beta.WriteLine(" -- pointer to (" + (condensedPointers.IndexOf(BitConverter.ToInt32(internalFile.splitData[0], i)) + 1) + ")");
                        i += 3;
                    }
                    else
                        beta.Write(BitConverter.ToString(internalFile.splitData[0], i, 1));
                }
                beta.WriteLine();
                beta.WriteLine();
                //for (int i = 0; i < bigassfile.Length; i++)
                //    beta.WriteLine(bigassfile[i]);
                bool previousPtr = true;
                int displayNum = 1;
                for (int i = 1; i < internalFile.splitData.Length; i++)
                {
                    if (internalFile.splitData[i].Length > 0)
                    {
                        beta.WriteLine("(" + displayNum + ")");
                        displayNum++;
                        previousPtr = true;
                        int lineLength = 0;
                        for (int j = 0; j < internalFile.splitData[i].Length; j++)
                        {

                            if (internalFile.sortedPtrLocs.Contains(internalFile.sortedPtrs[i - 1] + j))
                            {
                                if (!previousPtr)
                                    beta.WriteLine();
                                beta.Write(BitConverter.ToString(internalFile.splitData[i], j, 4).Replace("-", ""));
                                beta.WriteLine(" -- pointer to (" + (condensedPointers.IndexOf(BitConverter.ToInt32(internalFile.splitData[i], j)) + 1) + ")");
                                //beta.WriteLine(" -- pointer to (" + Array.IndexOf(internalFile.sortedPtrLocs, internalFile.sortedPtrLocs[i] + j) + ")");
                                j += 3;
                                previousPtr = true;
                                lineLength = 0;
                            }
                            else
                            {
                                previousPtr = false;
                                beta.Write(BitConverter.ToString(internalFile.splitData[i], j, 1));
                                lineLength++;
                                if (lineLength % 4 == 0)
                                    beta.Write(" ");
                                if (lineLength == 16)
                                {
                                    beta.WriteLine();
                                    lineLength = 0;
                                }

                            }
                        }
                        beta.WriteLine();
                        beta.WriteLine();
                    }
                }
                richTextBox1.Text = beta.ToString();
            }
        }

        public PointeredFileViewer(PointeredFile360 toImport)
        {
            InitializeComponent();
            alternateFile = toImport;
            richTextBox1.Text = "";
            if (alternateFile.splitData != null)
            {
                List<int> condensedPointers = new List<int>();
                for (int i = 0; i < alternateFile.sortedPtrs.Length; i++)
                {
                    if (!condensedPointers.Contains(alternateFile.sortedPtrs[i]))
                    {
                        condensedPointers.Add(alternateFile.sortedPtrs[i]);
                    }
                }
                StringWriter beta = new StringWriter();
                beta.WriteLine("Header:");
                beta.WriteLine(ASCIIEncoding.ASCII.GetString(alternateFile.splitData[0], 0, 4).Replace("\0", ""));
                for (int i = 4; i < alternateFile.splitData[0].Length; i++)
                {
                    if (alternateFile.sortedPtrLocs.Contains(i))
                    {
                        if (i != 4)
                            beta.WriteLine();
                        beta.Write(BitConverter.ToString(alternateFile.splitData[0], i, 4).Replace("-", ""));
                        beta.WriteLine(" -- pointer to (" + (condensedPointers.IndexOf((int)PointeredFile360.ReverseBytes(BitConverter.ToUInt32(alternateFile.splitData[0], i))) + 1) + ")");
                        i += 3;
                    }
                    else
                        beta.Write(BitConverter.ToString(alternateFile.splitData[0], i, 1));
                }
                beta.WriteLine();
                beta.WriteLine();
                //for (int i = 0; i < bigassfile.Length; i++)
                //    beta.WriteLine(bigassfile[i]);
                bool previousPtr = true;
                int displayNum = 1;
                for (int i = 1; i < alternateFile.splitData.Length; i++)
                {
                    if (alternateFile.splitData[i].Length > 0)
                    {
                        beta.WriteLine("(" + displayNum + ")");
                        displayNum++;
                        previousPtr = true;
                        int lineLength = 0;
                        for (int j = 0; j < alternateFile.splitData[i].Length; j++)
                        {

                            if (alternateFile.sortedPtrLocs.Contains(alternateFile.sortedPtrs[i - 1] + j))
                            {
                                if (!previousPtr)
                                    beta.WriteLine();
                                beta.Write(BitConverter.ToString(alternateFile.splitData[i], j, 4).Replace("-", ""));
                                beta.WriteLine(" -- pointer to (" + (condensedPointers.IndexOf((int)PointeredFile360.ReverseBytes(BitConverter.ToUInt32(alternateFile.splitData[i], j))) + 1) + ")");
                                //beta.WriteLine(" -- pointer to (" + Array.IndexOf(alternateFile.sortedPtrLocs, alternateFile.sortedPtrLocs[i] + j) + ")");
                                j += 3;
                                previousPtr = true;
                                lineLength = 0;
                            }
                            else
                            {
                                previousPtr = false;
                                beta.Write(BitConverter.ToString(alternateFile.splitData[i], j, 1));
                                lineLength++;
                                if (lineLength % 4 == 0)
                                    beta.Write(" ");
                                if (lineLength == 16)
                                {
                                    beta.WriteLine();
                                    lineLength = 0;
                                }

                            }
                        }
                        beta.WriteLine();
                        beta.WriteLine();
                    }
                }
                richTextBox1.Text = beta.ToString();
            }
        }
        //public 
    }
}
