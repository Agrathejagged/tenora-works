using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using PSULib.Support;
using PSULib.FileClasses.General.Scripts;

namespace PSULib.FileClasses.General
{
    public class ScriptFile : PsuFile
    {
        public enum OpCodeOperandTypes
        {
            None, //Lots of these.
            Integer, //obvious
            Float, //obvious
            StoredString, //Stored as a string in the subroutine
            EmbeddedString, //Stupid opcode has a shift-JIS string encoded INTO THE OPERAND
            BranchTarget, //This one's custom for the parser to identify branch opcodes, since those are going to end up stored as ints.
            FunctionName, //This is a local string, just a mnemonic to help the parser
            NumericVariableName, //This is a local string, just a mnemonic to help the parser
            StringVariableName, //This is a local string, just a mnemonic to help the parser
        }

        //As far as I can tell, these are the only ones that aren't type NONE.
        //The old list had a bunch of the unused opcodes marked as taking strings. I'm not sure why.
        private static readonly Dictionary<int, OpCodeOperandTypes> opCodeOperandTypeDictionary = new Dictionary<int, OpCodeOperandTypes>()
        {
            { 2, OpCodeOperandTypes.Integer },
            { 3, OpCodeOperandTypes.Float },
            { 0x2C, OpCodeOperandTypes.BranchTarget},
            { 0x2D, OpCodeOperandTypes.BranchTarget},
            { 0x2E, OpCodeOperandTypes.BranchTarget},
            { 0x3C, OpCodeOperandTypes.NumericVariableName},
            { 0x3D, OpCodeOperandTypes.NumericVariableName},
            { 0x46, OpCodeOperandTypes.EmbeddedString},
            { 0x49, OpCodeOperandTypes.StringVariableName},
            { 0x4A, OpCodeOperandTypes.StringVariableName},
            { 0x4C, OpCodeOperandTypes.FunctionName},
            { 0x4D, OpCodeOperandTypes.Integer },
            { 0x60, OpCodeOperandTypes.StoredString},
            { 0x61, OpCodeOperandTypes.Integer },
            //String-arg NOPs (TODO: verify these are accurate):
            { 0x2F, OpCodeOperandTypes.StoredString},
            { 0x30, OpCodeOperandTypes.StoredString},
            { 0x31, OpCodeOperandTypes.StoredString},
            { 0x32, OpCodeOperandTypes.StoredString},
            { 0x33, OpCodeOperandTypes.StoredString},
            { 0x34, OpCodeOperandTypes.StoredString},
            { 0x35, OpCodeOperandTypes.StoredString},
            { 0x36, OpCodeOperandTypes.StoredString},
            { 0x37, OpCodeOperandTypes.StoredString},
            { 0x38, OpCodeOperandTypes.StoredString},
            { 0x39, OpCodeOperandTypes.StoredString},
            { 0x3A, OpCodeOperandTypes.StoredString},
            { 0x3B, OpCodeOperandTypes.StoredString},
            { 0x3E, OpCodeOperandTypes.StoredString},
            { 0x3F, OpCodeOperandTypes.StoredString},
            { 0x40, OpCodeOperandTypes.StoredString},
            { 0x41, OpCodeOperandTypes.StoredString},
            { 0x42, OpCodeOperandTypes.StoredString},
            { 0x43, OpCodeOperandTypes.StoredString},
            { 0x44, OpCodeOperandTypes.StoredString},
            { 0x45, OpCodeOperandTypes.StoredString},
            { 0x47, OpCodeOperandTypes.StoredString},
            { 0x48, OpCodeOperandTypes.StoredString},
            { 0x4B, OpCodeOperandTypes.StoredString},
            { 0x4E, OpCodeOperandTypes.StoredString},
            { 0x4F, OpCodeOperandTypes.StoredString},
            { 0x53, OpCodeOperandTypes.StoredString},
            { 0x54, OpCodeOperandTypes.StoredString},
            { 0x57, OpCodeOperandTypes.StoredString},
            { 0x58, OpCodeOperandTypes.StoredString},
            { 0x59, OpCodeOperandTypes.StoredString},
            { 0x5A, OpCodeOperandTypes.StoredString},
            { 0x5B, OpCodeOperandTypes.StoredString},
            { 0x5C, OpCodeOperandTypes.StoredString},
            { 0x5D, OpCodeOperandTypes.StoredString},
            { 0x5E, OpCodeOperandTypes.StoredString},
            { 0x5F, OpCodeOperandTypes.StoredString}
        };

