using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Models
{
    //Bone list.
    public class XnaFile : PsuFile
    {
        public Dictionary<int, string> nameList
        {
            get
            {
                return boneReferences.ToDictionary(val => val.BoneId, val => val.BoneName);
            }
            set
            {
                boneReferences = value.Select(thing => new BoneReference { BoneId = thing.Key, BoneName = thing.Value }).ToList();
                boneReferences.Sort((entry1, entry2) =>
                {
                    if (entry1.BoneId < entry2.BoneId)
                    {
                        return -1;
                    }
                    if (entry1.BoneId > entry2.BoneId)
                    {
                        return 1;
                    }
                    return 0;
                });
            }
        }
        public List<BoneReference> boneReferences = new List<BoneReference>();
        public class BoneReference
        {
            public int BoneId { get; set; }
            public string BoneName { get; set; }
        }

        public XnaFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            header = subHeader;
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int topLevelLoc = inReader.ReadInt32();
            inStream.Seek(topLevelLoc, SeekOrigin.Begin);

            int unknown1 = inReader.ReadInt32();
            int nameCount = inReader.ReadInt32();
            int listLoc = inReader.ReadInt32() - baseAddr;
            for (int i = 0; i < nameCount; i++)
            {
                inStream.Seek(listLoc + i * 8, SeekOrigin.Begin);
                int stringIndex = inReader.ReadInt32();
                int stringLoc = inReader.ReadInt32() - baseAddr;
                inStream.Seek(stringLoc, SeekOrigin.Begin);
                string currentString = readNullTerminatedString(inReader);
                nameList.Add(stringIndex, currentString);
                boneReferences.Add(new BoneReference { BoneId = stringIndex, BoneName = currentString });
            }
        }

        private string readNullTerminatedString(BinaryReader reader)
        {
            StringBuilder builder = new StringBuilder();
            byte nextChar = reader.ReadByte();
            while (nextChar != 0)
            {
                builder.Append(Convert.ToChar(nextChar));
                nextChar = reader.ReadByte();
            }
            return builder.ToString();
        }
        public override byte[] ToRaw()
        {
            string formatChar = Path.GetExtension(filename).ElementAt(1).ToString().ToUpper();
            calculatedPointers = new int[nameList.Count + 1];
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            List<int> stringOffsets = new List<int>();
            int stringTableLoc = 16;
            int stringHeaderLoc = 16 + nameList.Count * 8;
            int stringContentsLoc = 16 + nameList.Count * 8 + 12;
            outStream.Seek(stringContentsLoc, SeekOrigin.Begin);
            int[] stringPointers = new int[nameList.Count];
            for (int i = 0; i < boneReferences.Count; i++)
            {
                stringOffsets.Add((int)outStream.Position);
                outWriter.Write(Encoding.ASCII.GetBytes(boneReferences[i].BoneName + '\0'));
            }

            int fileLength = (int)outStream.Position;
            outStream.Seek(fileLength + 0xF & 0xFFFFFFF0, SeekOrigin.Begin);
            int paddedFileLength = (int)outStream.Position;
            outStream.Seek(stringTableLoc, SeekOrigin.Begin);
            for (int i = 0; i < boneReferences.Count; i++)
            {
                outWriter.Write(boneReferences[i].BoneId);
                calculatedPointers[i] = (int)outStream.Position;
                outWriter.Write(stringOffsets[i]);
            }
            outWriter.Write(0);
            outWriter.Write(boneReferences.Count);
            calculatedPointers[stringPointers.Length] = (int)outStream.Position;
            outWriter.Write(stringTableLoc);

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(Encoding.ASCII.GetBytes("N" + formatChar + "NN"));
            outWriter.Write(fileLength);
            outWriter.Write(stringHeaderLoc);

            //Build subheader
            header = new byte[0x20];
            MemoryStream headerStream = new MemoryStream(header);
            BinaryWriter headerWriter = new BinaryWriter(headerStream);
            headerWriter.Write(0x4649584E);
            headerWriter.Write(0x18);
            headerWriter.Write(1);
            headerWriter.Write(0x20);
            headerWriter.Write(paddedFileLength);
            headerWriter.Write(paddedFileLength + 0x20);
            headerWriter.Write((int)(calculatedPointers.Length * 4 + 0x1F & 0xFFFFFFE0));
            headerWriter.Write(1);
            return outStream.ToArray();
        }
    }
}
