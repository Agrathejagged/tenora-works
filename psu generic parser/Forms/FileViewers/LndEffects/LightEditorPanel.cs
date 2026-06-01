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
    public partial class LightEditorPanel : UserControl
    {
        private LndLight editedLight = null;
        public delegate void ValuesChanged();
        public ValuesChanged ValuesChangedListeners;

        public LndLight Light
        {
            get => editedLight;
            set { editedLight = value; loadValues(); }
        }

        private void loadValues()
        {
            if (editedLight != null)
            {
                colorEditor3.SetColor(editedLight.LightColor);
                xUpDown.Value = (decimal)editedLight.X;
                yUpDown.Value = (decimal)editedLight.Y;
                zUpDown.Value = (decimal)editedLight.Z;
            }
        }

        public LightEditorPanel()
        {
            InitializeComponent();
        }

        private void xUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(editedLight != null)
            {
                editedLight.X = (float)xUpDown.Value;
            }
        }

        private void yUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedLight != null)
            {
                editedLight.Y = (float)yUpDown.Value;
            }
        }

        private void zUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedLight != null)
            {
                editedLight.Z = (float)zUpDown.Value;
            }
        }
    }
}
