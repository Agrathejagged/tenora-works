using PSULib.FileClasses.General;
using PSULib.FileClasses.Maps.Common;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps
{
    public class LndEffectFile : PsuFile
    {
        public class Gradient
        {
            public float StartHeight { get; set; }
            public float EndHeight { get; set; }
            public Color4f StartColor { get; set; }
            public Color4f EndColor { get; set; }
            public float GradientMultiplier { get; set; }
            public float DestinationMultiplier { get; set; }
        }

        public LndLight PlayerLight1 { get; set; }
        public LndLight PlayerLight2 { get; set; }
        public LndLight PlayerLightAmbient { get; set; }
        public Gradient TopGradient { get; set; }
        public Gradient BottomGradient { get; set; }
        public LndFog Fog { get; set; }

        public float SunX { get; set; }
        public float SunY { get; set; }
        public float SunZ { get; set; }
        public float SunUnknown { get; set; }

        public float BlurStartDistance { get; set; }
        public float BlurUnknown { get; set; }
        public int BlurPixelCount { get; set; }
        public float BlurDistance { get; set; }
        public float BlurOpacity { get; set; }


        public LndEffectFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            List<int> pointerList = new List<int>(ptrs.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                pointerList.Add(ptrs[i] - baseAddr);
            }
            calculatedPointers = pointerList.ToArray();
            byte[] headers = inReader.ReadBytes(4);
            int fileLength = inReader.ReadInt32();
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int lightLoc1 = inReader.ReadInt32() - baseAddr;
            int lightLoc2 = inReader.ReadInt32() - baseAddr;
            int lightLoc3 = inReader.ReadInt32() - baseAddr;
            int gradientLoc1 = inReader.ReadInt32() - baseAddr;
            int fogLoc = inReader.ReadInt32() - baseAddr;
            int sunLoc = inReader.ReadInt32() - baseAddr;
            int blurLoc = inReader.ReadInt32() - baseAddr;

            inStream.Seek(lightLoc1, SeekOrigin.Begin);
            PlayerLight1 = new LndLight(inReader);
            inStream.Seek(lightLoc2, SeekOrigin.Begin);
            PlayerLight2 = new LndLight(inReader);
            inStream.Seek(lightLoc3, SeekOrigin.Begin);
            PlayerLightAmbient = new LndLight(inReader);

            inStream.Seek(gradientLoc1, SeekOrigin.Begin);
            TopGradient = readGradient(inReader);
            BottomGradient = readGradient(inReader);

            inStream.Seek(fogLoc, SeekOrigin.Begin);
            Fog = new LndFog(inReader);

            inStream.Seek(sunLoc, SeekOrigin.Begin);
            SunX = inReader.ReadSingle();
            SunY = inReader.ReadSingle();
            SunZ = inReader.ReadSingle();
            SunUnknown = inReader.ReadSingle();

            inStream.Seek(blurLoc, SeekOrigin.Begin);
            BlurStartDistance = inReader.ReadSingle();
            BlurUnknown = inReader.ReadSingle();
            BlurPixelCount = inReader.ReadInt32();
            BlurDistance = inReader.ReadSingle();
            BlurOpacity = inReader.ReadSingle();
        }

        private Gradient readGradient(BinaryReader inReader)
        {
            Gradient gradient = new Gradient();
            gradient.StartHeight = inReader.ReadSingle();
            gradient.EndHeight = inReader.ReadSingle();
            gradient.StartColor = inReader.ReadColor4f();
            gradient.EndColor = inReader.ReadColor4f();
            gradient.GradientMultiplier = inReader.ReadSingle();
            gradient.DestinationMultiplier = inReader.ReadSingle();
            return gradient;
        }

        public override byte[] ToRaw()
        {
            calculatedPointers = new int[7];
            MemoryStream outStream = new MemoryStream(0x110); //This one's pretty standard sized.
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            int lightLoc1 = (int)outStream.Position;
            outWriter.Write(PlayerLight1);
            int lightLoc2 = (int)outStream.Position;
            outWriter.Write(PlayerLight2);
            int lightLoc3 = (int)outStream.Position;
            outWriter.Write(PlayerLightAmbient);

            int gradientLoc = (int)outStream.Position;
            writeGradient(outWriter, TopGradient);
            writeGradient(outWriter, BottomGradient);

            int fogLoc = (int)outStream.Position;
            Fog.Write(outWriter);

            int sunLoc = (int)outStream.Position;
            outWriter.Write(SunX);
            outWriter.Write(SunY);
            outWriter.Write(SunZ);
            outWriter.Write(SunUnknown);

            int blurLoc = (int)outStream.Position;
            outWriter.Write(BlurStartDistance);
            outWriter.Write(BlurUnknown);
            outWriter.Write(BlurPixelCount);
            outWriter.Write(BlurDistance);
            outWriter.Write(BlurOpacity);

            int headerLoc = (int)outStream.Position;
            calculatedPointers[0] = (int)outStream.Position;
            outWriter.Write(lightLoc1);
            calculatedPointers[1] = (int)outStream.Position;
            outWriter.Write(lightLoc2);
            calculatedPointers[2] = (int)outStream.Position;
            outWriter.Write(lightLoc3);
            calculatedPointers[3] = (int)outStream.Position;
            outWriter.Write(gradientLoc);
            calculatedPointers[4] = (int)outStream.Position;
            outWriter.Write(fogLoc);
            calculatedPointers[5] = (int)outStream.Position;
            outWriter.Write(sunLoc);
            calculatedPointers[6] = (int)outStream.Position;
            outWriter.Write(blurLoc);
            //Probably padding, but who knows.
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(0x110);
            outWriter.Write(headerLoc);
            outWriter.Write(0);

            header = buildSubheader((int)outStream.Length);
            return outStream.ToArray();
        }

        private void writeGradient(BinaryWriter outWriter, Gradient gradient)
        {
            outWriter.Write(gradient.StartHeight);
            outWriter.Write(gradient.EndHeight);
            outWriter.Write(gradient.StartColor);
            outWriter.Write(gradient.EndColor);
            outWriter.Write(gradient.GradientMultiplier);
            outWriter.Write(gradient.DestinationMultiplier);
        }
    }
}
