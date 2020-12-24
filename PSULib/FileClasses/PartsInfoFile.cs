using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib;

namespace psu_generic_parser
{
    public class PartsInfoFile : PsuFile
    {
        public struct partsInfo
        {
            public int partNumber;
            public int stringPointer; //Used to get the string.
            public int fileNumber;
            public int unknownFlags;
            public string fileName;
        }
        public partsInfo[] parts;

        public PartsInfoFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int listLoc = inReader.ReadInt32() - baseAddr;
            int entryCount = inReader.ReadInt32();
            inStream.Seek(listLoc, SeekOrigin.Begin);
            parts = new partsInfo[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                parts[i].partNumber = inReader.ReadInt32();
                parts[i].stringPointer = inReader.ReadInt32() - baseAddr;
                parts[i].fileNumber = inReader.ReadInt32();
                parts[i].unknownFlags = inReader.ReadInt32();
            }

            for (int i = 0; i < entryCount; i++)
            {
                inStream.Seek(parts[i].stringPointer, SeekOrigin.Begin);
                parts[i].fileName = Encoding.GetEncoding("shift-jis").GetString(inReader.ReadBytes(32));
                parts[i].fileName = parts[i].fileName.Remove(parts[i].fileName.IndexOf('\0'));
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(0x0052584E);
            calculatedPointers = new int[parts.Length + 1];
            outStream.Seek(0x10, SeekOrigin.Begin);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].stringPointer = (int)outStream.Position;
                string temp = parts[i].fileName + "\0";
                byte[] encodedString = Encoding.GetEncoding("shift-jis").GetBytes(temp);
                //this one has to be handled manually, because the padding length is variable
                int paddingLength = (4 - encodedString.Length % 4) % 4;
                outWriter.Write(encodedString);
                outWriter.Write(new byte[paddingLength]);
            }
            int dataLoc = (int)outStream.Position;
            for (int i = 0; i < parts.Length; i++)
            {
                outWriter.Write(parts[i].partNumber);
                calculatedPointers[i] = (int)outStream.Position;
                outWriter.Write(parts[i].stringPointer);
                outWriter.Write(parts[i].fileNumber);
                outWriter.Write(parts[i].unknownFlags);
            }
            int headerLoc = (int)outStream.Position;
            calculatedPointers[calculatedPointers.Length - 1] = headerLoc;
            outWriter.Write(dataLoc);
            outWriter.Write((int)parts.Length);
            int fileLength = (int)outStream.Position;
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            return outStream.ToArray();
        }

        public void importFile(Stream toImport)
        {
            BinaryReader importReader = new BinaryReader(toImport);
            int header = importReader.ReadInt32();
            partsInfo[] tempParts;
            if (header == 0x52584e)//No container--make sure to calculate base loc!
            {
                toImport.Seek(0x8, SeekOrigin.Begin);
                int headerLoc = importReader.ReadInt32();
                toImport.Seek(headerLoc, SeekOrigin.Begin);
                int baseAddr = importReader.ReadInt32();// - baseAddr;
                int entryCount = importReader.ReadInt32();
                int listLoc = headerLoc - (entryCount * 0x10);
                baseAddr -= listLoc;
                toImport.Seek(listLoc, SeekOrigin.Begin);
                tempParts = new partsInfo[entryCount];
                for (int i = 0; i < entryCount; i++)
                {
                    tempParts[i].partNumber = importReader.ReadInt32();
                    tempParts[i].stringPointer = importReader.ReadInt32() - baseAddr;
                    tempParts[i].fileNumber = importReader.ReadInt32();
                    tempParts[i].unknownFlags = importReader.ReadInt32();
                }

                for (int i = 0; i < entryCount; i++)
                {
                    toImport.Seek(tempParts[i].stringPointer, SeekOrigin.Begin);
                    tempParts[i].fileName = Encoding.GetEncoding("shift-jis").GetString(importReader.ReadBytes(32));
                    tempParts[i].fileName = tempParts[i].fileName.Remove(tempParts[i].fileName.IndexOf('\0'));
                }
                //if(
            }
            else if (header == 0x524E58) //Container.
            {
                int baseAddr = -0x60;// Add 0x60 to get into the file!
                toImport.Seek(0x68, SeekOrigin.Begin);
                int headerLoc = importReader.ReadInt32();
                toImport.Seek(headerLoc + 0x60, SeekOrigin.Begin);
                int listLoc = importReader.ReadInt32() - baseAddr;
                int entryCount = importReader.ReadInt32();
                toImport.Seek(listLoc, SeekOrigin.Begin);
                tempParts = new partsInfo[entryCount];
                for (int i = 0; i < entryCount; i++)
                {
                    tempParts[i].partNumber = importReader.ReadInt32();
                    tempParts[i].stringPointer = importReader.ReadInt32() - baseAddr;
                    tempParts[i].fileNumber = importReader.ReadInt32();
                    tempParts[i].unknownFlags = importReader.ReadInt32();
                }

                for (int i = 0; i < entryCount; i++)
                {
                    toImport.Seek(tempParts[i].stringPointer, SeekOrigin.Begin);
                    tempParts[i].fileName = Encoding.GetEncoding("shift-jis").GetString(importReader.ReadBytes(32));
                    tempParts[i].fileName = tempParts[i].fileName.Remove(tempParts[i].fileName.IndexOf('\0'));
                }
            }
            else
                throw new Exception("Invalid filetype.");
            parts = tempParts;
        }
    }
}
