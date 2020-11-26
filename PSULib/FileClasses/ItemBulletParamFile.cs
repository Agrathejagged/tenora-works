using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class ItemBulletParamFile : PsuFile
    {
        public class BulletTier
        {
            public short unkShort1 { get; set; }
            public byte unkByte1 { get; set; }
            public byte flags { get; set; }
            public byte ppCost { get; set; }
            public byte unkByte2 { get; set; }
            public byte element { get; set; }
            public byte elePercent { get; set; }
            public byte dmgType { get; set; }
            public byte hitEffect { get; set; }
            public byte status { get; set; }
            public byte statusLevel { get; set; }
            public short fireDelay { get; set; }
            public short bulletVelocity { get; set; }
            public short bulletWidth { get; set; }
            public short bulletRange { get; set; }
            public int expPerLevel { get; set; }
            public short expGrowth { get; set; }
            public short atpModifier { get; set; }
            public short ataModifier { get; set; }
            public byte atpGrowth { get; set; }
            public byte ataGrowth { get; set; }
            public byte bulletCount { get; set; }
            public byte unkByte3 { get; set; }
            public short unkShort2 { get; set; }
        }
        public BulletTier[][] allBullets;
        public ushort[][] hitBoxes;

        public ItemBulletParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int tiers = inReader.ReadInt32();
            short bulletCount = inReader.ReadInt16();
            short hitboxCount = inReader.ReadInt16();
            int hitboxLoc = inReader.ReadInt32() - baseAddr;
            int skillLoc = inReader.ReadInt32() - baseAddr;
            allBullets = new BulletTier[9][];
            hitBoxes = new ushort[hitboxCount][];
            inStream.Seek(hitboxLoc, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i] = new ushort[2];
                hitBoxes[i][0] = inReader.ReadUInt16();
                hitBoxes[i][1] = inReader.ReadUInt16();
            }
            inStream.Seek(skillLoc, SeekOrigin.Begin);
            for (int i = 0; i < 9; i++)
            {
                if(i > 0)
                    allBullets[i] = new BulletTier[tiers];
                else
                    allBullets[i] = new BulletTier[1];
                for (int j = 0; j < allBullets[i].Length; j++)
                {
                    BulletTier tempTier = new BulletTier();
                    tempTier.unkShort1 = inReader.ReadInt16();
                    tempTier.unkByte1 = inReader.ReadByte();
                    tempTier.flags = inReader.ReadByte();
                    tempTier.ppCost = inReader.ReadByte();
                    tempTier.unkByte2 = inReader.ReadByte();
                    tempTier.element = inReader.ReadByte();
                    tempTier.elePercent = inReader.ReadByte();
                    tempTier.dmgType = inReader.ReadByte();
                    tempTier.hitEffect = inReader.ReadByte();
                    tempTier.status = inReader.ReadByte();
                    tempTier.statusLevel = inReader.ReadByte();
                    tempTier.fireDelay = inReader.ReadInt16();
                    tempTier.bulletVelocity = inReader.ReadInt16();
                    tempTier.bulletWidth = inReader.ReadInt16();
                    tempTier.bulletRange = inReader.ReadInt16();
                    tempTier.expPerLevel = inReader.ReadInt32();
                    tempTier.expGrowth = inReader.ReadInt16();
                    tempTier.atpModifier = inReader.ReadInt16();
                    tempTier.ataModifier = inReader.ReadInt16();
                    tempTier.atpGrowth = inReader.ReadByte();
                    tempTier.ataGrowth = inReader.ReadByte();
                    tempTier.bulletCount = inReader.ReadByte();
                    tempTier.unkByte3 = inReader.ReadByte();
                    tempTier.unkShort2 = inReader.ReadInt16();
                    allBullets[i][j] = tempTier;
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
            for (int i = 0; i < allBullets.Length; i++)
            {
                for (int j = 0; j < allBullets[i].Length; j++)
                {
                    outWriter.Write(allBullets[i][j].unkShort1);
                    outWriter.Write(allBullets[i][j].unkByte1);
                    outWriter.Write(allBullets[i][j].flags);
                    outWriter.Write(allBullets[i][j].ppCost);
                    outWriter.Write(allBullets[i][j].unkByte2);
                    outWriter.Write(allBullets[i][j].element);
                    outWriter.Write(allBullets[i][j].elePercent);
                    outWriter.Write(allBullets[i][j].dmgType);
                    outWriter.Write(allBullets[i][j].hitEffect);
                    outWriter.Write(allBullets[i][j].status);
                    outWriter.Write(allBullets[i][j].statusLevel);
                    outWriter.Write(allBullets[i][j].fireDelay);
                    outWriter.Write(allBullets[i][j].bulletVelocity);
                    outWriter.Write(allBullets[i][j].bulletWidth);
                    outWriter.Write(allBullets[i][j].bulletRange);
                    outWriter.Write(allBullets[i][j].expPerLevel);
                    outWriter.Write(allBullets[i][j].expGrowth);
                    outWriter.Write(allBullets[i][j].atpModifier);
                    outWriter.Write(allBullets[i][j].ataModifier);
                    outWriter.Write(allBullets[i][j].atpGrowth);
                    outWriter.Write(allBullets[i][j].ataGrowth);
                    outWriter.Write(allBullets[i][j].bulletCount);
                    outWriter.Write(allBullets[i][j].unkByte3);
                    outWriter.Write(allBullets[i][j].unkShort2);
                }
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write(allBullets[1].Length);
            outWriter.Write((short)allBullets.Length);
            outWriter.Write((short)hitBoxes.Length);
            ptrs[0] = (int)outStream.Position;
            outWriter.Write((int)0x1C);
            ptrs[1] = (int)outStream.Position;
            outWriter.Write(tierLoc);
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

            calculatedPointers = ptrs;
            return outStream.ToArray();
        }
    }
}
