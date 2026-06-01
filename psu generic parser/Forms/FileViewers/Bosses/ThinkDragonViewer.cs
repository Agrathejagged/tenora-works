using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSULib.FileClasses.Bosses;

namespace psu_generic_parser.FileViewers
{
    public partial class ThinkDragonViewer : UserControl
    {
        ThinkDragonFile thinkFile;

        public ThinkDragonViewer()
        {
            InitializeComponent();
        }

        public ThinkDragonViewer(ThinkDragonFile thinkDragon) : this()
        {
            thinkFile = thinkDragon;
            StringBuilder strBuild = new StringBuilder();
            
            ThinkDragonFile.GroupOneEntry gr1 = thinkFile.groupOne;
            strBuild.AppendLine("Filename: " + thinkFile.filename);
            strBuild.AppendLine("\tGroup 1: ");
            strBuild.AppendLine("\t\tFloats: (" + gr1.floatOne + ", " + gr1.floatTwo + "), (" + gr1.floatThree + ", " + gr1.floatFour + ")");
            strBuild.AppendLine("\t\tBytes: " + gr1.byteOne + ", " + gr1.byteTwo);
            strBuild.AppendLine("\t\tFloat-float list:");
            for(int i = 0; i < gr1.groupOneChildOne.Count; i++)
            {
                strBuild.AppendLine("\t\t\t" + i + ": " + gr1.groupOneChildOne[i]);
            }

            strBuild.AppendLine("\t\tFloat-float-float list:");
            for (int i = 0; i < gr1.groupOneChildTwo.Count; i++)
            {
                strBuild.AppendLine("\t\t\t" + i + ": " + gr1.groupOneChildTwo[i]);
            }
            strBuild.AppendLine();

            ThinkDragonFile.GroupTwoEntry gr2 = thinkFile.groupTwo;
            strBuild.AppendLine("\tGroup 2:");
            strBuild.AppendLine("\tBytes: " + gr2.byte1 + ", " + gr2.byte2 + ", " + gr2.byte3);
            strBuild.AppendLine("\tTuple tree 1:");
            for(int i = 0; i < gr2.branchOne.Count; i++)
            {
                strBuild.AppendLine("\t\tBranch " + i);
                for (int j = 0; j < gr2.branchOne[i].Count; j++)
                {
                    strBuild.AppendLine("\t\t\t" + j + ": " + gr2.branchOne[i][j]);
                }
            }
            strBuild.AppendLine("\tTuple tree 2:");
            for (int i = 0; i < gr2.branchTwo.Count; i++)
            {
                strBuild.AppendLine("\t\tBranch " + i);
                for (int j = 0; j < gr2.branchTwo[i].Count; j++)
                {
                    strBuild.AppendLine("\t\t\t" + j + ": " + gr2.branchTwo[i][j]);
                }
            }
            strBuild.AppendLine("\tTuple tree 3:");
            for (int i = 0; i < gr2.branchThree.Count; i++)
            {
                strBuild.AppendLine("\t\tBranch " + i);
                for (int j = 0; j < gr2.branchThree[i].Count; j++)
                {
                    strBuild.AppendLine("\t\t\t" + j + ": " + gr2.branchThree[i][j]);
                }
            }
            strBuild.AppendLine("\tTuple tree 4:");
            for (int i = 0; i < gr2.branchFour.Count; i++)
            {
                strBuild.AppendLine("\t\tBranch " + i);
                for (int j = 0; j < gr2.branchFour[i].Count; j++)
                {
                    strBuild.AppendLine("\t\t\t" + j + ": " + gr2.branchFour[i][j]);
                }
            }

            strBuild.AppendLine("\tCustom data tree:");
            for (int i = 0; i < gr2.branchFive.Count; i++)
            {
                strBuild.AppendLine("\t\tBranch " + i);
                for (int j = 0; j < gr2.branchFive[i].Count; j++)
                {
                    strBuild.AppendLine("\t\t\t" + j.ToString().PadRight(3, ' ') + ": (" + gr2.branchFive[i][j].unkFloat1 + ", " + gr2.branchFive[i][j].unkFloat2+ ")");
                    strBuild.AppendLine("\t\t\t   : " + gr2.branchFive[i][j].unkByte1 + " " + gr2.branchFive[i][j].unkByte2 + " " + gr2.branchFive[i][j].unkByte3 + " " + gr2.branchFive[i][j].unkByte4 + " " + gr2.branchFive[i][j].unkByte5 + " " + gr2.branchFive[i][j].unkByte6 + " " + gr2.branchFive[i][j].unkByte7);
                    strBuild.AppendLine("\t\t\t   tuple list:");
                    for (int k = 0; k < gr2.branchFive[i][j].tupleList.Count; k++)
                    {
                        strBuild.AppendLine("\t\t\t\t" + k + ": " + gr2.branchFive[i][j].tupleList[k]);
                    }
                }
            }

            richTextBox1.Text = strBuild.ToString();
        }
    }
}
