using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class ListFile : PsuFile
    {
        public class GenericFilenameReference
        {
            public string Filename { get; set; }
        }
        public List<List<GenericFilenameReference>> filenames = new List<List<GenericFilenameReference>>();
        public ListFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            header = subHeader;
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int topLevelLoc = inReader.ReadInt32();
            int[] topLevelPointers = new int[16];
            inStream.Seek(topLevelLoc, SeekOrigin.Begin);
            for (int i = 0; i < 16 && inStream.Position < inStream.Length; i++)
            {
                topLevelPointers[i] = inReader.ReadInt32();
            }
            for (int i = 0; i < 16; i++)
            {
                List<GenericFilenameReference> currentFilenameList = new List<GenericFilenameReference>();
                if (topLevelPointers[i] != 0)
                {
                    inStream.Seek(topLevelPointers[i] - baseAddr, SeekOrigin.Begin);
                    int currListSize = inReader.ReadInt32();
                    int currListAddr = inReader.ReadInt32() - baseAddr;
                    int[] stringLocs = new int[currListSize];
                    inStream.Seek(currListAddr, SeekOrigin.Begin);
                    for (int j = 0; j < currListSize; j++)
                    {
                        stringLocs[j] = inReader.ReadInt32() - baseAddr;
                    }
                    for (int j = 0; j < currListSize; j++)
                    {
                        inStream.Seek(stringLocs[j], SeekOrigin.Begin);
                        StringBuilder strbld = new StringBuilder();
                        byte currByte = 0xFF;
                        while ((currByte = inReader.ReadByte()) != 0)
                        {
                            strbld.Append(Convert.ToChar(currByte));
                        }
                        currentFilenameList.Add(new GenericFilenameReference { Filename = strbld.ToString() });
                    }
                }
                filenames.Add(currentFilenameList);
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            List<int> ptrList = new List<int>();

            int[][] stringLocs = new int[filenames.Count][];
            outStream.Seek(0x10, SeekOrigin.Begin);
            for(int i = 0; i < filenames.Count; i++)
            {
                stringLocs[i] = new int[filenames[i].Count];
                for(int j = 0; j < filenames[i].Count; j++)
                {
                    stringLocs[i][j] = (int)outStream.Position;
                    outWriter.Write(ASCIIEncoding.ASCII.GetBytes(filenames[i][j].Filename));
                    outStream.Seek(4 - (outStream.Position % 4), SeekOrigin.Current);
                }
            }

            int[] listLocs = new int[filenames.Count];
            for(int i = 0; i < listLocs.Length; i++)
            {
                listLocs[i] = (int)outStream.Position;
                for(int j = 0; j < filenames[i].Count; j++)
                {
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(stringLocs[i][j]);
                }
            }

            int[] listUpperLocs = new int[filenames.Count];
            for(int i = 0; i < listUpperLocs.Length; i++)
            {
                if (stringLocs[i].Length != 0)
                {
                    listUpperLocs[i] = (int)outStream.Position;
                    outWriter.Write(stringLocs[i].Length);
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(listLocs[i]);
                }
            }

            int headerLoc = (int)outStream.Position;
            for(int i = 0; i < listUpperLocs.Length; i++)
            {
                if (listUpperLocs[i] != 0)
                {
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(listUpperLocs[i]);
                }
                else
                    outWriter.Write((int)0);
            }
            int fileEnd = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(((uint)0x52584E));
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
