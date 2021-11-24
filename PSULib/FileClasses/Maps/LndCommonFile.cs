using PSULib.FileClasses.General;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps
{
    public class LndCommonFile : PsuFile
    {
        public string NblFilenameFragment { get; set; } = "scene";
        public string XntFilenameFragment1 { get; set; } = "";
        public string XntFilenameFragment2 { get; set; } = "";
        public float UnknownFloat { get; set; } = 0.0f;

        public LndCommonFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

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
            int floatLocation = inReader.ReadInt32();
            int subListLoc = inReader.ReadInt32();

            if (floatLocation != 0)
            {
                int actualFloatLocation = floatLocation - baseAddr;
                if (actualFloatLocation < rawData.Length)
                {
                    inStream.Seek(actualFloatLocation, SeekOrigin.Begin);
                    UnknownFloat = inReader.ReadSingle();
                }
            }

            if (subListLoc != 0)
            {
                inStream.Seek(subListLoc - baseAddr, SeekOrigin.Begin);
                int xntLoc1 = inReader.ReadInt32();
                int xntLoc2 = inReader.ReadInt32();
                int nblLoc = inReader.ReadInt32();

                if (xntLoc1 != 0)
                {
                    XntFilenameFragment1 = inReader.ReadAsciiString(xntLoc1 - baseAddr);
                }
                if (xntLoc2 != 0)
                {
                    XntFilenameFragment2 = inReader.ReadAsciiString(xntLoc2 - baseAddr);
                }
                if (nblLoc != 0)
                {
                    NblFilenameFragment = inReader.ReadAsciiString(nblLoc - baseAddr);
                }
            }

        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            Dictionary<string, int> stringLocs = new Dictionary<string, int>();
            List<int> pointerLocs = new List<int>();

            outStream.Seek(0x10, SeekOrigin.Begin);

            foreach (string currString in new string[] { XntFilenameFragment1, XntFilenameFragment2, NblFilenameFragment })
            {
                stringLocs[currString] = (int)outStream.Position;
                outWriter.Write(Encoding.GetEncoding("shift-jis").GetBytes(currString));
                outStream.Seek(4 - outStream.Position % 4, SeekOrigin.Current);
            }

            int subListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(stringLocs[XntFilenameFragment1]);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(stringLocs[XntFilenameFragment2]);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(stringLocs[NblFilenameFragment]);

            int topLevelListLoc = (int)outStream.Position;
            int floatLoc = (int)outStream.Position + 8;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(floatLoc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(subListLoc);

            //Sega shoves this into the padding space, but I prefer the filesize to include it.
            outWriter.Write(UnknownFloat);

            int fileLength = (int)outStream.Position;
            outStream.Seek(15 - outStream.Position % 16, SeekOrigin.Current);
            if (outStream.Position > fileLength)
            {
                outWriter.Write((byte)0);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(0x52584E);
            outWriter.Write(fileLength);
            outWriter.Write(topLevelListLoc);
            calculatedPointers = pointerLocs.ToArray();

            header = buildSubheader(fileLength);
            return outStream.ToArray();
        }
    }
}
