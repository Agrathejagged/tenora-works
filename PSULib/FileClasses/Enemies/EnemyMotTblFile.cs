using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Enemies
{
    //Despite both being "MotTbl", the ones for enemies are a substantially different format from the ones for weapon types.
    public class EnemyMotTblFile : PsuFile
    {
        public class MotTblEntry
        {
            public int FileIdentifier { get; set; }
            public float StartFrame { get; set; }
            public float EndFrame { get; set; }
            public float AnimationSpeed { get; set; }
            public float SmoothingModifier { get; set; }
            public short UnknownShort1 { get; set; }
            public short RepeatCount { get; set; }
        }

        public List<MotTblEntry> MotTblEntries { get; set; } = new List<MotTblEntry>();

        public EnemyMotTblFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);

            int listLoc = inReader.ReadInt32();
            int listCount = inReader.ReadInt32();

            if (listLoc != 0 && listCount > 0)
            {
                inStream.Seek(listLoc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < listCount; i++)
                {
                    MotTblEntry entry = new MotTblEntry();
                    entry.FileIdentifier = inReader.ReadInt32();
                    entry.StartFrame = inReader.ReadSingle();
                    entry.EndFrame = inReader.ReadSingle();
                    entry.AnimationSpeed = inReader.ReadSingle();
                    entry.SmoothingModifier = inReader.ReadSingle();
                    entry.UnknownShort1 = inReader.ReadInt16();
                    entry.RepeatCount = inReader.ReadInt16();
                    MotTblEntries.Add(entry);
                }
            }
            return;
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();

            int tableLocation = (int)outStream.Position;
            foreach (var entry in MotTblEntries)
            {
                outWriter.Write(entry.FileIdentifier);
                outWriter.Write(entry.StartFrame);
                outWriter.Write(entry.EndFrame);
                outWriter.Write(entry.AnimationSpeed);
                outWriter.Write(entry.SmoothingModifier);
                outWriter.Write(entry.UnknownShort1);
                outWriter.Write(entry.RepeatCount);
            }

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(tableLocation);
            outWriter.Write(MotTblEntries.Count);

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
