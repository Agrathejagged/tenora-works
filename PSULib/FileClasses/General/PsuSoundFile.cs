using PSULib.FileClasses.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses
{
    /// <summary>
    /// This is specifically PSU's .dat xobxKPTD/xobxDDNS files, not the PSP equivalent.
    /// </summary>
    public class PsuSoundFile : PsuFile
    {
        public class SoundEntry
        {
            public int unknownInt;
            public int sampleCount;
            public byte[] rawSound;
            public short unknownShort1;
            public ushort sampleRate;
        }

        public List<SoundEntry> Sounds { get; private set; } = new List<SoundEntry>();

        private byte[] originalContents;

        public PsuSoundFile(string inFilename, byte[] rawData)
        {
            //This file has no pointers, so we just need to read the data directly.
            //The header may or may not matter...?
            filename = inFilename;
            originalContents = rawData;
            MemoryStream rawStream = new MemoryStream(rawData);
            BinaryReader rawReader = new BinaryReader(rawStream);
            byte[] dtpkBytes = rawReader.ReadBytes(12);
            int dtpkLength = rawReader.ReadInt32();
            int dtpkActualLength = rawReader.ReadInt32();

            rawStream.Seek(0x60, SeekOrigin.Begin);
            byte[] snddBytes = rawReader.ReadBytes(12);
            int snddLength = rawReader.ReadInt32();

            //This stuff's all wrapper. It looks like the actual data starts at 0x288 always (pointer at 0xB4), then other stream locations from there.
            //Could rebuild it from the lengths of the tables of contents, but it's right there.
            rawStream.Seek(0xB4, SeekOrigin.Begin);
            int contentsLoc = rawReader.ReadInt32();
            int metadataLoc = rawReader.ReadInt32();
            int lastNonZero = rawReader.ReadInt32();

            rawStream.Seek(contentsLoc, SeekOrigin.Begin);
            int contentsMystery1 = rawReader.ReadInt32();
            short contentsMystery2 = rawReader.ReadInt16();
            short contentsCount = rawReader.ReadInt16();

            for(int i = 0; i < contentsCount; i++)
            {
                //Do we care about this part? I don't think we do, I think it's flags for the game.
            }

            rawStream.Seek(metadataLoc, SeekOrigin.Begin);
            int metadataCount = rawReader.ReadInt32();
            for(int i = 0; i <= metadataCount; i++)
            {
                SoundEntry entry = new SoundEntry();
                int offset = rawReader.ReadInt32();
                entry.unknownInt = rawReader.ReadInt32();
                entry.sampleCount = rawReader.ReadInt32();
                entry.unknownShort1 = rawReader.ReadInt16();
                entry.sampleRate = rawReader.ReadUInt16();
                int length = rawReader.ReadInt32();

                long currentLoc = rawStream.Position;
                rawStream.Seek((int)offset, SeekOrigin.Begin);
                entry.rawSound = rawReader.ReadBytes(length);
                rawStream.Seek(currentLoc, SeekOrigin.Begin);
                Sounds.Add(entry);
            }
            rawReader.Close();
        }

        public override byte[] ToRaw()
        {
            //TODO: Once importing is permitted, properly write out the parsed contents.
            return originalContents;
        }
    }
}
