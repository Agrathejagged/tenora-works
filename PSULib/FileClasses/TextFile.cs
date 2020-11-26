using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace psu_generic_parser
{
    public class TextFile : PsuFile
    {
        //public string filename;       //The way it works, don't need to redeclare here.
        public List<string>[][] stringArray;

        public TextFile()
        {
            filename = "DUMMY";
        }
        public TextFile(string filename)
        {
            this.filename = filename;
            stringArray = new List<string>[1][];
        }

        public TextFile(string inFilename, byte[] rawData)
        {
            filename = inFilename;
            MemoryStream rawStream = new MemoryStream(rawData);
            BinaryReader rawReader = new BinaryReader(rawStream);
            int fileLength = rawReader.ReadInt32();
            int lowestSecondLevel = fileLength;
            ArrayList firstLevelAddrs = new ArrayList();
            while (rawStream.Position < lowestSecondLevel)
            {
                int currValue = rawReader.ReadInt32();
                firstLevelAddrs.Add(currValue);
                if (currValue < lowestSecondLevel)
                    lowestSecondLevel = currValue;
            }
            stringArray = new List<string>[firstLevelAddrs.Count][];
            int lowestThirdLevel = fileLength;
            ArrayList[] secondLevelAddrs = new ArrayList[firstLevelAddrs.Count];
            for (int i = 0; i < firstLevelAddrs.Count; i++)
            {
                rawStream.Seek((int)firstLevelAddrs[i], SeekOrigin.Begin);
                int endCondition;
                if (firstLevelAddrs.Count > i + 1)
                    endCondition = (int)firstLevelAddrs[i + 1];
                else
                    endCondition = lowestThirdLevel;
                secondLevelAddrs[i] = new ArrayList();
                while (rawStream.Position < endCondition)
                {
                    int currValue = rawReader.ReadInt32();
                    secondLevelAddrs[i].Add(currValue);
                    if (currValue < endCondition)
                        endCondition = currValue;
                    if (currValue < lowestThirdLevel)
                        lowestThirdLevel = currValue;
                }
                stringArray[i] = new List<string>[secondLevelAddrs[i].Count];
            }
            int lowestFourthLevel = fileLength;
            ArrayList[][] thirdLevelAddrs = new ArrayList[firstLevelAddrs.Count][];
            for (int i = 0; i < firstLevelAddrs.Count; i++)
            {
                thirdLevelAddrs[i] = new ArrayList[secondLevelAddrs[i].Count];
                for (int j = 0; j < secondLevelAddrs[i].Count; j++)
                {
                    rawStream.Seek((int)secondLevelAddrs[i][j], SeekOrigin.Begin);
                    int endCondition;
                    if (secondLevelAddrs[i].Count > j + 1)
                        endCondition = (int)secondLevelAddrs[i][j + 1];
                    else
                        endCondition = lowestFourthLevel;
                    thirdLevelAddrs[i][j] = new ArrayList();
                    while (rawStream.Position < endCondition)
                    {
                        int currValue = rawReader.ReadInt32();
                        thirdLevelAddrs[i][j].Add(currValue);
                        if (currValue < endCondition)
                            endCondition = currValue;
                        if (currValue < lowestFourthLevel)
                            lowestFourthLevel = currValue;
                    }
                    stringArray[i][j] = new List<string>(thirdLevelAddrs[i][j].Count);
                }
            }
            for (int i = 0; i < firstLevelAddrs.Count; i++)
            {
                for (int j = 0; j < secondLevelAddrs[i].Count; j++)
                {
                    for (int k = 0; k < thirdLevelAddrs[i][j].Count; k++)
                    {
                        rawStream.Seek((int)thirdLevelAddrs[i][j][k], SeekOrigin.Begin);
                        int stringLength = 255;
                        if (k < thirdLevelAddrs[i][j].Count - 1)
                        {
                            stringLength = (int)thirdLevelAddrs[i][j][k + 1] - (int)thirdLevelAddrs[i][j][k];
                        }
                        else if (j < thirdLevelAddrs[i].Length - 1 && thirdLevelAddrs[i][j + 1].Count > 0)
                            stringLength = (int)thirdLevelAddrs[i][j + 1][0] - (int)thirdLevelAddrs[i][j][k];
                        else
                            stringLength = fileLength - (int)thirdLevelAddrs[i][j][k];
                        byte[] rawString = rawReader.ReadBytes(stringLength);
                        string toStore = "";
                        for (int m = 0; m < rawString.Length - 1; m += 2)
                        {
                            ushort currChar = BitConverter.ToUInt16(rawString, m);
                            if (currChar == '\r')
                            {
                                toStore += "\r\n";
                            }
                            else if (currChar == '\n')
                            {
                                toStore += "\r\n";
                            }
                            else if (currChar == 0)
                                break;
                            else if ((currChar > 0x9fa5 && currChar < 0xff00) || (currChar < 0x20))
                            {
                                toStore += "<" + BitConverter.ToString(rawString, m, 2).Replace("-", "");
                                if (currChar == 0xf890 || currChar == 0xf701)
                                {
                                    toStore += BitConverter.ToString(rawString, m + 2, 2).Replace("-", "");
                                    m += 2;
                                }
                                if (currChar == 0xf703 || currChar == 0xf700)
                                {
                                    toStore += BitConverter.ToString(rawString, m + 2, 4).Replace("-", "");
                                    m += 4;
                                }
                                toStore += ">";
                            }
                            else
                                toStore += new string(ASCIIEncoding.Unicode.GetChars(rawString, m, 2));
                        }
                        stringArray[i][j].Add(toStore);
                        //stringArray[i][j][k] = stringArray[i][j][k].Replace("\n", "\r\n");
                    }
                }
            }
            rawReader.Close();
        }

        public override byte[] ToRaw()
        {
            MemoryStream rawStream = new MemoryStream();
            BinaryWriter rawWriter = new BinaryWriter(rawStream);
            Regex filestring = new Regex("<([0-9a-fA-F]{2})+>");

            int initialFirst = 4;
            int initialSecond = initialFirst + 4 * stringArray.Length;
            int initialThird = initialSecond;
            int initialFourth = 0;
            for(int i = 0; i < stringArray.Length; i++)
            {
                initialThird += 4 * stringArray[i].Length;
                for(int j = 0; j < stringArray[i].Length; j++)
                    initialFourth += 4 * stringArray[i][j].Count;
            }
            initialFourth += initialThird;
            int[][][] pointers = new int[stringArray.Length][][];
            //rawWriter.Write(new byte[initialFourth]);
            rawStream.Seek(initialFourth, SeekOrigin.Begin);
            for (int i = 0; i < stringArray.Length; i++)
            {
                pointers[i] = new int[stringArray[i].Length][];
                for (int j = 0; j < stringArray[i].Length; j++)
                {
                    pointers[i][j] = new int[stringArray[i][j].Count];
                    for (int k = 0; k < stringArray[i][j].Count; k++)
                    {
                        MatchCollection tempMatches = filestring.Matches(stringArray[i][j][k]);
                        pointers[i][j][k] = (int)rawStream.Position;
                        if (stringArray[i][j][k].Contains("<"))
                            stringArray[i][j][k] = stringArray[i][j][k];
                        int startPos = 0;
                        foreach (Match match in tempMatches)
                        {
                            byte[] matchBytes = new byte[match.Value.Length / 2 - 1];
                            for (int m = 0; m < matchBytes.Length; m++)
                            {
                                matchBytes[m] = byte.Parse(match.Value.Substring(2 * m + 1, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                            }
                            if (match.Index > 0)
                                rawWriter.Write(Encoding.Unicode.GetBytes(stringArray[i][j][k].Substring(startPos, match.Index - startPos).Replace("\\n", "\n").Replace("\r\n", "\n")));
                            rawWriter.Write(matchBytes);
                            startPos = match.Index + match.Length;
                        }
                        rawWriter.Write(Encoding.Unicode.GetBytes(stringArray[i][j][k].Substring(startPos).Replace("\r\n", "\n") + "\0"));
                    }
                }
            }
            int maxLength = (int)rawStream.Position;
            rawStream.Seek(0, SeekOrigin.Begin);
            rawWriter.Write(maxLength);
            int currentLoc = initialSecond;
            for (int i = 0; i < stringArray.Length; i++)
            {
                rawWriter.Write(currentLoc);
                currentLoc += 4;
            }
            currentLoc = initialThird;
            for (int i = 0; i < stringArray.Length; i++)
            {
                for (int j = 0; j < stringArray[i].Length; j++)
                {
                    rawWriter.Write(currentLoc);
                    currentLoc += stringArray[i][j].Count * 4;
                }
            }
            for (int i = 0; i < stringArray.Length; i++)
            {
                for (int j = 0; j < stringArray[i].Length; j++)
                {
                    for (int k = 0; k < stringArray[i][j].Count; k++)
                    {
                        rawWriter.Write(pointers[i][j][k]);
                    }
                }
            }
            byte[] toReturn = rawStream.ToArray();
            return toReturn;// rawStream.ToArray();
        }

        /*
        public void importBinNoReplace(Stream toImport)
        {
            BinaryReader importer = new BinaryReader(toImport);
            TextFile inText = new TextFile("temp", importer.)
            toImport.Seek(4, SeekOrigin.Begin);
            int secondLevelStart = importer.ReadInt32();
            toImport.Seek(secondLevelStart, SeekOrigin.Begin);
            int thirdLevelStart = importer.ReadInt32();
            int[] firstLevel = new int[(secondLevelStart - 4) / 4];
            int[] secondLevel = new int[(thirdLevelStart - secondLevelStart) / 4];
            toImport.Seek(thirdLevelStart, SeekOrigin.Begin);
            int firstString = importer.ReadInt32();
            int[] thirdLevel = new int[(firstString - thirdLevelStart) / 4];
            toImport.Seek(4, SeekOrigin.Begin);
            while (toImport.Position < secondLevelStart)
            {
                int currPos = (int)toImport.Position;        //Juuuuuust in case I'm forgetting my order of operations.
                firstLevel[(currPos - 4) / 4] = (importer.ReadInt32() - secondLevelStart) / 4;
            }
            while (toImport.Position < thirdLevelStart)
            {
                int currPos = (int)toImport.Position;        //Juuuuuust in case I'm forgetting my order of operations.
                secondLevel[(currPos - secondLevelStart) / 4] = (importer.ReadInt32() - thirdLevelStart) / 4;
            }
            while (toImport.Position < firstString)
            {
                int currPos = (int)toImport.Position;        //Juuuuuust in case I'm forgetting my order of operations.
                thirdLevel[(currPos - thirdLevelStart) / 4] = importer.ReadInt32();
            }
            string[][][] tempStrings = new string[firstLevel.Length][][];
            for (int i = 0; i < firstLevel.Length; i++)
            {
                if(i < firstLevel.Length - 1)
                    tempStrings[i] = new string[firstLevel[i + 1] - firstLevel[i]][];
                else
                    tempStrings[i] = new string[secondLevel.Length - firstLevel[i]][];
                for (int j = 0; j < tempStrings[i].Length ; j++)
                {
                    if (j < secondLevel.Length - 1)
                        tempStrings[i][j] = new string[secondLevel[j + 1] - secondLevel[j]];
                    else 
                        tempStrings[i][j] = new string[thirdLevel.Length - secondLevel[j]];
                    for (int k = 0; k < tempStrings[i][j].Length; k++)
                    {
                        toImport.Seek(thirdLevel[secondLevel[j] + k], SeekOrigin.Begin);
                        int length = 0;
                        if (secondLevel[j] + k + 1 < thirdLevel.Length)
                            length = thirdLevel[secondLevel[j] + k + 1] - thirdLevel[secondLevel[j] + k];
                        else
                            length = (int)toImport.Length - thirdLevel[secondLevel[j] + k];
                        tempStrings[i][j][k] = new string(ASCIIEncoding.Unicode.GetChars(importer.ReadBytes(length - 2)));
                    }
                }
            }
            int shift = 0;
            for (int i = 0; i < stringArray.Length && i < tempStrings.Length; i++)
            {
                if (stringArray[i] == null)
                    stringArray[i] = tempStrings[i];
                else if (stringArray[i].Length < tempStrings[i].Length)
                    Array.Resize(ref stringArray[i], tempStrings[i].Length);
                for (int j = 0; j + shift < stringArray[i].Length && j < tempStrings[i].Length; j++)
                {
                    if (stringArray[i].Length == 0x31 && tempStrings[i].Length == 0x26)
                    {
                        if (j > 0x20)
                            shift = 0xB;
                        else if (j > 0x17)
                            shift = 2;
                    }
                    if (stringArray[i][j + shift] == null)
                        stringArray[i][j + shift] = new List<string>(tempStrings[i][j]);
                    else if (stringArray[i][j + shift].Count < tempStrings[i][j].Length)
                        Array.Resize(ref stringArray[i][j + shift], tempStrings[i][j].Length);
                    for (int k = 0; k < stringArray[i][j + shift].Count && k < tempStrings[i][j].Length; k++)
                    {
                        if (stringArray[i][j + shift][k] == null || stringArray[i][j + shift][k] == "")
                            stringArray[i][j + shift][k] = tempStrings[i][j][k];
                    }
                }
            }
            importer.Close();

        }*/

        public void saveToTextFile(Stream outputStream)
        {
            StreamWriter outWriter = new StreamWriter(outputStream);
            for (int i = 0; i < stringArray.Length; i++)
            {
                for (int j = 0; j < stringArray[i].Length; j++)
                {
                    for (int k = 0; k < stringArray[i][j].Count; k++)
                    {
                        if(stringArray.Length > 1)
                            outWriter.Write(i + ":");
                        outWriter.WriteLine(j.ToString("X2") + ":" + k.ToString("X2") + ": " + stringArray[i][j][k]);
                        outWriter.WriteLine();
                    }
                    outWriter.WriteLine();
                }
            }
            outWriter.Close();
        }
    }
}
