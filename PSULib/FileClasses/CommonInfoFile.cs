using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class CommonInfoFile : PsuFile
    {
        public class CommonEntry
        {
            public byte rarity { get; set; }
            public byte sortOrder { get; set; }
        }
        public List<CommonEntry>[][][] entries = new List<CommonEntry>[15][][];
        int totalEntries = 0;

        public CommonInfoFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;
            calculatedPointers = ptrs;
            MemoryStream inputStream = new MemoryStream(rawData);
            BinaryReader inputReader = new BinaryReader(inputStream);

            inputStream.Seek(4, SeekOrigin.Begin);
            int origFilesize = inputReader.ReadInt32();
            int headerLoc = inputReader.ReadInt32();
            inputStream.Seek(headerLoc, SeekOrigin.Begin);
            int mainListLoc = inputReader.ReadInt32() - baseAddr;
            int mainListCountsLoc = inputReader.ReadInt32() - baseAddr;
            byte[] mainListLocs = new byte[15];
            inputStream.Seek(mainListCountsLoc, SeekOrigin.Begin);
            for (int i = 0; i < 15; i++)
            {
                mainListLocs[i] = inputReader.ReadByte();
                entries[i] = new List<CommonEntry>[inputReader.ReadByte()][];
                totalEntries += entries[i].Length;
            }
            for (int i = 0; i < 15; i++)
            {
                inputStream.Seek(mainListLoc + mainListLocs[i] * 16, SeekOrigin.Begin);
                for (int j = 0; j < entries[i].Length; j++)
                {
                    int entryLocs = inputReader.ReadInt32() - baseAddr;
                    short totalNumEntries = inputReader.ReadInt16();
                    short numSubcats = inputReader.ReadInt16();
                    short expandSubcats = numSubcats;
                    if (i == 0 && numSubcats < 0x1B)
                    {
                        expandSubcats = 0x1B;
                    }
                    int subcatLocsLoc = inputReader.ReadInt32() - baseAddr;
                    int subcatLengthsLoc = inputReader.ReadInt32() - baseAddr;
                    entries[i][j] = new List<CommonEntry>[expandSubcats];
                    long currLoc = inputStream.Position;
                    int[] subcatLocs = new int[expandSubcats];
                    int[] subcatLengths = new int [expandSubcats];
                    inputStream.Seek(subcatLocsLoc, SeekOrigin.Begin);
                    for(int k = 0; k < expandSubcats; k++)
                    {
                        if(k < numSubcats)
                            subcatLocs[k] = inputReader.ReadInt16();
                    }
                    inputStream.Seek(subcatLengthsLoc, SeekOrigin.Begin);
                    for (int k = 0; k < expandSubcats; k++)
                    {
                        if (k < numSubcats)
                            subcatLengths[k] = inputReader.ReadByte();
                    }
                    for (int k = 0; k < expandSubcats; k++)
                    {
                        if (k < numSubcats)
                        {
                            inputStream.Seek(entryLocs + subcatLocs[k] * 2, SeekOrigin.Begin);
                            entries[i][j][k] = new List<CommonEntry>(subcatLengths[k]);
                            for (int m = 0; m < subcatLengths[k]; m++)
                            {
                                CommonEntry temp = new CommonEntry();
                                temp.rarity = inputReader.ReadByte();
                                temp.sortOrder = inputReader.ReadByte();
                                entries[i][j][k].Insert(m, temp);//inputReader.ReadBytes(subcatLengths[k] * 2);//new byte[subcatLengths[k] * 2];
                            }
                        }
                        else
                            entries[i][j][k] = new List<CommonEntry>(0);
                    }
                    inputStream.Seek(currLoc, SeekOrigin.Begin);
                }
            }
            //inputStream.Seek(mainListLocs
        }


        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outWriter.Write(ASCIIEncoding.ASCII.GetBytes("NXR\0"));
            outWriter.Write((int)0);    //Size to be.
            outWriter.Write((int)0);    //Header location.
            outWriter.Write((int)0);    //I don't even know if this stuff does anything...
            outWriter.Write((int)1);
            outWriter.Write((int)-1);
            outWriter.Write((int)-1);

            int[] dataPtrs = new int[totalEntries];
            int[] dataIndexPtrs = new int[totalEntries];
            int[] dataCountPtrs = new int[totalEntries];
            short[] totalDataCounts = new short[totalEntries];
            short[] subcatCounts = new short[totalEntries];
            int currentPtrIndex = 0;
            int topCount = entries.Length;
            int currentIndex = 0;
            for (int i = 0; i < topCount; i++)
            {
                for (int j = 0; j < entries[i].Length; j++)
                {
                    short[] indexes = new short[entries[i][j].Length];
                    byte[] lengths = new byte[entries[i][j].Length];
                    dataPtrs[currentIndex] = (int)outStream.Position;
                    short currSubcatIndex = 0;
                    for (int k = 0; k < entries[i][j].Length; k++)
                    {
                        indexes[k] = currSubcatIndex;
                        lengths[k] = (byte)(entries[i][j][k].Count);
                        currSubcatIndex += lengths[k];
                        for (int m = 0; m < entries[i][j][k].Count; m++)
                        {
                            outWriter.Write(entries[i][j][k][m].rarity);
                            outWriter.Write(entries[i][j][k][m].sortOrder);
                        }
                    }
                    dataIndexPtrs[currentIndex] = (int)outStream.Position;
                    for (int k = 0; k < indexes.Length; k++)
                    {
                        outWriter.Write(indexes[k]);
                    }
                    dataCountPtrs[currentIndex] = (int)outStream.Position;
                    outWriter.Write(lengths);

                    totalDataCounts[currentIndex] = currSubcatIndex;
                    subcatCounts[currentIndex] = (short)entries[i][j].Length;
                    currentIndex++;
                }
            }
            outStream.Seek((outStream.Position + 3) & 0xFFFFFFFC, SeekOrigin.Begin);
            int headersLoc = (int)outStream.Position;
            for (int i = 0; i < totalEntries; i++)
            {
                calculatedPointers[currentPtrIndex++] = (int)outStream.Position;
                outWriter.Write(dataPtrs[i]);
                outWriter.Write(totalDataCounts[i]);
                outWriter.Write(subcatCounts[i]);
                calculatedPointers[currentPtrIndex++] = (int)outStream.Position;
                outWriter.Write(dataIndexPtrs[i]);
                calculatedPointers[currentPtrIndex++] = (int)outStream.Position;
                outWriter.Write(dataCountPtrs[i]);
            }
            int headerIndexLoc = (int)outStream.Position;
            currentIndex = 0;
            for (int i = 0; i < 15; i++)
            {
                outWriter.Write((byte)currentIndex);
                outWriter.Write((byte)entries[i].Length);
                currentIndex += entries[i].Length;
            }
            outWriter.Write((int)-1);
            outWriter.Write((short)0);  //Because this is fixed size and sandwiched between pointers, it will always pad the same.
            int headerLoc = (int)outStream.Position;
            calculatedPointers[currentPtrIndex++] = (int)outStream.Position;
            outWriter.Write(headersLoc);
            calculatedPointers[currentPtrIndex++] = (int)outStream.Position;
            outWriter.Write(headerIndexLoc);
            outWriter.Write(new byte[0x10 - (outStream.Position % 0x10)]);
            int totalSize = (int)outStream.Position;
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(totalSize);
            outWriter.Write(headerLoc);
            byte[] toReturn = outStream.ToArray();
            return toReturn;
        }
    }
}
