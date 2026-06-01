using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    public class Color4f
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }
        public Color4f(float R, float G, float B, float A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public Color4f()
        {
            R = G = B = A = 0.0f;
        }

        public Color4f(BinaryReader inReader)
        {
            R = inReader.ReadSingle();
            G = inReader.ReadSingle();
            B = inReader.ReadSingle();
            A = inReader.ReadSingle();
        }

        public void Write(BinaryWriter outWriter)
        {
            outWriter.Write(R);
            outWriter.Write(G);
            outWriter.Write(B);
            outWriter.Write(A);
        }

        public Color ToColor()
        {
            return Color.FromArgb(Math.Min((int)(A * 255), 255), Math.Min((int)(R * 255), 255), Math.Min((int)(G * 255), 255), Math.Min((int)(B * 255), 255));
        }

        public void LoadRgbValues(Color chosenColor)
        {
            R = (chosenColor.R) / 255f;
            G = (chosenColor.G) / 255f;
            B = (chosenColor.B) / 255f;
        }
    }
}
