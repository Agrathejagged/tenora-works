using PSULib.FileClasses.Bosses.Data;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Maps.Common;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PSULib.FileClasses.Enemies.AtkDatFile;
using static PSULib.FileClasses.Enemies.EnemyParamFile;

namespace PSULib.FileClasses.Bosses
{
    /// <summary>
    /// File used for the sorts of data the server would manage (stats, hitbox damage modifiers, attack modifiers). This data is rolled into the Form1 files for dragons and Onma/Dimma.
    /// </summary>
    public class ParamServerFile : PsuFile
    {
        public List<BossAttackStat> AttackModifiers { get; set; } = new List<BossAttackStat>();
        public List<BossHitboxStat> HitboxModifiers { get; set; } = new List<BossHitboxStat>();
        //11 floats...
        public float[] MysteryFloats = new float[11];
        public BossModifiers StatModifiers { get; set; }

        public class BossModifiers
        {
            public float HpModifier { get; set; } // 0
            public float AtpModifier { get; set; }
            public float DfpModifier { get; set; }
            public float AtaModifier { get; set; }
            public float EvpModifier { get; set; }
            public float StaModifier { get; set; }
            public float LckModifier { get; set; }
            public float TpModifier { get; set; }
            public float MstModifier { get; set; }
            public float ElementModifier { get; set; }
            public float ExpModifier { get; set; } // 10
        }

        public ushort UnknownShort1 { get; set; } // generally 0xCFFF
        public ushort UnknownShort2 { get; set; } // generally 3
        public int Element { get; set; } //I think.

        public ParamServerFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            List<int> pointerList = new List<int>(ptrs.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                pointerList.Add(ptrs[i] - baseAddr);
            }
            calculatedPointers = pointerList.ToArray();
            byte[] headers = inReader.ReadBytes(4);
            int fileLength = inReader.ReadInt32();
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);

            for(int i = 0; i < 11; i++)
            {
                MysteryFloats[i] = inReader.ReadSingle();
            }
            StatModifiers = readBaseParams(inReader);
            UnknownShort1 = inReader.ReadUInt16();
            UnknownShort2 = inReader.ReadUInt16();
            Element = inReader.ReadInt32();
            int hitboxLoc = inReader.ReadInt32() - baseAddr;
            int hitboxCount = inReader.ReadInt32();
            int attackLoc = inReader.ReadInt32() - baseAddr;
            int attackCount = inReader.ReadInt32();

            inStream.Seek(hitboxLoc, SeekOrigin.Begin);
            for (int i = 0; i < hitboxCount; i++)
            {
                HitboxModifiers.Add(inReader.ReadBossHitbox());
            }

            inStream.Seek(attackLoc, SeekOrigin.Begin);
            for (int i = 0; i < attackCount; i++)
            {
                AttackModifiers.Add(inReader.ReadBossAttack());
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();
            int hitboxLoc = (int)outStream.Position;
            for(int i = 0; i < HitboxModifiers.Count; i++)
            {
                outWriter.Write(HitboxModifiers[i]);
            }

            int attackLoc = (int)outStream.Position;
            for(int i = 0; i <  AttackModifiers.Count; i++)
            {
                outWriter.Write(AttackModifiers[i]);
            }

            int tableLoc = (int)outStream.Position;
            for(int i = 0; i < MysteryFloats.Length; i++)
            {
                outWriter.Write(MysteryFloats[i]);
            }
            writeBaseParams(StatModifiers, outWriter);
            outWriter.Write(UnknownShort1);
            outWriter.Write(UnknownShort2);
            outWriter.Write(Element);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(hitboxLoc);
            outWriter.Write(HitboxModifiers.Count);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackLoc);
            outWriter.Write(AttackModifiers.Count);
            outWriter.Trim(0x10);
            outStream.SetLength(outStream.Position);

            int fileLength = (int)outStream.Position;


            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write((int)0x52584E);
            outWriter.Write(fileLength);
            outWriter.Write(tableLoc);
            calculatedPointers = pointerLocs.ToArray();

            header = buildSubheader(fileLength);
            return outStream.ToArray();
        }

        private BossModifiers readBaseParams(BinaryReader inReader)
        {
            BossModifiers modifiers = new BossModifiers();
            modifiers.HpModifier = inReader.ReadSingle();
            modifiers.AtpModifier = inReader.ReadSingle();
            modifiers.DfpModifier = inReader.ReadSingle();
            modifiers.AtaModifier = inReader.ReadSingle();
            modifiers.EvpModifier = inReader.ReadSingle();
            modifiers.StaModifier = inReader.ReadSingle();
            modifiers.LckModifier = inReader.ReadSingle();
            modifiers.TpModifier = inReader.ReadSingle();
            modifiers.MstModifier = inReader.ReadSingle();
            modifiers.ElementModifier = inReader.ReadSingle();
            modifiers.ExpModifier = inReader.ReadSingle();
            return modifiers;
        }

        private void writeBaseParams(BossModifiers modifiers, BinaryWriter writer)
        {
            writer.Write(modifiers.HpModifier);
            writer.Write(modifiers.AtpModifier);
            writer.Write(modifiers.DfpModifier);
            writer.Write(modifiers.AtaModifier);
            writer.Write(modifiers.EvpModifier);
            writer.Write(modifiers.StaModifier);
            writer.Write(modifiers.LckModifier);
            writer.Write(modifiers.TpModifier);
            writer.Write(modifiers.MstModifier);
            writer.Write(modifiers.ElementModifier);
            writer.Write(modifiers.ExpModifier);
        }
    }
}