        public static OpCodeOperandTypes GetOpcodeType(int opCode)
        {
            if (opCodeOperandTypeDictionary.ContainsKey(opCode))
            {
                return opCodeOperandTypeDictionary[opCode];
            }
            return OpCodeOperandTypes.None;
        }

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
            "Copy top after next",
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

        public List<string> stringNames = new List<string>();

        public interface Operand
        {
            string StringRepresentation { get; set; }
            byte[] toRawValue();
            Type OperandType { get; }
        }
        public class NullOperand : Operand
        {
            public string StringRepresentation
            {
                get { return ""; }
                set { }
            }

            public byte[] toRawValue()
            {
                return new byte[0];
            }

            public Type OperandType { get { return typeof(void); } }
        }

        public class IntegerOperand : Operand
        {
            int val = 0;

            public IntegerOperand(int inVal)
            {
                val = inVal;
            }

            public string StringRepresentation
            {
                get { return val.ToString(); }
                set { val = int.Parse(value); }
            }

            public byte[] toRawValue()
            {
                return BitConverter.GetBytes(val);
            }

            public Type OperandType { get { return typeof(int); } }
        }

        public class FloatOperand : Operand
        {
            float val = 0;

            public FloatOperand(float inVal)
            {
                val = inVal;
            }

            public string StringRepresentation
            {
                get { return val.ToString(); }
                set { val = float.Parse(value); }
            }

            public byte[] toRawValue()
            {
                return BitConverter.GetBytes(val);
            }

            public Type OperandType { get { return typeof(float); } }
        }

        public class ShiftJISStringOperand : Operand
        {
            string val = "";

            public ShiftJISStringOperand(string inVal)
            {
                val = inVal;
            }
            public string StringRepresentation
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

            public byte[] toRawValue()
            {
                byte[] convertedString = Encoding.GetEncoding("shift-jis").GetBytes(val + '\0');
                int stringLength = (convertedString.Length + 3) / 4;
                byte[] returnValue = new byte[stringLength * 4 + 4];
                Array.Copy(BitConverter.GetBytes(stringLength), returnValue, 4);
                Array.Copy(convertedString, 0, returnValue, 4, convertedString.Length);
                return returnValue;
            }

            public Type OperandType { get { return typeof(string); } }
        }

        public class StringOperand : Operand
        {
            string val = "";

            public StringOperand(string inVal)
            {
                val = inVal;
            }
            public string StringRepresentation
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

            public byte[] toRawValue()
            {
                return new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            }

            public Type OperandType { get { return typeof(string); } }
        }

        public class Operation
        {
            public string Label { get; set; } = "";
            private int opCode;
            public int OpCode
            {
                get
                {
                    return opCode;
                }
                set
                {
                    Operand newOperand = getEmptyOperand(GetOpcodeType(value));
                    if (newOperand.OperandType == typeof(string) || newOperand.OperandType == Operand.OperandType)
                    {
                        newOperand.StringRepresentation = Operand.StringRepresentation;
                    }
                    else if (newOperand.OperandType == typeof(int) && Operand.OperandType == typeof(float))
                    {
                        float floatValue = float.Parse(Operand.StringRepresentation);
                        newOperand.StringRepresentation = ((int)floatValue).ToString();
                    }
                    else if (newOperand.OperandType == typeof(float) && Operand.OperandType == typeof(int))
                    {
                        int intValue = int.Parse(Operand.StringRepresentation);
                        newOperand.StringRepresentation = ((float)intValue).ToString();
                    }
                    Operand = newOperand;
                    opCode = value;
                }
            }
            public Operand Operand { get; set; } = new NullOperand();
            public string OpCodeName
            {
                get { return opcodes[OpCode]; }
                set { OpCode = Array.IndexOf(opcodes, value); }
            }
            public string OperandText { get { return Operand.StringRepresentation; } set { Operand.StringRepresentation = value; } }
            public OpCodeOperandTypes OpCodeType { get { return GetOpcodeType(OpCode); } }
        }

