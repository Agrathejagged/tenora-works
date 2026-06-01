using PSULib.FileClasses.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class LndEnemyLightViewer : UserControl
    {
        LndEnemyLightFile lightFile;
        public LndEnemyLightViewer(LndEnemyLightFile inFile)
        {
            lightFile = inFile;
            InitializeComponent();
            lightEditorPanel1.Light = lightFile.Light1;
            lightEditorPanel2.Light = lightFile.Light2;
            lightEditorPanel3.Light = lightFile.LightAmbient;
        }
    }
}
