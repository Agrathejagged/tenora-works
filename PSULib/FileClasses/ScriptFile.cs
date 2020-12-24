using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using PSULib;

namespace psu_generic_parser
{
    public class ScriptFile : PsuFile
    {
        public static string[] opcodes = 
        {
            "return",
            "NOP",
            "Push int",
            "Push float",
            "Add",
            "Subtract",
            "Multiply",
            "Divide",
            "Modulo",
            "Shift left",
            "Shift right",
            "Binary And",
            "Binary Or",
            "Binary Xor",
            "Increment",
            "Decrement",
            "Negate",
            "Absolute Value",
            "Test equal",
            "Test not equal",
            "Test greater than or equal",
            "Test greater than",
            "Test less than or equal",
            "Test less than",
            "Logical And",
            "Logical Or",
            "Save",
            "Pop Stack",
            "Swap top two",
            "SaveP",
            "Roll 3 (down)",
            "Copy top two",
            "Pop two",
            "Swap top four",
            "Copy top three",
            "Pop three",
            "Swap top six",
            "Load pushed value",
            "Remove second value",
            "Swap top two",
            "Roll 3 (up)",
            "Clear stack",
            "Cast (integer)",
            "Cast (float)",
            "Branch",
            "Branch (false)",
            "Branch (true)",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Push variable",
            "Set variable",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Load string array",
            "",
            "",
            "Set string value",
            "Get string value",
            "Compare/call top subroutine?",
            "Call subroutine (script)",
            "Call parsed sub (do not use)",
            "Output stack value",
            "Output all stack types",
            "Output top 8 stack types",
            "Output stack size",
            "Enumerate events/syscalls",
            "",
            "",
            "Open menu (async)",
            "Open menu (sync)",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "Call subroutine (system)",
            "System call"
        };

        public string[] stringNames = new string[0];
        public int[] opcodeTypes = 
        {
            1, 1, 2, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 99, 4, 4, 4, 4, 1, 4, 4, 1, 1,
            1, 1, 1, 4, 4, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 2
        };

        public class operation
        {
            public int opcode {get; set;}
            public string strArg {get; set;}
            public int intArg {get; set;}
            public float floatArg { get; set; }

            public string opcodeName
            {
                get { return ScriptFile.opcodes[opcode]; }
                set { opcode = Array.IndexOf(ScriptFile.opcodes, value); }
            }

            public operation()
            {
                opcode = 0;
                strArg = "";
                intArg = int.MaxValue;
                floatArg = float.MaxValue;
            }
        }

        public interface Operand
        {
            string stringRepresentation { get; set; }
            byte[] getRawValue();
        }


        public class IntegerOperand : Operand
        {
            int val = 3;

            public string stringRepresentation
            { get { return val.ToString(); }
                set { val = int.Parse(value); }
            }

            public byte[] getRawValue()
            {
                return BitConverter.GetBytes(val);
            }
        }

        public class ShiftJISStringOperand : Operand
        {
            string val = "hello";

            public string stringRepresentation
            {
                get
                {
                    return val;
                }

                set
                {
                    val = value;
                }
            }

            public byte[] getRawValue()
            {
                byte[] convertedString = ASCIIEncoding.GetEncoding("shift-jis").GetBytes(val + '\0');
                int stringLength = (convertedString.Length + 3)/ 4;
                byte[] returnValue = new byte[stringLength * 4];
                Array.Copy(BitConverter.GetBytes(stringLength), returnValue, 4);
                Array.Copy(convertedString, 0, returnValue, 4, convertedString.Length);
                return returnValue;
            }
        }

        public class subroutine
        {
            public string name {get; set;}
            public int subType { get; set; }
            public int miscData { get; set; } //Use only for not-subroutines.
            public List<operation> opcodes {get; set;}
            public subroutine()
            {
                name = "new_sub";
                opcodes = new List<operation>();
            }
        }

        public List<subroutine> subroutines = new List<subroutine>();

        private ScriptFile()
        {
            
        }

