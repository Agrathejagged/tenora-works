using PSULib.FileClasses.General;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Maps
{
    public class ObjectParamFile : PsuFile
    {
        //Either 360's not endian aware on THIS PIECE ONLY, or it's a length 8 byte array internally... whatever it is.
        public class ObjectGroup1Entry
        {
            public byte byte1 { get; set; }
            public byte byte2 { get; set; }
            public byte byte3 { get; set; }
            public byte byte4 { get; set; }
            public byte byte5 { get; set; }
            public byte byte6 { get; set; }
            public byte byte7 { get; set; }
            public byte byte8 { get; set; }
        }

        //9 floats? ints? some combination of both?
        public class ObjectHitbox
        {
            public int hitboxShape { get; set; } //this is definitely an int, values 0, 1, 2, 3.
            public float unknownFloat2 { get; set; }
            public float unknownFloat3 { get; set; }
            public float unknownFloat4 { get; set; }
            public int unknownInt5 { get; set; } //this is definitely an int
            public float unknownFloat6 { get; set; }
            public int unusedValue7 { get; set; } //value's 0 across PSU
            public int unusedValue8 { get; set; } //value's 0 across PSU
            public int unknownInt9 { get; set; } //this is definitely an int, values 0, 4, 8.

            public override bool Equals(object obj)
            {
                return obj is ObjectHitbox hitbox &&
                       hitboxShape == hitbox.hitboxShape &&
                       unknownFloat2 == hitbox.unknownFloat2 &&
                       unknownFloat3 == hitbox.unknownFloat3 &&
                       unknownFloat4 == hitbox.unknownFloat4 &&
                       unknownInt5 == hitbox.unknownInt5 &&
                       unknownFloat6 == hitbox.unknownFloat6 &&
                       unusedValue7 == hitbox.unusedValue7 &&
                       unusedValue8 == hitbox.unusedValue8 &&
                       unknownInt9 == hitbox.unknownInt9;
            }

            public override int GetHashCode()
            {
                int hashCode = 1863892702;
                hashCode = hashCode * -1521134295 + hitboxShape.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownFloat2.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownFloat3.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownFloat4.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt5.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownFloat6.GetHashCode();
                hashCode = hashCode * -1521134295 + unusedValue7.GetHashCode();
                hashCode = hashCode * -1521134295 + unusedValue8.GetHashCode();
                hashCode = hashCode * -1521134295 + unknownInt9.GetHashCode();
                return hashCode;
            }
        }

        //int, pointer to string, float float float, int, pointer to string, float, float float, int?, int?
        public class AnimationReference
        {
            public int unknownIdentifier1 { get; set; }
            public string texAnimName { get; set; }
            public float unknownFloat1 { get; set; }
            public float unknownFloat2 { get; set; }
            public float unknownFloat3 { get; set; }
            public int unknownIdentifier2 { get; set; }
            public string boneAnimName { get; set; }
            public float unknownFloat4 { get; set; }
            public float unknownFloat5 { get; set; }
            public float unknownFloat6 { get; set; }
            public int unknownInt1 { get; set; }
            public int unknownInt2 { get; set; }
        }

        //This one appears to be particle/sound event bindings.
        public class ObjectGroup4Entry
        {
            public List<ParticleBinding> particleBindings { get; set; } = new List<ParticleBinding>();
            public List<SoundBinding> soundBindings { get; set; } = new List<SoundBinding>();
            //This may be padding. It's the last 16 bits of the bind list counts (which are each a byte)
            public ushort mysteryData { get; set; }
        }

        public class ParticleBinding
        {
            public string particleName { get; set; }
            public string eventName { get; set; }
            //Considering sounds, I expect this to be a float. I haven't seen this one, though.
            public int emptyInt3 { get; set; }
            public int emptyInt4 { get; set; }
            public int emptyInt5 { get; set; }
            public int usedInt6 { get; set; }
        }

        public class SoundBinding
        {
            public int soundId { get; set; }
            public string eventName { get; set; }
            public float unknownFloat2 { get; set; }
            public int emptyInt3 { get; set; }
            public int emptyInt4 { get; set; }
            public int emptyInt5 { get; set; }
        }

        public class ModelReference
        {
            public int id { get; set; }
            public string fileName { get; set; }
            public float unknownFloat { get; set; }
        }

        public class ObjectEntry
        {
            public List<ObjectGroup1Entry> group1Entries { get; } = new List<ObjectGroup1Entry>();
            public ObjectHitbox group2Entry { get; set; }
            public List<AnimationReference> group3Entries { get; } = new List<AnimationReference>();
            public ObjectGroup4Entry group4Entry { get; set; }
            public List<ModelReference> group5Entries { get; } = new List<ModelReference>(); // always 8? why is there even a count?
        }

        public Dictionary<int, ObjectEntry> ObjectDefinitions = new Dictionary<int, ObjectEntry>();

        public ObjectParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            List<int> pointerList = new List<int>(ptrs.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                pointerList.Add(ptrs[i] - baseAddr);
            }
            byte[] headers = inReader.ReadBytes(4);
            int fileLength = inReader.ReadInt32();
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);

            //Okay, topmost header's just count+pointer
            int objectCount = inReader.ReadInt32();
            int tableOfContentsLocation = inReader.ReadInt32() - baseAddr;

            inStream.Seek(tableOfContentsLocation, SeekOrigin.Begin);
            int[] objectNums = new int[objectCount];
            int[] objectPointers = new int[objectCount];

            //ID + pointer per object
            for (int i = 0; i < objectCount; i++)
            {
                objectNums[i] = inReader.ReadInt32();
                objectPointers[i] = inReader.ReadInt32() - baseAddr;
            }

            //5 pointers per object
            for (int i = 0; i < objectCount; i++)
            {
                inStream.Seek(objectPointers[i], SeekOrigin.Begin);
                int[] pointersForObject = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    pointersForObject[j] = inReader.ReadInt32();
                    if (pointersForObject[j] != 0)
                    {
                        pointersForObject[j] -= baseAddr;
                    }
                }

                ObjectEntry entry = new ObjectEntry();

                //read each group now...
                if (pointersForObject[0] != 0)
                {
                    inStream.Seek(pointersForObject[0], SeekOrigin.Begin);
                    int group1Count = inReader.ReadInt32();
                    int group1Loc = inReader.ReadInt32() - baseAddr;

                    inStream.Seek(group1Loc, SeekOrigin.Begin);
                    for (int j = 0; j < group1Count; j++)
                    {
                        ObjectGroup1Entry group1Entry = new ObjectGroup1Entry();
                        group1Entry.byte1 = inReader.ReadByte();
                        group1Entry.byte2 = inReader.ReadByte();
                        group1Entry.byte3 = inReader.ReadByte();
                        group1Entry.byte4 = inReader.ReadByte();
                        group1Entry.byte5 = inReader.ReadByte();
                        group1Entry.byte6 = inReader.ReadByte();
                        group1Entry.byte7 = inReader.ReadByte();
                        group1Entry.byte8 = inReader.ReadByte();
                        entry.group1Entries.Add(group1Entry);
                    }
                }

                if (pointersForObject[1] != 0)
                {
                    inStream.Seek(pointersForObject[1], SeekOrigin.Begin);
                    ObjectHitbox group2Entry = new ObjectHitbox();
                    group2Entry.hitboxShape = inReader.ReadInt32();
                    group2Entry.unknownFloat2 = inReader.ReadSingle();
                    group2Entry.unknownFloat3 = inReader.ReadSingle();
                    group2Entry.unknownFloat4 = inReader.ReadSingle();
                    group2Entry.unknownInt5 = inReader.ReadInt32();
                    group2Entry.unknownFloat6 = inReader.ReadSingle();
                    group2Entry.unusedValue7 = inReader.ReadInt32();
                    group2Entry.unusedValue8 = inReader.ReadInt32();
                    group2Entry.unknownInt9 = inReader.ReadInt32();
                    entry.group2Entry = group2Entry;
                }

                if (pointersForObject[2] != 0)
                {
                    inStream.Seek(pointersForObject[2], SeekOrigin.Begin);
                    int group3Count = inReader.ReadInt32();
                    int group3Loc = inReader.ReadInt32() - baseAddr;

                    for (int j = 0; j < group3Count; j++)
                    {
                        inStream.Seek(group3Loc + j * 0x30, SeekOrigin.Begin);
                        AnimationReference group3Entry = new AnimationReference();
                        group3Entry.unknownIdentifier1 = inReader.ReadInt32();
                        int string1Address = inReader.ReadInt32();
                        if (string1Address != 0)
                        {
                            string1Address -= baseAddr;
                        }
                        group3Entry.unknownFloat1 = inReader.ReadSingle();
                        group3Entry.unknownFloat2 = inReader.ReadSingle();
                        group3Entry.unknownFloat3 = inReader.ReadSingle();
                        group3Entry.unknownIdentifier2 = inReader.ReadInt32();
                        int string2Address = inReader.ReadInt32();
                        if (string2Address != 0)
                        {
                            string2Address -= baseAddr;
                        }
                        group3Entry.unknownFloat4 = inReader.ReadSingle();
                        group3Entry.unknownFloat5 = inReader.ReadSingle();
                        group3Entry.unknownFloat6 = inReader.ReadSingle();
                        group3Entry.unknownInt1 = inReader.ReadInt32();
                        group3Entry.unknownInt2 = inReader.ReadInt32();
                        if (string1Address != 0)
                        {
                            group3Entry.texAnimName = inReader.ReadAsciiString(string1Address);
                        }
                        if (string2Address != 0)
                        {
                            group3Entry.boneAnimName = inReader.ReadAsciiString(string2Address);
                        }
                        entry.group3Entries.Add(group3Entry);
                    }
                }

                if (pointersForObject[3] != 0)
                {
                    //This is gonna have _lots_ of "if thing isn't as expected", because I know almost nothing about this data type.
                    inStream.Seek(pointersForObject[3], SeekOrigin.Begin);
                    ObjectGroup4Entry group4Entry = new ObjectGroup4Entry();
                    int subentry1Pointer = inReader.ReadInt32();
                    int subentry2Pointer = inReader.ReadInt32();
                    byte subentry1Count = inReader.ReadByte();
                    byte subentry2Count = inReader.ReadByte();
                    group4Entry.mysteryData = inReader.ReadUInt16();

                    if (subentry1Count > 0 && subentry1Pointer != 0)
                    {
                        for (int j = 0; j < subentry1Count; j++)
                        {
                            inStream.Seek(subentry1Pointer + j * 24 - baseAddr, SeekOrigin.Begin);
                            ParticleBinding subentry1 = new ParticleBinding();
                            int string1Ptr = inReader.ReadInt32();
                            int string2Ptr = inReader.ReadInt32();
                            subentry1.emptyInt3 = expectNonPointerInt(inReader, pointerList);
                            subentry1.emptyInt4 = expectNonPointerInt(inReader, pointerList);
                            subentry1.emptyInt5 = expectNonPointerInt(inReader, pointerList);
                            subentry1.usedInt6 = expectNonPointerInt(inReader, pointerList);
                            if (string1Ptr != 0)
                            {
                                subentry1.particleName = inReader.ReadAsciiString(string1Ptr - baseAddr);
                            }
                            if (string2Ptr != 0)
                            {
                                subentry1.eventName = inReader.ReadAsciiString(string2Ptr - baseAddr);
                            }
                            group4Entry.particleBindings.Add(subentry1);
                        }
                    }
                    if (subentry2Count > 0 && subentry2Pointer != 0)
                    {
                        for (int j = 0; j < subentry2Count; j++)
                        {
                            inStream.Seek(subentry2Pointer + j * 24 - baseAddr, SeekOrigin.Begin);
                            SoundBinding subentry2 = new SoundBinding();
                            subentry2.soundId = inReader.ReadInt32();
                            int stringPtr = inReader.ReadInt32();
                            subentry2.unknownFloat2 = inReader.ReadSingle();
                            subentry2.emptyInt3 = expectNonPointerInt(inReader, pointerList);
                            subentry2.emptyInt4 = expectNonPointerInt(inReader, pointerList);
                            subentry2.emptyInt5 = expectNonPointerInt(inReader, pointerList);
                            if (stringPtr != 0)
                            {
                                subentry2.eventName = inReader.ReadAsciiString(stringPtr - baseAddr);
                            }
                            group4Entry.soundBindings.Add(subentry2);
                        }
                    }
                    entry.group4Entry = group4Entry;
                }

                if (pointersForObject[4] != 0)
                {
                    inStream.Seek(pointersForObject[4], SeekOrigin.Begin);
                    int group5Count = inReader.ReadInt32();
                    int group5Loc = inReader.ReadInt32() - baseAddr;

                    for (int j = 0; j < group5Count; j++)
                    {
                        inStream.Seek(group5Loc + j * 8, SeekOrigin.Begin);
                        ModelReference group5Entry = new ModelReference();
                        group5Entry.id = inReader.ReadInt32();
                        int entryLoc = inReader.ReadInt32();
                        if (entryLoc != 0)
                        {
                            entryLoc -= baseAddr;
                        }
                        if (entryLoc != 0)
                        {
                            inStream.Seek(entryLoc, SeekOrigin.Begin);
                            int stringLoc = inReader.ReadInt32();
                            if (stringLoc != 0)
                            {
                                stringLoc -= baseAddr;
                            }
                            group5Entry.unknownFloat = inReader.ReadSingle();
                            group5Entry.fileName = inReader.ReadAsciiString(stringLoc);
                            entry.group5Entries.Add(group5Entry);
                        }
                    }
                }

                ObjectDefinitions[objectNums[i]] = entry;
            }
        }

        public override byte[] ToRaw()
        {
            //Order probably doesn't matter too much here, but it's conventionally 2-1-5-3-4.
            List<string> stringList = new List<string>();
            foreach (var currentObject in ObjectDefinitions.Values)
            {
                foreach (var group5 in currentObject.group5Entries)
                {
                    addIfNotNull(stringList, group5.fileName);
                }
                foreach (var group3 in currentObject.group3Entries)
                {
                    addIfNotNull(stringList, group3.boneAnimName);
                    addIfNotNull(stringList, group3.texAnimName);
                }
                if (currentObject.group4Entry != null)
                {
                    foreach (var particleBind in currentObject.group4Entry.particleBindings)
                    {
                        addIfNotNull(stringList, particleBind.particleName);
                        addIfNotNull(stringList, particleBind.eventName);
                    }
                    foreach (var soundBind in currentObject.group4Entry.soundBindings)
                    {
                        addIfNotNull(stringList, soundBind.eventName);
                    }
                }
            }

            List<int> objectIds = ObjectDefinitions.Keys.ToList();
            objectIds.Sort();



            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            Dictionary<string, int> stringLocs = new Dictionary<string, int>();
            List<int> pointerLocs = new List<int>();

            outStream.Seek(0x10, SeekOrigin.Begin);

            foreach (string currString in stringList)
            {
                stringLocs[currString] = (int)outStream.Position;
                outWriter.Write(Encoding.GetEncoding("shift-jis").GetBytes(currString));
                outStream.Seek(4 - outStream.Position % 4, SeekOrigin.Current);
            }

            List<int> objectLocs = new List<int>();

            //Objects are each written individually, so I don't need to keep track of pointers in between objects (only the root pointer per object, which is naturally at the end).
            //Per object, it goes 2-1-5-3-4, then root
            //At least 4 is sometimes absent.
            for (int i = 0; i < objectIds.Count; i++)
            {
                var currentObject = ObjectDefinitions[objectIds[i]];
                int group2Loc = (int)outStream.Position;
                var group2Entry = currentObject.group2Entry;
                outWriter.Write(group2Entry.hitboxShape);
                outWriter.Write(group2Entry.unknownFloat2);
                outWriter.Write(group2Entry.unknownFloat3);
                outWriter.Write(group2Entry.unknownFloat4);
                outWriter.Write(group2Entry.unknownInt5);
                outWriter.Write(group2Entry.unknownFloat6);
                outWriter.Write(group2Entry.unusedValue7);
                outWriter.Write(group2Entry.unusedValue8);
                outWriter.Write(group2Entry.unknownInt9);

                int group1LeafLoc = (int)outStream.Position;
                foreach (var group1Entry in currentObject.group1Entries)
                {
                    outWriter.Write(group1Entry.byte1);
                    outWriter.Write(group1Entry.byte2);
                    outWriter.Write(group1Entry.byte3);
                    outWriter.Write(group1Entry.byte4);
                    outWriter.Write(group1Entry.byte5);
                    outWriter.Write(group1Entry.byte6);
                    outWriter.Write(group1Entry.byte7);
                    outWriter.Write(group1Entry.byte8);
                }
                int group1BranchLoc = (int)outStream.Position;
                if (currentObject.group1Entries.Count > 0)
                {
                    outWriter.Write(currentObject.group1Entries.Count);
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group1LeafLoc);
                }

                //Group 5 is always 8 entries.
                List<int> group5LeafLocs = new List<int>();
                foreach (var group5Entry in currentObject.group5Entries)
                {
                    group5LeafLocs.Add((int)outStream.Position);
                    writeIfNotNull(outWriter, group5Entry.fileName, stringLocs, pointerLocs);
                    outWriter.Write(group5Entry.unknownFloat);
                }

                int group5BranchLoc = (int)outStream.Position;
                for (int j = 0; j < 8; j++)
                {
                    if (j < currentObject.group5Entries.Count)
                    {
                        outWriter.Write(currentObject.group5Entries[j].id);
                        pointerLocs.Add((int)outStream.Position);
                        outWriter.Write(group5LeafLocs[j]);
                    }
                    else
                    {
                        outWriter.Write(-1);
                        outWriter.Write(0);
                    }
                }

                int group5RootLoc = (int)outStream.Position;
                outWriter.Write(Math.Max(currentObject.group5Entries.Count, 8));
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(group5BranchLoc);

                int group3LeafLoc = (int)outStream.Position;
                foreach (var group3Entry in currentObject.group3Entries)
                {
                    outWriter.Write(group3Entry.unknownIdentifier1);
                    writeIfNotNull(outWriter, group3Entry.texAnimName, stringLocs, pointerLocs);
                    outWriter.Write(group3Entry.unknownFloat1);
                    outWriter.Write(group3Entry.unknownFloat2);
                    outWriter.Write(group3Entry.unknownFloat3);
                    outWriter.Write(group3Entry.unknownIdentifier2);
                    writeIfNotNull(outWriter, group3Entry.boneAnimName, stringLocs, pointerLocs);
                    outWriter.Write(group3Entry.unknownFloat4);
                    outWriter.Write(group3Entry.unknownFloat5);
                    outWriter.Write(group3Entry.unknownFloat6);
                    outWriter.Write(group3Entry.unknownInt1);
                    outWriter.Write(group3Entry.unknownInt2);
                }

                int group3BranchLoc = (int)outStream.Position;
                outWriter.Write(currentObject.group3Entries.Count);
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(group3LeafLoc);

                bool group4Exists = currentObject.group4Entry != null && (currentObject.group4Entry.particleBindings.Count > 0 || currentObject.group4Entry.soundBindings.Count > 0);
                int group4BranchLoc;
                if (group4Exists)
                {
                    int group4Leaf1Loc = (int)outStream.Position;
                    foreach (var group4SubEntry1 in currentObject.group4Entry.particleBindings)
                    {
                        writeIfNotNull(outWriter, group4SubEntry1.particleName, stringLocs, pointerLocs);
                        writeIfNotNull(outWriter, group4SubEntry1.eventName, stringLocs, pointerLocs);
                        outWriter.Write(group4SubEntry1.emptyInt3);
                        outWriter.Write(group4SubEntry1.emptyInt4);
                        outWriter.Write(group4SubEntry1.emptyInt5);
                        outWriter.Write(group4SubEntry1.usedInt6);
                    }
                    int group4Leaf2Loc = (int)outStream.Position;
                    foreach (var group4SubEntry2 in currentObject.group4Entry.soundBindings)
                    {
                        outWriter.Write(group4SubEntry2.soundId);
                        writeIfNotNull(outWriter, group4SubEntry2.eventName, stringLocs, pointerLocs);
                        outWriter.Write(group4SubEntry2.unknownFloat2);
                        outWriter.Write(group4SubEntry2.emptyInt3);
                        outWriter.Write(group4SubEntry2.emptyInt4);
                        outWriter.Write(group4SubEntry2.emptyInt5);
                    }
                    group4BranchLoc = (int)outStream.Position;
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group4Leaf1Loc);
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group4Leaf2Loc);
                    outWriter.Write((byte)currentObject.group4Entry.particleBindings.Count);
                    outWriter.Write((byte)currentObject.group4Entry.soundBindings.Count);
                    outWriter.Write(currentObject.group4Entry.mysteryData);
                }
                else
                {
                    group4BranchLoc = 0;
                }
                objectLocs.Add((int)outStream.Position);
                if (currentObject.group1Entries.Count > 0)
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group1BranchLoc);
                }
                else
                {
                    outWriter.Write(0);
                }
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(group2Loc);
                if (currentObject.group3Entries.Count > 0)
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group3BranchLoc);
                }
                else
                {
                    outWriter.Write(0);
                }
                if (group4Exists)
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group4BranchLoc);
                }
                else
                {
                    outWriter.Write(0);
                }
                if (currentObject.group5Entries.Count > 0)
                {
                    pointerLocs.Add((int)outStream.Position);
                    outWriter.Write(group5RootLoc);
                }
                else
                {
                    outWriter.Write(0);
                }
            }

            int tableOfContentsLoc = (int)outStream.Position;
            for (int i = 0; i < objectIds.Count; i++)
            {
                outWriter.Write(objectIds[i]);
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(objectLocs[i]);
            }

            int topLevelListLoc = (int)outStream.Position;
            outWriter.Write(objectIds.Count);
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(tableOfContentsLoc);
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

        private void addIfNotNull(List<string> stringList, string stringToAdd)
        {
            if (stringToAdd != null && !stringList.Contains(stringToAdd))
            {
                stringList.Add(stringToAdd);
            }
        }

        private void writeIfNotNull(BinaryWriter outWriter, string stringToWrite, Dictionary<string, int> stringLocs, List<int> pointerLocs)
        {
            if (stringToWrite != null)
            {
                pointerLocs.Add((int)outWriter.BaseStream.Position);
                outWriter.Write(stringLocs[stringToWrite]);
            }
            else
            {
                outWriter.Write(0);
            }
        }
    }
}
