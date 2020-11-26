using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class ItemTechParamFile : PsuFile
    {
        public class TechTier
        {
            public byte targets { get; set; }
            public byte channelTime { get; set; }
            public ushort range { get; set; }
            public ushort velocity { get; set; }
            public ushort hitboxSize { get; set; }
            public ushort unkShort1 { get; set; }
            public ushort turningSpeed { get; set; }
            public ushort tpMod { get; set; }
            public ushort tpGrowth { get; set; }
            public int expPerLevel { get; set; }
            public ushort expGrowth { get; set; }
            public ushort ppCost { get; set; }
            public byte statusEffect { get; set; }
            public byte statusLevel { get; set; }
            public byte hitEffect { get; set; }
            public byte unkByte1 { get; set; }
            public byte unkByte2 { get; set; }
            public byte castTime { get; set; }
            public byte chargeTime { get; set; }
            public byte unkByte3 { get; set; }
        }

        public byte[] techIndexes = new byte[0];
        public TechTier[][] allTechs;
        public byte[][] techInfo; //Don't know enough about this to really split it.
        bool isPsp1 = false;
        //4 bytes flags?
        //4 bytes unknown
        //4 bytes cast sound
        //4 bytes hold sound
        //4 bytes release sound

        public ItemTechParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int tiers = inReader.ReadInt32();
            int types = inReader.ReadInt32();
            int highestIndex = types;
            if (!ptrs.Contains((int)inStream.Position + 8 + baseAddr))
                isPsp1 = true;
            if (!isPsp1)
            {
                if (ptrs.Contains((int)inStream.Position + baseAddr))
                {
                    int indexLoc = inReader.ReadInt32() - baseAddr;
                    inStream.Seek(indexLoc, SeekOrigin.Begin);
                    techIndexes = inReader.ReadBytes(types);
                    inStream.Seek(headerLoc + 12, SeekOrigin.Begin);
                    highestIndex = techIndexes[techIndexes.Length - 1] + 1;
                }
                else inReader.ReadInt32();
            }
            allTechs = new TechTier[highestIndex][];
            techInfo = new byte[highestIndex][];
            int miscLoc = inReader.ReadInt32() - baseAddr;
            int tiersLoc = inReader.ReadInt32() - baseAddr;
            for (int i = 0; i < highestIndex; i++)
            {
                inStream.Seek(miscLoc + 20 * i, SeekOrigin.Begin);
                techInfo[i] = inReader.ReadBytes(20);
            }
            for (int i = 0; i < highestIndex; i++)
            {
                allTechs[i] = new TechTier[tiers];
                for (int j = 0; j < tiers; j++)
                {
                    inStream.Seek(tiersLoc + i * tiers * 32 + j * 32, SeekOrigin.Begin);
                    TechTier tempTier = new TechTier();
                    tempTier.targets = inReader.ReadByte();
                    tempTier.channelTime = inReader.ReadByte();
                    tempTier.range = inReader.ReadUInt16();
                    tempTier.velocity = inReader.ReadUInt16();
                    tempTier.hitboxSize = inReader.ReadUInt16();
                    tempTier.unkShort1 = inReader.ReadUInt16();
                    tempTier.turningSpeed = inReader.ReadUInt16();
                    tempTier.tpMod = inReader.ReadUInt16();
                    tempTier.tpGrowth = inReader.ReadUInt16();
                    tempTier.expPerLevel = inReader.ReadInt32();
                    tempTier.expGrowth = inReader.ReadUInt16();
                    tempTier.ppCost = inReader.ReadUInt16();
                    tempTier.statusEffect = inReader.ReadByte();
                    tempTier.statusLevel = inReader.ReadByte();
                    tempTier.hitEffect = inReader.ReadByte();
                    tempTier.unkByte1 = inReader.ReadByte();
                    tempTier.unkByte2 = inReader.ReadByte();
                    tempTier.castTime = inReader.ReadByte();
                    tempTier.chargeTime = inReader.ReadByte();
                    tempTier.unkByte3 = inReader.ReadByte();
                    allTechs[i][j] = tempTier;
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            int ptrShift = techIndexes.Length > 0 ? 1 : 0;
            int[] ptrs = new int[2 + ptrShift];
            outStream.Seek(0x1C, SeekOrigin.Begin);
            for (int i = 0; i < techInfo.Length; i++)
            {
                outWriter.Write(techInfo[i]);
            }
            int tierLoc = (int)outStream.Position;
            for (int i = 0; i < allTechs.Length; i++)
            {
                for (int j = 0; j < allTechs[i].Length; j++)
                {
                    outWriter.Write(allTechs[i][j].targets);
                    outWriter.Write(allTechs[i][j].channelTime);
                    outWriter.Write(allTechs[i][j].range);
                    outWriter.Write(allTechs[i][j].velocity);
                    outWriter.Write(allTechs[i][j].hitboxSize);
                    outWriter.Write(allTechs[i][j].unkShort1);
                    outWriter.Write(allTechs[i][j].turningSpeed);
                    outWriter.Write(allTechs[i][j].tpMod);
                    outWriter.Write(allTechs[i][j].tpGrowth);
                    outWriter.Write(allTechs[i][j].expPerLevel);
                    outWriter.Write(allTechs[i][j].expGrowth);
                    outWriter.Write(allTechs[i][j].ppCost);
                    outWriter.Write(allTechs[i][j].statusEffect);
                    outWriter.Write(allTechs[i][j].statusLevel);
                    outWriter.Write(allTechs[i][j].hitEffect);
                    outWriter.Write(allTechs[i][j].unkByte1);
                    outWriter.Write(allTechs[i][j].unkByte2);
                    outWriter.Write(allTechs[i][j].castTime);
                    outWriter.Write(allTechs[i][j].chargeTime);
                    outWriter.Write(allTechs[i][j].unkByte3);
                }
            }
            int indexLoc = 0;
            if (techIndexes.Length > 0)
            {
                indexLoc = (int)outStream.Position;
                outWriter.Write(techIndexes);
                outStream.Seek((outStream.Position + 3) & 0xFFFFFFFC, SeekOrigin.Begin);
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write(allTechs[0].Length);
            outWriter.Write(Math.Max(techIndexes.Length, techInfo.Length));
            if (!isPsp1)
            {
                if (techIndexes.Length > 0)
                {
                    ptrs[0] = (int)outStream.Position;
                }
                outWriter.Write(indexLoc);
            }
            ptrs[ptrShift] = (int)outStream.Position;
            outWriter.Write((int)0x1C);
            ptrs[ptrShift + 1] = (int)outStream.Position;
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
