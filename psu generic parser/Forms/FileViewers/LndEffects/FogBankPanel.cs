using PSULib.FileClasses.Maps.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.LndEffects
{
    public partial class FogBankPanel : UserControl
    {
        private LndFog fog = null;
        public LndFog Fog { 
            get { return fog; } 
            set  {
                fog = value; updateUserControls();
            } 
        }

        public FogBankPanel()
        {
            InitializeComponent();
        }

        private void updateUserControls()
        {
            if(fog != null)
            {
                colorEditor1.SetColor(fog.FogColor);
                nearPlaneUpDown.Value = (decimal)fog.NearPlane;
                farPlaneUpDown.Value = (decimal)fog.FarPlane;
                initialIntensityUpDown.Value = (decimal)fog.InitialIntensity;
                rampSpeedUpDown.Value = (decimal)fog.RampUp;
            }
        }

        private void nearPlaneUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (fog != null)
            {
                fog.NearPlane = (float)nearPlaneUpDown.Value;
            }

        }

        private void farPlaneUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (fog != null)
            {
                fog.FarPlane = (float)farPlaneUpDown.Value;
            }
        }

        private void initialIntensityUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (fog != null)
            {
                fog.InitialIntensity = (float)initialIntensityUpDown.Value;
            }
        }

        private void rampSpeedUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (fog != null)
            {
                fog.RampUp = (float)rampSpeedUpDown.Value;
            }
        }
    }
}
