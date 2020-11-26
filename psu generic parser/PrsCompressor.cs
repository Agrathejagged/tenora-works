using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    class PrsCompressor
    {
        class CompressionBuffer
        {
            byte[] buffer;
            int ctrlByteCounter;
            int outLoc;
            int ctrlBitCounter;
        }

        public struct OffsetDictionaryEntry
        {
            public List<int> matchList;
            public int currentFirstOffset;
            public OffsetDictionaryEntry(List<int> inList, int first)
            {
                matchList = inList;
                currentFirstOffset = first;
            }
        }

        interface CompressionChunk
        {
            void encode(CompressionBuffer buff);
        }

        byte[] toCompress;
        byte[] compBuffer;
        int ctrlByteCounter;
        int outLoc;
        int ctrlBitCounter;
        Tuple<List<int>, int> emptyTuple = new Tuple<List<int>, int>(new List<int>(), 0);

        //This one is aiming for speed at the expense of space.
        public byte[] compress(byte[] inCompress)
        {
            toCompress = inCompress;
            //Quadruple the filesize!... hopefully this'll give a speed bonus.
            var offsetDictionary = buildOffsetDictionary(toCompress);

            //Initialization is going to stay how it was, basically.
            ctrlByteCounter = 0; //Current control byte location in output array.
            int compressLength = toCompress.Length;
            compBuffer = new byte[compressLength];
            outLoc = 0x3;   //First byte is a control byte, then 2 bytes of direct copy, THEN start calculating.
            //Can't copy through a control byte, but can start copying after. 0 = control byte, then 2 direct copy bytes.
            //Wouldn't bother with a 1-byte string copy, anyway.
            ctrlBitCounter = 2;
            compBuffer[0] = 0x3;
            Array.Copy(toCompress, 0, compBuffer, 1, 2);
            int i = 2;
            
            while (i < compressLength - 1)
            {
                OffsetDictionaryEntry currentOffsetList = getOffsetList(offsetDictionary, toCompress[i], i); ;
                
                var findLoc = findBestRange(i, currentOffsetList.matchList, currentOffsetList.currentFirstOffset);
                int maxMatchOffset = findLoc.Item1;
                int maxMatch = findLoc.Item2;


                int maxMatchIfCopy = 0;
                int maxMatchIfDirect = 0;
                if (maxMatchOffset != -1 && i + maxMatch < compressLength)
                {
                    var nextIfCopy = getOffsetList(offsetDictionary, toCompress[i + maxMatch], i + maxMatch, false);
                    var lookaheadIfCopy = findBestRange(i + maxMatch, nextIfCopy.matchList, nextIfCopy.currentFirstOffset);
                    maxMatchIfCopy = lookaheadIfCopy.Item2 - 1;

                    var nextIfDirect = getOffsetList(offsetDictionary, toCompress[i + 1], i + 1, false);
                    var lookaheadIfDirect = findBestRange(i + 1, nextIfDirect.matchList, nextIfDirect.currentFirstOffset);
                    maxMatchIfDirect = lookaheadIfDirect.Item2 + 1;
                }


                if (maxMatchOffset == -1 || ((i - maxMatchOffset > 0x100) && maxMatch < 3) || maxMatchIfDirect > (maxMatch + maxMatchIfCopy))
                {
                    writeRawByte(toCompress[i++]);
                }
                else
                {
                    if (maxMatch < 6 && (i - maxMatchOffset < 0x100))
                    {
                        writeShortReference(maxMatch, (byte)(maxMatchOffset - (i - 0x100)));
                    }
                    else
                        writeLongReference(maxMatch, (maxMatchOffset - (i - 0x2000)));
                    i += maxMatch;
                }

            }
            if(i < compressLength)
                writeRawByte(toCompress[i++]);
            finalizeCompression();

            Array.Resize(ref compBuffer, outLoc);
            return compBuffer;
        }
        private Tuple<int, int> findBestRange(int currentOffset, List<int> matches, int matchOffset)
        {
            int maxMatch = 2;
            int maxMatchOffset = -1;
            int smallCopyWindow = currentOffset - 0x100;

            //Take each instance of the byte that's in range, check 
            for (int j = matchOffset; j < matches.Count && (matches[j] < currentOffset); j++)
            {
                int startLoc = matches[j];
                int currentFind = 0;
                while (currentOffset + currentFind < toCompress.Length && toCompress[startLoc + currentFind] == toCompress[currentOffset + currentFind] && currentFind < 0x100)
                    currentFind++;
                if (currentFind > 2 || startLoc > smallCopyWindow)
                    if (currentFind > maxMatch || (currentFind == maxMatch && startLoc > maxMatchOffset))
                    {
                        maxMatch = currentFind;
                        maxMatchOffset = startLoc;
                    }
            }
            return new Tuple<int, int>(maxMatchOffset, maxMatch);
        }

        private OffsetDictionaryEntry getOffsetList(Dictionary<byte, OffsetDictionaryEntry> offsetDictionary, byte currentVal, int currentOffset, bool persistUpdates = true)
        {
            var offsets = offsetDictionary[currentVal];
            if (offsets.currentFirstOffset < currentOffset - 0x1FF0)
            {
                int startIndex = offsets.currentFirstOffset;
                while ((offsets.matchList[offsets.currentFirstOffset] < currentOffset - 0x1FF0) && offsets.currentFirstOffset < offsets.matchList.Count)
                    offsets.currentFirstOffset++;
                if (persistUpdates)
                {
                    offsetDictionary[currentVal] = offsets;
                }
            }
            return offsets;
        }

        private Dictionary<byte, OffsetDictionaryEntry> buildOffsetDictionary(byte[] toCompress)
        {
            var offsetDictionary = new Dictionary<byte, OffsetDictionaryEntry>();//Tuple<List<int>, int>>();
            for (int i = 0; i < toCompress.Length; i++)
            {
                byte currentByte = toCompress[i];
                if (!offsetDictionary.ContainsKey(currentByte))
                    offsetDictionary.Add(currentByte, new OffsetDictionaryEntry(new List<int>(), 0));
                offsetDictionary[currentByte].matchList.Add(i);
            }
            return offsetDictionary;
        }

        private void finalizeCompression()
        {
            addCtrlBit(0);
            addCtrlBit(1);
            compBuffer[outLoc++] = 0;
            compBuffer[outLoc++] = 0;
        }

        private void writeRawByte(byte val)
        {
            addCtrlBit(1);
            compBuffer[outLoc++] = val;
        }

        private void writeShortReference(int count, byte offset)
        {
            addCtrlBit(0);
            addCtrlBit(0);
            addCtrlBit((count - 2) >> 1);
            addCtrlBit((count - 2) & 1);
            compBuffer[outLoc++] = offset;
        }

        private void writeLongReference(int count, int offset)
        {
            addCtrlBit(0);
            addCtrlBit(1);
            ushort toAdd = (ushort)(offset << 3);
            byte[] temp;
            if (count <= 9)
            {
                toAdd |= (ushort)(count - 2);
            }
            temp = BitConverter.GetBytes(toAdd);
            temp.CopyTo(compBuffer, outLoc);
            outLoc += 2;
            //CHANGED FOR PSO2
            if (count > 9)
                compBuffer[outLoc++] = (byte)(count - 1);
        }

        private void addCtrlBit(int input)
        {
            if (ctrlBitCounter == 8)
            {
                ctrlBitCounter = 0;
                ctrlByteCounter = outLoc++;
            }
            compBuffer[ctrlByteCounter] |= (byte)(input << ctrlBitCounter);//(byte)((compBuffer[ctrlByteCounter] >> 1) | (0x80 * input));
            ctrlBitCounter++;
        }
    }
}
