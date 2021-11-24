using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace PSULib.FileClasses.General
{
    [DataContract]
    public abstract class PsuFile
    {
        public string filename;
        public bool dirty = false;      //For if modified.
        public int[] calculatedPointers;    //Do not do not DO NOT access before ToRaw()! Values are meaningless at best until file is finalized.
        public byte[] header;

        public abstract byte[] ToRaw();

        public RawFile ToRawFile(uint fileLocation)
        {
            RawFile toRet = new RawFile();
            toRet.fileContents = ToRaw();
            toRet.filename = filename;
            toRet.subHeader = header;
            toRet.chunkSize = 0x60;
            if (calculatedPointers != null)
                toRet.pointers = new List<int>(calculatedPointers);
            toRet.fileOffset = fileLocation;
            for (int i = 0; i < toRet.pointers.Count; i++)
            {
                Array.Copy(BitConverter.GetBytes(BitConverter.ToInt32(toRet.fileContents, toRet.pointers[i]) + (int)fileLocation), 0, toRet.fileContents, toRet.pointers[i], 4);
                toRet.pointers[i] += (int)fileLocation;
            }
            return toRet;
        }

        protected byte[] buildSubheader(int fileLength)
        {
            byte[] toRet = new byte[0x20];
            MemoryStream outStream = new MemoryStream(toRet);
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(0x4649584E); //"NXIF"
            outWriter.Write(0x18);
            outWriter.Write(1);
            outWriter.Write(0x20);
            outWriter.Write(fileLength);
            outWriter.Write(fileLength + 0x20);
            outWriter.Write((int)((calculatedPointers.Length * 4 + 0xF & 0xFFFFFFF0) + 0x10));
            outWriter.Write(1);
            outWriter.Close();
            return toRet;
        }

        protected int expectNonPointerInt(BinaryReader reader, List<int> ptrs)
        {
            if (ptrs.Contains((int)reader.BaseStream.Position))
            {
                throw new Exception("Unexpected pointer");
            }
            return reader.ReadInt32();
        }
    }
}
