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
    public partial class GradientEditorPanel : UserControl
    {
        LndEffectFile.Gradient editedGradient = null;
        public delegate void ValuesChanged();
        public ValuesChanged ValuesChangedListeners;

        public LndEffectFile.Gradient EditedGradient 
        { 
            get => editedGradient; 
            set { editedGradient = value; loadValues(); } 
        }

        public GradientEditorPanel()
        {
            InitializeComponent();
        }

        private void loadValues()
        {
            if (editedGradient != null)
            {
                thicknessUpDown.Value = (decimal)editedGradient.GradientMultiplier; 
                unknownValueUpDown.Value = (decimal)editedGradient.DestinationMultiplier;
                startHeightUpDown.Value = (decimal)editedGradient.StartHeight;
                startRedUpDown.Value = (decimal)editedGradient.StartColor.R;
                startGreenUpDown.Value = (decimal)editedGradient.StartColor.G;
                startBlueUpDown.Value = (decimal)editedGradient.StartColor.B;
                startOpacityUpDown.Value = (decimal)editedGradient.StartColor.A;
                endHeightUpDown.Value = (decimal)editedGradient.EndHeight;
                endRedUpDown.Value = (decimal)editedGradient.EndColor.R;
                endGreenUpDown.Value = (decimal)editedGradient.EndColor.G;
                endBlueUpDown.Value = (decimal)editedGradient.EndColor.B;
                endOpacityUpDown.Value = (decimal)editedGradient.EndColor.A;
            }
        }

        private void thicknessUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(editedGradient != null)
            {
                editedGradient.GradientMultiplier = (float)thicknessUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void destinationMultiplierUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.DestinationMultiplier = (float)unknownValueUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void startHeightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.StartHeight = (float)startHeightUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void startRedUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.StartColor.R = (float)startRedUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void startGreenUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.StartColor.G = (float)startGreenUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void startBlueUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.StartColor.B = (float)startBlueUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void startOpacityUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.StartColor.A = (float)startOpacityUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void endHeightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.EndHeight = (float)endHeightUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void endRedUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.EndColor.R = (float)endRedUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void endGreenUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.EndColor.G = (float)endGreenUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void endBlueUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.EndColor.B = (float)endBlueUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void endOpacityUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                editedGradient.EndColor.A = (float)endOpacityUpDown.Value;
                if (ValuesChangedListeners != null)
                {
                    ValuesChangedListeners.Invoke();
                }
            }
        }

        private void chooseStartColorButton_Click(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                gradientColorChooseDialog.Color = editedGradient.StartColor.ToColor();

                if (gradientColorChooseDialog.ShowDialog() == DialogResult.OK)
                {
                    Color chosenColor = gradientColorChooseDialog.Color;
                    editedGradient.StartColor.LoadRgbValues(chosenColor);
                    loadValues();
                }
            }
        }

        private void chooseEndColorButton_Click(object sender, EventArgs e)
        {
            if (editedGradient != null)
            {
                gradientColorChooseDialog.Color = editedGradient.EndColor.ToColor();

                if (gradientColorChooseDialog.ShowDialog() == DialogResult.OK)
                {
                    Color chosenColor = gradientColorChooseDialog.Color;
                    editedGradient.EndColor.LoadRgbValues(chosenColor);
                    loadValues();
                }
            }
        }
    }
}
