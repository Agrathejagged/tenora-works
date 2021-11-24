using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Enemies
{
    public class ActDataFile : PsuFile
    {
        public class ActDataSubEntry
        {
            public int UnknownInt1 { get; set; }
            public float UnknownFloat { get; set; }
            public int UnknownInt2 { get; set; }
        }

        public class ActionEntry
        {
            public int UnknownInt1 { get; set; } //Definite.
            public int MotTblID { get; set; } //Definite.
            public float UnknownFloatAt3 { get; set; } //Definite.
            public int VerticalExaggeration { get; set; } //Definite.
            public float MotionFloat1 { get; set; } //Definite.
            public float MotionFloat2 { get; set; } //Definite.
            public int HorizontalUnknown { get; set; } //Probably.
            public float UnknownFloatAt8 { get; set; }
            public float UnknownFloatAt9 { get; set; }
            public int UnknownAngleDegrees1 { get; set; } //Definite. Looks like angles?
            public int UnknownIntAt11 { get; set; } //Definite.
            public int UnknownAngleDegrees2 { get; set; } //Definite.
            public int UnknownAngleDegrees3 { get; set; } //Definite. Looks like angles?
            public int UnknownStateValue { get; set; } //-1 is disabled, seen values are 0/1/2.
            public float UnknownStateModifier1 { get; set; } //Controlled by UnknownStateValue.
            public float UnknownStateModifier2 { get; set; } //Controlled by UnknownStateValue.
            public List<ActDataSubEntry> SubEntryList1 = new List<ActDataSubEntry>();
            public List<ActDataSubEntry> SubEntryList2 = new List<ActDataSubEntry>();
            public int AttackID { get; set; } //Definite.
            public int UnknownInt15 { get; set; } //Definite.
            public float UnknownFloat6 { get; set; } //Definite.
            public int UnknownInt16 { get; set; } //Definite.
            public float UnknownFloat7 { get; set; } //Definite.
            public int DamageDataList { get; set; } //List in DamageData file to use when monster is attacked during this action.
            public int UnknownInt18 { get; set; } //Definite.
            public int UnknownInt19 { get; set; } //Definite.
            public int UnknownInt20 { get; set; } //Bitflags.
            public float UnknownFloatAt21 { get; set; } 
            public int UnusedInt22 { get; set; }
            public int UnusedInt23 { get; set; }
            public int UnusedInt24 { get; set; }
            public int UnusedInt25 { get; set; }
        }

        public class Action
        {
            public List<ActionEntry> ActionEntries = new List<ActionEntry>();
        }

        public List<Action> Actions = new List<Action>();

        public ActDataFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            header = subHeader;

            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();
            inStream.Seek(headerLoc, SeekOrigin.Begin);

            int listLoc = inReader.ReadInt32();
            int listCount = inReader.ReadInt32();
            List<int> pointerList = new List<int>(ptrs);
            for(int i = 0; i < pointerList.Count; i++)
            {
                pointerList[i] = pointerList[i] - baseAddr;
            }
            List<int> actionLocs = new List<int>();
            List<int> actionEntryCounts = new List<int>();

            if (listLoc != 0 && listCount > 0)
            {
                inStream.Seek(listLoc - baseAddr, SeekOrigin.Begin);
                for (int i = 0; i < listCount; i++)
                {
                    actionLocs.Add(inReader.ReadInt32());
                    actionEntryCounts.Add(inReader.ReadInt32());
                }
                for (int i = 0; i < listCount; i++)
                {
                    Action attack = new Action();
                    for (int j = 0; j < actionEntryCounts[i]; j++)
                    {
                        inStream.Seek((actionLocs[i] - baseAddr) + j * 0x88, SeekOrigin.Begin);
                        ActionEntry action = new ActionEntry();

                        action.UnknownInt1 = expectNonPointerInt(inReader, pointerList);
                        action.MotTblID = expectNonPointerInt(inReader, pointerList);
                        action.UnknownFloatAt3 = inReader.ReadSingle();
                        action.VerticalExaggeration = expectNonPointerInt(inReader, pointerList);
                        action.MotionFloat1 = inReader.ReadSingle();
                        action.MotionFloat2 = inReader.ReadSingle();
                        action.HorizontalUnknown = expectNonPointerInt(inReader, pointerList);
                        action.UnknownFloatAt8 = inReader.ReadSingle();
                        action.UnknownFloatAt9 = inReader.ReadSingle();
                        action.UnknownAngleDegrees1 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownIntAt11 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownAngleDegrees2 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownAngleDegrees3 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownStateValue = expectNonPointerInt(inReader, pointerList);
                        action.UnknownStateModifier1 = inReader.ReadSingle();
                        action.UnknownStateModifier2 = inReader.ReadSingle();
                        int subEntry1Loc = inReader.ReadInt32();
                        int subEntry1Count = expectNonPointerInt(inReader, pointerList);
                        int subEntry2Loc = inReader.ReadInt32();
                        int subEntry2Count = expectNonPointerInt(inReader, pointerList);
                        action.AttackID = expectNonPointerInt(inReader, pointerList);
                        action.UnknownInt15 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownFloat6 = inReader.ReadSingle();
                        action.UnknownInt16 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownFloat7 = inReader.ReadSingle();
                        action.DamageDataList = expectNonPointerInt(inReader, pointerList);
                        action.UnknownInt18 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownInt19 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownInt20 = expectNonPointerInt(inReader, pointerList);
                        action.UnknownFloatAt21 = inReader.ReadSingle();
                        action.UnusedInt22 = expectNonPointerInt(inReader, pointerList);
                        action.UnusedInt23 = expectNonPointerInt(inReader, pointerList);
                        action.UnusedInt24 = expectNonPointerInt(inReader, pointerList);
                        action.UnusedInt25 = expectNonPointerInt(inReader, pointerList);

                        if (subEntry1Count > 0)
                        {
                            inStream.Seek(subEntry1Loc - baseAddr, SeekOrigin.Begin);
                            for (int k = 0; k < subEntry1Count; k++)
                            {
                                ActDataSubEntry subEntry = new ActDataSubEntry();
                                subEntry.UnknownInt1 = inReader.ReadInt32();
                                subEntry.UnknownFloat = inReader.ReadSingle();
                                subEntry.UnknownInt2 = inReader.ReadInt32();
                                action.SubEntryList1.Add(subEntry);
                            }
                        }
                        if (subEntry2Count > 0)
                        {
                            inStream.Seek(subEntry2Loc - baseAddr, SeekOrigin.Begin);
                            for (int k = 0; k < subEntry2Count; k++)
                            {
                                ActDataSubEntry subEntry = new ActDataSubEntry();
                                subEntry.UnknownInt1 = inReader.ReadInt32();
                                subEntry.UnknownFloat = inReader.ReadSingle();
                                subEntry.UnknownInt2 = inReader.ReadInt32();
                                action.SubEntryList2.Add(subEntry);
                            }
                        }
                        attack.ActionEntries.Add(action);
                    }
                    Actions.Add(attack);
                }
            }
        }

        private void writeLeaves(BinaryWriter writer, List<int> leafLocs, List<ActDataSubEntry> subEntries)
        {
            if (subEntries.Count > 0)
            {
                leafLocs.Add((int)(writer.BaseStream.Position));
                foreach(var leaf in subEntries)
                {
                    writer.Write(leaf.UnknownInt1);
                    writer.Write(leaf.UnknownFloat);
                    writer.Write(leaf.UnknownInt2);
                }
            }
            else
            {
                leafLocs.Add(0);
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            List<int> pointerLocs = new List<int>();

            //Contents are leaves-subactions-actions

            //Each subentry has 2 of these.
            List< List < List<int>>> leafListLocs = new List<List< List<int>>>();
            foreach(var action in Actions)
            {
                List<List<int>> currentTierTwo = new List<List<int>>();
                foreach(var subAction in action.ActionEntries)
                {
                    List<int> currentTierThree = new List<int>();
                    writeLeaves(outWriter, currentTierThree, subAction.SubEntryList1);
                    writeLeaves(outWriter, currentTierThree, subAction.SubEntryList2);
                    currentTierTwo.Add(currentTierThree);
                }
                leafListLocs.Add(currentTierTwo);
            }

            List<int> actionLocs = new List<int>();
            for(int i = 0; i < Actions.Count; i++)
            {
                var parentAction = Actions[i];
                actionLocs.Add((int)outStream.Position);
                for(int j = 0; j < parentAction.ActionEntries.Count; j++)
                {
                    var action = parentAction.ActionEntries[j];
                    outWriter.Write(action.UnknownInt1);
                    outWriter.Write(action.MotTblID);
                    outWriter.Write(action.UnknownFloatAt3);
                    outWriter.Write(action.VerticalExaggeration);
                    outWriter.Write(action.MotionFloat1);
                    outWriter.Write(action.MotionFloat2);
                    outWriter.Write(action.HorizontalUnknown);
                    outWriter.Write(action.UnknownFloatAt8);
                    outWriter.Write(action.UnknownFloatAt9);
                    outWriter.Write(action.UnknownAngleDegrees1);
                    outWriter.Write(action.UnknownIntAt11);
                    outWriter.Write(action.UnknownAngleDegrees2);
                    outWriter.Write(action.UnknownAngleDegrees3);
                    outWriter.Write(action.UnknownStateValue);
                    outWriter.Write(action.UnknownStateModifier1);
                    outWriter.Write(action.UnknownStateModifier2);
                    if(action.SubEntryList1.Count > 0)
                    {
                        pointerLocs.Add((int)outStream.Position);
                        outWriter.Write(leafListLocs[i][j][0]);
                    }
                    else
                    {
                        outWriter.Write((int)0);
                    }
                    outWriter.Write(action.SubEntryList1.Count);
                    if (action.SubEntryList2.Count > 0)
                    {
                        pointerLocs.Add((int)outStream.Position);
                        outWriter.Write(leafListLocs[i][j][1]);
                    }
                    else
                    {
                        outWriter.Write((int)0);
                    }
                    outWriter.Write(action.SubEntryList2.Count);
                    outWriter.Write(action.AttackID);
                    outWriter.Write(action.UnknownInt15);
                    outWriter.Write(action.UnknownFloat6);
                    outWriter.Write(action.UnknownInt16);
                    outWriter.Write(action.UnknownFloat7);
                    outWriter.Write(action.DamageDataList);
                    outWriter.Write(action.UnknownInt18);
                    outWriter.Write(action.UnknownInt19);
                    outWriter.Write(action.UnknownInt20);
                    outWriter.Write(action.UnknownFloatAt21);
                    outWriter.Write(action.UnusedInt22);
                    outWriter.Write(action.UnusedInt23);
                    outWriter.Write(action.UnusedInt24);
                    outWriter.Write(action.UnusedInt25);
                }
            }

            int listLoc = (int)outStream.Position;
            for(int i = 0; i < Actions.Count; i++)
            {
                pointerLocs.Add((int)outStream.Position);
                outWriter.Write(actionLocs[i]);
                outWriter.Write(Actions[i].ActionEntries.Count);
            }

            //Write the table of contents
            int topLevelListLoc = (int)outStream.Position;
            pointerLocs.Add((int)outStream.Position);
            outWriter.Write(listLoc);
            outWriter.Write(Actions.Count);

            int fileLength = (int)outStream.Position;
            outStream.Seek(15 - (outStream.Position % 16), SeekOrigin.Current);
            if (outStream.Position > fileLength)
            {
                outWriter.Write((byte)0);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write((int)0x52584E);
            outWriter.Write(fileLength);
            outWriter.Write(topLevelListLoc);
            calculatedPointers = pointerLocs.ToArray();

            header = buildSubheader(fileLength);
            return outStream.ToArray();
        }
    }
}
