using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace psu_generic_parser
{
    public partial class PointeredFileViewer : UserControl
    {
        private PointeredFile internalFile;
        private StringWriter beta;
        private Dictionary<int, int> pointerLineRefs; //Holds references to the lines of particular pointers
        private Dictionary<int, int> pointedLineRefs; //Holds reference to split data lines
        private int writerLine;                       //Tracks the current line the StringWriter is on so we don't have to manually calculate that.
        private List<int> condensedPointers;          //Utility to avoid having to jump around in the file constantly.

        public PointeredFileViewer(PointeredFile toImport)
        {
            InitializeComponent();
            internalFile = toImport;
            richTextBox1.Text = "";
            if (internalFile.splitData != null)
            {
                condensedPointers = new List<int>(internalFile.destinationToPointers.Keys);
                condensedPointers.Sort();
                beta = new StringWriter();
                pointerLineRefs = new Dictionary<int, int>();
                pointedLineRefs = new Dictionary<int, int>(); pointedLineRefs.Add(0, 0);
                writerLine = 0;

                writeTrackedLine("Header:");
                writeTrackedLine(Encoding.ASCII.GetString(internalFile.splitData[0].contents, 0, 4).Replace("\0", ""));
                for (int i = 4; i < internalFile.splitData[0].contents.Length; i++)
                {
                    if (internalFile.pointerToDestination.ContainsKey(i))
                    {
                        if (i != 4)
                            writeTrackedLine();
                        uint ptr = (uint)internalFile.pointerToDestination[i];
                        uint index = ptr;
                        pointerLineRefs.Add(i, writerLine);
                        if (!internalFile.isBigEndian)
                        {
                            ptr = PointeredFile.ReverseBytes(ptr);
                        }
                        beta.Write(ptr.ToString("X8").Replace("-", ""));
                        writeTrackedLine(" -- pointer to (" + (condensedPointers.IndexOf((int)index) + 1) + ")");
                        i += 3;
                    }
                    else
                        beta.Write(BitConverter.ToString(internalFile.splitData[0].contents, i, 1));
                }
                writeTrackedLine();
                writeTrackedLine();

                bool previousPtr = true;
                int displayNum = 1;
                int currentAddress = internalFile.splitData[1].chunkLocation;
                for (int i = 1; i < internalFile.splitData.Count; i++)
                {
                    pointedLineRefs.Add(i, writerLine);
                    if (internalFile.splitData[i].contents.Length > 0)
                    {
                        writeTrackedLine("(" + displayNum + ")");
                        displayNum++;
                        previousPtr = true;
                        int lineLength = 0;
                        for (int j = 0; j < internalFile.splitData[i].contents.Length; j++)
                        {
                            if (internalFile.pointerToDestination.ContainsKey(currentAddress))
                            {
                                if (!previousPtr)
                                    writeTrackedLine();
                                uint ptr = (uint)internalFile.pointerToDestination[currentAddress];
                                uint index = ptr;
                                pointerLineRefs.Add(currentAddress, writerLine);
                                if (!internalFile.isBigEndian)
                                {
                                    ptr = PointeredFile.ReverseBytes(ptr);
                                }
                                beta.Write(ptr.ToString("X8").Replace("-", ""));
                                writeTrackedLine(" -- pointer to (" + (condensedPointers.IndexOf((int)index) + 1) + ")");
                                j += 3;
                                currentAddress += 4;
                                previousPtr = true;
                                lineLength = 0;
                            }
                            else
                            {
                                previousPtr = false;
                                beta.Write(BitConverter.ToString(internalFile.splitData[i].contents, j, 1));
                                lineLength++;
                                if (lineLength % 4 == 0)
                                    beta.Write(" ");
                                if (lineLength == 16)
                                {
                                    writeTrackedLine();
                                    lineLength = 0;
                                }
                                currentAddress++;
                            }
                        }
                        writeTrackedLine();
                        writeTrackedLine();
                    }
                }
                richTextBox1.Text = beta.ToString();

                if (!internalFile.isBrokenFile)
                {
                    //Set up the treeview
                    TreeNode rootNode = new TreeNode();
                    rootNode.Text = "File root @ 0";
                    rootNode.Tag = 0;
                    treeView1.Nodes.Insert(1, rootNode);

                    for (int i = 1; i < internalFile.splitData.Count; i++)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = $"({i}) @ {internalFile.splitData[i].chunkLocation.ToString("X").Replace("-", "")}";
                        treeNode.Tag = internalFile.splitData[i].chunkLocation;

                        treeView1.Nodes.Insert(treeView1.Nodes.Count + 1, treeNode);
                        populateSubPointersShallow(treeNode);
                    }
                }
            }
            else
            {
                condensedPointers = new List<int>();
            }
        }

        private void populateSubPointersShallow(TreeNode node)
        {
            int nodeStart = (int)node.Tag;
            if (node.Nodes.Count == 0 && (internalFile.splitData.Count < 2 || nodeStart >= internalFile.splitData[1].chunkLocation))
            {

                //Check within the range of the current pointer's data if there's subpointers and add them and data references as needed
                foreach(int currentPointer in internalFile.destinationToPointers[nodeStart].OrderBy(val=>val))
                {
                    //Possible future optimization: is it actually likely that one destination will be pointed multiple times from the same origin? I don't think so...
                    //But if it was, this could probably be made to have a "start from" location.
                    int originGroup = findGroup(currentPointer);
                    TreeNode newNode = new TreeNode();
                    newNode.Text = getNodeLabel(originGroup, currentPointer);
                    newNode.Tag = internalFile.splitData[originGroup].chunkLocation;
                    node.Nodes.Add(newNode);
                }
            }
        }

        private int findGroup(int pointerOrigin)
        {
            for(int i = 0; i < internalFile.splitData.Count; i ++)
            {
                var chunk = internalFile.splitData[i];
                int chunkStart = chunk.chunkLocation; //just for ease of writing/reading
                if (chunkStart < pointerOrigin && chunkStart + chunk.contents.Length >= pointerOrigin)
                {
                    return i;
                }
            }
            return -1;
        }

        private string getNodeLabel(int originIndex, int originLocation = 0)
        {
                return "From " + originLocation.ToString("X").Replace("-", "") + $" / group ({originIndex}) @ {internalFile.splitData[originIndex].chunkLocation}";
        }

        private void writeTrackedLine(string str = "")
        {
            beta.WriteLine(str);
            writerLine++;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int index = condensedPointers.IndexOf((int)treeView1.SelectedNode.Tag) + 1;
                gotoLine(pointedLineRefs[index]);
            }
        }

        //Marsh-wiggle's richTextBox gotoLine solution here:
        //https://stackoverflow.com/questions/4323105/how-can-i-scroll-to-a-specified-line-number-of-a-richtextbox-control-using-c/37657901
        void gotoLine(int wantedLine_zero_based) // int wantedLine_zero_based = wanted line number; 1st line = 0
        {
            int index = this.richTextBox1.GetFirstCharIndexFromLine(wantedLine_zero_based);
            this.richTextBox1.Select(index, 0);
            this.richTextBox1.ScrollToCaret();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node.Nodes.Count > 0)
            {
                foreach(TreeNode node in e.Node.Nodes)
                {
                    populateSubPointersShallow(node);
                }
            }
        }
    }
}
