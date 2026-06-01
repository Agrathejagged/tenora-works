using GimSharp;
using PSULib.FileClasses.Bosses.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter outWriter, Color3f color)
        {
            outWriter.Write(color.R);
            outWriter.Write(color.G);
            outWriter.Write(color.B);
        }

        public static void Write(this BinaryWriter outWriter, Color4f color)
        {
            outWriter.Write(color.R);
            outWriter.Write(color.G);
            outWriter.Write(color.B);
            outWriter.Write(color.A);
        }

        public static void Write(this BinaryWriter outWriter, BossAttackStat attackStat)
        {
            outWriter.Write(attackStat.AttackBitFlags);
            outWriter.Write(attackStat.StatusEffect);
            outWriter.Write(attackStat.StatusEffectLevel);
            outWriter.Write(attackStat.UnknownModifier1);
            outWriter.Write(attackStat.AttackModifier);
            outWriter.Write(attackStat.AccuracyModifier);
        }

        public static void Write(this BinaryWriter outWriter, BossHitboxStat hitboxStat) {
            outWriter.Write(hitboxStat.HitboxFlags);
            outWriter.Write(hitboxStat.DefenseModifier);
            outWriter.Write(hitboxStat.EvadeModifier);
            outWriter.Write(hitboxStat.MysteryModifier);
        }

        /// <summary>
        /// Trims the writer to the nearest padFactor bytes. Used to ensure that files end at the nearest 0x10 bytes, or that 32-bit values are at the nearest 4 byte offset.
        /// </summary>
        /// <param name="outWriter">writer whose base stream should be trimmed</param>
        /// <param name="trimFactor">trim to the nearest X bytes</param>
        public static void Trim(this BinaryWriter outWriter, int trimFactor)
        {
            var stream = outWriter.BaseStream;
            if (stream.Position % trimFactor != 0)
            {
                stream.Seek(trimFactor - (stream.Position % trimFactor), SeekOrigin.Current);
                if(stream.Length < stream.Position)
                {
                    stream.SetLength(stream.Position);
                }
            }
        }
    }
}
