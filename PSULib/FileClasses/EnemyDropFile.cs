using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class EnemyDropFile : PsuFile
    {
        public class MonsterDrop
        {
            private ushort enemyNum;

            public ushort EnemyNum
            {
                get { return enemyNum; }
                set { enemyNum = value; }
            }
            private ushort specialNum;

            public ushort SpecialNum
            {
                get { return specialNum; }
                set { specialNum = value; }
            }
            private ushort dropNothing;

            public ushort DropNothing
            {
                get { return dropNothing; }
                set { dropNothing = value; }
            }
            private ushort areaProb;

            public ushort AreaProb
            {
                get { return areaProb; }
                set { areaProb = value; }
            }
            private ushort mesetaProb;

            public ushort MesetaProb
            {
                get { return mesetaProb; }
                set { mesetaProb = value; }
            }
            private ushort tier1Prob;

            public ushort Tier1Prob
            {
                get { return tier1Prob; }
                set { tier1Prob = value; }
            }
            private ushort tier2Prob;

            public ushort Tier2Prob
            {
                get { return tier2Prob; }
                set { tier2Prob = value; }
            }
            private ushort tier3Prob;

            public ushort Tier3Prob
            {
                get { return tier3Prob; }
                set { tier3Prob = value; }
            }
            private ushort tier4Prob;

            public ushort Tier4Prob
            {
                get { return tier4Prob; }
                set { tier4Prob = value; }
            }
            private ushort tier5Prob;

            public ushort Tier5Prob
            {
                get { return tier5Prob; }
                set { tier5Prob = value; }
            }
            private ushort mesetaMin;

            public ushort MesetaMin
            {
                get { return mesetaMin; }
                set { mesetaMin = value; }
            }
            private ushort mesetaMax;

            public ushort MesetaMax
            {
                get { return mesetaMax; }
                set { mesetaMax = value; }
            }
            private int tier1Drop;

            public int Tier1Drop
            {
                get { return tier1Drop; }
                set { tier1Drop = value; }
            }
            private int tier2Drop;

            public int Tier2Drop
            {
                get { return tier2Drop; }
                set { tier2Drop = value; }
            }
            private int tier3Drop;

            public int Tier3Drop
            {
                get { return tier3Drop; }
                set { tier3Drop = value; }
            }
            private int tier4Drop;

            public int Tier4Drop
            {
                get { return tier4Drop; }
                set { tier4Drop = value; }
            }
            private int tier5Drop;

            public int Tier5Drop
            {
                get { return tier5Drop; }
                set { tier5Drop = value; }
            }
            private int specDrop1;

            public int SpecDrop1
            {
                get { return specDrop1; }
                set { specDrop1 = value; }
            }
            private int specDrop2;

            public int SpecDrop2
            {
                get { return specDrop2; }
                set { specDrop2 = value; }
            }
            private int specDrop3;

            public int SpecDrop3
            {
                get { return specDrop3; }
                set { specDrop3 = value; }
            }
            private ushort specProb1;

            public ushort SpecProb1
            {
                get { return specProb1; }
                set { specProb1 = value; }
            }
            private ushort specProb2;

            public ushort SpecProb2
            {
                get { return specProb2; }
                set { specProb2 = value; }
            }
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
                temp.Tier2Prob = inReader.ReadUInt16();
                temp.Tier3Prob = inReader.ReadUInt16();
                temp.Tier4Prob = inReader.ReadUInt16();
                temp.Tier5Prob = inReader.ReadUInt16();
                temp.MesetaMin = inReader.ReadUInt16();
                temp.MesetaMax = inReader.ReadUInt16();
                temp.Tier1Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                temp.Tier2Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                temp.Tier3Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                temp.Tier4Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
                temp.Tier5Drop = inReader.ReadByte() << 24 | inReader.ReadByte() << 16 | inReader.ReadByte() << 8 | inReader.ReadByte();
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

            Dictionary<String, int> specDrops = new Dictionary<string, int>();

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
                outWriter.Write(monsterDrops[i].Tier2Prob);
                outWriter.Write(monsterDrops[i].Tier3Prob);
                outWriter.Write(monsterDrops[i].Tier4Prob);
                outWriter.Write(monsterDrops[i].Tier5Prob);
                outWriter.Write(monsterDrops[i].MesetaMin);
                outWriter.Write(monsterDrops[i].MesetaMax);
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier1Drop).Reverse().ToArray());
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier2Drop).Reverse().ToArray());
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier3Drop).Reverse().ToArray());
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier4Drop).Reverse().ToArray());
                outWriter.Write(BitConverter.GetBytes(monsterDrops[i].Tier5Drop).Reverse().ToArray());
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write((short)specDrops.Count);
            outWriter.Write((short)monsterDrops.Length);
            calculatedPointers[0] = (int)outStream.Position;
            outWriter.Write(specDropLoc);
            calculatedPointers[1] = (int)outStream.Position;
            outWriter.Write(mainDropLoc);
            int padSize = (int)((outStream.Position + 0xF) & 0xFFFFFFF0);
            outWriter.Write(new byte[padSize - outStream.Position]);
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(ASCIIEncoding.ASCII.GetBytes("NXR\0"));
            outWriter.Write((int)padSize);    //Size to be.
            outWriter.Write((int)headerLoc);    //Header location.
            outWriter.Write((int)0);    //I don't even know if this stuff does anything...
            outWriter.Write((int)1);
            outWriter.Write((int)-1);
            outWriter.Write((int)-1);
            return outStream.ToArray();

        }
    }
}
