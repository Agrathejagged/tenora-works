using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Enemies
{
    public class AtkDatFile : PsuFile
    {
        public class AttackEntry
        {
            public int UnknownIndex1 { get; set; }
            public float UnknownModifer1 { get; set; }
            public float UnknownModifer2 { get; set; }
            public int TechType { get; set; }
            public int TechElement { get; set; }
            public int TechNumber { get; set; }
            public int TechLevel { get; set; }
        }

        public class Attack
        {
            public List<AttackEntry> AttackEntries { get; set; } = new List<AttackEntry>();
        }

        public List<Attack> Attacks { get; set; } = new List<Attack>();

        public AtkDatFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
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
            List<int> attackLocs = new List<int>();
            List<int> attackEntryCounts = new List<int>();

            if(listLoc != 0 && listCount > 0)
            {
                inStream.Seek(listLoc - baseAddr, SeekOrigin.Begin);
                for(int i = 0; i < listCount; i++)
                {
                    attackLocs.Add(inReader.ReadInt32());
                    attackEntryCounts.Add(inReader.ReadInt32());
                }
                for(int i = 0; i < listCount; i++)
                {
                    Attack attack = new Attack();
                    inStream.Seek(attackLocs[i] - baseAddr, SeekOrigin.Begin);
                    for(int j = 0; j < attackEntryCounts[i]; j++)
                    {
                        AttackEntry entry = new AttackEntry();
                        entry.UnknownIndex1 = inReader.ReadInt32();
                        entry.UnknownModifer1 = inReader.ReadSingle();
                        entry.UnknownModifer2 = inReader.ReadSingle();
                        entry.TechType = inReader.ReadInt32();
                        entry.TechElement = inReader.ReadInt32();
                        entry.TechNumber = inReader.ReadInt32();
                        entry.TechLevel = inReader.ReadInt32();
                        attack.AttackEntries.Add(entry);
                    }
                    Attacks.Add(attack);
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();

            List<int> attackLocations = new List<int>();
            for(int i = 0; i < Attacks.Count; i++)
            {
                attackLocations.Add((int)outStream.Position);
                foreach(AttackEntry entry in Attacks[i].AttackEntries)
                {
                    outWriter.Write(entry.UnknownIndex1);
                    outWriter.Write(entry.UnknownModifer1);
                    outWriter.Write(entry.UnknownModifer2);
                    outWriter.Write(entry.TechType);
                    outWriter.Write(entry.TechElement);
                    outWriter.Write(entry.TechNumber);
                    outWriter.Write(entry.TechLevel);
                }
            }

            int attackListLoc = (int)outStream.Position;
            for(int i = 0; i < Attacks.Count; i++)
            {
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(attackLocations[i]);
                outWriter.Write(Attacks[i].AttackEntries.Count);
            }

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackListLoc);
            outWriter.Write(Attacks.Count);

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
