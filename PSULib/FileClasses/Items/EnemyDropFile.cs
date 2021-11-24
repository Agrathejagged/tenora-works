using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Items
{
    public class EnemyDropFile : PsuFile
    {
        public bool isV1File = false;
        public class MonsterDrop
        {
            public ushort EnemyNum { get; set; }
            public ushort SpecialNum { get; set; }
            public ushort DropNothing { get; set; }
            public ushort AreaProb { get; set; }
            public ushort MesetaProb { get; set; }
            public ushort Tier1Prob { get; set; }
            public ushort Tier2Prob { get; set; }
            public ushort Tier3Prob { get; set; }
            public ushort Tier4Prob { get; set; }
            public ushort Tier5Prob { get; set; }
            public uint MesetaMin { get; set; }
            public uint MesetaMax { get; set; }
            public int Tier1Drop { get; set; }
            public int Tier2Drop { get; set; }
            public int Tier3Drop { get; set; }
            public int Tier4Drop { get; set; }
            public int Tier5Drop { get; set; }
            public int SpecDrop1 { get; set; }
            public int SpecDrop2 { get; set; }
            public int SpecDrop3 { get; set; }
            public ushort SpecProb1 { get; set; }
            public ushort SpecProb2 { get; set; }
        }


        public MonsterDrop[] monsterDrops;

        public EnemyDropFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;
            calculatedPointers = (int[])ptrs.Clone();
            byte[] rawTemp = new byte[rawData.Length];
            Array.Copy(rawData, rawTemp, rawData.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                calculatedPointers[i] -= baseAddr;
                int ptr = BitConverter.ToInt32(rawData, calculatedPointers[i]);
                ptr -= baseAddr;
                Array.Copy(BitConverter.GetBytes(ptr), 0, rawTemp, calculatedPointers[i], 4);
            }
            MemoryStream inStream = new MemoryStream(rawTemp);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int mainPointer = inReader.ReadInt32();

            inStream.Seek(mainPointer, SeekOrigin.Begin);
            int specialDropCount = inReader.ReadInt16();
            int levelDropCount = inReader.ReadInt16();
            int specialDropLoc = inReader.ReadInt32();
            int levelDropLoc = inReader.ReadInt32();
            //Try to figure out whether this is a v1 or AotI file.
            //We can worry about PSP1 (and expanded drop tables) later...

            //So v1 enemy entries are 24 bytes long (1 level drop). AotI ones are 44 bytes long (5 level drops).
            //There are guaranteed to be lots of monsters, so if the size is doubled, it should be way too big if it's a v1 file.
            //v1 has 0x5e monsters, AotI has 0x88; not every monster is guaranteed to have an entry, but if there's too many, we aren't in v1.
            isV1File = levelDropCount <= 0x5e && levelDropCount * 0x44 + levelDropLoc > rawData.Length;
            inStream.Seek(specialDropLoc, SeekOrigin.Begin);
            byte[] specialDrops = inReader.ReadBytes(specialDropCount * 16);
            inStream.Seek(levelDropLoc, SeekOrigin.Begin);
            monsterDrops = new MonsterDrop[levelDropCount];
            for (int i = 0; i < levelDropCount; i++)
            {
                MonsterDrop temp = new MonsterDrop();
                temp.EnemyNum = inReader.ReadUInt16();
                temp.SpecialNum = inReader.ReadUInt16();
                temp.DropNothing = inReader.ReadUInt16();
                temp.MesetaProb = inReader.ReadUInt16();
                temp.AreaProb = inReader.ReadUInt16();
                temp.Tier1Prob = inReader.ReadUInt16();
                if (isV1File)
                {
                    temp.MesetaMin = inReader.ReadUInt32();
                    temp.MesetaMax = inReader.ReadUInt32();
                }
                if (!isV1File)
                {
                    temp.Tier2Prob = inReader.ReadUInt16();
                    temp.Tier3Prob = inReader.ReadUInt16();
                    temp.Tier4Prob = inReader.ReadUInt16();
                    temp.Tier5Prob = inReader.ReadUInt16();
                    temp.MesetaMin = inReader.ReadUInt16();
                    temp.MesetaMax = inReader.ReadUInt16();
                }
                temp.Tier1Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                if (!isV1File)
                {
                    temp.Tier2Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                    temp.Tier3Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                    temp.Tier4Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                    temp.Tier5Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                }
                temp.SpecDrop1 = specialDrops[temp.SpecialNum * 16 + 0] << 24 | specialDrops[temp.SpecialNum * 16 + 1] << 16 | specialDrops[temp.SpecialNum * 16 + 2] << 8 | specialDrops[temp.SpecialNum * 16 + 3];
                temp.SpecDrop2 = specialDrops[temp.SpecialNum * 16 + 4] << 24 | specialDrops[temp.SpecialNum * 16 + 5] << 16 | specialDrops[temp.SpecialNum * 16 + 6] << 8 | specialDrops[temp.SpecialNum * 16 + 7];
                temp.SpecDrop3 = specialDrops[temp.SpecialNum * 16 + 8] << 24 | specialDrops[temp.SpecialNum * 16 + 9] << 16 | specialDrops[temp.SpecialNum * 16 + 10] << 8 | specialDrops[temp.SpecialNum * 16 + 11];
                temp.SpecProb1 = BitConverter.ToUInt16(specialDrops, temp.SpecialNum * 16 + 12);
                temp.SpecProb2 = BitConverter.ToUInt16(specialDrops, temp.SpecialNum * 16 + 14);
                monsterDrops[i] = temp;
            }
        }




        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            MemoryStream specStream = new MemoryStream();
            BinaryWriter specWriter = new BinaryWriter(specStream);

            Dictionary<string, int> specDrops = new Dictionary<string, int>();

            for (int i = 0; i < monsterDrops.Length; i++)
            {
                byte[] currentSpecialDrops = new byte[16];
                BitConverter.GetBytes(monsterDrops[i].SpecDrop1).Reverse().ToArray().CopyTo(currentSpecialDrops, 0);
                BitConverter.GetBytes(monsterDrops[i].SpecDrop2).Reverse().ToArray().CopyTo(currentSpecialDrops, 4);
                BitConverter.GetBytes(monsterDrops[i].SpecDrop3).Reverse().ToArray().CopyTo(currentSpecialDrops, 8);
                BitConverter.GetBytes(monsterDrops[i].SpecProb1).CopyTo(currentSpecialDrops, 0xC);
                BitConverter.GetBytes(monsterDrops[i].SpecProb2).CopyTo(currentSpecialDrops, 0xE);
                if (!specDrops.ContainsKey(BitConverter.ToString(currentSpecialDrops)))
                {
                    specDrops.Add(BitConverter.ToString(currentSpecialDrops), specDrops.Count);
                    specWriter.Write(currentSpecialDrops);
                }
                monsterDrops[i].SpecialNum = (byte)specDrops[BitConverter.ToString(currentSpecialDrops)];
            }
            outStream.Seek(0x1C, SeekOrigin.Begin);
            int specDropLoc = (int)outStream.Position;
            outWriter.Write(specStream.ToArray());
            int mainDropLoc = (int)outStream.Position;
            for (int i = 0; i < monsterDrops.Length; i++)
            {
                outWriter.Write(monsterDrops[i].EnemyNum);
                outWriter.Write(monsterDrops[i].SpecialNum);
                outWriter.Write(monsterDrops[i].DropNothing);
                outWriter.Write(monsterDrops[i].AreaProb);
                outWriter.Write(monsterDrops[i].MesetaProb);
                outWriter.Write(monsterDrops[i].Tier1Prob);
                if (isV1File)
                {
                    outWriter.Write(monsterDrops[i].MesetaMin);
                    outWriter.Write(monsterDrops[i].MesetaMax);
                }
                if (!isV1File)
                {
                    outWriter.Write(monsterDrops[i].Tier2Prob);
                    outWriter.Write(monsterDrops[i].Tier3Prob);
                    outWriter.Write(monsterDrops[i].Tier4Prob);
                    outWriter.Write(monsterDrops[i].Tier5Prob);
                    outWriter.Write((ushort)monsterDrops[i].MesetaMin);
                    outWriter.Write((ushort)monsterDrops[i].MesetaMax);
                }
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier1Drop).Reverse().ToArray());
                if (!isV1File)
                {
                    outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier2Drop).Reverse().ToArray());
                    outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier3Drop).Reverse().ToArray());
                    outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier4Drop).Reverse().ToArray());
                    outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier5Drop).Reverse().ToArray());
                }
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write((short)specDrops.Count);
            outWriter.Write((short)monsterDrops.Length);
            calculatedPointers[0] = (int)outStream.Position;
            outWriter.Write(specDropLoc);
            calculatedPointers[1] = (int)outStream.Position;
            outWriter.Write(mainDropLoc);
            int padSize = (int)(outStream.Position + 0xF & 0xFFFFFFF0);
            outWriter.Write(new byte[padSize - outStream.Position]);
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(Encoding.ASCII.GetBytes("NXR\0"));
            outWriter.Write(padSize);    //Size to be.
            outWriter.Write(headerLoc);    //Header location.
            outWriter.Write(0);    //I don't even know if this stuff does anything...
            outWriter.Write(1);
            outWriter.Write(-1);
            outWriter.Write(-1);
            return outStream.ToArray();

        }
    }
}
