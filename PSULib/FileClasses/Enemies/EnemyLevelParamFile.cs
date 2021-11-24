using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Enemies
{
    /// <summary>
    /// Defines the stats per level. This file is only used offline and in the portable games.
    /// </summary>
    public class EnemyLevelParamFile : PsuFile
    {
        public int[] HPperLevel;
        public short[,] allOtherStats;
        public float[] unknownValues;

        public EnemyLevelParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;
            byte[] rawTemp = new byte[rawData.Length];
            Array.Copy(rawData, rawTemp, rawData.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                int ptr = BitConverter.ToInt32(rawData, ptrs[i]);
                ptr -= baseAddr;
                Array.Copy(BitConverter.GetBytes(ptr), 0, rawTemp, ptrs[i], 4);
            }
            MemoryStream inStream = new MemoryStream(rawTemp);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int mainPointer = inReader.ReadInt32();
            int levelCount = (mainPointer - 0x10) / 24;
            HPperLevel = new int[levelCount];
            allOtherStats = new short[levelCount, 10];
            unknownValues = new float[(rawTemp.Length - mainPointer) / 4 - 1];
            inStream.Seek(mainPointer, SeekOrigin.Begin);
            int levelPointer = inReader.ReadInt32();
            for (int i = 0; i < unknownValues.Length; i++)
                unknownValues[i] = inReader.ReadSingle();
            inStream.Seek(levelPointer, SeekOrigin.Begin);
            for (int i = 0; i < levelCount; i++)
            {
                HPperLevel[i] = inReader.ReadInt32();
                for (int j = 0; j < 10; j++)
                    allOtherStats[i, j] = inReader.ReadInt16();
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(Encoding.ASCII.GetBytes("NXR\0"));
            //Go back and fill in pointers later!
            outWriter.Seek(0x10, SeekOrigin.Begin);
            int levelPointer = (int)outStream.Position;
            for (int i = 0; i < HPperLevel.Length; i++)
            {
                outWriter.Write(HPperLevel[i]);
                //outWriter.Write((int)1);
                for (int j = 0; j < 10; j++)
                {
                    /*    if (j == 6)
                            outWriter.Write((short)8000);
                        else if (j == 0)
                            outWriter.Write((short)20000);
                        else*/
                    outWriter.Write(allOtherStats[i, j]);
                }
            }
            int mainPointer = (int)outStream.Position;
            outWriter.Write(levelPointer);
            for (int i = 0; i < unknownValues.Length; i++)
                outWriter.Write(unknownValues[i]);
            int fileLength = (int)outStream.Position;
            outStream.Seek(0x4, SeekOrigin.Begin);
            outWriter.Write(fileLength - 0x8);
            outWriter.Write(mainPointer);
            calculatedPointers = new int[1];
            calculatedPointers[0] = mainPointer;
            return outStream.ToArray();
        }
    }
}