        public class Subroutine
        {
            public string SubroutineName { get; set; }
            public int SubType { get; set; }
            public int BufferLength { get; set; }
            public List<Operation> Operations { get; set; }
            public Subroutine()
            {
                SubroutineName = "new_sub";
                Operations = new List<Operation>();
            }
        }

        public List<Subroutine> Subroutines = new List<Subroutine>();

        private ScriptFile()
        {

        }

        private static Operand getEmptyOperand(OpCodeOperandTypes operandType)
        {
            switch (operandType)
            {
                case OpCodeOperandTypes.Float: return new FloatOperand(0.0f);
                case OpCodeOperandTypes.Integer: return new IntegerOperand(0);
                case OpCodeOperandTypes.BranchTarget: return new StringOperand("");
                case OpCodeOperandTypes.NumericVariableName:
                case OpCodeOperandTypes.FunctionName:
                case OpCodeOperandTypes.StringVariableName:
                case OpCodeOperandTypes.StoredString: return new StringOperand("");
                case OpCodeOperandTypes.EmbeddedString: return new ShiftJISStringOperand("");
                case OpCodeOperandTypes.None:
                default: return new NullOperand();
            }
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
            if (bigEndian)
                scriptReader = new BigEndianBinaryReader(scriptStream);
            else
                scriptReader = new BinaryReader(scriptStream);
            scriptStream.Seek(4, SeekOrigin.Begin);
            int stringListPtr = scriptReader.ReadInt32();
            int stringListLength = scriptReader.ReadInt32();

            scriptStream.Seek(stringListPtr + 0xC, SeekOrigin.Begin);
            int stringListCount = stringListLength / 0x20;
            for (int i = 0; i < stringListCount; i++)
            {
                stringNames.Add(Encoding.UTF8.GetString(scriptReader.ReadBytes(0x20)).TrimEnd('\0'));
            }
            scriptStream.Seek(0xC, SeekOrigin.Begin);

            //ArrayList subroutines = new ArrayList();
            while (scriptStream.Position < stringListPtr)
            {
                int nextSubLoc = scriptReader.ReadInt32() + 0xC;
                string subroutineName = Encoding.UTF8.GetString(scriptReader.ReadBytes(0x20)).TrimEnd('\0');
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
                List<Operation> newOperations = new List<Operation>();
                List<int> opcodeLocs = new List<int>();
                HashSet<int> branchDestinations = new HashSet<int>();

                scriptStream.Seek(currLoc, SeekOrigin.Begin);
                while (scriptStream.Position < subEnd)
                {
                    int currentOpcodeLoc = (int)(scriptStream.Position - currLoc);
                    opcodeLocs.Add(currentOpcodeLoc);
                    int currOpcode = scriptReader.ReadInt32();

                    Operation newOp = new Operation();
                    switch (GetOpcodeType(currOpcode))
                    {
                        case OpCodeOperandTypes.None: newOp.Operand = new NullOperand(); break;
                        case OpCodeOperandTypes.Integer:
                            newOp.Operand = new IntegerOperand(scriptReader.ReadInt32()); break;
                        case OpCodeOperandTypes.Float:
                            float floatArg = scriptReader.ReadSingle();
                            newOp.Operand = new FloatOperand(floatArg);
                            break;
                        case OpCodeOperandTypes.FunctionName:
                        case OpCodeOperandTypes.NumericVariableName:
                        case OpCodeOperandTypes.StringVariableName:
                        case OpCodeOperandTypes.StoredString:
                            string stringArg;
                            if (localLocs.Contains((int)(scriptStream.Position - currLoc)))
                            {
                                stringArg = stringNames[localNums[Array.IndexOf(localLocs, (int)(scriptStream.Position - currLoc))]];
                            }
                            else
                            {
                                stringArg = "UNKNOWN_LOCAL"; //don't choke on malformed scripts.
                            }
                            scriptReader.ReadInt32();
                            newOp.Operand = new StringOperand(stringArg);
                            break;
                        case OpCodeOperandTypes.EmbeddedString:
                            int stringLength = scriptReader.ReadInt32();
                            newOp.Operand = new ShiftJISStringOperand(Encoding.GetEncoding("shift-jis").GetString(scriptReader.ReadBytes(stringLength * 4)).TrimEnd('\0'));
                            break;
                        case OpCodeOperandTypes.BranchTarget:
                            int destinationBranch = scriptReader.ReadInt32();
                            int destinationReal = destinationBranch * 4 + currentOpcodeLoc + 8;
                            newOp.Operand = new StringOperand("label_" + destinationReal);
                            branchDestinations.Add(destinationReal);
                            break;
                        default: break;
                    }
                    newOp.OpCode = currOpcode;
                    newOperations.Add(newOp);
                }

                for (int i = 0; i < opcodeLocs.Count; i++)
                {
                    if (branchDestinations.Contains(opcodeLocs[i]))
                    {
                        newOperations[i].Label = "label_" + opcodeLocs[i];
                    }
                }

                Subroutine tempSub = new Subroutine();
                tempSub.SubroutineName = subroutineName;
                tempSub.SubType = subType;
                if (subType != 0x4C)
                {
                    tempSub.BufferLength = locals;
                }
                tempSub.Operations = newOperations;

                Subroutines.Add(tempSub);
                if (nextSubLoc == 0xC)
                    break;
                scriptStream.Seek(nextSubLoc, SeekOrigin.Begin);
            }
            scriptReader.Close();
        }

