using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Missions
{
    [DataContract]
    public class EnemyLayoutFile : PsuFile
    {
        [DataContract]
        public class MonsterEntry
        {
            [DataMember]
            public short monsterNum;
            [DataMember]
            public short element;
            [DataMember]
            public byte kingBuff;
            [DataMember]
            public byte buff1;
            [DataMember]
            public byte buff2;
            [DataMember]
            public byte buff3;
            [DataMember]
            public byte buff4;
            [DataMember]
            public byte unkByte1;
            [DataMember]
            public short spawnAnimation;
            //Possibly related to SpawnData::unkShort1 and unkShort2. Valid values are 0, 1, 2, 3 (any value above 3 is treated as equivalent to 3)
            [DataMember]
            public short unkShort2;
            [DataMember]
            public short spawnDelay;
            [DataMember]
            public short count;
            [DataMember]
            public short unkShort3;
            [DataMember]
            public short unkShort4;
            [DataMember]
            public short unknownShort5;
            [DataMember]
            public short levelModifier;
            [DataMember]
            public short levelCapUnused;
            [DataMember]
            public short unkShort7;
            [DataMember]
            public short unkShort8;
            [DataMember]
            public int unkInt1;
            public override string ToString()
            {
                return "monsterNum:" + monsterNum + "\telement:" + element + "\tkingBuff:" + kingBuff + "\tbuff1:" + buff1 + "\tbuff2:" + buff2 + "\tbuff3:" + buff3 + "\tbuff4:" + buff4 +
                    "\tunkByte1:" + unkByte1.ToString("X2") + "\tspawnAnimation:" + spawnAnimation.ToString("X4") + "\tunkShort2:" + unkShort2.ToString("X4") + "\tspawnDelay:" + spawnDelay + "\tcount:" + count +
                    "\tunkShort3:" + unkShort3.ToString("X4") + "\tunkShort4:" + unkShort4.ToString("X4") + "\tlevelMod:" + unknownShort5 + "\tunkShort5:" + levelModifier.ToString("X4") + "\tunkShort6:" + levelCapUnused.ToString("X4") +
                    "\tunkShort7:" + unkShort7.ToString("X4") + "\tunkShort8:" + unkShort8.ToString("X4") + "\tunkInt1:" + unkInt1.ToString("X8");
            }
        }

        [DataContract]
        public class Arrangement
        {
            [DataMember]
            public short arrangementId;
            [DataMember]
            public short arrangementDelay;
            [DataMember]
            public short formation;
            [DataMember]
            public short initialCount;
            [DataMember]
            public short respawnTrigger;
            [DataMember]
            public short unknownShort1;
            [DataMember]
            public short unknownShort2;
            [DataMember]
            public short unknownShort3;
            public override string ToString()
            {
                return "ID: " + arrangementId + "\tarrangementDelay: " + arrangementDelay + "\tformation: " + formation + "\tinitialCount: " + initialCount + "\trespawnTrigger: " + respawnTrigger + "\tunknownShort1: " + unknownShort1.ToString("X4") + "\tunknownShort2: " + unknownShort2.ToString("X4") + "\tunknownShort3: " + unknownShort3.ToString("X4");
            }
        }

        [DataContract]
        public class SpawnData
        {
            [DataMember]
            public short spawnNum;
            //unkShort1 and unkShort2 appear potentially related to MonsterEntry::unkShort3 (they're stored adjacent in memory). These are also apparently flags (only known values are 0 and 1).
            [DataMember]
            public short unknownFlag1;
            [DataMember]
            public short unknownFlag2;
            [DataMember]
            public short unusedShort1;
            [DataMember]
            public short unusedShort2;
            [DataMember]
            public short unusedShort3;

            public override string ToString()
            {
                return "num: " + spawnNum + "\tunknownFlag1: " + unknownFlag1.ToString("X4") + "\tunknownFlag2: " + unknownFlag2.ToString("X4") + "\tunusedShort1: " + unusedShort1.ToString("X4") + "\tunusedShort2: " + unusedShort2.ToString("X4") + "\tunusedShort3: " + unusedShort3.ToString("X4");
            }
        }

        [DataContract]
        public class SpawnEntry
        {
            [DataMember]
            public List<List<MonsterEntry>> monsters = new List<List<MonsterEntry>>();
            [DataMember]
            public List<Arrangement> arrangements = new List<Arrangement>();
            [DataMember]
            public List<SpawnData> spawnData = new List<SpawnData>();
        }

        [DataMember]
        public List<SpawnEntry> spawns;

        public EnemyLayoutFile(string inFilename, byte[] rawData, byte[] inHeader, int[] ptrs, int baseAddr)
        {
            header = inHeader;
            filename = inFilename;  //Assuming they'll always base 0.
            calculatedPointers = ptrs;
            MemoryStream transFile = new MemoryStream(rawData);
            BinaryReader fileReader = new BinaryReader(transFile);
            transFile.Seek(8, SeekOrigin.Begin);
            int headerLoc = fileReader.ReadInt32();
            transFile.Seek(headerLoc, SeekOrigin.Begin);
            int listLoc = fileReader.ReadInt32() - baseAddr;
            int listCount = fileReader.ReadInt32();
            transFile.Seek(listLoc, SeekOrigin.Begin);
            spawns = new List<SpawnEntry>(listCount);
            int[] listData = new int[3 * listCount];
            int[] spawnDataCounts = new int[listCount];
            int[] arrangementCounts = new int[listCount];
            int[] monsterCounts = new int[listCount];
            for (int i = 0; i < listCount; i++)
            {
                listData[i * 3] = fileReader.ReadInt32() - baseAddr;
                spawnDataCounts[i] = fileReader.ReadInt32();
                listData[i * 3 + 1] = fileReader.ReadInt32() - baseAddr;
                arrangementCounts[i] = fileReader.ReadInt32();
                listData[i * 3 + 2] = fileReader.ReadInt32() - baseAddr;
                monsterCounts[i] = fileReader.ReadInt32();
            }
            for (int i = 0; i < listCount; i++)
            {
                SpawnEntry currentEntry = new SpawnEntry();
                for (int j = 0; j < monsterCounts[i]; j++)
                {
                    transFile.Seek(listData[i * 3 + 2] + j * 8, SeekOrigin.Begin);
                    int entryLoc = fileReader.ReadInt32() - baseAddr;
                    int monsterCount = fileReader.ReadInt32();
                    List<MonsterEntry> entryList = new List<MonsterEntry>();
                    transFile.Seek(entryLoc, SeekOrigin.Begin);
                    for (int k = 0; k < monsterCount; k++)
                    {
                        //transFile.Seek(entryLoc + k * 36, SeekOrigin.Begin);
                        MonsterEntry monsterEntry = new MonsterEntry();
                        monsterEntry.monsterNum = fileReader.ReadInt16();
                        monsterEntry.element = fileReader.ReadInt16();
                        monsterEntry.kingBuff = fileReader.ReadByte();
                        monsterEntry.buff1 = fileReader.ReadByte();
                        monsterEntry.buff2 = fileReader.ReadByte();
                        monsterEntry.buff3 = fileReader.ReadByte();
                        monsterEntry.buff4 = fileReader.ReadByte();
                        monsterEntry.unkByte1 = fileReader.ReadByte();
                        monsterEntry.spawnAnimation = fileReader.ReadInt16();
                        monsterEntry.unkShort2 = fileReader.ReadInt16();
                        monsterEntry.spawnDelay = fileReader.ReadInt16();
                        monsterEntry.count = fileReader.ReadInt16();
                        monsterEntry.unkShort3 = fileReader.ReadInt16();
                        monsterEntry.unkShort4 = fileReader.ReadInt16();
                        monsterEntry.unknownShort5 = fileReader.ReadInt16();
                        monsterEntry.levelModifier = fileReader.ReadInt16();
                        monsterEntry.levelCapUnused = fileReader.ReadInt16();
                        monsterEntry.unkShort7 = fileReader.ReadInt16();
                        monsterEntry.unkShort8 = fileReader.ReadInt16();
                        monsterEntry.unkInt1 = fileReader.ReadInt32();
                        entryList.Add(monsterEntry);
                    }
                    currentEntry.monsters.Add(entryList);
                }
                for (int j = 0; j < arrangementCounts[i]; j++)
                {
                    transFile.Seek(listData[i * 3 + 1] + j * 16, SeekOrigin.Begin);
                    Arrangement arrangement = new Arrangement();
                    arrangement.arrangementId = fileReader.ReadInt16();
                    arrangement.arrangementDelay = fileReader.ReadInt16();
                    arrangement.formation = fileReader.ReadInt16();
                    arrangement.initialCount = fileReader.ReadInt16();
                    arrangement.respawnTrigger = fileReader.ReadInt16();
                    arrangement.unknownShort1 = fileReader.ReadInt16();
                    arrangement.unknownShort2 = fileReader.ReadInt16();
                    arrangement.unknownShort3 = fileReader.ReadInt16();
                    currentEntry.arrangements.Add(arrangement);
                }
                for (int j = 0; j < spawnDataCounts[i]; j++)
                {
                    transFile.Seek(listData[i * 3] + j * 12, SeekOrigin.Begin);
                    SpawnData spawnData = new SpawnData();
                    spawnData.spawnNum = fileReader.ReadInt16();
                    spawnData.unknownFlag1 = fileReader.ReadInt16();
                    spawnData.unknownFlag2 = fileReader.ReadInt16();
                    spawnData.unusedShort1 = fileReader.ReadInt16();
                    spawnData.unusedShort2 = fileReader.ReadInt16();
                    spawnData.unusedShort3 = fileReader.ReadInt16();
                    currentEntry.spawnData.Add(spawnData);
                }
                spawns.Add(currentEntry);
            }
        }

        public override byte[] ToRaw()
        {
            List<int> ptrList = new List<int>();
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Seek(0x10, SeekOrigin.Begin);
            int[] monsterPointers = new int[spawns.Count];
            int[] arrangementPointers = new int[spawns.Count];
            int[] spawnInfoPointers = new int[spawns.Count];

            for (int i = 0; i < spawns.Count; i++)
            {
                int[] monsterIndPointers = new int[spawns[i].monsters.Count];
                //Write monster data, then pointers, then arrangement info, then spawn info.
                for (int j = 0; j < spawns[i].monsters.Count; j++)
                {
                    monsterIndPointers[j] = (int)outStream.Position;
                    //Write monster data.
                    for (int k = 0; k < spawns[i].monsters[j].Count; k++)
                    {
                        MonsterEntry tempMonster = spawns[i].monsters[j][k];
                        outWriter.Write(tempMonster.monsterNum);
                        outWriter.Write(tempMonster.element);
                        outWriter.Write(tempMonster.kingBuff);
                        outWriter.Write(tempMonster.buff1);
                        outWriter.Write(tempMonster.buff2);
                        outWriter.Write(tempMonster.buff3);
                        outWriter.Write(tempMonster.buff4);
                        outWriter.Write(tempMonster.unkByte1);
                        outWriter.Write(tempMonster.spawnAnimation);
                        outWriter.Write(tempMonster.unkShort2);
                        outWriter.Write(tempMonster.spawnDelay);
                        outWriter.Write(tempMonster.count);
                        outWriter.Write(tempMonster.unkShort3);
                        outWriter.Write(tempMonster.unkShort4);
                        outWriter.Write(tempMonster.unknownShort5);
                        outWriter.Write(tempMonster.levelModifier);
                        outWriter.Write(tempMonster.levelCapUnused);
                        outWriter.Write(tempMonster.unkShort7);
                        outWriter.Write(tempMonster.unkShort8);
                        outWriter.Write(tempMonster.unkInt1);
                    }
                }
                //Write pointers to monster data.
                monsterPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].monsters.Count; j++)
                {
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(monsterIndPointers[j]);
                    outWriter.Write(spawns[i].monsters[j].Count);
                }
                //Write arrangement info.
                arrangementPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].arrangements.Count; j++)
                {
                    Arrangement tempArrange = spawns[i].arrangements[j];
                    outWriter.Write(tempArrange.arrangementId);
                    outWriter.Write(tempArrange.arrangementDelay);
                    outWriter.Write(tempArrange.formation);
                    outWriter.Write(tempArrange.initialCount);
                    outWriter.Write(tempArrange.respawnTrigger);
                    outWriter.Write(tempArrange.unknownShort1);
                    outWriter.Write(tempArrange.unknownShort2);
                    outWriter.Write(tempArrange.unknownShort3);
                }
                //Write spawn info.
                spawnInfoPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].spawnData.Count; j++)
                {
                    SpawnData tempSpawnData = spawns[i].spawnData[j];
                    outWriter.Write(tempSpawnData.spawnNum);
                    outWriter.Write(tempSpawnData.unknownFlag1);
                    outWriter.Write(tempSpawnData.unknownFlag2);
                    outWriter.Write(tempSpawnData.unusedShort1);
                    outWriter.Write(tempSpawnData.unusedShort2);
                    outWriter.Write(tempSpawnData.unusedShort3);
                }
            }
            int ptrListLoc = (int)outStream.Position;
            for (int i = 0; i < spawns.Count; i++)
            {
                ptrList.Add((int)outStream.Position);
                outWriter.Write(spawnInfoPointers[i]);
                outWriter.Write(spawns[i].spawnData.Count);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(arrangementPointers[i]);
                outWriter.Write(spawns[i].arrangements.Count);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(monsterPointers[i]);
                outWriter.Write(spawns[i].monsters.Count);
            }
            int mainPtr = (int)outStream.Position;
            ptrList.Add((int)outStream.Position);
            outWriter.Write(ptrListLoc);
            outWriter.Write(spawns.Count);
            int fileLength = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(fileLength);
            outWriter.Write(mainPtr);
            outWriter.Write(0);
            calculatedPointers = ptrList.ToArray();
            return outStream.ToArray();
        }
    }
}
