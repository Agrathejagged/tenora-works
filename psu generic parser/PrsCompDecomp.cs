using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace psu_generic_parser
{
    class PrsCompDecomp
    {
        int ctrlByteCounter;
        int ctrlBytePos = 0;
        byte origCtrlByte = 0;
        byte ctrlByte = 0;
        byte[] decompBuffer;
        int currDecompPos = 0;
        int numCtrlBytes = 1;

 
        private bool getCtrlBit()
        {
            ctrlByteCounter--;
            if (ctrlByteCounter == 0)
            {
                ctrlBytePos = currDecompPos;
                origCtrlByte = decompBuffer[currDecompPos];
                ctrlByte = decompBuffer[currDecompPos++];
                ctrlByteCounter = 8;
                numCtrlBytes++;
            }
            bool temp = ((ctrlByte & 1) != 0);
            ctrlByte >>= 1;
            return temp;
        }

        public static byte[] Decompress(byte[] input, uint outCount)
        {
            return new PrsCompDecomp().localDecompress(input, outCount);
        }

        public byte[] localDecompress(byte[] input, uint outCount)
        {
            byte[] output = new byte[outCount];
            decompBuffer = input;
            ctrlByte = 0;
            ctrlByteCounter = 1;
            numCtrlBytes = 1;
            currDecompPos = 0;
            int destPos = 0;
            int tempPos = 0;
            int tempCount = 0;
            int origCount = 0;
            int origPos = 0;

            try
            {
                while (destPos < outCount && currDecompPos < input.Length)
                {
                    
                    while (getCtrlBit())
                        output[destPos++] = decompBuffer[currDecompPos++];

                    if (getCtrlBit())
                    {
                        if (currDecompPos >= decompBuffer.Length)
                            break;
                        origCount = decompBuffer[currDecompPos++];
                        origPos = decompBuffer[currDecompPos++];
                        tempCount = origCount;
                        tempPos = origPos;
                        if (tempCount == 0 && tempPos == 0)
                            break;
                        tempPos = (tempPos << 5) + (tempCount >> 3) - 0x2000;
                        tempCount &= 7;

                        if (tempCount == 0)
                            tempCount = decompBuffer[currDecompPos++] + 1;
                        else
                            tempCount += 2;
                    }
                    else
                    {
                        tempCount = 2;
                        if (getCtrlBit())
                            tempCount += 2;
                        if (getCtrlBit())
                            tempCount++;
                        tempPos = decompBuffer[currDecompPos++] - 0x100;
                    }
                    tempPos += destPos;
                    for (int i = 0; i < tempCount && destPos < output.Length; i++)
                        output[destPos++] = output[tempPos++];
                }
            }
            catch (Exception e)
            {
                //Man, I know this is awful coding, but... if it fails, either the decompression fucked up, or it's one of those files that overruns.
            }
            return output;
        }

        public byte[] localDecompressWithLogging(byte[] input, uint outCount, StreamWriter logStream)
        {
            byte[] output = new byte[outCount];
            decompBuffer = input;
            ctrlByte = 0;
            ctrlByteCounter = 1;
            numCtrlBytes = 1;
            currDecompPos = 0;
            int destPos = 0;
            int tempPos = 0;
            int tempCount = 0;
            int origCount = 0;
            int origPos = 0;

            try
            {
                while (destPos < outCount && currDecompPos < input.Length)
                {

                    while (getCtrlBit())
                    {
                        logStream.WriteLine("Raw write: " + decompBuffer[currDecompPos].ToString("X2"));
                        output[destPos++] = decompBuffer[currDecompPos++];
                    }

                    if (getCtrlBit())
                    {
                        if (currDecompPos >= decompBuffer.Length)
                            break;
                        origCount = decompBuffer[currDecompPos++];
                        origPos = decompBuffer[currDecompPos++];
                        tempCount = origCount;
                        tempPos = origPos;
                        if (tempCount == 0 && tempPos == 0)
                            break;
                        tempPos = (tempPos << 5) + (tempCount >> 3) - 0x2000;
                        tempCount &= 7;

                        if (tempCount == 0)
                            tempCount = decompBuffer[currDecompPos++] + 1;
                        else
                            tempCount += 2;
                    }
                    else
                    {
                        tempCount = 2;
                        if (getCtrlBit())
                            tempCount += 2;
                        if (getCtrlBit())
                            tempCount++;
                        tempPos = decompBuffer[currDecompPos++] - 0x100;
                    }
                    tempPos += destPos;
                    logStream.WriteLine("Copy: " + tempCount + " bytes from " + tempPos.ToString("X8") + " to " + destPos.ToString("X8"));
                    for (int i = 0; i < tempCount && destPos < output.Length; i++)
                    {
                        output[destPos++] = output[tempPos++];
                    }
                }
            }
            catch (Exception e)
            {
                //Man, I know this is awful coding, but... if it fails, either the decompression fucked up, or it's one of those files that overruns.
            }
            return output;
        }

        byte[] compBuffer;
        //int ctrlByteCounter;  Using the same definition as for the decompressor. Hopefully this doesn't lead to errors! ha ha of course it will.
        int ctrlBitCounter;
        //bool directCopy;
        int outLoc;

        public static byte[] compress(byte[] toCompress)
        {
            return new PrsCompressor().compress(toCompress);
        }

        public byte[] localCompress(byte[] toCompress)
        {
            ctrlByteCounter = 0; //Current control byte location in output array.
            int compressLength = toCompress.Length;
            compBuffer = new byte[compressLength];
            outLoc = 0x3;   //First byte is a control byte, then 2 bytes of direct copy, THEN start calculating.
            //Can't copy through a control byte, but can start copying after. 0 = control byte, then 2 direct copy bytes.
            //Wouldn't bother with a 1-byte string copy, anyway.
            ctrlBitCounter = 2;
            compBuffer[0] = 0x3;
            Array.Copy(toCompress, 0, compBuffer, 1, 2);
            int i = 0;
            for (i = 2; i < compressLength; )
            {
                int maxCount = 1; //Not worth copying values less than 2 bytes long. Ever.
                int maxPos = -1; //This value's just gonna get overwritten once I find something better, so -1 is a flag.
                int findCount = 0;
                int minValue = Math.Min(i, 0x1FFF);
                for (int j = 1; j <= minValue; j++)
                {
                    findCount = 0;
                    while (i + findCount < compressLength && toCompress[i - j + findCount] == toCompress[i + findCount] && findCount < 0x100)
                        findCount++;
                    if (findCount > maxCount)
                    {
                        maxCount = findCount;
                        maxPos = j;
                    }
                }
                if (i > 0x175)
                    i = i;
                if (maxPos != -1 && (maxPos < 0x101 || maxCount > 2))
                {
                    addCtrlBit(0);
                    if (maxPos > 0x100 || maxCount > 5)
                    {
                        addCtrlBit(1);
                        ushort toAdd = (ushort)((0x2000 - maxPos) << 3);
                        byte[] temp;
                        if (maxCount <= 9)
                        {
                            toAdd |= (ushort)(maxCount - 2);
                        }
                        temp = BitConverter.GetBytes(toAdd);
                        temp.CopyTo(compBuffer, outLoc);
                        outLoc += 2;
                        if (maxCount > 9)
                            compBuffer[outLoc++] = (byte)(maxCount - 1);
                    }
                    else
                    {
                        addCtrlBit(0);
                        addCtrlBit((maxCount - 2) >> 1);
                        addCtrlBit((maxCount - 2) & 1);
                        compBuffer[outLoc++] = (byte)(0x100 - maxPos);
                    }
                    i += maxCount;
                }
                else
                {
                    addCtrlBit(1);
                    compBuffer[outLoc++] = toCompress[i++];
                }
            }
            addCtrlBit(0);
            addCtrlBit(1);
            compBuffer[outLoc++] = 0;
            compBuffer[outLoc++] = 0;
            Array.Resize(ref compBuffer, outLoc);
            return compBuffer;

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
