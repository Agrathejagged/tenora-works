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
    public partial class ThinkFakis1MotherBrainViewer : UserControl
    {
        ThinkFakis1MotherBrainFile thinkFile;

        public ThinkFakis1MotherBrainViewer()
        {
            InitializeComponent();
        }

        public ThinkFakis1MotherBrainViewer(ThinkFakis1MotherBrainFile ThinkFakis1MotherBrain) : this()
        {
            thinkFile = ThinkFakis1MotherBrain;
            StringBuilder strBuild = new StringBuilder();
            
            strBuild.AppendLine("Filename: " + thinkFile.filename);
            strBuild.AppendLine("Float group 1: (" + thinkFile.floatGroup1Entry1 + ", " + thinkFile.floatGroup1Entry2 + ", " + thinkFile.floatGroup1Entry3 + ")");
            strBuild.AppendLine("Float group 2: (" + thinkFile.floatGroup2Entry1 + ", " + thinkFile.floatGroup2Entry2 + ", " + thinkFile.floatGroup2Entry3 + ")");
            strBuild.AppendLine("\tGroup 1: ");
            for(int i = 0; i < thinkFile.groupOneEntries.Count; i++)
            {
                ThinkFakis1MotherBrainFile.GroupOneEntry gr1 = thinkFile.groupOneEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.byte1 + "\t" + gr1.byte2 + "\t" + gr1.byte3 + "\t" + gr1.byte4);
            }
            strBuild.AppendLine();

            strBuild.AppendLine("\tGroup 2: ");
            for (int i = 0; i < thinkFile.groupTwoEntries.Count; i++)
            {
                ThinkFakis1MotherBrainFile.GroupTwoEntry gr1 = thinkFile.groupTwoEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.short1 + "\t" + gr1.short2);
            }
            strBuild.AppendLine();

            strBuild.AppendLine("\tGroup 2: ");
            for (int i = 0; i < thinkFile.groupThreeEntries.Count; i++)
            {
                ThinkFakis1MotherBrainFile.GroupThreeEntry gr1 = thinkFile.groupThreeEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.float1 + "\t" + gr1.float2);
            }

            richTextBox1.Text = strBuild.ToString();
        }
    }
}
