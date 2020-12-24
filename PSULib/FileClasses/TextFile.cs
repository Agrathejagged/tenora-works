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
        public List<List<List<string>>> strings = new List<List<List<string>>>(1);

        public TextFile()
        {
            filename = "DUMMY";
            strings.Add(new List<List<string>>());
        }

        public TextFile(string filename)
        {
            this.filename = filename; 
            strings.Add(new List<List<string>>());
        }

        public TextFile(string inFilename, byte[] rawData)
        {
            filename = inFilename;
            MemoryStream rawStream = new MemoryStream(rawData);
            BinaryReader rawReader = new BinaryReader(rawStream);
            int fileLength = rawReader.ReadInt32();
            int lowestSecondLevel = fileLength;
            List<int> firstLevelAddrs = new List<int>();
            while (rawStream.Position < lowestSecondLevel)
            {
                int currValue = rawReader.ReadInt32();
                firstLevelAddrs.Add(currValue);
                if (currValue < lowestSecondLevel)
                    lowestSecondLevel = currValue;
            }
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
                strings.Add(new List<List<string>>(secondLevelAddrs[i].Count));
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
                    strings[i].Add(new List<string>(thirdLevelAddrs[i][j].Count));
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
                        strings[i][j].Add(toStore);
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
            int initialSecond = initialFirst + 4 * strings.Count;
            int initialThird = initialSecond;
            int initialFourth = 0;
            for(int i = 0; i < strings.Count; i++)
            {
                initialThird += 4 * strings[i].Count;
                for(int j = 0; j < strings[i].Count; j++)
                    initialFourth += 4 * strings[i][j].Count;
            }
            initialFourth += initialThird;
            int[][][] pointers = new int[strings.Count][][];
            rawStream.Seek(initialFourth, SeekOrigin.Begin);
            for (int i = 0; i < strings.Count; i++)
            {
                pointers[i] = new int[strings[i].Count][];
                for (int j = 0; j < strings[i].Count; j++)
                {
                    pointers[i][j] = new int[strings[i][j].Count];
                    for (int k = 0; k < strings[i][j].Count; k++)
                    {
                        MatchCollection tempMatches = filestring.Matches(strings[i][j][k]);
                        pointers[i][j][k] = (int)rawStream.Position;
                        if (strings[i][j][k].Contains("<"))
                            strings[i][j][k] = strings[i][j][k];
                        int startPos = 0;
                        foreach (Match match in tempMatches)
                        {
                            byte[] matchBytes = new byte[match.Value.Length / 2 - 1];
                            for (int m = 0; m < matchBytes.Length; m++)
                            {
                                matchBytes[m] = byte.Parse(match.Value.Substring(2 * m + 1, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                            }
                            if (match.Index > 0)
                                rawWriter.Write(Encoding.Unicode.GetBytes(strings[i][j][k].Substring(startPos, match.Index - startPos).Replace("\\n", "\n").Replace("\r\n", "\n")));
                            rawWriter.Write(matchBytes);
                            startPos = match.Index + match.Length;
                        }
                        rawWriter.Write(Encoding.Unicode.GetBytes(strings[i][j][k].Substring(startPos).Replace("\r\n", "\n") + "\0"));
                    }
                }
            }
            int maxLength = (int)rawStream.Position;
            rawStream.Seek(0, SeekOrigin.Begin);
            rawWriter.Write(maxLength);
            int currentLoc = initialSecond;
            for (int i = 0; i < strings.Count; i++)
            {
                rawWriter.Write(currentLoc);
                currentLoc += 4;
            }
            currentLoc = initialThird;
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Count; j++)
                {
                    rawWriter.Write(currentLoc);
                    currentLoc += strings[i][j].Count * 4;
                }
            }
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Count; j++)
                {
                    for (int k = 0; k < strings[i][j].Count; k++)
                    {
                        rawWriter.Write(pointers[i][j][k]);
                    }
                }
            }
            byte[] toReturn = rawStream.ToArray();
            return toReturn;// rawStream.ToArray();
        }

        public void saveToTextFile(Stream outputStream)
        {
            StreamWriter outWriter = new StreamWriter(outputStream);
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Count; j++)
                {
                    for (int k = 0; k < strings[i][j].Count; k++)
                    {
                        if(strings.Count > 1)
                            outWriter.Write(i + ":");
                        outWriter.WriteLine(j.ToString("X2") + ":" + k.ToString("X2") + ": " + strings[i][j][k]);
                        outWriter.WriteLine();
                    }
                    outWriter.WriteLine();
                }
            }
            outWriter.Close();
        }
    }
}
