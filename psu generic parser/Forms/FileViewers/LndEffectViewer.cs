using PSULib.FileClasses.Maps;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace psu_generic_parser.Forms.FileViewers
{
    public partial class LndEffectViewer : UserControl
    {
        private LndEffectFile loadedFile;

        private Color topGradientStartColor;
        private Color topGradientEndColor;
        private Color bottomGradientStartColor;
        private Color bottomGradientEndColor;

        public LndEffectViewer(LndEffectFile lndEffectFile)
        {
            loadedFile = lndEffectFile;
            InitializeComponent();
            loadValues();
        }

        private void loadValues()
        {
            topGradientEditorPanel.EditedGradient = loadedFile.TopGradient;
            topGradientEditorPanel.ValuesChangedListeners += refreshGradient;
            bottomGradientEditorPanel.EditedGradient = loadedFile.BottomGradient;
            bottomGradientEditorPanel.ValuesChangedListeners += refreshGradient;
            fogBankPanel1.Fog = loadedFile.Fog;
            lightEditorPanel1.Light = loadedFile.PlayerLight1;
            lightEditorPanel2.Light = loadedFile.PlayerLight2;
            lightEditorPanel3.Light = loadedFile.PlayerLightAmbient;

            sunXUpDown.Value = (decimal)loadedFile.SunX;
            sunYUpDown.Value = (decimal)loadedFile.SunY;
            sunZUpDown.Value = (decimal)loadedFile.SunZ;
            sunUnknownUpDown.Value = (decimal)loadedFile.SunUnknown;

            blurAfterImageDistanceUpDown.Value = (decimal)loadedFile.BlurDistance;
            blurOpacityUpDown.Value = (decimal)loadedFile.BlurOpacity;
            blurStartDistanceUpDown.Value = (decimal)loadedFile.BlurStartDistance;
            blurPixelCountUpDown.Value = loadedFile.BlurPixelCount;
            blurUnknownUpDown.Value = (decimal)loadedFile.BlurUnknown;

            refreshGradient();
        }

        private void refreshGradient()
        {
            topGradientStartColor = loadedFile.TopGradient.StartColor.ToColor();
            topGradientEndColor = loadedFile.TopGradient.EndColor.ToColor();
            bottomGradientStartColor = loadedFile.BottomGradient.StartColor.ToColor();
            bottomGradientEndColor = loadedFile.BottomGradient.EndColor.ToColor();
            panel1.Refresh();
        }

        private void sunXUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.SunX = (float)sunXUpDown.Value;
        }

        private void sunYUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.SunY = (float)sunYUpDown.Value;
        }

        private void sunZUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.SunZ = (float)sunZUpDown.Value;
        }

        private void sunUnknownUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.SunUnknown = (float)sunUnknownUpDown.Value;
        }

        private void blurStartDistanceUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.BlurStartDistance = (float)blurStartDistanceUpDown.Value;
        }

        private void blurUnknownUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.BlurUnknown = (float)blurUnknownUpDown.Value;
        }

        private void blurPixelCountUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.BlurPixelCount = (int)blurPixelCountUpDown.Value;
        }

        private void blurAfterImageDistanceUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.BlurDistance = (float)blurAfterImageDistanceUpDown.Value;
        }

        private void blurOpacityUpDown_ValueChanged(object sender, EventArgs e)
        {
            loadedFile.BlurOpacity = (float)blurOpacityUpDown.Value;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            float panelHeight = panel1.Height;
            if (loadedFile.TopGradient.StartHeight != 0)
            {
                float gradientEnd = panelHeight * (loadedFile.TopGradient.EndHeight / 1.75f) / 2.0f;
                LinearGradientBrush topBrush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, gradientEnd), topGradientStartColor, topGradientEndColor);
                e.Graphics.FillRectangle(topBrush, 0, 0, panel1.Width, gradientEnd);
            }
            if (loadedFile.BottomGradient.StartHeight != 0)
            {
                float gradientEnd = panelHeight - (panelHeight * (loadedFile.BottomGradient.EndHeight / 1.75f) / 2.0f);
                LinearGradientBrush bottomBrush = new LinearGradientBrush(new PointF(0, panel1.Height), new PointF(0, gradientEnd), bottomGradientStartColor, bottomGradientEndColor);
                e.Graphics.FillRectangle(bottomBrush, 0, gradientEnd, panel1.Width, panel1.Height);
            }
        }
    }
}
