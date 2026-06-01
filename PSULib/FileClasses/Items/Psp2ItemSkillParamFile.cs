using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using PSULib.Support;

namespace PSULib.FileClasses.Items
{
    public class Psp2ItemSkillParamFile : PsuFile
    {
        public class SkillTier
        {
            public short Hit1AtpMod { get; set; }
            public short Hit2AtpMod { get; set; }
            public short Hit3AtpMod { get; set; }
            public byte AtaMod { get; set; }
            public byte AtaGrowth { get; set; }
            public byte UnknownByte1 { get; set; }
            public byte UnknownByte2 { get; set; }
            public byte UnknownByte3 { get; set; }
            public byte UnknownByte4 { get; set; }
            public byte Hit1PP { get; set; }
            public byte Hit2PP { get; set; }
            public byte Hit3PP { get; set; }
            public sbyte Hit1TargetMod { get; set; }
            public sbyte Hit2TargetMod { get; set; }
            public sbyte Hit3TargetMod { get; set; }
            public byte AttackCount { get; set; }
            
            public byte Hit1HitBox { get; set; }
            public byte Hit2HitBox { get; set; }
            public byte Hit3HitBox { get; set; }

            public short Hit1HitBoxValue1 { get; set; }
            public short Hit1HitBoxValue2 { get; set; }
            public short Hit2HitBoxValue1 { get; set; }
            public short Hit2HitBoxValue2 { get; set; }
            public short Hit3HitBoxValue1 { get; set; }
            public short Hit3HitBoxValue2 { get; set; }
        }

        public sbyte[] skillIndexes;
        public SkillTier[][] allSkills;
        public short[][] hitBoxes;

        public Psp2ItemSkillParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int hitboxLoc = inReader.ReadInt32() - baseAddr;
            int skillLoc = inReader.ReadInt32() - baseAddr;
            //assuming the hitboxes precede the tiers--this seems safe.
            int hitboxCount = (skillLoc - hitboxLoc) / 4;
            hitBoxes = new short[hitboxCount][];
            int highestIndex = 0;
            byte[] tempIndexes = inReader.ReadBytes(7);
            skillIndexes = new sbyte[7];
            int tiers = 3;
            for (int i = 0; i < tempIndexes.Length; i++)
            {
                if (tempIndexes[i] == 0xFF)
                    skillIndexes[i] = -1;
                else
                    skillIndexes[i] = (sbyte)(tempIndexes[i] / 3);
            }
            highestIndex = skillIndexes.Max() + 1;
            inStream.Seek(hitboxLoc, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i] = new short[2];
                hitBoxes[i][0] = inReader.ReadInt16();
                hitBoxes[i][1] = inReader.ReadInt16();
            }
            allSkills = new SkillTier[highestIndex][];
            inStream.Seek(skillLoc, SeekOrigin.Begin);
            for (int i = 0; i < highestIndex; i++)
            {
                allSkills[i] = new SkillTier[tiers];
                for (int j = 0; j < tiers; j++)
                {
                    SkillTier tempTier = new SkillTier();
                    tempTier.Hit1AtpMod = inReader.ReadInt16();
                    tempTier.Hit2AtpMod = inReader.ReadInt16();
                    tempTier.Hit3AtpMod = inReader.ReadInt16();
                    tempTier.AtaMod = inReader.ReadByte();
                    tempTier.AtaGrowth = inReader.ReadByte();
                    tempTier.UnknownByte1 = inReader.ReadByte();
                    tempTier.UnknownByte2 = inReader.ReadByte();
                    tempTier.UnknownByte3 = inReader.ReadByte();
                    tempTier.UnknownByte4 = inReader.ReadByte();
                    tempTier.Hit1PP = inReader.ReadByte();
                    tempTier.Hit2PP = inReader.ReadByte();
                    tempTier.Hit3PP = inReader.ReadByte();
                    tempTier.Hit1TargetMod = inReader.ReadSByte();
                    tempTier.Hit2TargetMod = inReader.ReadSByte();
                    tempTier.Hit3TargetMod = inReader.ReadSByte();
                    tempTier.AttackCount = inReader.ReadByte();

                    tempTier.Hit1HitBox = inReader.ReadByte();
                    tempTier.Hit2HitBox = inReader.ReadByte();
                    tempTier.Hit3HitBox = inReader.ReadByte();
                    if(tempTier.Hit1HitBox < hitBoxes.Length)
                    {
                        tempTier.Hit1HitBoxValue1 = hitBoxes[tempTier.Hit1HitBox][0];
                        tempTier.Hit1HitBoxValue2 = hitBoxes[tempTier.Hit1HitBox][1];
                    }
                    if (tempTier.Hit2HitBox < hitBoxes.Length)
                    {
                        tempTier.Hit2HitBoxValue1 = hitBoxes[tempTier.Hit2HitBox][0];
                        tempTier.Hit2HitBoxValue2 = hitBoxes[tempTier.Hit2HitBox][1];
                    }
                    if (tempTier.Hit3HitBox < hitBoxes.Length)
                    {
                        tempTier.Hit3HitBoxValue1 = hitBoxes[tempTier.Hit3HitBox][0];
                        tempTier.Hit3HitBoxValue2 = hitBoxes[tempTier.Hit3HitBox][1];
                    }
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
                    outWriter.Write(allSkills[i][j].Hit1AtpMod);
                    outWriter.Write(allSkills[i][j].Hit2AtpMod);
                    outWriter.Write(allSkills[i][j].Hit3AtpMod);
                    outWriter.Write(allSkills[i][j].AtaMod);
                    outWriter.Write(allSkills[i][j].AtaGrowth);
                    outWriter.Write(allSkills[i][j].UnknownByte1);
                    outWriter.Write(allSkills[i][j].UnknownByte2);
                    outWriter.Write(allSkills[i][j].UnknownByte3);
                    outWriter.Write(allSkills[i][j].UnknownByte4);
                    outWriter.Write(allSkills[i][j].Hit1PP);
                    outWriter.Write(allSkills[i][j].Hit2PP);
                    outWriter.Write(allSkills[i][j].Hit3PP);
                    outWriter.Write(allSkills[i][j].Hit1TargetMod);
                    outWriter.Write(allSkills[i][j].Hit2TargetMod);
                    outWriter.Write(allSkills[i][j].Hit3TargetMod);
                    outWriter.Write(allSkills[i][j].AttackCount);
                    outWriter.Write(allSkills[i][j].Hit1HitBox);
                    outWriter.Write(allSkills[i][j].Hit2HitBox);
                    outWriter.Write(allSkills[i][j].Hit3HitBox);
                }
            }
            int headerLoc = (int)outStream.Position;
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
            outWriter.Trim(8);
            int fileLength = (int)outStream.Position;
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(0x0052554E);
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
