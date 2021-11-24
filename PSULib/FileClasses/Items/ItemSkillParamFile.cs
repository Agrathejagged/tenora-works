using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Items
{
    public class ItemSkillParamFile : PsuFile
    {
        public class SkillTier
        {
            public byte hit1Unknown { get; set; }
            public byte hit2Unknown { get; set; }
            public byte hit3Unknown { get; set; }
            public byte unknownByte1 { get; set; }
            public byte hit1PP { get; set; }
            public byte hit2PP { get; set; }
            public byte hit3PP { get; set; }
            public byte hit1Targets { get; set; }
            public byte hit2Targets { get; set; }
            public byte hit3Targets { get; set; }
            public short numAttacks { get; set; }
            public int expPerLevel { get; set; }
            public short expGrowth { get; set; }
            public short hit1AtpMod { get; set; }
            public short hit1AtaMod { get; set; }
            public byte hit1AtpGrowth { get; set; }
            public byte hit1AtaGrowth { get; set; }
            public byte hit1Hitbox { get; set; }
            public byte hit2Hitbox { get; set; }
            public byte hit3Hitbox { get; set; }
            public byte unknownByte2 { get; set; }
            public short hit2AtpMod { get; set; }
            public short hit3AtpMod { get; set; }
            public byte hit2AtpGrowth { get; set; }
            public byte hit3AtpGrowth { get; set; }
            public byte unknownByte3 { get; set; }
            public byte unknownByte4 { get; set; }
        }

        public sbyte[] skillIndexes;
        public SkillTier[][] allSkills;
        public ushort[][] hitBoxes;

        public ItemSkillParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int tiers = inReader.ReadInt32();
            int hitboxLoc = inReader.ReadInt32() - baseAddr;
            int skillLoc = inReader.ReadInt32() - baseAddr;
            int hitboxCount = (skillLoc - hitboxLoc) / 4;
            hitBoxes = new ushort[hitboxCount][];
            int highestIndex = 0;
            byte[] tempIndexes = inReader.ReadBytes(7);
            skillIndexes = new sbyte[7];
            for (int i = 0; i < tempIndexes.Length; i++)
            {
                if (tempIndexes[i] == 0xFF)
                    skillIndexes[i] = -1;
                else
                    skillIndexes[i] = (sbyte)(tempIndexes[i] / tiers);
            }
            highestIndex = skillIndexes.Max() + 1;
            inStream.Seek(hitboxLoc, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i] = new ushort[2];
                hitBoxes[i][0] = inReader.ReadUInt16();
                hitBoxes[i][1] = inReader.ReadUInt16();
            }
            allSkills = new SkillTier[highestIndex][];
            inStream.Seek(skillLoc, SeekOrigin.Begin);
            for (int i = 0; i < highestIndex; i++)
            {
                allSkills[i] = new SkillTier[tiers];
                for (int j = 0; j < tiers; j++)
                {
                    SkillTier tempTier = new SkillTier();
                    tempTier.hit1Unknown = inReader.ReadByte();
                    tempTier.hit2Unknown = inReader.ReadByte();
                    tempTier.hit3Unknown = inReader.ReadByte();
                    tempTier.unknownByte1 = inReader.ReadByte();
                    tempTier.hit1PP = inReader.ReadByte();
                    tempTier.hit2PP = inReader.ReadByte();
                    tempTier.hit3PP = inReader.ReadByte();
                    tempTier.hit1Targets = inReader.ReadByte();
                    tempTier.hit2Targets = inReader.ReadByte();
                    tempTier.hit3Targets = inReader.ReadByte();
                    tempTier.numAttacks = inReader.ReadInt16();
                    tempTier.expPerLevel = inReader.ReadInt32();
                    tempTier.expGrowth = inReader.ReadInt16();
                    tempTier.hit1AtpMod = inReader.ReadInt16();
                    tempTier.hit1AtaMod = inReader.ReadInt16();
                    tempTier.hit1AtpGrowth = inReader.ReadByte();
                    tempTier.hit1AtaGrowth = inReader.ReadByte();
                    tempTier.hit1Hitbox = inReader.ReadByte();
                    tempTier.hit2Hitbox = inReader.ReadByte();
                    tempTier.hit3Hitbox = inReader.ReadByte();
                    tempTier.unknownByte2 = inReader.ReadByte();
                    tempTier.hit2AtpMod = inReader.ReadInt16();
                    tempTier.hit3AtpMod = inReader.ReadInt16();
                    tempTier.hit2AtpGrowth = inReader.ReadByte();
                    tempTier.hit3AtpGrowth = inReader.ReadByte();
                    tempTier.unknownByte3 = inReader.ReadByte();
                    tempTier.unknownByte4 = inReader.ReadByte();
                    allSkills[i][j] = tempTier;
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            int[] ptrs = new int[2];
            outStream.Seek(0x1C, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                for (int j = 0; j < hitBoxes[i].Length; j++)
                    outWriter.Write(hitBoxes[i][j]);
            }
            int tierLoc = (int)outStream.Position;
            for (int i = 0; i < allSkills.Length; i++)
            {
                for (int j = 0; j < allSkills[i].Length; j++)
                {
                    outWriter.Write(allSkills[i][j].hit1Unknown);
                    outWriter.Write(allSkills[i][j].hit2Unknown);
                    outWriter.Write(allSkills[i][j].hit3Unknown);
                    outWriter.Write(allSkills[i][j].unknownByte1);
                    outWriter.Write(allSkills[i][j].hit1PP);
                    outWriter.Write(allSkills[i][j].hit2PP);
                    outWriter.Write(allSkills[i][j].hit3PP);
                    outWriter.Write(allSkills[i][j].hit1Targets);
                    outWriter.Write(allSkills[i][j].hit2Targets);
                    outWriter.Write(allSkills[i][j].hit3Targets);
                    outWriter.Write(allSkills[i][j].numAttacks);
                    outWriter.Write(allSkills[i][j].expPerLevel);
                    outWriter.Write(allSkills[i][j].expGrowth);
                    outWriter.Write(allSkills[i][j].hit1AtpMod);
                    outWriter.Write(allSkills[i][j].hit1AtaMod);
                    outWriter.Write(allSkills[i][j].hit1AtpGrowth);
                    outWriter.Write(allSkills[i][j].hit1AtaGrowth);
                    outWriter.Write(allSkills[i][j].hit1Hitbox);
                    outWriter.Write(allSkills[i][j].hit2Hitbox);
                    outWriter.Write(allSkills[i][j].hit3Hitbox);
                    outWriter.Write(allSkills[i][j].unknownByte2);
                    outWriter.Write(allSkills[i][j].hit2AtpMod);
                    outWriter.Write(allSkills[i][j].hit3AtpMod);
                    outWriter.Write(allSkills[i][j].hit2AtpGrowth);
                    outWriter.Write(allSkills[i][j].hit3AtpGrowth);
                    outWriter.Write(allSkills[i][j].unknownByte3);
                    outWriter.Write(allSkills[i][j].unknownByte4);
                }
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write(allSkills[0].Length);
            ptrs[0] = (int)outStream.Position;
            outWriter.Write(0x1C);
            ptrs[1] = (int)outStream.Position;
            outWriter.Write(tierLoc);
            for (int i = 0; i < skillIndexes.Length; i++)
            {
                if (skillIndexes[i] != -1)
                    outWriter.Write((byte)(skillIndexes[i] * allSkills[0].Length));
                else
                    outWriter.Write((byte)0xFF);
            }
            outStream.Seek(outStream.Position + 7 & 0xFFFFFFF8, SeekOrigin.Begin);
            int fileLength = (int)outStream.Position;
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(0x0052584E);
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            outWriter.Write(0);
            outWriter.Write(1);
            outWriter.Write(-1);
            outWriter.Write(-1);

            calculatedPointers = ptrs;
            return outStream.ToArray();
        }
    }
}
