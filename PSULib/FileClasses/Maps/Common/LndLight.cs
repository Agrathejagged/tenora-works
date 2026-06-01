using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps.Common
{
    public class LndLight
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Color3f LightColor { get; set; }

        public LndLight(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            LightColor = reader.ReadColor3f();
        }

        public LndLight() { }
    }

    public static class BinaryWriterLightSupport
    {
        public static void Write(this BinaryWriter writer, LndLight light)
        {
            writer.Write(light.X);
            writer.Write(light.Y);
            writer.Write(light.Z);
            writer.Write(light.LightColor);
        }
    }
}
