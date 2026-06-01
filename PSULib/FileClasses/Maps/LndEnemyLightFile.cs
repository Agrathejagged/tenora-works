using PSULib.FileClasses.General;
using PSULib.FileClasses.Maps.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps
{
    public class LndEnemyLightFile : PsuFile
    {
        public LndLight Light1 { get; set; }
        public LndLight Light2 { get; set; }
        public LndLight LightAmbient { get; set; }

        public LndEnemyLightFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            List<int> pointerList = new List<int>(ptrs.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                pointerList.Add(ptrs[i] - baseAddr);
            }
            byte[] headers = inReader.ReadBytes(4);
            int fileLength = inReader.ReadInt32();
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int lightLoc1 = inReader.ReadInt32() - baseAddr;
            int lightLoc2 = inReader.ReadInt32() - baseAddr;
            int lightLoc3 = inReader.ReadInt32() - baseAddr;

            inStream.Seek(lightLoc1, SeekOrigin.Begin);
            Light1 = new LndLight(inReader);
            inStream.Seek(lightLoc2, SeekOrigin.Begin);
            Light2 = new LndLight(inReader);
            inStream.Seek(lightLoc3, SeekOrigin.Begin);
            LightAmbient = new LndLight(inReader);
        }

        public override byte[] ToRaw()
        {
            calculatedPointers = new int[3];
            MemoryStream outStream = new MemoryStream(0x90); //This one's pretty standard sized.
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            int lightLoc1 = (int)outStream.Position;
            outWriter.Write(Light1);
            int lightLoc2 = (int)outStream.Position;
            outWriter.Write(Light2);
            int lightLoc3 = (int)outStream.Position;
            outWriter.Write(LightAmbient);

            int headerLoc = (int)outStream.Position;
            calculatedPointers[0] = (int)outStream.Position;
            outWriter.Write(lightLoc1);
            calculatedPointers[1] = (int)outStream.Position;
            outWriter.Write(lightLoc2);
            calculatedPointers[2] = (int)outStream.Position;
            outWriter.Write(lightLoc3);
            //Probably padding, but who knows.
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);
            outWriter.Write((int)0);

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(0x70);
            outWriter.Write(headerLoc);
            outWriter.Write(0);
            header = buildSubheader((int)outStream.Length);
            return outStream.ToArray();
        }
    }
}
