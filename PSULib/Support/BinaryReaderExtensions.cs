using PSULib.FileClasses.Bosses.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    static class BinaryReaderExtensions
    {
        public static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            return BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);
        }

        public static string ReadAsciiString(this BinaryReader inReader, int location)
        {
            inReader.BaseStream.Seek(location, SeekOrigin.Begin);
            StringBuilder sb = new StringBuilder();
            char c = (char)inReader.ReadByte();
            while (c != '\0')
            {
                sb.Append(c);
                c = (char)inReader.ReadByte();
            }
            return sb.ToString();
        }

        public static Color3f ReadColor3f(this BinaryReader inReader)
        {
            float r = inReader.ReadSingle();
            float g = inReader.ReadSingle();
            float b = inReader.ReadSingle();
            return new Color3f(r, g, b);
        }

        public static Color4f ReadColor4f(this BinaryReader inReader)
        {
            float r = inReader.ReadSingle();
            float g = inReader.ReadSingle();
            float b = inReader.ReadSingle();
            float a = inReader.ReadSingle();
            return new Color4f(r, g, b, a);
        }

        public static BossAttackStat ReadBossAttack(this BinaryReader inReader)
        {
            BossAttackStat modifier = new BossAttackStat();
            modifier.AttackBitFlags = inReader.ReadInt32();
            modifier.StatusEffect = inReader.ReadInt32();
            modifier.StatusEffectLevel = inReader.ReadInt32();
            modifier.UnknownModifier1 = inReader.ReadSingle();
            modifier.AttackModifier = inReader.ReadSingle();
            modifier.AccuracyModifier = inReader.ReadSingle();
            return modifier;
        }

        public static BossHitboxStat ReadBossHitbox(this BinaryReader inReader)
        {
            BossHitboxStat modifier = new BossHitboxStat();
            modifier.HitboxFlags = inReader.ReadInt32();
            modifier.DefenseModifier = inReader.ReadSingle();
            modifier.EvadeModifier = inReader.ReadSingle();
            modifier.MysteryModifier = inReader.ReadSingle();
            return modifier;
        }
    }
}
