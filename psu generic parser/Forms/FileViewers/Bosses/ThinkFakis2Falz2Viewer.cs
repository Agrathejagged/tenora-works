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
    public partial class ThinkFakis2Falz2Viewer : UserControl
    {
        ThinkFakis2Falz2File thinkFile;

        public ThinkFakis2Falz2Viewer()
        {
            InitializeComponent();
        }

        public ThinkFakis2Falz2Viewer(ThinkFakis2Falz2File ThinkFakis2Falz2) : this()
        {
            thinkFile = ThinkFakis2Falz2;
            StringBuilder strBuild = new StringBuilder();
            
            strBuild.AppendLine("Filename: " + thinkFile.filename);
            strBuild.AppendLine("\tGroup 1: ");
            for(int i = 0; i < thinkFile.groupOneEntries.Count; i++)
            {
                ThinkFakis2Falz2File.GroupOneEntry gr1 = thinkFile.groupOneEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.byte1 + "\t" + gr1.byte2 + "\t" + gr1.byte3 + "\t" + gr1.byte4);
            }
            strBuild.AppendLine();

            strBuild.AppendLine("\tGroup 2: ");
            for (int i = 0; i < thinkFile.groupTwoEntries.Count; i++)
            {
                ThinkFakis2Falz2File.GroupTwoEntry gr1 = thinkFile.groupTwoEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.short1 + "\t" + gr1.short2);
            }
            strBuild.AppendLine();

            strBuild.AppendLine("\tGroup 2: ");
            for (int i = 0; i < thinkFile.groupThreeEntries.Count; i++)
            {
                ThinkFakis2Falz2File.GroupThreeEntry gr1 = thinkFile.groupThreeEntries[i];
                strBuild.AppendLine("\t\t" + i + ":\t" + gr1.float1 + "\t" + gr1.float2);
            }

            richTextBox1.Text = strBuild.ToString();
        }
    }
}
