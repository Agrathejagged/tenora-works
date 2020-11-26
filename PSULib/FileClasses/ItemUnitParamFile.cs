using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class ItemUnitParamFile : PsuFile
    {

        public class HeadUnit
        {
            public bool valid { get; set; }
            public byte unitRank { get; set; }
            public byte armorRank { get; set; }
            public byte manufacturer { get; set; }
            public byte setId { get; set; }
            public short tpMod { get; set; }
            public short mstMod { get; set; }
            public byte rangeMod { get; set; }
            public byte ppMod { get; set; }
            public byte techSpd1 { get; set; }
            public byte techSpd2 { get; set; }
        }

        public class ArmUnit
        {
            public bool valid { get; set; }
            public byte unitRank { get; set; }
            public byte armorRank { get; set; }
            public byte manufacturer { get; set; }
            public byte setId { get; set; }
            public short atpMod { get; set; }
            public short ataMod { get; set; }
            public byte autoDamageLevel { get; set; }
            public byte strikeRangeMod { get; set; }
            public byte strikePpMod { get; set; }
            public byte strikeSpd { get; set; }
            public byte firearmRangeMod { get; set; }
            public byte firearmPpMod { get; set; }
            public byte firearmSpd { get; set; }
            public byte unknownByte { get; set; }
        }

        public class BodyUnit
        {
            public bool valid { get; set; }
            public byte unitRank { get; set; }
            public byte armorRank { get; set; }
            public byte manufacturer { get; set; }
            public byte setId { get; set; }
            public short dfpMod { get; set; }
            public short evpMod { get; set; }
            public byte elementResist { get; set; }
            public byte resists1;
            public byte resists2;
            public byte resists3;
            public bool burnResist
            {
                get { return (resists1 & 0x1) != 0; }
                set { resists1 = value ? (byte)(resists1 | 1) : (byte)(resists1 & 0x1E); }
            }
            public bool poisonResist
            {
                get { return (resists1 & 0x2) != 0; }
                set { resists1 = value ? (byte)(resists1 | 2) : (byte)(resists1 & 0x1D); }
            }
            public bool virusResist
            {
                get { return (resists1 & 0x4) != 0; }
                set { resists1 = value ? (byte)(resists1 | 4) : (byte)(resists1 & 0x1B); }
            }
            public bool shockResist
            {
                get { return (resists1 & 0x8) != 0; }
                set { resists1 = value ? (byte)(resists1 | 8) : (byte)(resists1 & 0x17); }
            }
            public bool silenceResist
            {
                get { return (resists1 & 0x10) != 0; }
                set { resists1 = value ? (byte)(resists1 | 16) : (byte)(resists1 & 0x0F); }
            }
            public bool freezeResist
            {
                get { return (resists2 & 0x1) != 0; }
                set { resists2 = value ? (byte)(resists2 | 1) : (byte)(resists2 & 0xE); }
            }
            public bool sleepResist
            {
                get { return (resists2 & 0x2) != 0; }
                set { resists2 = value ? (byte)(resists2 | 2) : (byte)(resists2 & 0xD); }
            }
            public bool stunResist
            {
                get { return (resists2 & 0x4) != 0; }
                set { resists2 = value ? (byte)(resists2 | 4) : (byte)(resists2 & 0xB); }
            }
            public bool confuseResist
            {
                get { return (resists2 & 0x8) != 0; }
                set { resists2 = value ? (byte)(resists2 | 8) : (byte)(resists2 & 0x7); }
            }
            public bool incapResist
            {
                get { return (resists3 & 0x1) != 0; }
                set { resists3 = value ? (byte)(resists3 | 1) : (byte)(resists3 & 0x6); }
            }
            public bool buffResist
            {
                get { return (resists3 & 0x2) != 0; }
                set { resists3 = value ? (byte)(resists3 | 2) : (byte)(resists3 & 0x5); }
            }
            public bool debuffResist
            {
                get { return (resists3 & 0x4) != 0; }
                set { resists3 = value ? (byte)(resists3 | 4) : (byte)(resists3 & 0x3); }
            }
        }

        public class ExtraUnit
        {
            public bool valid { get; set; }
            public byte unitRank { get; set; }
            public byte armorRank { get; set; }
            public byte manufacturer { get; set; }
            public byte setId { get; set; }
            public short staMod1 { get; set; }
            public short staMod2 { get; set; }
            public byte autoRecoveryLevel { get; set; }
            public byte ppRegen { get; set; }
            public byte suvId { get; set; }
            public byte visualEffect { get; set; }
        }

        public List<HeadUnit> headUnits;
        public List<ArmUnit> armUnits;
        public List<BodyUnit> bodyUnits;
        public List<ExtraUnit> extraUnits;

        public ItemUnitParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);

            int[] listLocs = new int[4];
            int[] indexLocs = new int[4];
            short[] indexCounts = new short[4];
            short[] listCounts = new short[4];

            byte[][] indexes = new byte[4][];
            for(int i = 0; i < 4; i++)
            {
                inStream.Seek(headerLoc + 12 * i, SeekOrigin.Begin);
                listLocs[i] = inReader.ReadInt32() - baseAddr;
                indexLocs[i] = inReader.ReadInt32() - baseAddr;
                indexCounts[i] = inReader.ReadInt16();
                listCounts[i] = inReader.ReadInt16();
                if(indexCounts[i] != 0)
                {
                    inStream.Seek(indexLocs[i], SeekOrigin.Begin);
                    indexes[i] = inReader.ReadBytes(indexCounts[i]);
                }
            }

            int headLoopCounter = Math.Max(indexCounts[0], listCounts[0]);
            headUnits = new List<HeadUnit>(headLoopCounter);
            for (int i = 0; i < headLoopCounter; i++)
            {
                int currentIndex = i;
                if (indexCounts[0] != 0)
                    currentIndex = indexes[0][i];
                HeadUnit tempUnit = new HeadUnit();
                if (currentIndex != 0xFF)
                {
                    inStream.Seek(listLocs[0] + 12 * currentIndex, SeekOrigin.Begin);
                    tempUnit.valid = true;
                    tempUnit.unitRank = inReader.ReadByte();
                    tempUnit.armorRank = inReader.ReadByte();
                    tempUnit.manufacturer = inReader.ReadByte();
                    tempUnit.setId = inReader.ReadByte();
                    tempUnit.tpMod = inReader.ReadInt16();
                    tempUnit.mstMod = inReader.ReadInt16();
                    tempUnit.rangeMod = inReader.ReadByte();
                    tempUnit.ppMod = inReader.ReadByte();
                    tempUnit.techSpd1 = inReader.ReadByte();
                    tempUnit.techSpd2 = inReader.ReadByte();
                }
                else
                    tempUnit.valid = false;
                headUnits.Insert(i, tempUnit);
            }

            int armLoopCounter = Math.Max(indexCounts[1], listCounts[1]);
            armUnits = new List<ArmUnit>(armLoopCounter);
            for (int i = 0; i < armLoopCounter; i++)
            {
                int currentIndex = i;
                if (indexCounts[1] != 0)
                    currentIndex = indexes[1][i];
                ArmUnit tempUnit = new ArmUnit();
                if (currentIndex != 0xFF)
                {
                    inStream.Seek(listLocs[1] + 16 * currentIndex, SeekOrigin.Begin);
                    tempUnit.valid = true;
                    tempUnit.unitRank = inReader.ReadByte();
                    tempUnit.armorRank = inReader.ReadByte();
                    tempUnit.manufacturer = inReader.ReadByte();
                    tempUnit.setId = inReader.ReadByte();
                    tempUnit.atpMod = inReader.ReadInt16();
                    tempUnit.ataMod = inReader.ReadInt16();
                    tempUnit.autoDamageLevel = inReader.ReadByte();
                    tempUnit.strikeRangeMod = inReader.ReadByte();
                    tempUnit.strikePpMod = inReader.ReadByte();
                    tempUnit.strikeSpd = inReader.ReadByte();
                    tempUnit.firearmRangeMod = inReader.ReadByte();
                    tempUnit.firearmPpMod = inReader.ReadByte();
                    tempUnit.firearmSpd = inReader.ReadByte();
                    tempUnit.unknownByte = inReader.ReadByte();
                }
                else
                    tempUnit.valid = false;
                armUnits.Insert(i, tempUnit);
            }

            int bodyLoopCounter = Math.Max(indexCounts[2], listCounts[2]);
            bodyUnits = new List<BodyUnit>(bodyLoopCounter);
            for (int i = 0; i < bodyLoopCounter; i++)
            {
                int currentIndex = i;
                if (indexCounts[2] != 0)
                    currentIndex = indexes[2][i];
                BodyUnit tempUnit = new BodyUnit();

                if (currentIndex != 0xFF)
                {
                    inStream.Seek(listLocs[2] + 12 * currentIndex, SeekOrigin.Begin);
                    tempUnit.valid = true;
                    tempUnit.unitRank = inReader.ReadByte();
                    tempUnit.armorRank = inReader.ReadByte();
                    tempUnit.manufacturer = inReader.ReadByte();
                    tempUnit.setId = inReader.ReadByte();
                    tempUnit.dfpMod = inReader.ReadInt16();
                    tempUnit.evpMod = inReader.ReadInt16();
                    tempUnit.elementResist = inReader.ReadByte();
                    tempUnit.resists1 = inReader.ReadByte();
                    tempUnit.resists2 = inReader.ReadByte();
                    tempUnit.resists3 = inReader.ReadByte();
                }
                else
                    tempUnit.valid = false;
                bodyUnits.Insert(i, tempUnit);
            }

            int extraLoopCounter = Math.Max(indexCounts[3], listCounts[3]);
            extraUnits = new List<ExtraUnit>(extraLoopCounter);
            for (int i = 0; i < extraLoopCounter; i++)
            {
                int currentIndex = i;
                if (indexCounts[3] != 0)
                    currentIndex = indexes[3][i];
                ExtraUnit tempUnit = new ExtraUnit();
                if (currentIndex != 0xFF)
                {
                    inStream.Seek(listLocs[3] + 12 * currentIndex, SeekOrigin.Begin);
                    tempUnit.valid = true;
                    tempUnit.unitRank = inReader.ReadByte();
                    tempUnit.armorRank = inReader.ReadByte();
                    tempUnit.manufacturer = inReader.ReadByte();
                    tempUnit.setId = inReader.ReadByte();
                    tempUnit.staMod1 = inReader.ReadInt16();
                    tempUnit.staMod2 = inReader.ReadInt16();
                    tempUnit.autoRecoveryLevel = inReader.ReadByte();
                    tempUnit.ppRegen = inReader.ReadByte();
                    tempUnit.suvId = inReader.ReadByte();
                    tempUnit.visualEffect = inReader.ReadByte();
                }
                else
                    tempUnit.valid = false;
                extraUnits.Insert(i, tempUnit);
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outStream.Seek(0x1C, SeekOrigin.Begin);

            byte[][] indexes = new byte[4][];
            indexes[0] = new byte[headUnits.Count];
            indexes[1] = new byte[armUnits.Count];
            indexes[2] = new byte[bodyUnits.Count];
            indexes[3] = new byte[extraUnits.Count];
            byte[] counts = new byte[4];
            bool[] invalids = new bool[4];
            int[] unitLocs = new int[4];
            int[] indexLocs = new int[4];

            unitLocs[0] = (int)outStream.Position;
            for (int i = 0; i < headUnits.Count; i++)
            {
                if (headUnits[i].valid)
                {
                    indexes[0][i] = counts[0]++;
                    outWriter.Write(headUnits[i].unitRank);
                    outWriter.Write(headUnits[i].armorRank);
                    outWriter.Write(headUnits[i].manufacturer);
                    outWriter.Write(headUnits[i].setId);
                    outWriter.Write(headUnits[i].tpMod);
                    outWriter.Write(headUnits[i].mstMod);
                    outWriter.Write(headUnits[i].rangeMod);
                    outWriter.Write(headUnits[i].ppMod);
                    outWriter.Write(headUnits[i].techSpd1);
                    outWriter.Write(headUnits[i].techSpd2);
                }
                else
                {
                    invalids[0] = true;
                    indexes[0][i] = 0xFF;
                }
            }
            if (invalids[0])
            {
                indexLocs[0] = (int)outStream.Position;
                outWriter.Write(indexes[0]);
            }
            outStream.Seek((outStream.Position + 0x3) & 0xFFFFFFFC, SeekOrigin.Begin);

            unitLocs[1] = (int)outStream.Position;
            for (int i = 0; i < armUnits.Count; i++)
            {
                if (armUnits[i].valid)
                {
                    indexes[1][i] = counts[1]++;
                    outWriter.Write(armUnits[i].unitRank);
                    outWriter.Write(armUnits[i].armorRank);
                    outWriter.Write(armUnits[i].manufacturer);
                    outWriter.Write(armUnits[i].setId);
                    outWriter.Write(armUnits[i].atpMod);
                    outWriter.Write(armUnits[i].ataMod);
                    outWriter.Write(armUnits[i].autoDamageLevel);
                    outWriter.Write(armUnits[i].strikeRangeMod);
                    outWriter.Write(armUnits[i].strikePpMod);
                    outWriter.Write(armUnits[i].strikeSpd);
                    outWriter.Write(armUnits[i].firearmRangeMod);
                    outWriter.Write(armUnits[i].firearmPpMod);
                    outWriter.Write(armUnits[i].firearmSpd);
                    outWriter.Write(armUnits[i].unknownByte);
                }
                else
                {
                    invalids[1] = true;
                    indexes[1][i] = 0xFF;
                }
            }
            if (invalids[1])
            {
                indexLocs[1] = (int)outStream.Position;
                outWriter.Write(indexes[1]);
            }
            outStream.Seek((outStream.Position + 0x3) & 0xFFFFFFFC, SeekOrigin.Begin);

            unitLocs[2] = (int)outStream.Position;
            for (int i = 0; i < bodyUnits.Count; i++)
            {
                if (bodyUnits[i].valid)
                {
                    indexes[2][i] = counts[2]++;
                    outWriter.Write(bodyUnits[i].unitRank);
                    outWriter.Write(bodyUnits[i].armorRank);
                    outWriter.Write(bodyUnits[i].manufacturer);
                    outWriter.Write(bodyUnits[i].setId);
                    outWriter.Write(bodyUnits[i].dfpMod);
                    outWriter.Write(bodyUnits[i].evpMod);
                    outWriter.Write(bodyUnits[i].elementResist);
                    outWriter.Write(bodyUnits[i].resists1);
                    outWriter.Write(bodyUnits[i].resists2);
                    outWriter.Write(bodyUnits[i].resists3);
                }
                else
                {
                    invalids[2] = true;
                    indexes[2][i] = 0xFF;
                }
            }
            if (invalids[2])
            {
                indexLocs[2] = (int)outStream.Position;
                outWriter.Write(indexes[2]);
            }
            outStream.Seek((outStream.Position + 0x3) & 0xFFFFFFFC, SeekOrigin.Begin);

            unitLocs[3] = (int)outStream.Position;
            for (int i = 0; i < extraUnits.Count; i++)
            {
                if (extraUnits[i].valid)
                {
                    indexes[3][i] = counts[3]++;
                    outWriter.Write(extraUnits[i].unitRank);
                    outWriter.Write(extraUnits[i].armorRank);
                    outWriter.Write(extraUnits[i].manufacturer);
                    outWriter.Write(extraUnits[i].setId);
                    outWriter.Write(extraUnits[i].staMod1);
                    outWriter.Write(extraUnits[i].staMod2);
                    outWriter.Write(extraUnits[i].autoRecoveryLevel);
                    outWriter.Write(extraUnits[i].ppRegen);
                    outWriter.Write(extraUnits[i].suvId);
                    outWriter.Write(extraUnits[i].visualEffect);
                }
                else
                {
                    invalids[3] = true;
                    indexes[3][i] = 0xFF;
                }
            }
            if (invalids[3])
            {
                indexLocs[3] = (int)outStream.Position;
                outWriter.Write(indexes[3]);
            }
            outStream.Seek((outStream.Position + 0x3) & 0xFFFFFFFC, SeekOrigin.Begin);

            List<int> ptrs = new List<int>();
            int headerLoc = (int)outStream.Position;
            for (int i = 0; i < 4; i++)
            {
                ptrs.Add((int)outStream.Position);
                outWriter.Write(unitLocs[i]);
                if (indexLocs[i] != 0)
                    ptrs.Add((int)outStream.Position);
                outWriter.Write(indexLocs[i]);
                if (invalids[i])
                    outWriter.Write((short)indexes[i].Length);
                else
                    outWriter.Write((short)0);
                outWriter.Write((short)counts[i]);
            }
            outStream.Seek((outStream.Position + 7) & 0xFFFFFFF8, SeekOrigin.Begin);
            int fileLength = (int)outStream.Position;
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write((int)0x0052584E);
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            outWriter.Write((int)0);
            outWriter.Write((int)1);
            outWriter.Write((int)-1);
            outWriter.Write((int)-1);

            calculatedPointers = ptrs.ToArray();
            return outStream.ToArray();
        }
    }
}
