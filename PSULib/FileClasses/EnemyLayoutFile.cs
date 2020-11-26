using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace psu_generic_parser
{
    [DataContract]
    public class EnemyLayoutFile : PsuFile
    {
        [DataContract]
        public struct MonsterEntry
        {
            [DataMember]
            public short monsterNum;
            [DataMember]
            public short element;
            [DataMember]
            public byte kingBuff;
            [DataMember]
            public byte shieldBuff;
            [DataMember]
            public byte swordBuff;
            [DataMember]
            public byte unkBuff;
            [DataMember]
            public byte staffBuff;
            [DataMember]
            public byte unkByte1;
            [DataMember]
            public short unkShort1;
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
            public short levelMod;
            [DataMember]
            public short unkShort5;
            [DataMember]
            public short unkShort6;
            [DataMember]
            public short unkShort7;
            [DataMember]
            public short unkShort8;
            [DataMember]
            public int unkInt1;
        }

        [DataContract]
        public struct Arrangement
        {
            [DataMember]
            public int unknownInt1;
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
        }

        [DataContract]
        public struct SpawnData
        {
            [DataMember]
            public short spawnNum;
            [DataMember]
            public short unkShort1;
            [DataMember]
            public short unkShort2;
            [DataMember]
            public short unkShort3;
            [DataMember]
            public short unkShort4;
            [DataMember]
            public short unkShort5;
        }

        [DataContract]
        public struct SpawnEntry
        {
            [DataMember]
            public MonsterEntry[][] monsters;
            [DataMember]
            public Arrangement[] arrangements;
            [DataMember]
            public SpawnData[] spawnData;
        }

        [DataMember]
        public SpawnEntry[] spawns;

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
            spawns = new SpawnEntry[listCount];
            int[] listData = new int[3 * listCount];
            for (int i = 0; i < listCount; i++)
            {
                listData[i * 3] = fileReader.ReadInt32() - baseAddr;
                spawns[i].spawnData = new SpawnData[fileReader.ReadInt32()];
                listData[i * 3 + 1] = fileReader.ReadInt32() - baseAddr;
                spawns[i].arrangements = new Arrangement[fileReader.ReadInt32()];
                listData[i * 3 + 2] = fileReader.ReadInt32() - baseAddr;
                spawns[i].monsters = new MonsterEntry[fileReader.ReadInt32()][];
            }
            for (int i = 0; i < listCount; i++)
            {
                for (int j = 0; j < spawns[i].monsters.Length; j++)
                {
                    transFile.Seek(listData[i * 3 + 2] + j * 8, SeekOrigin.Begin);
                    int entryLoc = fileReader.ReadInt32() - baseAddr;
                    spawns[i].monsters[j] = new MonsterEntry[fileReader.ReadInt32()];
                    transFile.Seek(entryLoc, SeekOrigin.Begin);
                    for (int k = 0; k < spawns[i].monsters[j].Length; k++)
                    {
                        //transFile.Seek(entryLoc + k * 36, SeekOrigin.Begin);
                        spawns[i].monsters[j][k] = new MonsterEntry();
                        spawns[i].monsters[j][k].monsterNum = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].element = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].kingBuff = fileReader.ReadByte();
                        spawns[i].monsters[j][k].shieldBuff = fileReader.ReadByte();
                        spawns[i].monsters[j][k].swordBuff = fileReader.ReadByte();
                        spawns[i].monsters[j][k].unkBuff = fileReader.ReadByte();
                        spawns[i].monsters[j][k].staffBuff = fileReader.ReadByte();
                        spawns[i].monsters[j][k].unkByte1 = fileReader.ReadByte();
                        spawns[i].monsters[j][k].unkShort1 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort2 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].spawnDelay = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].count = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort3 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort4 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].levelMod = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort5 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort6 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort7 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkShort8 = fileReader.ReadInt16();
                        spawns[i].monsters[j][k].unkInt1 = fileReader.ReadInt32();
                    }
                }
                for (int j = 0; j < spawns[i].arrangements.Length; j++)
                {
                    transFile.Seek(listData[i * 3 + 1] + j * 16, SeekOrigin.Begin);
                    spawns[i].arrangements[j] = new Arrangement();
                    spawns[i].arrangements[j].unknownInt1 = fileReader.ReadInt32();
                    spawns[i].arrangements[j].formation = fileReader.ReadInt16();
                    spawns[i].arrangements[j].initialCount = fileReader.ReadInt16();
                    spawns[i].arrangements[j].respawnTrigger = fileReader.ReadInt16();
                    spawns[i].arrangements[j].unknownShort1 = fileReader.ReadInt16();
                    spawns[i].arrangements[j].unknownShort2 = fileReader.ReadInt16();
                    spawns[i].arrangements[j].unknownShort3 = fileReader.ReadInt16();
                }
                for (int j = 0; j < spawns[i].spawnData.Length; j++)
                {
                    transFile.Seek(listData[i * 3] + j * 12, SeekOrigin.Begin);
                    spawns[i].spawnData[j] = new SpawnData();
                    spawns[i].spawnData[j].spawnNum = fileReader.ReadInt16();
                    spawns[i].spawnData[j].unkShort1 = fileReader.ReadInt16();
                    spawns[i].spawnData[j].unkShort2 = fileReader.ReadInt16();
                    spawns[i].spawnData[j].unkShort3 = fileReader.ReadInt16();
                    spawns[i].spawnData[j].unkShort4 = fileReader.ReadInt16();
                    spawns[i].spawnData[j].unkShort5 = fileReader.ReadInt16();
                }
            }
        }

        public override byte[] ToRaw()
        {
            List<int> ptrList = new List<int>();
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Seek(0x10, SeekOrigin.Begin);
            int[] monsterPointers = new int[spawns.Length];
            int[] arrangementPointers = new int[spawns.Length];
            int[] spawnInfoPointers = new int[spawns.Length];
            
            for (int i = 0; i < spawns.Length; i++)
            {
                int[] monsterIndPointers = new int[spawns[i].monsters.Length];
                //Write monster data, then pointers, then arrangement info, then spawn info.
                for (int j = 0; j < spawns[i].monsters.Length; j++)
                {
                    monsterIndPointers[j] = (int)outStream.Position;
                    //Write monster data.
                    for (int k = 0; k < spawns[i].monsters[j].Length; k++)
                    {
                        MonsterEntry tempMonster = spawns[i].monsters[j][k];
                        outWriter.Write(tempMonster.monsterNum);
                        outWriter.Write(tempMonster.element);
                        outWriter.Write(tempMonster.kingBuff);
                        outWriter.Write(tempMonster.shieldBuff);
                        outWriter.Write(tempMonster.swordBuff);
                        outWriter.Write(tempMonster.unkBuff);
                        outWriter.Write(tempMonster.staffBuff);
                        outWriter.Write(tempMonster.unkByte1);
                        outWriter.Write(tempMonster.unkShort1);
                        outWriter.Write(tempMonster.unkShort2);
                        outWriter.Write(tempMonster.spawnDelay);
                        outWriter.Write(tempMonster.count);
                        outWriter.Write(tempMonster.unkShort3);
                        outWriter.Write(tempMonster.unkShort4);
                        outWriter.Write(tempMonster.levelMod);
                        outWriter.Write(tempMonster.unkShort5);
                        outWriter.Write(tempMonster.unkShort6);
                        outWriter.Write(tempMonster.unkShort7);
                        outWriter.Write(tempMonster.unkShort8);
                        outWriter.Write(tempMonster.unkInt1);
                    }
                }
                //Write pointers to monster data.
                monsterPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].monsters.Length; j++)
                {
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(monsterIndPointers[j]);
                    outWriter.Write(spawns[i].monsters[j].Length);
                }
                //Write arrangement info.
                arrangementPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].arrangements.Length; j++)
                {
                    Arrangement tempArrange = spawns[i].arrangements[j];
                    outWriter.Write(tempArrange.unknownInt1);
                    outWriter.Write(tempArrange.formation);
                    outWriter.Write(tempArrange.initialCount);
                    outWriter.Write(tempArrange.respawnTrigger);
                    outWriter.Write(tempArrange.unknownShort1);
                    outWriter.Write(tempArrange.unknownShort2);
                    outWriter.Write(tempArrange.unknownShort3);
                }
                //Write spawn info.
                spawnInfoPointers[i] = (int)outStream.Position;
                for (int j = 0; j < spawns[i].spawnData.Length; j++)
                {
                    SpawnData tempSpawnData = spawns[i].spawnData[j];
                    outWriter.Write(tempSpawnData.spawnNum);
                    outWriter.Write(tempSpawnData.unkShort1);
                    outWriter.Write(tempSpawnData.unkShort2);
                    outWriter.Write(tempSpawnData.unkShort3);
                    outWriter.Write(tempSpawnData.unkShort4);
                    outWriter.Write(tempSpawnData.unkShort5);
                }
            }
            int ptrListLoc = (int)outStream.Position;
            for (int i = 0; i < spawns.Length; i++)
            {
                ptrList.Add((int)outStream.Position);
                outWriter.Write(spawnInfoPointers[i]);
                outWriter.Write(spawns[i].spawnData.Length);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(arrangementPointers[i]);
                outWriter.Write(spawns[i].arrangements.Length);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(monsterPointers[i]);
                outWriter.Write(spawns[i].monsters.Length);
            }
            int mainPtr = (int)outStream.Position;
            ptrList.Add((int)outStream.Position);
            outWriter.Write(ptrListLoc);
            outWriter.Write(spawns.Length);
            int fileLength = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(fileLength);
            outWriter.Write(mainPtr);
            outWriter.Write((int)0);
            calculatedPointers = ptrList.ToArray();
            return (outStream.ToArray());
        }
    }
}