        public void Validate()
        {
            var foundErrors = new List<ScriptValidationException.ScriptValidationError>();
            var subroutineNames = Subroutines.Where(subroutine => subroutine.SubType == 0x4C).Select(subroutine => subroutine.SubroutineName);
            var numericVariableNames = Subroutines.Where(subroutine => subroutine.SubType == 0x3C).Select(subroutine => subroutine.SubroutineName);
            var stringVariableNames = Subroutines.Where(subroutine => subroutine.SubType == 0x49).Select(subroutine => subroutine.SubroutineName);
            foreach (var subroutine in Subroutines)
            {
                var foundLabels = subroutine.Operations.Select(operation => operation.Label).Distinct();
                if (subroutine.SubType == 0x49 && subroutine.BufferLength <= 0)
                {
                    foundErrors.Add(new ScriptValidationException.ScriptValidationError { FunctionName = subroutine.SubroutineName, LineNumber = -1, Description = "String variable with length less than 1" });
                }
                for (int i = 0; i < subroutine.Operations.Count; i++)
                {
                    Operation operation = subroutine.Operations[i];
                    switch (GetOpcodeType(operation.OpCode))
                    {
                        case OpCodeOperandTypes.BranchTarget:
                            if (!foundLabels.Contains(operation.OperandText))
                            {
                                foundErrors.Add(new ScriptValidationException.ScriptValidationError { FunctionName = subroutine.SubroutineName, LineNumber = i, Description = "Branch to missing label \"" + operation.OperandText + "\"" });
                            }
                            break;
                        case OpCodeOperandTypes.FunctionName:
                            if (!subroutineNames.Contains(operation.OperandText))
                            {
                                foundErrors.Add(new ScriptValidationException.ScriptValidationError { FunctionName = subroutine.SubroutineName, LineNumber = i, Description = "Call missing subroutine \"" + operation.OperandText + "\"" });
                            }
                            break;
                        case OpCodeOperandTypes.NumericVariableName:
                            if (!numericVariableNames.Contains(operation.OperandText))
                            {
                                foundErrors.Add(new ScriptValidationException.ScriptValidationError { FunctionName = subroutine.SubroutineName, LineNumber = i, Description = "Reference missing or invalid numeric variable \"" + operation.OperandText + "\"" });
                            }
                            break;
                        case OpCodeOperandTypes.StringVariableName:
                            if (!stringVariableNames.Contains(operation.OperandText))
                            {
                                foundErrors.Add(new ScriptValidationException.ScriptValidationError { FunctionName = subroutine.SubroutineName, LineNumber = i, Description = "Reference missing or invalid string variable \"" + operation.OperandText + "\"" });
                            }
                            break;
                    }
                    //TODO: Verify system functions?
                    //Verify stack integrity? (this one would be a LOT of work)
                }
            }
            if (foundErrors.Count > 0)
            {
                throw new ScriptValidationException { ScriptValidationErrors = foundErrors, FileName = filename };
            }
        }

