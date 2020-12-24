using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    public class XntFile : PsuFile
    {
        public class XntTextureEntry
        {
            public int mysteryIndex { get; set; }
            public string filename { get; set; }
            public short firstValue { get; set; }
            public short secondValue { get; set; }
            public int thirdValue { get; set; }
            public int fourthValue { get; set; }
        }

        public List<XntTextureEntry> fileEntries = new List<XntTextureEntry>();

        public XntFile(string inFilename, List<string> names)
        {
            filename = inFilename;
            for(int i = 0; i < names.Count; i++)
            {
                XntTextureEntry entry = new XntTextureEntry();
                entry.mysteryIndex = 0;
                entry.filename = names[i];
                entry.firstValue = 4;
                entry.secondValue = 1;
                fileEntries.Add(entry);
            }
        }

        public XntFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            header = subHeader;
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int topLevelLoc = inReader.ReadInt32();
            inStream.Seek(topLevelLoc, SeekOrigin.Begin);
            int stringCount = inReader.ReadInt32();
            int startLoc = inReader.ReadInt32() - baseAddr;

            inStream.Seek(startLoc, SeekOrigin.Begin);
            int[] stringLocs = new int[stringCount];
            for (int i = 0; i < stringCount; i++)
            {
                XntTextureEntry entry = new XntTextureEntry();
                entry.mysteryIndex = inReader.ReadInt32();
                stringLocs[i] = inReader.ReadInt32() - baseAddr;
                entry.firstValue = inReader.ReadInt16();
                entry.secondValue = inReader.ReadInt16();
                entry.thirdValue = inReader.ReadInt32();
                entry.fourthValue = inReader.ReadInt32();
                fileEntries.Add(entry);
            }

            for(int i = 0; i < stringCount; i++)
            {
                inStream.Seek(stringLocs[i], SeekOrigin.Begin);
                List<byte> bytes = new List<byte>();
                byte currentChar = inReader.ReadByte();
                while(currentChar != 0)
                {
                    bytes.Add(currentChar);
                    currentChar = inReader.ReadByte();
                }
                fileEntries[i].filename = Encoding.GetEncoding("shift-jis").GetString(bytes.ToArray());
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            List<int> ptrList = new List<int>();

            //Step 1: Write each string, keeping their offset.
            int[] textOffsets = new int[fileEntries.Count];
            int listLoc = 0x10;

            outStream.Seek(0x10 + 0x14 * textOffsets.Length + 0x8, SeekOrigin.Begin);
            for (int i = 0; i < textOffsets.Length; i++)
            {
                textOffsets[i] = (int)outStream.Position;
                outWriter.Write(Encoding.GetEncoding("shift-jis").GetBytes(fileEntries[i].filename));
                outWriter.Write(new byte[4 - (outStream.Position % 4)]);
            }

            outStream.Seek(listLoc, SeekOrigin.Begin);
            for (int i = 0; i < textOffsets.Length; i++)
            {
                outWriter.Write(fileEntries[i].mysteryIndex);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(textOffsets[i]);
                outWriter.Write(fileEntries[i].firstValue);
                outWriter.Write(fileEntries[i].secondValue);
                outWriter.Write(fileEntries[i].thirdValue);
                outWriter.Write(fileEntries[i].fourthValue);
            }

            int headerLoc = (int)outStream.Position;
            outWriter.Write(textOffsets.Length);
            ptrList.Add((int)outStream.Position);
            outWriter.Write(listLoc);
            int fileEnd = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(((uint)0x4C54584E));
            outWriter.Write(fileEnd);
            outWriter.Write(headerLoc);
            outWriter.Write((int)0);

            calculatedPointers = ptrList.ToArray();

            //Build subheader
            header = new byte[0x20];
            MemoryStream headerStream = new MemoryStream(header);
            BinaryWriter headerWriter = new BinaryWriter(headerStream);
            headerWriter.Write((int)0x4649584E);
            headerWriter.Write((int)0x18);
            headerWriter.Write((int)1);
            headerWriter.Write((int)0x20);
            headerWriter.Write(fileEnd);
            headerWriter.Write(fileEnd + 0x20);
            headerWriter.Write((int)(((calculatedPointers.Length * 4) + 0x1F) & 0xFFFFFFE0));
            headerWriter.Write((int)1);

            return outStream.ToArray();
        }
    }
}
