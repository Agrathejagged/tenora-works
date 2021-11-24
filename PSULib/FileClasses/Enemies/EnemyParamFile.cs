using PSULib.FileClasses.General;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Enemies
{
    public class EnemyParamFile : PsuFile
    {
        public class EnemyBaseParams
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
            public int UnknownValue1 { get; set; } //skip 1 -- this is always 0 in both PSU and PSP2i. It's either read once on load, or never read at all.
            public int UnknownValue2 { get; set; } //skip 2 -- this is always 0 in both PSU and PSP2i. It's either read once on load, or never read at all.
            public int UnknownValue3 { get; set; } //skip 3 -- this is always 0 in both PSU and PSP2i. It's either read once on load, or never read at all.
            public int StatusResists { get; set; }
            public int DamageResists { get; set; }
            public float UnknownModifier3 { get; set; } // 11
            public float UnknownModifier4 { get; set; } // 12 -- I think this is weight.
            public int UnknownValue4 { get; set; } //skip 1
            public int UnknownValue5 { get; set; } //skip 2
            public float UnknownModifier5 { get; set; } //skip 3
            public float UnknownModifier6 { get; set; } // 13
            public float UnknownModifier7 { get; set; } // 14
            public int MonsterElement { get; set; }

            public override string ToString()
            {
                return HpModifier + "\t" + AtpModifier + "\t" + DfpModifier + "\t" + AtaModifier + "\t" + EvpModifier + "\t" + StaModifier + "\t" + LckModifier + "\t" + TpModifier + "\t" + MstModifier + "\t" + ElementModifier + "\t" + ExpModifier + "\t" + UnknownValue1 + "\t" + UnknownValue2 + "\t" + UnknownValue3 + "\t" + StatusResists.ToString("X") + "\t" + DamageResists.ToString("X") + "\t" + UnknownModifier3 + "\t" + UnknownModifier4 + "\t" + UnknownValue4 + "\t" + UnknownValue5 + "\t" + UnknownModifier5 + "\t" + UnknownModifier6 + "\t" + UnknownModifier7 + "\t" + MonsterElement;
            }
        }

        public class BuffParams
        {
            public short UnknownValue1 { get; set; }
            public short UnknownValue2 { get; set; }
            public short UnknownValue3 { get; set; }
            public short UnknownValue4 { get; set; }
            public int UnusedIntValue1 { get; set; }
            public float AtpModifier { get; set; }
            public float DfpModifier { get; set; }
            public float AtaModifier { get; set; }
            public float EvpModifier { get; set; }
            public float StaModifier { get; set; }
            public float LckModifier { get; set; }
            public float TpModifier { get; set; }
            public float MstModifier { get; set; }
            public float ExpModifier { get; set; }
            public int UnusedIntValue2 { get; set; }
            public float UnusedModifier1 { get; set; }
            public float UnusedModifier2 { get; set; }
            public float UnusedModifier3 { get; set; } //Are these three a group?
            public int StatusResists { get; set; }
            public int DamageResists { get; set; }
        }

        public class AttackParams
        {
            //Subgroup 1 stuff
            public string BoneName { get; set; } = "";
            public float OffsetX { get; set; }
            public float OffsetY { get; set; }
            public float OffsetZ { get; set; }
            public float BoundCylinderWidth { get; set; } //I think it's a cylinder.
            public float BoundCylinderHeight { get; set; } //I think it's a cylinder.

            //Subgroup 2 stuff
            public int OnHitEffect { get; set; }
            public int StatusEffect { get; set; } //this one appears to be bitflags
            public int UnknownSubgroup2Int3 { get; set; }
            public int UnknownSubgroup2Int4 { get; set; }
            public int UnknownSubgroup2Int5 { get; set; } //this one appears to be hitbox data

            //Subgroup 3 stuff
            public float HpModifier { get; set; } // This would probably be HP, but obvious reasons.
            public float AtpModifier { get; set; }
            public float DfpModifier { get; set; }
            public float AtaModifier { get; set; }
            public float EvpModifier { get; set; }
            public float StaModifier { get; set; }
            public float LckModifier { get; set; } // Weirdly, this one seems to be unused--it should be used?
            public float TpModifier { get; set; }
            public float MstModifier { get; set; }
            public float ElementModifier { get; set; } // Element again? It doesn't act like an elemental multiplier.
            public float ExpModifier { get; set; } // This would probably be experience, but obvious reasons it can't show up.
            public float UnusedModifier1 { get; set; }
            public float UnusedModifier2 { get; set; }
            public float UnusedModifier3 { get; set; }
        }

        public class HitboxParams
        {
            //Subgroup 1--1 short per entry
            public bool Targetable { get; set; }

            //Subgroup 2--this may match attack subgroup 1
            public string BoneName { get; set; }
            public float OffsetX { get; set; }
            public float OffsetY { get; set; }
            public float OffsetZ { get; set; }
            public float BoundCylinderWidth { get; set; }
            public float BoundCylinderHeight { get; set; }


            //Presumably stat mods again; subgroup 3.
            public float HpModifier { get; set; } // This would probably be HP, but obvious reasons.
            public float AtpModifier { get; set; }
            public float DfpModifier { get; set; }
            public float AtaModifier { get; set; }
            public float EvpModifier { get; set; }
            public float StaModifier { get; set; }
            public float LckModifier { get; set; } // Weirdly, this one seems to be unused--it should be used?
            public float TpModifier { get; set; }
            public float MstModifier { get; set; }
            public float ElementModifier { get; set; } // Element again? It doesn't act like an elemental multiplier.
            public float ExpModifier { get; set; } // This would probably be experience, but obvious reasons it can't show up.
            public float UnusedModifier1 { get; set; }
            public float UnusedModifier2 { get; set; }
            public float UnusedModifier3 { get; set; }
        }

        //This is the last thing in the file.
        //This may be a bone binding?
        //There are 4 of these.
        public class UnknownEntrySubEntry1
        {
            public int UnknownInt1 { get; set; } //Probably a pointer. Maybe a bone?
            public float OffsetX { get; set; } //Possibly a float.
            public float OffsetY { get; set; } //Possibly a float.
            public float OffsetZ { get; set; } //Possibly a float.
            public float Scale1 { get; set; }
            public float Scale2 { get; set; }
        }

        //There are two of these. Maybe front and back?
        public class UnknownEntrySubEntry2
        {
            public int UnknownInt1 { get; set; } //Definitely not a pointer. Gets read on monster spawn.
            public int UnknownInt2 { get; set; } //Read on despawn?
            public float UnknownFloat1 { get; set; }
            public int UnknownInt3 { get; set; }
            public int UnknownInt4 { get; set; }
            public int UnknownInt5 { get; set; }
            public int UnknownInt6 { get; set; }
            public int UnknownInt7 { get; set; }
            public int UnknownInt8 { get; set; }
        }

        public EnemyBaseParams baseParams;
        public List<BuffParams> buffParams = new List<BuffParams>(5); //This is always going to be length 5. Frequently crown-sword-everything else, but this is definitely per monster.
        public List<AttackParams> attackParams = new List<AttackParams>(); //Arbitrary length.
        public List<HitboxParams> hitboxParams = new List<HitboxParams>(); //Also arbitrary length.
        public List<UnknownEntrySubEntry1> unknownSubEntry1List = new List<UnknownEntrySubEntry1>();
        public List<UnknownEntrySubEntry2> unknownSubEntry2List = new List<UnknownEntrySubEntry2>();

        public EnemyParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);
            List<int> pointerList = new List<int>(ptrs);
            for (int i = 0; i < pointerList.Count; i++)
            {
                pointerList[i] = pointerList[i] - baseAddr;
            }

            int baseDataLocation = inReader.ReadInt32() - baseAddr;
            int buffDataLocation = inReader.ReadInt32() - baseAddr;
            int attackDataLocation = inReader.ReadInt32() - baseAddr;
            int hitboxDataLocation = inReader.ReadInt32() - baseAddr;
            int unknownDataLocation = inReader.ReadInt32() - baseAddr;

            inStream.Seek(baseDataLocation, SeekOrigin.Begin);
            baseParams = readBaseParams(inReader);
            inStream.Seek(buffDataLocation, SeekOrigin.Begin);
            readBuffParams(inReader, buffParams);
            inStream.Seek(attackDataLocation, SeekOrigin.Begin);
            readAttackParams(inStream, inReader, baseAddr, attackParams);
            inStream.Seek(hitboxDataLocation, SeekOrigin.Begin);
            readHitboxParams(inStream, inReader, baseAddr, hitboxParams);
            inStream.Seek(unknownDataLocation, SeekOrigin.Begin);
            readUnknownDataParams(inStream, inReader, baseAddr, unknownSubEntry1List, unknownSubEntry2List, pointerList);
            return;
        }

        private void readUnknownDataParams(Stream inStream, BinaryReader inReader, int baseAddr, List<UnknownEntrySubEntry1> unknownSubEntry1List, List<UnknownEntrySubEntry2> unknownSubEntry2List, List<int> pointers)
        {
            int subgroup1Loc = inReader.ReadInt32();
            int subgroup2Loc = inReader.ReadInt32();

            if (subgroup1Loc != 0)
            {
                inStream.Seek(subgroup1Loc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < 4; i++)
                {
                    UnknownEntrySubEntry1 tempEntry = new UnknownEntrySubEntry1();
                    tempEntry.UnknownInt1 = expectNonPointerInt(inReader, pointers);
                    tempEntry.OffsetX = inReader.ReadSingle();
                    tempEntry.OffsetY = inReader.ReadSingle();
                    tempEntry.OffsetZ = inReader.ReadSingle();
                    tempEntry.Scale1 = inReader.ReadSingle();
                    tempEntry.Scale2 = inReader.ReadSingle();
                    unknownSubEntry1List.Add(tempEntry);
                }
            }

            if (subgroup2Loc != 0)
            {
                inStream.Seek(subgroup2Loc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < 2; i++)
                {
                    UnknownEntrySubEntry2 tempEntry = new UnknownEntrySubEntry2();
                    tempEntry.UnknownInt1 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt2 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownFloat1 = inReader.ReadSingle();
                    tempEntry.UnknownInt3 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt4 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt5 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt6 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt7 = expectNonPointerInt(inReader, pointers);
                    tempEntry.UnknownInt8 = expectNonPointerInt(inReader, pointers);
                    unknownSubEntry2List.Add(tempEntry);
                }
            }
        }

        private void readHitboxParams(Stream inStream, BinaryReader inReader, int baseAddr, List<HitboxParams> paramsList)
        {
            //based on 360, counts in this file are shorts
            short hitboxParamCount = inReader.ReadInt16();
            short discardValue = inReader.ReadInt16();
            int subGroup1Loc = inReader.ReadInt32() - baseAddr;
            int subGroup2Loc = inReader.ReadInt32() - baseAddr;
            int subGroup3Loc = inReader.ReadInt32() - baseAddr;
            inStream.Seek(subGroup1Loc, SeekOrigin.Begin);
            for (int i = 0; i < hitboxParamCount; i++)
            {
                HitboxParams temp = new HitboxParams();
                temp.Targetable = inReader.ReadInt16() == 1;
                paramsList.Add(temp);

            }
            for (int i = 0; i < hitboxParamCount; i++)
            {
                HitboxParams temp = paramsList[i];
                inStream.Seek(subGroup2Loc + i * 24, SeekOrigin.Begin);
                int stringLoc = inReader.ReadInt32();
                temp.OffsetX = inReader.ReadSingle();
                temp.OffsetY = inReader.ReadSingle();
                temp.OffsetZ = inReader.ReadSingle();
                temp.BoundCylinderWidth = inReader.ReadSingle();
                temp.BoundCylinderHeight = inReader.ReadSingle();
                if (stringLoc != 0)
                {
                    temp.BoneName = inReader.ReadAsciiString(stringLoc - baseAddr);
                }

                inStream.Seek(subGroup3Loc + i * 56, SeekOrigin.Begin);
                temp.HpModifier = inReader.ReadSingle();
                temp.AtpModifier = inReader.ReadSingle();
                temp.DfpModifier = inReader.ReadSingle();
                temp.AtaModifier = inReader.ReadSingle();
                temp.EvpModifier = inReader.ReadSingle();
                temp.StaModifier = inReader.ReadSingle();
                temp.LckModifier = inReader.ReadSingle();
                temp.TpModifier = inReader.ReadSingle();
                temp.MstModifier = inReader.ReadSingle();
                temp.ElementModifier = inReader.ReadSingle();
                temp.ExpModifier = inReader.ReadSingle();
                temp.UnusedModifier1 = inReader.ReadSingle();
                temp.UnusedModifier2 = inReader.ReadSingle();
                temp.UnusedModifier3 = inReader.ReadSingle();
            }
        }

        private EnemyBaseParams readBaseParams(BinaryReader inReader)
        {
            EnemyBaseParams temp = new EnemyBaseParams();
            temp.HpModifier = inReader.ReadSingle();
            temp.AtpModifier = inReader.ReadSingle();
            temp.DfpModifier = inReader.ReadSingle();
            temp.AtaModifier = inReader.ReadSingle();
            temp.EvpModifier = inReader.ReadSingle();
            temp.StaModifier = inReader.ReadSingle();
            temp.LckModifier = inReader.ReadSingle();
            temp.TpModifier = inReader.ReadSingle();
            temp.MstModifier = inReader.ReadSingle();
            temp.ElementModifier = inReader.ReadSingle();
            temp.ExpModifier = inReader.ReadSingle();
            temp.UnknownValue1 = inReader.ReadInt32();
            temp.UnknownValue2 = inReader.ReadInt32();
            temp.UnknownValue3 = inReader.ReadInt32();
            temp.StatusResists = inReader.ReadInt32();
            temp.DamageResists = inReader.ReadInt32();
            temp.UnknownModifier3 = inReader.ReadSingle();
            temp.UnknownModifier4 = inReader.ReadSingle();
            temp.UnknownValue4 = inReader.ReadInt32();
            temp.UnknownValue5 = inReader.ReadInt32();
            temp.UnknownModifier5 = inReader.ReadSingle();
            temp.UnknownModifier6 = inReader.ReadSingle();
            temp.UnknownModifier7 = inReader.ReadSingle();
            temp.MonsterElement = inReader.ReadInt32();
            return temp;
        }

        private void readBuffParams(BinaryReader inReader, List<BuffParams> paramsList)
        {
            for (int i = 0; i < 5; i++)
            {
                BuffParams temp = new BuffParams();
                temp.UnknownValue1 = inReader.ReadInt16();
                temp.UnknownValue2 = inReader.ReadInt16();
                temp.UnknownValue3 = inReader.ReadInt16();
                temp.UnknownValue4 = inReader.ReadInt16();
                temp.UnusedIntValue1 = inReader.ReadInt32();
                temp.AtpModifier = inReader.ReadSingle();
                temp.DfpModifier = inReader.ReadSingle();
                temp.AtaModifier = inReader.ReadSingle();
                temp.EvpModifier = inReader.ReadSingle();
                temp.StaModifier = inReader.ReadSingle();
                temp.LckModifier = inReader.ReadSingle();
                temp.TpModifier = inReader.ReadSingle();
                temp.MstModifier = inReader.ReadSingle();
                temp.ExpModifier = inReader.ReadSingle();
                temp.UnusedIntValue2 = inReader.ReadInt32();
                temp.UnusedModifier1 = inReader.ReadSingle();
                temp.UnusedModifier2 = inReader.ReadSingle();
                temp.UnusedModifier3 = inReader.ReadSingle();
                temp.StatusResists = inReader.ReadInt32();
                temp.DamageResists = inReader.ReadInt32();
                paramsList.Add(temp);
            }
        }

        private void readAttackParams(Stream inStream, BinaryReader inReader, int baseAddr, List<AttackParams> paramsList)
        {
            //based on 360, counts in this file are shorts
            short attackParamCount = inReader.ReadInt16();
            short discardValue = inReader.ReadInt16();
            int subGroup1Loc = inReader.ReadInt32() - baseAddr;
            int subGroup2Loc = inReader.ReadInt32() - baseAddr;
            int subGroup3Loc = inReader.ReadInt32() - baseAddr;
            for (int i = 0; i < attackParamCount; i++)
            {
                inStream.Seek(subGroup1Loc + i * 24, SeekOrigin.Begin);
                AttackParams temp = new AttackParams();
                int stringLoc = inReader.ReadInt32();
                temp.OffsetX = inReader.ReadSingle();
                temp.OffsetY = inReader.ReadSingle();
                temp.OffsetZ = inReader.ReadSingle();
                temp.BoundCylinderWidth = inReader.ReadSingle();
                temp.BoundCylinderHeight = inReader.ReadSingle();
                if (stringLoc != 0)
                {
                    temp.BoneName = inReader.ReadAsciiString(stringLoc - baseAddr);
                }

                inStream.Seek(subGroup2Loc + i * 20, SeekOrigin.Begin);
                temp.OnHitEffect = inReader.ReadInt32();
                temp.StatusEffect = inReader.ReadInt32();
                temp.UnknownSubgroup2Int3 = inReader.ReadInt32();
                temp.UnknownSubgroup2Int4 = inReader.ReadInt32();
                temp.UnknownSubgroup2Int5 = inReader.ReadInt32();

                inStream.Seek(subGroup3Loc + i * 56, SeekOrigin.Begin);
                temp.HpModifier = inReader.ReadSingle();
                temp.AtpModifier = inReader.ReadSingle();
                temp.DfpModifier = inReader.ReadSingle();
                temp.AtaModifier = inReader.ReadSingle();
                temp.EvpModifier = inReader.ReadSingle();
                temp.StaModifier = inReader.ReadSingle();
                temp.LckModifier = inReader.ReadSingle();
                temp.TpModifier = inReader.ReadSingle();
                temp.MstModifier = inReader.ReadSingle();
                temp.ElementModifier = inReader.ReadSingle();
                temp.ExpModifier = inReader.ReadSingle();
                temp.UnusedModifier1 = inReader.ReadSingle();
                temp.UnusedModifier2 = inReader.ReadSingle();
                temp.UnusedModifier3 = inReader.ReadSingle();
                paramsList.Add(temp);
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            Dictionary<string, int> stringLocs = new Dictionary<string, int>();
            List<int> pointerLocs = new List<int>();

            foreach (var currentAttack in attackParams)
            {
                if (currentAttack.BoneName != null && currentAttack.BoneName != "" && !stringLocs.ContainsKey(currentAttack.BoneName))
                {
                    stringLocs[currentAttack.BoneName] = (int)outStream.Position;
                    outWriter.Write(Encoding.ASCII.GetBytes(currentAttack.BoneName));
                    outStream.Seek(4 - (int)(outStream.Position & 3), SeekOrigin.Current);
                }
            }
            foreach (var currentHitbox in hitboxParams)
            {
                if (currentHitbox.BoneName != null && currentHitbox.BoneName != "" && !stringLocs.ContainsKey(currentHitbox.BoneName))
                {
                    stringLocs[currentHitbox.BoneName] = (int)outStream.Position;
                    outWriter.Write(Encoding.ASCII.GetBytes(currentHitbox.BoneName));
                    outStream.Seek(4 - (int)(outStream.Position & 3), SeekOrigin.Current);
                }
            }

            //Write the base params
            int baseParamsLoc = (int)outStream.Position;
            outWriter.Write(baseParams.HpModifier);
            outWriter.Write(baseParams.AtpModifier);
            outWriter.Write(baseParams.DfpModifier);
            outWriter.Write(baseParams.AtaModifier);
            outWriter.Write(baseParams.EvpModifier);
            outWriter.Write(baseParams.StaModifier);
            outWriter.Write(baseParams.LckModifier);
            outWriter.Write(baseParams.TpModifier);
            outWriter.Write(baseParams.MstModifier);
            outWriter.Write(baseParams.ElementModifier);
            outWriter.Write(baseParams.ExpModifier);
            outWriter.Write(baseParams.UnknownValue1);
            outWriter.Write(baseParams.UnknownValue2);
            outWriter.Write(baseParams.UnknownValue3);
            outWriter.Write(baseParams.StatusResists);
            outWriter.Write(baseParams.DamageResists);
            outWriter.Write(baseParams.UnknownModifier3);
            outWriter.Write(baseParams.UnknownModifier4);
            outWriter.Write(baseParams.UnknownValue4);
            outWriter.Write(baseParams.UnknownValue5);
            outWriter.Write(baseParams.UnknownModifier5);
            outWriter.Write(baseParams.UnknownModifier6);
            outWriter.Write(baseParams.UnknownModifier7);
            outWriter.Write(baseParams.MonsterElement);

            //Write the buff params
            int buffLoc = (int)outStream.Position;
            foreach (var buff in buffParams)
            {
                outWriter.Write(buff.UnknownValue1);
                outWriter.Write(buff.UnknownValue2);
                outWriter.Write(buff.UnknownValue3);
                outWriter.Write(buff.UnknownValue4);
                outWriter.Write(buff.UnusedIntValue1);
                outWriter.Write(buff.AtpModifier);
                outWriter.Write(buff.DfpModifier);
                outWriter.Write(buff.AtaModifier);
                outWriter.Write(buff.EvpModifier);
                outWriter.Write(buff.StaModifier);
                outWriter.Write(buff.LckModifier);
                outWriter.Write(buff.TpModifier);
                outWriter.Write(buff.MstModifier);
                outWriter.Write(buff.ExpModifier);
                outWriter.Write(buff.UnusedIntValue2);
                outWriter.Write(buff.UnusedModifier1);
                outWriter.Write(buff.UnusedModifier2);
                outWriter.Write(buff.UnusedModifier3);
                outWriter.Write(buff.StatusResists);
                outWriter.Write(buff.DamageResists);
            }

            //Write the attack params
            //These are split into three groups, but all attack data from a given group is stored together.
            //In other words: 3 loops.
            int attackSubgroup1Loc = (int)outStream.Position;
            foreach (var attack in attackParams)
            {
                if (attack.BoneName != null && stringLocs.ContainsKey(attack.BoneName))
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(stringLocs[attack.BoneName]);
                }
                else
                {
                    outWriter.Write(0);
                }
                outWriter.Write(attack.OffsetX);
                outWriter.Write(attack.OffsetY);
                outWriter.Write(attack.OffsetZ);
                outWriter.Write(attack.BoundCylinderWidth);
                outWriter.Write(attack.BoundCylinderHeight);
            }
            int attackSubgroup2Loc = (int)outStream.Position;
            foreach (var attack in attackParams)
            {
                outWriter.Write(attack.OnHitEffect);
                outWriter.Write(attack.StatusEffect);
                outWriter.Write(attack.UnknownSubgroup2Int3);
                outWriter.Write(attack.UnknownSubgroup2Int4);
                outWriter.Write(attack.UnknownSubgroup2Int5);
            }
            int attackSubgroup3Loc = (int)outStream.Position;
            foreach (var attack in attackParams)
            {
                outWriter.Write(attack.HpModifier);
                outWriter.Write(attack.AtpModifier);
                outWriter.Write(attack.DfpModifier);
                outWriter.Write(attack.AtaModifier);
                outWriter.Write(attack.EvpModifier);
                outWriter.Write(attack.StaModifier);
                outWriter.Write(attack.LckModifier);
                outWriter.Write(attack.TpModifier);
                outWriter.Write(attack.MstModifier);
                outWriter.Write(attack.ElementModifier);
                outWriter.Write(attack.ExpModifier);
                outWriter.Write(attack.UnusedModifier1);
                outWriter.Write(attack.UnusedModifier2);
                outWriter.Write(attack.UnusedModifier3);
            }
            //Write the attack pointers
            int attackRootLoc = (int)outStream.Position;
            outWriter.Write((short)attackParams.Count);
            outWriter.Write((short)0);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackSubgroup1Loc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackSubgroup2Loc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackSubgroup3Loc);

            //Write the hitbox params
            //Same caveat as attacks.
            int hitboxSubgroup1Loc = (int)outStream.Position;
            foreach (var hitbox in hitboxParams)
            {
                if (hitbox.Targetable)
                {
                    outWriter.Write((short)1);
                }
                else
                {
                    outWriter.Write((short)0);
                }
            }
            if ((outStream.Position & 0x3) != 0)
            {
                outStream.Seek(4 - (int)(outStream.Position & 3), SeekOrigin.Current);
            }
            int hitboxSubgroup2Loc = (int)outStream.Position;
            foreach (var hitbox in hitboxParams)
            {
                if (hitbox.BoneName != null && stringLocs.ContainsKey(hitbox.BoneName))
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(stringLocs[hitbox.BoneName]);
                }
                else
                {
                    outWriter.Write(0);
                }
                outWriter.Write(hitbox.OffsetX);
                outWriter.Write(hitbox.OffsetY);
                outWriter.Write(hitbox.OffsetZ);
                outWriter.Write(hitbox.BoundCylinderWidth);
                outWriter.Write(hitbox.BoundCylinderHeight);
            }
            int hitboxSubgroup3Loc = (int)outStream.Position;
            foreach (var hitbox in hitboxParams)
            {
                outWriter.Write(hitbox.HpModifier);
                outWriter.Write(hitbox.AtpModifier);
                outWriter.Write(hitbox.DfpModifier);
                outWriter.Write(hitbox.AtaModifier);
                outWriter.Write(hitbox.EvpModifier);
                outWriter.Write(hitbox.StaModifier);
                outWriter.Write(hitbox.LckModifier);
                outWriter.Write(hitbox.TpModifier);
                outWriter.Write(hitbox.MstModifier);
                outWriter.Write(hitbox.ElementModifier);
                outWriter.Write(hitbox.ExpModifier);
                outWriter.Write(hitbox.UnusedModifier1);
                outWriter.Write(hitbox.UnusedModifier2);
                outWriter.Write(hitbox.UnusedModifier3);
            }
            //Write the hitbox pointers
            int hitboxRootLoc = (int)outStream.Position;
            outWriter.Write((short)hitboxParams.Count);
            outWriter.Write((short)0);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(hitboxSubgroup1Loc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(hitboxSubgroup2Loc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(hitboxSubgroup3Loc);
            //Write the mystery params

            int subEntry1Loc = (int)outStream.Position;
            foreach (var entry in unknownSubEntry1List)
            {
                outWriter.Write(entry.UnknownInt1);
                outWriter.Write(entry.OffsetX);
                outWriter.Write(entry.OffsetY);
                outWriter.Write(entry.OffsetZ);
                outWriter.Write(entry.Scale1);
                outWriter.Write(entry.Scale2);
            }

            int subEntry2Loc = (int)outStream.Position;
            foreach (var entry in unknownSubEntry2List)
            {
                outWriter.Write(entry.UnknownInt1);
                outWriter.Write(entry.UnknownInt2);
                outWriter.Write(entry.UnknownFloat1);
                outWriter.Write(entry.UnknownInt3);
                outWriter.Write(entry.UnknownInt4);
                outWriter.Write(entry.UnknownInt5);
                outWriter.Write(entry.UnknownInt6);
                outWriter.Write(entry.UnknownInt7);
                outWriter.Write(entry.UnknownInt8);
            }

            //Write the mystery pointers
            int subEntryRootLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(subEntry1Loc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(subEntry2Loc);

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(baseParamsLoc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(buffLoc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(attackRootLoc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(hitboxRootLoc);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(subEntryRootLoc);


            int fileLength = (int)outStream.Position;
            outStream.Seek(15 - outStream.Position % 16, SeekOrigin.Current);
            if (outStream.Position > fileLength)
            {
                outWriter.Write((byte)0);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(0x52584E);
            outWriter.Write(fileLength);
            outWriter.Write(topLevelListLoc);
            calculatedPointers = pointerLocs.ToArray();

            header = buildSubheader(fileLength);
            return outStream.ToArray();
        }
    }
}