        public ScriptFile(string inFilename, byte[] rawData, bool bigEndian = false)
        {
            filename = inFilename;
            int inFilesize, outFilesize;
            if (!bigEndian)
            {
                outFilesize = BitConverter.ToInt32(rawData, 0);
                inFilesize = BitConverter.ToInt32(rawData, 4);
            }
            else
            {
                byte[] outFilesizeBytes = new byte[] { rawData[3], rawData[2], rawData[1], rawData[0] };
                byte[] inFilesizeBytes = new byte[] { rawData[7], rawData[6], rawData[5], rawData[4] };
                if (BitConverter.ToUInt32(inFilesizeBytes, 0) > rawData.Length)
                {
                    //Stupid hack to try to fix tobitama
                    outFilesize = BitConverter.ToInt32(outFilesizeBytes.Reverse().ToArray(), 0);
                    inFilesize = BitConverter.ToInt32(inFilesizeBytes.Reverse().ToArray(), 0);
                }
                else
                {
                    outFilesize = BitConverter.ToInt32(outFilesizeBytes, 0);
                    inFilesize = BitConverter.ToInt32(inFilesizeBytes, 0);
                }
            }

            byte[] temp = new byte[inFilesize];
            Array.Copy(rawData, 0x1C, temp, 0, inFilesize);
            byte[] decompressed = PrsCompDecomp.Decompress(temp, (uint)outFilesize);

            MemoryStream scriptStream = new MemoryStream(decompressed);
            BinaryReader scriptReader;
            if(bigEndian)
                scriptReader = new BigEndianBinaryReader(scriptStream);
            else
                scriptReader = new BinaryReader(scriptStream);
            scriptStream.Seek(4, SeekOrigin.Begin);
            int stringListPtr = scriptReader.ReadInt32();
            int stringListLength = scriptReader.ReadInt32();

            scriptStream.Seek(stringListPtr + 0xC, SeekOrigin.Begin);
            stringNames = new string[stringListLength / 0x20];
            for (int i = 0; i < stringNames.Length; i++)
            {
                stringNames[i] = ASCIIEncoding.UTF8.GetString(scriptReader.ReadBytes(0x20));
                stringNames[i] = stringNames[i].TrimEnd('\0');
            }
            scriptStream.Seek(0xC, SeekOrigin.Begin);

            //ArrayList subroutines = new ArrayList();
            while (scriptStream.Position < stringListPtr)
            {
                int nextSubLoc = scriptReader.ReadInt32() + 0xC;
                string subroutineName = ASCIIEncoding.UTF8.GetString(scriptReader.ReadBytes(0x20)).TrimEnd('\0');
                int subLength = scriptReader.ReadInt32();
                int subType = scriptReader.ReadLittleEndianInt32();
                int locals = scriptReader.ReadInt32();
                int currLoc = (int)scriptStream.Position;
                int subEnd = currLoc + subLength;
                scriptStream.Seek(subLength, SeekOrigin.Current);
                int[] localNums = new int[locals];
                int[] localLocs = new int[locals];
                if (subType == 0x4C)
                {
                    for (int i = 0; i < locals; i++)
                    {
                        localNums[i] = scriptReader.ReadInt32();
                        localLocs[i] = scriptReader.ReadInt32() * 4;
                    }
                }
                List<operation> operations = new List<operation>();
                List<int> opcodeLocs = new List<int>();
                
                scriptStream.Seek(currLoc, SeekOrigin.Begin);
                while(scriptStream.Position < subEnd)
                {
                    opcodeLocs.Add((int)(scriptStream.Position - currLoc));
                    int currOpcode = scriptReader.ReadInt32();

                    operation tempOp = new operation();
                    if (opcodeTypes[currOpcode] == 3)
                        tempOp.floatArg = scriptReader.ReadSingle();
                    else if (opcodeTypes[currOpcode] > 1 && opcodeTypes[currOpcode] != 4 && !localLocs.Contains((int)(scriptStream.Position - currLoc)))
                        tempOp.intArg = scriptReader.ReadInt32();
                    else if (localLocs.Contains((int)(scriptStream.Position - currLoc)))
                    {
                        tempOp.strArg = stringNames[localNums[Array.IndexOf(localLocs,(int)(scriptStream.Position - currLoc))]];
                        tempOp.intArg = scriptReader.ReadInt32();
                    }
                    if (opcodeTypes[currOpcode] == 99)
                    {
                        tempOp.strArg = ASCIIEncoding.GetEncoding("shift-jis").GetString(scriptReader.ReadBytes(tempOp.intArg * 4)).TrimEnd('\0');
                    }
                    tempOp.opcode = currOpcode;
                    operations.Add(tempOp);
                }
                
                for (int i = 0; i < operations.Count; i++)
                {
                    if (((operation)operations[i]).opcode > 0x2B && ((operation)operations[i]).opcode < 0x2F)
                    {
                        int branchSource = opcodeLocs[i] + 0x8;
                        int branchDestRaw = branchSource + 4 * ((operation)operations[i]).intArg;
                        for (int j = 0; j < operations.Count; j++)
                        {
                            if (branchDestRaw == opcodeLocs[j])
                            {
                                ((operation)operations[i]).intArg = j;
                                break;
                            }
                        }
                    }
                }
                subroutine tempSub = new subroutine();
                tempSub.name = subroutineName;
                tempSub.subType = subType;
                if (subType != 0x4C)
                    tempSub.miscData = locals;
                else
                    tempSub.miscData = -1;
                tempSub.opcodes = operations;

                subroutines.Add(tempSub);
                if (nextSubLoc == 0xC)
                    break;
                scriptStream.Seek(nextSubLoc, SeekOrigin.Begin);
            }
            scriptReader.Close();
        }

