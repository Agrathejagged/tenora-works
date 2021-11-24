using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Items
{
    class ObjectDropFile : PsuFile
    {
        public class DropEntry
        {
            public int item1;
            public int item2;
            public int item3;
            public ushort prob1;
            public ushort prob2;
            public ushort prob3;
            public ushort prob4;
        }

        public DropEntry[][][] drops;
        public short[] dropTiers;

        public ObjectDropFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;
            calculatedPointers = (int[])ptrs.Clone();
            byte[] rawTemp = new byte[rawData.Length];
            Array.Copy(rawData, rawTemp, rawData.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                calculatedPointers[i] -= baseAddr;
                int ptr = BitConverter.ToInt32(rawData, calculatedPointers[i]);
                ptr -= baseAddr;
                Array.Copy(BitConverter.GetBytes(ptr), 0, rawTemp, calculatedPointers[i], 4);
            }
            MemoryStream inStream = new MemoryStream(rawTemp);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int mainPointer = inReader.ReadInt32();

            inStream.Seek(mainPointer, SeekOrigin.Begin);
            //int dropCount = inReader.ReadInt32();
            int tierCount = (calculatedPointers[0] - (mainPointer + 4)) / 2;
            dropTiers = new short[tierCount];
            int[] dropCounts = new int[5];
            int[] dropLocs = new int[5];

            for (int i = 0; i < tierCount; i++)
                dropTiers[i] = inReader.ReadInt16();
            for (int i = 0; i < 5; i++)
            {
                dropCounts[i] = inReader.ReadInt32();
                dropLocs[i] = inReader.ReadInt32();
            }
            drops = new DropEntry[5][][];
            inStream.Seek(0x1C, SeekOrigin.Begin);
            for (int i = 0; i < 5; i++)
            {
                drops[i] = new DropEntry[dropCounts[i]][];
                inStream.Seek(dropLocs[i], SeekOrigin.Begin);
                for (int j = 0; j < dropCounts[i]; j++)
                {
                    drops[i][j] = new DropEntry[tierCount >> 1];
                    for (int k = 0; k < tierCount >> 1; k++)
                    {
                        DropEntry temp = new DropEntry();
                        temp.item1 = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                        temp.item2 = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                        temp.item3 = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                        temp.prob1 = inReader.ReadUInt16();
                        temp.prob2 = inReader.ReadUInt16();
                        temp.prob3 = inReader.ReadUInt16();
                        temp.prob4 = inReader.ReadUInt16();
                        drops[i][j][k] = temp;
                    }
                }
            }
        }

        public override byte[] ToRaw()
        {
            calculatedPointers = new int[5];
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            int[] tableLocs = new int[5];

            outStream.Seek(0x1C, SeekOrigin.Begin);
            for (int i = 0; i < drops.Length; i++)
            {
                tableLocs[i] = (int)outStream.Position;
                for (int j = 0; j < drops[i].Length; j++)
                {
                    for (int k = 0; k < drops[i][j].Length; k++)
                    {
                        outWriter.Write(BitConverter.GetBytes(drops[i][j][k].item1).Reverse().ToArray());
                        outWriter.Write(BitConverter.GetBytes(drops[i][j][k].item2).Reverse().ToArray());
                        outWriter.Write(BitConverter.GetBytes(drops[i][j][k].item3).Reverse().ToArray());
                        outWriter.Write(drops[i][j][k].prob1);
                        outWriter.Write(drops[i][j][k].prob2);
                        outWriter.Write(drops[i][j][k].prob3);
                        outWriter.Write(drops[i][j][k].prob4);
                    }
                }
            }
            int headerLoc = (int)outStream.Position;
            for (int i = 0; i < dropTiers.Length; i++)
            {
                outWriter.Write(dropTiers[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                outWriter.Write(drops[i].Length);
                calculatedPointers[i] = (int)outStream.Position;
                outWriter.Write(tableLocs[i]);
            }
            int padSize = (int)(outStream.Position + 0xF & 0xFFFFFFF0);
            outWriter.Write(new byte[padSize - outStream.Position]);
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(Encoding.ASCII.GetBytes("NXR\0"));
            outWriter.Write(padSize);    //Size to be.
            outWriter.Write(headerLoc);    //Header location.
            outWriter.Write(0);    //I don't even know if this stuff does anything...
            outWriter.Write(1);
            outWriter.Write(-1);
            outWriter.Write(-1);
            return outStream.ToArray();
        }
    }
}
