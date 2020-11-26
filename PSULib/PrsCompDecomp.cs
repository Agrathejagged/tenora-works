using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace psu_generic_parser
{
    public class PrsCompDecomp
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

        public static byte[] compress(byte[] toCompress)
        {
            return new PrsCompressor().compress(toCompress);
        }

    }
}