        public override byte[] ToRaw()
        {
            MemoryStream rebuildingFile = new MemoryStream();
            BinaryWriter rebuildingWriter = new BinaryWriter(rebuildingFile);
            rebuildingWriter.Write(0x32425354); //TSB2
            int currentStart = 0xC;
            rebuildingFile.Seek(0xC, SeekOrigin.Begin);
            for (int i = 0; i < subroutines.Count; i++)
            {
                rebuildingFile.Seek(4, SeekOrigin.Current);
                subroutine temp = (subroutine)subroutines[i];
                rebuildingWriter.Write(ContainerUtilities.encodePaddedSjisString(temp.name, 0x20));
                int headerLoc = (int)rebuildingFile.Position;
                rebuildingFile.Seek(0xC, SeekOrigin.Current);
                //rebuildingFile.Write(temp.subType);       Fill this in with the rest!
                ArrayList localVariables = new ArrayList();
                int[] opcodeLocs = new int[temp.opcodes.Count];
                int writeStart = (int)rebuildingFile.Position;
                for (int j = 0; j < temp.opcodes.Count; j++)
                {
                    opcodeLocs[j] = (int)rebuildingFile.Position - writeStart;
                    operation tempOpcode = (operation)temp.opcodes[j];
                    rebuildingWriter.Write(tempOpcode.opcode);
                    if (tempOpcode.opcode == 0x46)  //Only one with an actual string argument
                    {
                        byte[] convertedString = ASCIIEncoding.GetEncoding("shift-jis").GetBytes(tempOpcode.strArg + '\0');
                        if (convertedString.Length % 4 != 0)
                        {
                            Array.Resize(ref convertedString, (convertedString.Length + 3) & 0xFFFFFFC);
                        }
                        tempOpcode.intArg = convertedString.Length / 4;
                        rebuildingWriter.Write(tempOpcode.intArg);
                        rebuildingWriter.Write(convertedString);
                    }
                    else if (opcodeTypes[tempOpcode.opcode] == 4) //These are the local variable ones!
                    {
                        if (!stringNames.Contains(tempOpcode.strArg))
                        {
                            Array.Resize(ref stringNames, stringNames.Length + 1);
                            stringNames[stringNames.Length - 1] = tempOpcode.strArg;
                        }
                        localVariables.Add(Array.IndexOf(stringNames, tempOpcode.strArg));
                        localVariables.Add((int)((rebuildingFile.Position - 0xC - headerLoc) / 4));
                        rebuildingWriter.Write((int)(-1));//Array.IndexOf(stringNames, tempOpcode.strArg));
                    }
                    else if (opcodeTypes[tempOpcode.opcode] == 3)
                    {
                        rebuildingWriter.Write(tempOpcode.floatArg);
                    }
                    else if (opcodeTypes[tempOpcode.opcode] == 2)
                        rebuildingWriter.Write(tempOpcode.intArg);
                }
                int localVarLoc = (int)rebuildingFile.Position;
                
                for (int j = 0; j < opcodeLocs.Length; j++)
                {
                    if (((operation)temp.opcodes[j]).opcode > 0x2B && ((operation)temp.opcodes[j]).opcode < 0x2F)
                    {
                        int branchSource = opcodeLocs[j] + 0x8;
                        int realDest = opcodeLocs[((operation)temp.opcodes[j]).intArg];
                        rebuildingFile.Seek(writeStart + opcodeLocs[j] + 0x4, SeekOrigin.Begin);
                        rebuildingWriter.Write((int)(realDest - branchSource) >> 2);
                    }
                }

                rebuildingFile.Seek(localVarLoc, SeekOrigin.Begin);
                for (int j = 0; j < localVariables.Count; j+=2)
                {
                    rebuildingWriter.Write((int)localVariables[localVariables.Count - 2 - j]);
                    rebuildingWriter.Write((int)localVariables[localVariables.Count - 1 - j]);
                }
                int nextSubLoc = (int)rebuildingFile.Position;
                rebuildingFile.Seek(headerLoc, SeekOrigin.Begin);
                rebuildingWriter.Write((int)localVarLoc - headerLoc - 0xC);
                rebuildingWriter.Write(temp.subType);
                rebuildingWriter.Write(localVariables.Count / 2);
                rebuildingFile.Seek(currentStart, SeekOrigin.Begin);
                if(i + 1 < subroutines.Count)
                    rebuildingWriter.Write(nextSubLoc - 0xC);
                else
                    rebuildingWriter.Write((int)0);
                rebuildingFile.Seek(nextSubLoc, SeekOrigin.Begin);
                currentStart = nextSubLoc;
            }
            int stringListLoc = (int)rebuildingFile.Position;
            for (int i = 0; i < stringNames.Length; i++)
            {
                rebuildingWriter.Write(ASCIIEncoding.UTF8.GetBytes(stringNames[i].PadRight(0x20, '\0')));
            }
            rebuildingFile.Seek(4, SeekOrigin.Begin);
            rebuildingWriter.Write(stringListLoc - 0xC);
            rebuildingWriter.Write(stringNames.Length * 0x20);
            byte[] compressedFile = PrsCompDecomp.compress(rebuildingFile.ToArray());
            byte[] toReturn = new byte[compressedFile.Length + 0x1C];
            Array.Copy(BitConverter.GetBytes(rebuildingFile.Length), 0, toReturn, 0, 4);
            Array.Copy(BitConverter.GetBytes(compressedFile.Length), 0, toReturn, 4, 4);
            Array.Copy(compressedFile, 0, toReturn, 0x1C, compressedFile.Length);
            return toReturn;
        }

        public byte[] dumpSubroutine(string[] subs)
        {
            ScriptFile tempScript = new ScriptFile();
            tempScript.filename = "dummy.bin";
            List<string> subsToAdd = new List<string>();
            List<string> addedSubs = new List<string>();
            foreach (string currSub in subs)
            {
                subsToAdd.Add(currSub);
            }
            for (int i = 0; i < subsToAdd.Count; i++)
            {
                if (!addedSubs.Contains(subsToAdd[i]))
                {
                    subroutine currSub = subroutines.Find(x => x.name.Equals(subsToAdd[i]));
                    tempScript.subroutines.Add(currSub);
                    addedSubs.Add(subsToAdd[i]);
                    foreach (operation op in currSub.opcodes)
                    {
                        if (op.opcode == 0x3C || op.opcode == 0x3D || op.opcode == 0x4C)
                        {
                            subsToAdd.Add(op.strArg);
                        }
                    }
                }
            }
            return tempScript.ToRaw();
        }

        public void importScript(ScriptFile toImport)
        {
            foreach (subroutine sub in toImport.subroutines)
            {
                if (!subroutines.Exists(x => x.name == sub.name))
                {
                    subroutines.Add(sub);
                }
            }
        }
    }
}
