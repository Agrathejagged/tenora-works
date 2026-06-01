using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Bosses.Data
{
    public class BossHitboxStat
    {
        public int HitboxFlags { get; set; }
        public bool IsResistMelee { get
            {
                return (HitboxFlags & 0x1) != 0;
            }
            set
            {
                setHitboxFlag(0x1, value);
            }
        }
        public bool IsAbsorbMelee
        {
            get
            {
                return (HitboxFlags & 0x2) != 0;
            }
            set
            {
                setHitboxFlag(0x2, value);
            }
        }
        public bool IsResistRanged
        {
            get
            {
                return (HitboxFlags & 0x4) != 0;
            }
            set
            {
                setHitboxFlag(0x4, value);
            }
        }
        public bool IsAbsorbRanged
        {
            get
            {
                return (HitboxFlags & 0x8) != 0;
            }
            set
            {
                setHitboxFlag(0x8, value);
            }
        }
        public bool IsResistTech
        {
            get
            {
                return (HitboxFlags & 0x10) != 0;
            }
            set
            {
                setHitboxFlag(0x10, value);
            }
        }
        public bool IsAbsorbTech
        {
            get
            {
                return (HitboxFlags & 0x20) != 0;
            }
            set
            {
                setHitboxFlag(0x20, value);
            }
        }
        private void setHitboxFlag(int flag, bool value)
        {
            if (value)
            {
                HitboxFlags |= flag;
            }
            else
            {
                HitboxFlags &= ~flag;
            }
        }
        public float DefenseModifier { get; set; }
        public float EvadeModifier { get; set; }
        //It's uncommon that this isn't 1 or 0; other values exist on De Rol Le, Dark Falz 1, Adahna Degahna
        public float MysteryModifier { get; set; }
    }
}