        public override byte[] ToRaw()
        {
            Validate();
            MemoryStream rebuildingFile = new MemoryStream();
            BinaryWriter rebuildingWriter = new BinaryWriter(rebuildingFile);
            rebuildingWriter.Write(0x32425354); //TSB2
            int currentStart = 0xC;
            rebuildingFile.Seek(0xC, SeekOrigin.Begin);
            for (int i = 0; i < Subroutines.Count; i++)
            {
                rebuildingFile.Seek(4, SeekOrigin.Current);
                Subroutine temp = Subroutines[i];
                rebuildingWriter.Write(StringUtilities.encodePaddedSjisString(temp.SubroutineName, 0x20));
                int headerLoc = (int)rebuildingFile.Position;
                rebuildingFile.Seek(0xC, SeekOrigin.Current);
                //rebuildingFile.Write(temp.subType);       Fill this in with the rest!
                ArrayList localVariables = new ArrayList();
                int[] opcodeLocs = new int[temp.Operations.Count];
                int writeStart = (int)rebuildingFile.Position;
                Dictionary<string, int> labelLocations = new Dictionary<string, int>();
                Dictionary<int, string> branchStarts = new Dictionary<int, string>();
                for (int j = 0; j < temp.Operations.Count; j++)
                {
                    opcodeLocs[j] = (int)rebuildingFile.Position - writeStart;
                    Operation tempOpcode = temp.Operations[j];
                    rebuildingWriter.Write(tempOpcode.OpCode);
                    if (tempOpcode.Label != null && tempOpcode.Label != "")
                    {
                        labelLocations.Add(tempOpcode.Label, opcodeLocs[j]);
                    }

                    byte[] toWrite;
                    //Sadly, we need special handling for a few of these things.
                    switch (GetOpcodeType(tempOpcode.OpCode))
                    {
                        //Local strings have to be added to the table.
                        case OpCodeOperandTypes.StoredString:
                        case OpCodeOperandTypes.StringVariableName:
                        case OpCodeOperandTypes.NumericVariableName:
                        case OpCodeOperandTypes.FunctionName:
                            if (!stringNames.Contains(tempOpcode.Operand.StringRepresentation))
                            {
                                stringNames.Add(tempOpcode.Operand.StringRepresentation);
                            }
                            localVariables.Add(stringNames.IndexOf(tempOpcode.Operand.StringRepresentation));
                            localVariables.Add((int)((rebuildingFile.Position - 0xC - headerLoc) / 4));
                            toWrite = tempOpcode.Operand.toRawValue();
                            break;
                        //This one's branch offsets. We need to calculate the offset based on the label.
                        //...except the label may not be present yet.
                        //...and we can't predict how long the rest of the operations will be.
                        //...because of those damned sjis strings.
                        case OpCodeOperandTypes.BranchTarget:
                            toWrite = BitConverter.GetBytes(-1);
                            branchStarts.Add(opcodeLocs[j] + 4, tempOpcode.Operand.StringRepresentation);
                            break;
                        default: toWrite = tempOpcode.Operand.toRawValue(); break;
                    }
                    rebuildingWriter.Write(toWrite);
                }
                int localVarLoc = (int)rebuildingFile.Position;

                foreach (var entry in branchStarts)
                {
                    rebuildingFile.Seek(writeStart + entry.Key, SeekOrigin.Begin);
                    if (labelLocations.ContainsKey(entry.Value))
                    {
                        int branchSource = entry.Key + 0x4;
                        int realDest = labelLocations[entry.Value];
                        rebuildingWriter.Write(realDest - branchSource >> 2);
                    }
                }

                rebuildingFile.Seek(localVarLoc, SeekOrigin.Begin);
                for (int j = 0; j < localVariables.Count; j += 2)
                {
                    rebuildingWriter.Write((int)localVariables[localVariables.Count - 2 - j]);
                    rebuildingWriter.Write((int)localVariables[localVariables.Count - 1 - j]);
                }
                int nextSubLoc = (int)rebuildingFile.Position;
                rebuildingFile.Seek(headerLoc, SeekOrigin.Begin);
                rebuildingWriter.Write(localVarLoc - headerLoc - 0xC);
                rebuildingWriter.Write(temp.SubType);
                if (temp.SubType == 0x4C)
                {
                    rebuildingWriter.Write(localVariables.Count / 2);
                }
                else
                {
                    rebuildingWriter.Write(temp.BufferLength);
                }
                rebuildingFile.Seek(currentStart, SeekOrigin.Begin);
                if (i + 1 < Subroutines.Count)
                    rebuildingWriter.Write(nextSubLoc - 0xC);
                else
                    rebuildingWriter.Write(0);
                rebuildingFile.Seek(nextSubLoc, SeekOrigin.Begin);
                currentStart = nextSubLoc;
            }
            int stringListLoc = (int)rebuildingFile.Position;
            for (int i = 0; i < stringNames.Count; i++)
            {
                rebuildingWriter.Write(Encoding.UTF8.GetBytes(stringNames[i].PadRight(0x20, '\0')));
            }
            rebuildingFile.Seek(4, SeekOrigin.Begin);
            rebuildingWriter.Write(stringListLoc - 0xC);
            rebuildingWriter.Write(stringNames.Count * 0x20);
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
                    Subroutine currSub = Subroutines.Find(x => x.SubroutineName.Equals(subsToAdd[i]));
                    tempScript.Subroutines.Add(currSub);
                    addedSubs.Add(subsToAdd[i]);
                    for (int j = 0; j < currSub.Operations.Count; j++)
                    {
                        Operation op = currSub.Operations[j];
                        OpCodeOperandTypes type = GetOpcodeType(op.OpCode);
                        if (type == OpCodeOperandTypes.FunctionName || type == OpCodeOperandTypes.NumericVariableName || type == OpCodeOperandTypes.StringVariableName)
                        {
                            subsToAdd.Add(op.Operand.StringRepresentation);
                        }
                        else if (op.OpCode == 0x60 && op.Operand.StringRepresentation == "thread.create")
                        {
                            //If we're creating a thread, the previous operation is usually pushing the subroutine for the thread
                            //Example (taken from basically every free mission)
                            //          Push int                    65
                            //          Call subroutine (script)	work.qstwork_get
                            //          Branch (false)              label_80
                            //          Load string array           cwMission_Bgm_Boss_Thread
                            //          Call subroutine (system)    thread.create
                            //          Pop Stack
                            //          Branch                      label_124
                            //label_80  Load string array           cwMission_Bgm_Thread
                            //          Call subroutine (system)    thread.create
                            //          Pop Stack	
                            //label_124	return	

                            //So in an ideal world, we would go and emulate the FULL STACK for this, but in many cases,
                            //we just can grab the previous instruction for the function.

                            if (j > 0)
                            {
                                Operation previousOp = currSub.Operations[j - 1];
                                if (GetOpcodeType(previousOp.OpCode) == OpCodeOperandTypes.EmbeddedString)
                                {
                                    subsToAdd.Add(previousOp.Operand.StringRepresentation);
                                }
                            }

                        }
                    }
                    /*
                    foreach (Operation op in currSub.Operations)
                    {

                        if(op.O)
                        if (op.OpCode == 0x3C || op.OpCode == 0x3D || op.OpCode == 0x4C)
                        {
                            subsToAdd.Add(op.Operand.StringRepresentation);
                        }
                    }*/
                }
            }
            return tempScript.ToRaw();
        }

        public void importScript(ScriptFile toImport)
        {
            foreach (Subroutine sub in toImport.Subroutines)
            {
                if (!Subroutines.Exists(x => x.SubroutineName == sub.SubroutineName))
                {
                    Subroutines.Add(sub);
                }
            }
        }
    }
}
