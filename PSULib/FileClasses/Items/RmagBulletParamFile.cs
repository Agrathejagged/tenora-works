using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Items
{
    public class RmagBulletParamFile : PsuFile
    {
        public class RmagBullet
        {
            public bool valid { get; set; }
            public byte fireTime { get; set; }
            public byte unkByte2 { get; set; }
            public ushort unkShort1 { get; set; }
            public ushort unkShort2 { get; set; }
            public ushort unkShort3 { get; set; }
            public byte unkByte1 { get; set; }
            public byte hitEffect { get; set; }
            public byte status { get; set; }
            public byte seLevel { get; set; }
            public byte shots { get; set; }
            public byte bulletType { get; set; }
            public ushort bulletSpeed { get; set; }
            public ushort bulletSize { get; set; }
            public ushort bulletRange { get; set; }
            public int unkInt1 { get; set; } //Experience?
            public ushort unkShort6 { get; set; } //Exp growth?
            public ushort tpMod { get; set; }
            public ushort ataMod { get; set; }
            public ushort unkShort7 { get; set; }
            public ushort unkShort8 { get; set; }
            public ushort unkShort9 { get; set; }
        }

        public class RmagStatus
        {
            public byte neutral { get; set; }
            public byte fire { get; set; }
            public byte ice { get; set; }
            public byte thunder { get; set; }
            public byte earth { get; set; }
            public byte light { get; set; }
            public byte dark { get; set; }
        }

        bool isPsp1 = false;

        public List<RmagBullet>[] allBullets = new List<RmagBullet>[4];
        public ushort[][] hitBoxes;
        public List<RmagStatus> statusEffects;
        public RmagBulletParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int hitboxLoc = inReader.ReadInt32() - baseAddr;
            int statusLoc = inReader.ReadInt32() - baseAddr;
            int[] mfrPointers = new int[4];
            mfrPointers[0] = inReader.ReadInt32() - baseAddr;
            mfrPointers[1] = inReader.ReadInt32() - baseAddr;
            mfrPointers[2] = inReader.ReadInt32() - baseAddr;
            mfrPointers[3] = inReader.ReadInt32() - baseAddr;
            int hitboxCount = (statusLoc - hitboxLoc) / 4;
            hitBoxes = new ushort[hitboxCount][];
            int lowestPtr = headerLoc;
            inStream.Seek(hitboxLoc, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i] = new ushort[2];
                hitBoxes[i][0] = inReader.ReadUInt16();
                hitBoxes[i][1] = inReader.ReadUInt16();
            }
            //Can't read SEs until I know where the earliest point is.
            for (int j = 0; j < 4; j++)
            {
                if (mfrPointers[j] > 0)
                {
                    inStream.Seek(mfrPointers[j], SeekOrigin.Begin);
                    int bulletLoc = inReader.ReadInt32() - baseAddr;
                    int indexLoc = inReader.ReadInt32() - baseAddr;
                    lowestPtr = Math.Min(lowestPtr, bulletLoc);
                    if (indexLoc > 0)
                        lowestPtr = Math.Min(lowestPtr, indexLoc);
                    short indexCount = inReader.ReadInt16();
                    short bulletCount = inReader.ReadInt16();
                    if (indexCount > 0)
                        inStream.Seek(indexLoc, SeekOrigin.Begin);
                    else
                        isPsp1 = true;
                    byte[] bulletIndexes = inReader.ReadBytes(indexCount);
                    allBullets[j] = new List<RmagBullet>(indexCount);
                    for (int i = 0; i < Math.Max(indexCount, bulletCount); i++)
                    {
                        RmagBullet tempBullet = new RmagBullet();
                        if (bulletIndexes.Length > 0 && bulletIndexes[i] == 0xFF)
                        {
                            tempBullet.valid = false;
                        }
                        else
                        {
                            tempBullet.valid = true;
                            int index = i;
                            if (indexCount > 0)
                                index = bulletIndexes[i];
                            inStream.Seek(bulletLoc + index * 0x24, SeekOrigin.Begin);
                            tempBullet.fireTime = inReader.ReadByte();
                            tempBullet.unkByte2 = inReader.ReadByte();
                            tempBullet.unkShort1 = inReader.ReadUInt16();
                            tempBullet.unkShort2 = inReader.ReadUInt16();
                            tempBullet.unkShort3 = inReader.ReadUInt16();
                            tempBullet.unkByte1 = inReader.ReadByte();
                            tempBullet.hitEffect = inReader.ReadByte();
                            tempBullet.status = inReader.ReadByte();
                            tempBullet.seLevel = inReader.ReadByte();
                            tempBullet.shots = inReader.ReadByte();
                            tempBullet.bulletType = inReader.ReadByte();
                            tempBullet.bulletSpeed = inReader.ReadUInt16();
                            tempBullet.bulletSize = inReader.ReadUInt16();
                            tempBullet.bulletRange = inReader.ReadUInt16();
                            tempBullet.unkInt1 = inReader.ReadInt32();
                            tempBullet.unkShort6 = inReader.ReadUInt16();
                            tempBullet.tpMod = inReader.ReadUInt16();
                            tempBullet.ataMod = inReader.ReadUInt16();
                            tempBullet.unkShort7 = inReader.ReadUInt16();
                            tempBullet.unkShort8 = inReader.ReadUInt16();
                            tempBullet.unkShort9 = inReader.ReadUInt16();
                        }
                        allBullets[j].Insert(i, tempBullet);
                    }
                }
                else
                    allBullets[j] = new List<RmagBullet>(0);
            }
            //NOW can get status effects
            inStream.Seek(statusLoc, SeekOrigin.Begin);
            int statusCount = (lowestPtr - statusLoc) / 7;
            statusEffects = new List<RmagStatus>(statusCount);
            for (int i = 0; i < statusCount; i++)
            {
                RmagStatus temp = new RmagStatus();
                temp.neutral = inReader.ReadByte();
                temp.fire = inReader.ReadByte();
                temp.ice = inReader.ReadByte();
                temp.thunder = inReader.ReadByte();
                temp.earth = inReader.ReadByte();
                temp.light = inReader.ReadByte();
                temp.dark = inReader.ReadByte();
                statusEffects.Insert(i, temp);
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            List<int> ptrs = new List<int>();
            outStream.Seek(0x1C, SeekOrigin.Begin);
            for (int i = 0; i < hitBoxes.Length; i++)
            {
                for (int j = 0; j < hitBoxes[i].Length; j++)
                    outWriter.Write(hitBoxes[i][j]);
            }
            int seLoc = (int)outStream.Position;
            for (int i = 0; i < statusEffects.Count; i++)
            {
                outWriter.Write(statusEffects[i].neutral);
                outWriter.Write(statusEffects[i].fire);
                outWriter.Write(statusEffects[i].ice);
                outWriter.Write(statusEffects[i].thunder);
                outWriter.Write(statusEffects[i].earth);
                outWriter.Write(statusEffects[i].light);
                outWriter.Write(statusEffects[i].dark);
            }
            outStream.Seek(outStream.Position + 3 & 0xFFFFFFFC, SeekOrigin.Begin);
            int[] mfrLocs = new int[4];
            for (int i = 0; i < 4; i++)
            {
                byte validNum = 0;
                int bulletLoc = (int)outStream.Position;
                byte[] bulletIndexes = new byte[allBullets[i].Count];
                for (int j = 0; j < allBullets[i].Count; j++)
                {
                    if (allBullets[i][j].valid)
                    {
                        bulletIndexes[j] = validNum++;
                        outWriter.Write(allBullets[i][j].fireTime);
                        outWriter.Write(allBullets[i][j].unkByte2);
                        outWriter.Write(allBullets[i][j].unkShort1);
                        outWriter.Write(allBullets[i][j].unkShort2);
                        outWriter.Write(allBullets[i][j].unkShort3);
                        outWriter.Write(allBullets[i][j].unkByte1);
                        outWriter.Write(allBullets[i][j].hitEffect);
                        outWriter.Write(allBullets[i][j].status);
                        outWriter.Write(allBullets[i][j].seLevel);
                        outWriter.Write(allBullets[i][j].shots);
                        outWriter.Write(allBullets[i][j].bulletType);
                        outWriter.Write(allBullets[i][j].bulletSpeed);
                        outWriter.Write(allBullets[i][j].bulletSize);
                        outWriter.Write(allBullets[i][j].bulletRange);
                        outWriter.Write(allBullets[i][j].unkInt1);
                        outWriter.Write(allBullets[i][j].unkShort6);
                        outWriter.Write(allBullets[i][j].tpMod);
                        outWriter.Write(allBullets[i][j].ataMod);
                        outWriter.Write(allBullets[i][j].unkShort7);
                        outWriter.Write(allBullets[i][j].unkShort8);
                        outWriter.Write(allBullets[i][j].unkShort9);
                    }
                    else
                        bulletIndexes[j] = 0xFF;
                }
                int indexLoc = 0;
                if (!isPsp1)
                    indexLoc = (int)outStream.Position;
                if (validNum > 0)
                {
                    if (!isPsp1)
                        outWriter.Write(bulletIndexes);
                    outStream.Seek(outStream.Position + 3 & 0xFFFFFFFC, SeekOrigin.Begin);
                    mfrLocs[i] = (int)outStream.Position;
                    ptrs.Add((int)outStream.Position);
                    outWriter.Write(bulletLoc);
                    if (!isPsp1)
                    {
                        ptrs.Add((int)outStream.Position);
                        outWriter.Write(indexLoc);
                    }
                    else
                        outWriter.Write(0);
                    outWriter.Write((short)bulletIndexes.Length);
                    if (!isPsp1)
                        outWriter.Write((short)validNum);
                    else
                        outWriter.Write((short)0);
                }
            }
            int headerLoc = (int)outStream.Position;
            ptrs.Add((int)outStream.Position);  //Hitboxes
            outWriter.Write(0x1C);
            ptrs.Add((int)outStream.Position);  //Status effects
            outWriter.Write(seLoc);
            for (int i = 0; i < 4; i++)
            {
                if (mfrLocs[i] != 0)
                    ptrs.Add((int)outStream.Position); //Manufacturer bullets
                outWriter.Write(mfrLocs[i]);
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

            calculatedPointers = ptrs.ToArray();
            return outStream.ToArray();
        }
    }
}
