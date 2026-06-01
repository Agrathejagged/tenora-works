using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Bosses.Data
{
    /// <summary>
    /// This is group 2 in BossParamServer files, but group 1 in the dragon/gargoyle Form1 file. Also the dragon/gargoyle one is stored in a different order (BossParamServer is as follows, dragon/gargoyle has StatusEffectLevel moved to the end).
    /// </summary>
    public class BossAttackStat
    {
        public int AttackBitFlags { get; set; }
        public int StatusEffect { get; set; }
        public int StatusEffectLevel { get; set; }
        public float UnknownModifier1 { get; set; } 
        public float AttackModifier { get; set; }
        public float AccuracyModifier { get; set; }
    }
}
