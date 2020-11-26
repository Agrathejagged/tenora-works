using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace psu_generic_parser
{
    public class ItemPriceFile : PsuFile
    {
        //Top two levels are 01xx**xx
        //Third is 01**xx**
        //Fourth is 00 item, 01 price
        private const int mainCount = 15;
        public class priceEntry
        {
            private int itemNumber;

            public int ItemNumber
            {
                get { return itemNumber; }
                set { itemNumber = value; }
            }
            private int itemPrice;

            public int ItemPrice
            {
                get { return itemPrice; }
                set { itemPrice = value; }
            }
        }

        public List<priceEntry>[][] itemPrices;
        short[][] sellRatio;

        public ItemPriceFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
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
            int[] countList = new int[mainCount];
            int[] pointerList = new int[mainCount];
            itemPrices = new List<priceEntry>[mainCount][];
            sellRatio = new short[mainCount][];
            inStream.Seek(mainPointer, SeekOrigin.Begin);
            for (int i = 0; i < mainCount; i++)
            {
                countList[i] = inReader.ReadInt32();
                pointerList[i] = inReader.ReadInt32();
            }
            for (int i = 0; i < mainCount; i++)
            {
                inStream.Seek(pointerList[i], SeekOrigin.Begin);
                itemPrices[i] = new List<priceEntry>[countList[i]];
                int[] secondLevelCounts = new int[countList[i]];
                int[] secondLevelPointers = new int[countList[i]];
                sellRatio[i] = new short[countList[i]];
                for (int j = 0; j < countList[i]; j++)
                {
                    secondLevelCounts[j] = inReader.ReadInt16();
                    sellRatio[i][j] = inReader.ReadInt16();
                    secondLevelPointers[j] = inReader.ReadInt32();
                    itemPrices[i][j] = new List<priceEntry>();//int[secondLevelCounts[j]][];
                }
                for (int j = 0; j < countList[i]; j++)
                {
                    inStream.Seek(secondLevelPointers[j], SeekOrigin.Begin);
                    for (int k = 0; k < secondLevelCounts[j]; k++)
                    {
                        priceEntry currEntry = new priceEntry();
                        currEntry.ItemNumber = inReader.ReadInt32();
                        currEntry.ItemPrice = inReader.ReadInt32();
                        itemPrices[i][j].Add(currEntry);
                    }
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(0x0052584E);
            outStream.Seek(0x10, SeekOrigin.Begin);
            outWriter.Write((int)1);
            outWriter.Write((int)-1);
            outWriter.Write((int)-1);
            int pointerCount = 0;
            int[][] fileLocs = new int[mainCount][];
            for (int i = 0; i < mainCount; i++)
            {
                fileLocs[i] = new int[itemPrices[i].Length];
                pointerCount += 1 + itemPrices[i].Length;
                for (int j = 0; j < itemPrices[i].Length; j++)
                {
                    fileLocs[i][j] = (int)outStream.Position;
                    for (int k = 0; k < itemPrices[i][j].Count; k++)
                    {
                        outWriter.Write(itemPrices[i][j][k].ItemNumber);
                        outWriter.Write(itemPrices[i][j][k].ItemPrice);
                    }
                }
            }
            int[] mainLocs = new int[mainCount];
            calculatedPointers = new int[pointerCount];
            int currPtr = 0;
            for (int i = 0; i < mainCount; i++)
            {
                mainLocs[i] = (int)outStream.Position;
                for (int j = 0; j < itemPrices[i].Length; j++)
                {
                    outWriter.Write((short)itemPrices[i][j].Count);
                    outWriter.Write(sellRatio[i][j]);
                    calculatedPointers[currPtr++] = (int)outStream.Position;
                    outWriter.Write(fileLocs[i][j]);
                }
            }
            int headerLoc = (int)outStream.Position;
            for (int i = 0; i < mainCount; i++)
            {
                outWriter.Write(itemPrices[i].Length);
                calculatedPointers[currPtr++] = (int)outStream.Position;
                outWriter.Write(mainLocs[i]);
            }
            int fileLength = (int)outStream.Position;
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            return outStream.ToArray();
        }
    }
}
