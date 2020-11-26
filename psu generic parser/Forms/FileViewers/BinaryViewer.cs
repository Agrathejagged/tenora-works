using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace psu_generic_parser
{
    public partial class BinaryViewer : UserControl
    {
        int[] internalPointers;
        int[] condensedPtrs;
        int[] ptrs;
        byte[][] splitFileData;
        bool hasPointers = true;

        public BinaryViewer()
        {
            Dock = DockStyle.Fill;
            InitializeComponent();
        }

        public BinaryViewer(byte[] fileData, int startAddress, int[] pointers) :this()
        {
            //ptrs;
            hasPointers = true;
            int startLoc = 0;
            if (ASCIIEncoding.ASCII.GetString(fileData, 0, 4) != "YPD0")
            {
                startLoc = 1;
                internalPointers = new int[pointers.Length + 1];
                internalPointers[0] = 8; //BitConverter.ToInt32(fileData, 4);
            }
            else
                internalPointers = new int[pointers.Length];
            
            Array.Copy(pointers, 0, internalPointers, startLoc, pointers.Length);
            ptrs = new int[internalPointers.Length];
            if (ptrs.Length > 0 && startLoc != 0)
                ptrs[0] = BitConverter.ToInt32(fileData, internalPointers[0]); // internalPointers[0];
            for (int i = 1; i < ptrs.Length; i++)
            {
                internalPointers[i] -= startAddress;
                ptrs[i] = BitConverter.ToInt32(fileData, internalPointers[i]) - startAddress;
            }
            int[] sortedPtrs = new int[ptrs.Length];
            Array.Copy(ptrs, sortedPtrs, ptrs.Length);
            Array.Sort(sortedPtrs);

            ArrayList temp = new ArrayList();
            temp.Add(0);
            if (sortedPtrs.Length > 0)
            {
                if(sortedPtrs[0] > 0)
                    temp.Add(sortedPtrs[0]);

                for (int i = 1; i < sortedPtrs.Length; i++)
                {
                    if (sortedPtrs[i] != sortedPtrs[i - 1] && sortedPtrs[i] > 0)
                    {
                        temp.Add(sortedPtrs[i]);
                    }
                }
            }
            condensedPtrs = (int[])temp.ToArray(typeof(int));
            splitFileData = new byte[condensedPtrs.Length][];
            for(int i = 0; i < condensedPtrs.Length - 1; i++)
            {
                splitFileData[i] = new byte[condensedPtrs[i + 1] - condensedPtrs[i]];
                Array.Copy(fileData, condensedPtrs[i], splitFileData[i], 0, Math.Min(splitFileData[i].Length, fileData.Length - condensedPtrs[i]));
            }
            splitFileData[splitFileData.Length - 1] = new byte[fileData.Length - condensedPtrs[splitFileData.Length - 1]];
            Array.Copy(fileData, condensedPtrs[splitFileData.Length - 1], splitFileData[splitFileData.Length - 1], 0, splitFileData[splitFileData.Length - 1].Length);
            StringWriter beta = new StringWriter();
            WriteFileToStream(beta);
            /*bigassfile = new string[condensedPtrs.Length * 4];
            bigassfile[0] = "Header:";
            bigassfile[1] = BitConverter.ToString(splitFileData[0]).Replace("-", "");
            bigassfile[2] = bigassfile[3] = "";
            for (int i = 1; i < splitFileData.Length; i++)
            {
                bigassfile[i * 4] = "(" + i + ")";
                bigassfile[i * 4 + 1] = BitConverter.ToString(splitFileData[i]).Replace("-", "");
                bigassfile[i * 4 + 2] = bigassfile[i * 4 + 3] = "";
                //textBox1.Text bigassfile += BitConverter.ToString(splitFileData[i]).Replace("-", "");
                //textBox1.Text bigassfile += "\r\n\r\n";
                //textBox1.AppendText(bigassfile[i * 4]);
                //textBox1.AppendText(bigassfile[i * 4 + 1]);
            //}*/
            if (fileData.Length < 10000000)//0x4400)
            {
                textBox1.SuspendLayout();
                textBox1.Text = beta.ToString();
                textBox1.ResumeLayout();
            }
            else
                textBox1.Text = "Too large for preview (TRY AGAIN NEXT VERSION)";
            //if (startAddress == 0)
            //    startAddress = startAddress;
        }

        public BinaryViewer(byte[] fileData)
            : this()
        {
            splitFileData = new byte[1][];
            splitFileData[0] = new byte[fileData.Length];
            Array.Copy(fileData, splitFileData[0], fileData.Length);
            string fileContents = BitConverter.ToString(fileData).Replace("-", "");
            hasPointers = false;
            if (fileData.Length < 100000000)//0x4400)
            {
                textBox1.SuspendLayout();
                textBox1.Text = fileContents;
                textBox1.ResumeLayout();
            }
            else
                textBox1.Text = "Too large for preview (TRY AGAIN NEXT VERSION)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream alpha = (FileStream)saveFileDialog1.OpenFile();
                StreamWriter beta = new StreamWriter(alpha);
                WriteFileToStream(beta);
                beta.Close();
            }
        }

        private void WriteFileToStream(TextWriter beta)
        {
            if (hasPointers)
            {
                beta.WriteLine("Header:");
                beta.WriteLine(ASCIIEncoding.ASCII.GetString(splitFileData[0], 0, 4).Replace("\0", ""));
                for (int i = 4; i < splitFileData[0].Length; i++)
                {
                    if (internalPointers.Contains(i))
                    {
                        if (i != 4)
                            beta.WriteLine();
                        beta.Write(BitConverter.ToString(splitFileData[0], i, 4).Replace("-", ""));
                        beta.WriteLine(" -- pointer to (" + Array.IndexOf(condensedPtrs, ptrs[Array.IndexOf(internalPointers, i)]) + ")");
                        i += 3;
                    }
                    else
                        beta.Write(BitConverter.ToString(splitFileData[0], i, 1));
                }
                beta.WriteLine();
                beta.WriteLine();
                //for (int i = 0; i < bigassfile.Length; i++)
                //    beta.WriteLine(bigassfile[i]);
                bool previousPtr = true;
                for (int i = 1; i < splitFileData.Length; i++)
                {
                    beta.WriteLine("(" + i + ")");
                    previousPtr = true;
                    int lineLength = 0;
                    for (int j = 0; j < splitFileData[i].Length; j++)
                    {
                        if (internalPointers.Contains(condensedPtrs[i] + j))
                        {
                            if (!previousPtr)
                                beta.WriteLine();
                            beta.Write(BitConverter.ToString(splitFileData[i], j, 4).Replace("-", ""));
                            beta.WriteLine(" -- pointer to (" + Array.IndexOf(condensedPtrs, ptrs[Array.IndexOf(internalPointers, condensedPtrs[i] + j)]) + ")");
                            //beta.WriteLine(" -- pointer to (" + Array.IndexOf(internalPointers, condensedPtrs[i] + j) + ")");
                            j += 3;
                            previousPtr = true;
                            lineLength = 0;
                        }
                        else
                        {
                            previousPtr = false;
                            beta.Write(BitConverter.ToString(splitFileData[i], j, 1));
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
            else
            {
                for (int i = 0; i < splitFileData[0].Length; i++)
                {
                    beta.Write(BitConverter.ToString(splitFileData[0], i, 1));
                    if (i > 0)
                    {
                        if (i % 4 == 0)
                            beta.Write(" ");
                        if (i % 16 == 0)
                            beta.WriteLine();
                    }
                }
            }
        }
    }
}
