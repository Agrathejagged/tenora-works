using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Enemies
{
    public class EnemySoundEffectFile : PsuFile
    {
        public class SoundEffect
        {
            public ushort SoundFileId { get; set; } //This one's a different format between PSU and PSP2i, so.
            public ushort SoundIndex { get; set; }
            public int BitFlags { get; set; }
            public int SoundEffectId { get; set; }
            public float UnknownFloat1 { get; set; }
            public float UnknownFloat2 { get; set; }
        }

        public List<SoundEffect> SoundEffects { get; set; } = new List<SoundEffect>();
        public EnemySoundEffectFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);

            int listCount = inReader.ReadInt32();
            int listLoc = inReader.ReadInt32();

            if (listLoc != 0 && listCount > 0)
            {
                inStream.Seek(listLoc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < listCount; i++)
                {
                    //inStream.Seek(attackLocs[i] - baseAddr, SeekOrigin.Begin);
                    SoundEffect entry = new SoundEffect();
                    uint rawSoundId = inReader.ReadUInt32();
                    entry.SoundFileId = (ushort)(rawSoundId & 0xFFFF);
                    entry.SoundIndex = (ushort)(rawSoundId >> 16);
                    entry.BitFlags = inReader.ReadInt32();
                    entry.SoundEffectId = inReader.ReadInt32();
                    entry.UnknownFloat1 = inReader.ReadSingle();
                    entry.UnknownFloat2 = inReader.ReadSingle();
                    SoundEffects.Add(entry);
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();
            int soundEffectsLocation = (int)outStream.Position;
            foreach(var entry in SoundEffects)
            {
                uint rawSoundId = (uint)((entry.SoundIndex << 16) | entry.SoundFileId);
                outWriter.Write(rawSoundId);
                outWriter.Write(entry.BitFlags);
                outWriter.Write(entry.SoundEffectId);
                outWriter.Write(entry.UnknownFloat1);
                outWriter.Write(entry.UnknownFloat2);
            }

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            outWriter.Write(SoundEffects.Count);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(soundEffectsLocation);

            int fileLength = (int)outStream.Position;
            outStream.Seek(15 - (outStream.Position % 16), SeekOrigin.Current);
            if (outStream.Position > fileLength)
            {
                outWriter.Write((byte)0);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write((int)0x52584E);
            outWriter.Write(fileLength);
            outWriter.Write(topLevelListLoc);
            calculatedPointers = pointerLocs.ToArray();

            header = buildSubheader(fileLength);
            return outStream.ToArray();
        }
    }
}
