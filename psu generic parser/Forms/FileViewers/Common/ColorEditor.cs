using PSULib.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace psu_generic_parser.Forms.FileViewers.Common
{
    public partial class ColorEditor : UserControl
    {
        private Color3f ownedColor = null;
        public ColorEditor()
        {
            InitializeComponent();
        }

        public void SetColor(Color3f color)
        {
            ownedColor = color;
            updateUserComponents();
        }

        private void updateUserComponents()
        {
            redUpDown.Value = (decimal)ownedColor.R;
            greenUpDown.Value = (decimal)ownedColor.G;
            blueUpDown.Value = (decimal)ownedColor.B;
            previewBox.BackColor = ownedColor.ToColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ownedColor != null)
            {
                colorChooser.Color = ownedColor.ToColor();

                if (colorChooser.ShowDialog() == DialogResult.OK)
                {
                    Color chosenColor = colorChooser.Color;
                    ownedColor.LoadColorValues(chosenColor);
                    updateUserComponents();
                }
            }
        }

        private void redUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(ownedColor != null)
            {
                ownedColor.R = (float)redUpDown.Value;
                updateUserComponents();
            }

        }

        private void greenUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ownedColor != null)
            {
                ownedColor.G = (float)greenUpDown.Value;
                updateUserComponents();
            }
        }

        private void blueUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ownedColor != null)
            {
                ownedColor.B = (float)blueUpDown.Value;
                updateUserComponents();
            }
        }
    }
}
