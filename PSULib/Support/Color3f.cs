using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    public class Color3f
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public Color3f(float R, float G, float B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public Color3f() 
        {
            R = G = B = 0.0f;
        }

        public Color3f(BinaryReader inReader)
        {
            R = inReader.ReadSingle();
            G = inReader.ReadSingle();
            B = inReader.ReadSingle();
        }

        public void Write(BinaryWriter outWriter)
        {
            outWriter.Write(R);
            outWriter.Write(G);
            outWriter.Write(B);
        }

        public Color ToColor()
        {
            return Color.FromArgb(Math.Min((int)(R * 255), 255), Math.Min((int)(G * 255), 255), Math.Min((int)(B * 255), 255));
        }

        public void LoadColorValues(Color chosenColor)
        {
            R = (chosenColor.R) / 255f;
            G = (chosenColor.G) / 255f;
            B = (chosenColor.B) / 255f;
        }
    }
}
