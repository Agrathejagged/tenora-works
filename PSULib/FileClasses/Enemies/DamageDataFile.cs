using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Enemies
{
    public class DamageDataFile : PsuFile
    {
        public class DamageAngleEntry
        {
            public int UnknownInt1 { get; set; }
            public int UnknownInt2 { get; set; }
            public List<int> ActionList { get; } = new List<int>();
        }

        public class DamageTypeEntry
        {
            public int DamageType { get; set; }
            public List<DamageAngleEntry> Angles { get; } = new List<DamageAngleEntry>();
        }

        public List<List<DamageTypeEntry>> DamageTypeEntries { get; } = new List<List<DamageTypeEntry>>();

        public DamageDataFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
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
            List<int> damageListLocs = new List<int>();
            List<int> damageListEntryCounts = new List<int>();

            if (listLoc != 0 && listCount > 0)
            {
                inStream.Seek(listLoc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < listCount; i++)
                {
                    damageListLocs.Add(inReader.ReadInt32());
                    damageListEntryCounts.Add(inReader.ReadInt32());
                }
                for (int i = 0; i < listCount; i++)
                {
                    List<DamageTypeEntry> currentList = new List<DamageTypeEntry>();
                    for (int j = 0; j < damageListEntryCounts[i]; j++)
                    {
                        inStream.Seek(damageListLocs[i] + j * 0xC - baseAddr, SeekOrigin.Begin);
                        DamageTypeEntry entry = new DamageTypeEntry();
                        entry.DamageType = inReader.ReadInt32();

                        int anglesLoc = inReader.ReadInt32();
                        int anglesCount = inReader.ReadInt32();
                        if(anglesCount != 0)
                        {
                            for(int k = 0; k < anglesCount; k++)
                            {
                                inStream.Seek(anglesLoc + k * 0x10 - baseAddr, SeekOrigin.Begin);
                                DamageAngleEntry angle = new DamageAngleEntry();
                                angle.UnknownInt1 = inReader.ReadInt32();
                                angle.UnknownInt2 = inReader.ReadInt32();
                                int actionListLoc = inReader.ReadInt32();
                                int actionCount = inReader.ReadInt32();

                                if(actionCount > 0)
                                {
                                    inStream.Seek(actionListLoc - baseAddr, SeekOrigin.Begin);
                                    for(int m = 0; m < actionCount; m++)
                                    {
                                        angle.ActionList.Add(inReader.ReadInt32());
                                    }
                                }
                                entry.Angles.Add(angle);
                            }
                        }
                        currentList.Add(entry);
                    }
                    DamageTypeEntries.Add(currentList);
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();

            //Write all the actions
            //Then all the angle data
            //Then the lists
            //Then the lists-of-lists
            //So we're gonna be iterating over the collections MANY times.
            List<List<List<int>>> actionLocations = new List<List<List<int>>>();
            for(int i = 0; i < DamageTypeEntries.Count; i++)
            {
                var listList = new List<List<int>>();
                for(int j = 0; j < DamageTypeEntries[i].Count; j++)
                {
                    var list = new List<int>();
                    
                    foreach(var angle in DamageTypeEntries[i][j].Angles)
                    {
                        if (angle.ActionList.Count > 0)
                        {
                            list.Add((int)outStream.Position);
                            foreach (var act in angle.ActionList)
                            {
                                outWriter.Write(act);
                            }
                        }
                        else
                        {
                            list.Add((int)0);
                        }
                    }
                    listList.Add(list);
                }
                actionLocations.Add(listList);
            }

            List<List<int>> angleLocations = new List<List<int>>();
            for (int i = 0; i < DamageTypeEntries.Count; i++)
            {
                var list = new List<int>();
                for (int j = 0; j < DamageTypeEntries[i].Count; j++)
                {

                    list.Add((int)outStream.Position);
                    for (int k = 0; k < DamageTypeEntries[i][j].Angles.Count; k++)
                    {
                        var angle = DamageTypeEntries[i][j].Angles[k];
                        outWriter.Write(angle.UnknownInt1);
                        outWriter.Write(angle.UnknownInt2);
                        if (actionLocations[i][j][k] != 0)
                        {
                            pointerLocs.Add((int)outStream.Position);
                        }
                        outWriter.Write(actionLocations[i][j][k]);
                        outWriter.Write(angle.ActionList.Count);
                    }
                }
                angleLocations.Add(list);
            }

            List<int> damageDataListLocations = new List<int>();
            for (int i = 0; i < DamageTypeEntries.Count; i++)
            {
                damageDataListLocations.Add((int)outStream.Position);
                for (int j = 0; j < DamageTypeEntries[i].Count; j++)
                {
                    outWriter.Write(DamageTypeEntries[i][j].DamageType);
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(angleLocations[i][j]);
                    outWriter.Write(DamageTypeEntries[i][j].Angles.Count);
                }
            }

            int damageDataListLoc = (int)outStream.Position;
            for (int i = 0; i < damageDataListLocations.Count; i++)
            {
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(damageDataListLocations[i]);
                outWriter.Write(DamageTypeEntries[i].Count);
            }

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(damageDataListLoc);
            outWriter.Write(DamageTypeEntries.Count);

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
