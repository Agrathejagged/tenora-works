using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps.Common
{
    public class LndFog
    {
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public float InitialIntensity { get; set; }
        public float RampUp { get; set; }
        public Color3f FogColor { get; set; }

        public LndFog(BinaryReader inReader)
        {
            NearPlane = inReader.ReadSingle();
            FarPlane = inReader.ReadSingle();
            InitialIntensity = inReader.ReadSingle();
            RampUp = inReader.ReadSingle();
            FogColor = inReader.ReadColor3f();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(NearPlane);
            writer.Write(FarPlane);
            writer.Write(InitialIntensity);
            writer.Write(RampUp);
            writer.Write(FogColor);
        }
    }
}
