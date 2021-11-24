using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.General
{
    public class Psp2TextFile : PsuFile
    {
        private UnpointeredFile backupRaw;

        public List<string>[] stringArray;

        public override byte[] ToRaw()
        {
            //Return this until a proper implementation is made
            return backupRaw.ToRaw();
        }

        public Psp2TextFile(string filename)
        {
            this.filename = filename;
            stringArray = new List<string>[1];
        }

        public Psp2TextFile(string inFilename, byte[] rawData)
        {
            backupRaw = new UnpointeredFile(inFilename, rawData, new byte[0]);

            filename = inFilename;
            MemoryStream rawStream = new MemoryStream(rawData);
            BinaryReader rawReader = new BinaryReader(rawStream);
            int categoryCount = rawReader.ReadInt32();
            stringArray = new List<string>[categoryCount];
            int categoryBase = (int)rawStream.Position;
            int[] categoryCounts = new int[categoryCount];
            for (int i = 0; i < categoryCount; i++)
            {
                categoryCounts[i] = rawReader.ReadInt16();
            }

            int highestValue = categoryCounts.Max();
            int[] stringOffsets = new int[highestValue];
            for (int i = 0; i < highestValue; i++)
            {
                stringOffsets[i] = rawReader.ReadInt16();
            }
            int charMapping = rawReader.ReadInt16();
            int stringStartLoc = (int)rawStream.Position;

            rawStream.Seek(charMapping + stringStartLoc + charMapping % 2, SeekOrigin.Begin);
            char[] mapping = new char[256];
            for (int i = 0; i < 256 && rawStream.Position < rawData.Length - 2; i++)
            {
                mapping[i] = Convert.ToChar(rawReader.ReadUInt16());
            }
            int currentCategory = 0;
            for (int i = 0; i < highestValue - 1; i++)
            {
                while (i >= categoryCounts[currentCategory])
                    currentCategory++;
                if (stringArray[currentCategory] == null)
                    stringArray[currentCategory] = new List<string>();
                rawStream.Seek(stringStartLoc + stringOffsets[i], SeekOrigin.Begin);
                byte[] stringBytes = rawReader.ReadBytes(stringOffsets[i + 1] - stringOffsets[i]);
                StringBuilder s = new StringBuilder();
                for (int k = 0; k < stringBytes.Length; k++)
                {
                    s.Append(mapping[stringBytes[k]]);
                }
                stringArray[currentCategory].Add(s.ToString());
            }
        }
    }
}
