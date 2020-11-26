using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace psu_generic_parser
{
    public class WeaponListFile : PsuFile
    {
        public short[][][] modelNums = new short[28][][];
        public int dataType = 0; //0 = standard, 2 = enhanced.

        public WeaponListFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            //Funny story. There's no actual pointers in this file.
            filename = inFilename;
            header = subHeader;
            calculatedPointers = ptrs;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream); //Seems slightly overkill to read a bunch of short, oh well.

            int weaponsPerGroup = 20;
            if(rawData.Length <= 0x1190)
            {
                dataType = 0;
                weaponsPerGroup = 20;
            }
            else
            {
                dataType = 2;
                weaponsPerGroup = 80;
            }
            inStream.Seek(0x10, SeekOrigin.Begin);
            for (int i = 0; i < 28; i++)
            {
                modelNums[i] = new short[4][];
                for (int j = 0; j < 4; j++)
                {
                    modelNums[i][j] = new short[weaponsPerGroup];
                    for (int k = 0; k < weaponsPerGroup; k++)
                    {
                        modelNums[i][j][k] = inReader.ReadInt16();
                    }
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outWriter.Write(0x52584E);
            outWriter.Write(0);
            outWriter.Write(16);
            outWriter.Write(0);
            for (int i = 0; i < modelNums.Length; i++)
            {
                for (int j = 0; j < modelNums[i].Length; j++)
                {
                    for (int k = 0; k < modelNums[i][j].Length; k++)
                    {
                        outWriter.Write(modelNums[i][j][k]);
                    }
                }
            }
            int fileLength = (int)outStream.Position;
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(fileLength);
            return outStream.ToArray();
        }

        public void FromDirectory(string inDirectory)
        {
            string[] filenames = Directory.GetFiles(inDirectory);
            for (int i = 0; i < filenames.Length; i++)
            {
                filenames[i] = Path.GetFileName(filenames[i]);
            }
            Array.Sort(filenames);
            ArrayList fileList = new ArrayList(filenames);

            for (int i = 1; i < 29; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    if (modelNums[i - 1][j - 1].Length < 80)
                        Array.Resize(ref modelNums[i - 1][j - 1], 80);
                    for (int k = 1; k < 81; k++)
                    {
                        string tempString = "IW_" + i.ToString("D2") + "_" + j + "_" + k.ToString("D2") + ".nbl";
                        if (fileList.Contains(tempString))
                        {
                            modelNums[i - 1][j - 1][k - 1] = (short)fileList.IndexOf(tempString);
                        }
                        else
                            modelNums[i - 1][j - 1][k - 1] = (short)-1;
                    }
                }
            }
        }
    }
}
