using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace psu_generic_parser
{
    public class WeaponParamFile : PsuFile
    {
        public class Weapon
        {
            public bool IsValid { get; set; }
            public byte AttackName { get; set; }
            public byte ShootEffect { get; set; }
            public byte WeaponModel { get; set; }
            public byte SelectedRange { get; set; }
            public short MinATP { get; set; }
            public short MaxATP { get; set; }
            public short Ata { get; set; }
            public byte StatusEffect { get; set; }
            public byte StatusLevel { get; set; }
            public byte MaxTargets { get; set; }
            public byte AvailableRaces { get; set; }
            public short ReqATP { get; set; }
            public byte Rank { get; set; }
            public byte DropElement { get; set; }
            public byte DropPercent { get; set; }
            public byte SoundEffect { get; set; }
            public byte SetID { get; set; }
            public short BasePP { get; set; }
            public short RegenPP { get; set; }
            public short AtpMod { get; set; }
            public short DfpMod { get; set; }
            public short AtaMod { get; set; }
            public short EvpMod { get; set; }
            public short StaMod { get; set; }
            public short TpMod { get; set; }
            public short MstMod { get; set; }
            public byte[] grindBoostAtp;// = new byte[5];
            public byte[] grindBoostPp;// = new byte[5];
        }

        public class EquipPacket
        {
            private short value1;

            public short Value1
            {
                get { return value1; }
                set { value1 = value; }
            }
            short value2;

            public short Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
            short value3;

            public short Value3
            {
                get { return value3; }
                set { value3 = value; }
            }
            short value4;

            public short Value4
            {
                get { return value4; }
                set { value4 = value; }
            }
        }

        public Weapon[][] parsedWeapons;
        //public EquipPacket[] parsedPackets;
        public System.ComponentModel.BindingList<EquipPacket> parsedPackets = new System.ComponentModel.BindingList<EquipPacket>();
        //public ArrayList parsedPackets = new ArrayList();
        public int weaponClassValue; //Used online. NOT SURE WHAT FOR, BUT KEEP VALUE
        public byte[] equipPacket; //If valid, keep! If not, discard!

        public byte packetCount;
        public byte weaponType; //Melee, range, tech
        public short equipStat; //Which bit of data to show for weapon (i.e "req ATA show TP/ATA" or whatever)
        public int dataType = 0; //0 = AotI, 1 = v1, 2 = enhanced.

        bool isPsp1 = false; //Hack, but works.

        public WeaponParamFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            header = subHeader;
            filename = inFilename;
            byte[] tempData = new byte[rawData.Length];
            Array.Copy(rawData, tempData, rawData.Length);
            calculatedPointers = new int[ptrs.Length]; //It may be populated now, but NOT FINAL UNTIL IT'S FINAL.
            for (int i = 0; i < ptrs.Length; i++)
            {
                calculatedPointers[i] = ptrs[i] - baseAddr;
                int ptrLoc = BitConverter.ToInt32(rawData, calculatedPointers[i]);
                byte[] temp = BitConverter.GetBytes(ptrLoc - baseAddr);
                Array.Copy(temp, 0, tempData, calculatedPointers[i], temp.Length);
            }
            int numEntries = 14; //Max number of weapons per manufacturer.
            MemoryStream rawStream = new MemoryStream(tempData);
            BinaryReader rawReader = new BinaryReader(rawStream);
            rawStream.Seek(4, SeekOrigin.Begin);
            int fileLength = rawReader.ReadInt32(); //Use to check dummy pointers? The file can happily go over!
            int headerLoc = rawReader.ReadInt32();
            rawStream.Seek(0x14, SeekOrigin.Begin);
            if (rawReader.ReadInt32() == 0x61726761)
            {
                dataType = 2;
                numEntries = 80;
            }

            rawStream.Seek(headerLoc, SeekOrigin.Begin);
            int equipPacketLoc = rawReader.ReadInt32();
            weaponClassValue = rawReader.ReadInt32();
            int[] dataLocs = new int[4];
            int lowestDataLoc = 0xFFFFFF;
            int lowestDataIndex = -1;
            int secondDataLoc = headerLoc;
            int secondDataIndex = -1;
            for (int i = 0; i < 4; i++)
            {
                dataLocs[i] = rawReader.ReadInt32();
                if (dataLocs[i] != 0 && dataLocs[i] < lowestDataLoc)
                {
                    lowestDataLoc = dataLocs[i];
                    lowestDataIndex = i;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (dataLocs[i] != 0 && dataLocs[i] != lowestDataLoc && dataLocs[i] < secondDataLoc)
                {
                    secondDataLoc = dataLocs[i];
                    secondDataIndex = i;
                }
            }
            byte[][] validWeapons = new byte[4][];
            if (dataType == 2)
            {
                packetCount = rawReader.ReadByte();
                weaponType = rawReader.ReadByte();
                equipStat = rawReader.ReadInt16();
            }

            int[] weaponCounts = new int[4];

            int indexLoc = (int)rawStream.Position;
            byte[] firstFour = rawReader.ReadBytes(4);
            byte expected = 0;
            for (int i = 0; i < 4; i++)
            {
                if (firstFour[i] != 0xFF && firstFour[i] != expected)
                {
                    isPsp1 = true;
                    packetCount = rawReader.ReadByte();
                    weaponType = rawReader.ReadByte();
                    equipStat = rawReader.ReadInt16();
                    break;
                }
                else if (firstFour[i] == expected)
                    expected++;
            }

            if (!isPsp1)
            {
                rawStream.Seek(indexLoc, SeekOrigin.Begin);
                for (int i = 0; i < 4; i++)
                {
                    validWeapons[i] = rawReader.ReadBytes(numEntries);
                    for (int j = 0; j < validWeapons[i].Length; j++)
                    {
                        if (validWeapons[i][j] != 0xFF)
                            weaponCounts[i]++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (dataLocs[i] != 0)
                    {
                        bool found = false;
                        for (int j = i + 1; j < 4; j++)
                        {
                            if (dataLocs[j] != 0)
                            {
                                weaponCounts[i] = (dataLocs[j] - dataLocs[i]) / 38;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            weaponCounts[i] = (headerLoc - dataLocs[i]) / 38;
                        }
                    }
                }
            }

            if (dataType != 2 && !isPsp1)
            {
                packetCount = rawReader.ReadByte();
                weaponType = rawReader.ReadByte();
                equipStat = rawReader.ReadInt16();
            }
            if (!isPsp1 && (lowestDataIndex == -1 || weaponCounts[lowestDataIndex] * 0x40 <= secondDataLoc - lowestDataLoc))
            {
                dataType = 1;
                throw new Exception("v1 weapons file not supported yet");
                //entry
            }
            if (equipPacketLoc < lowestDataLoc && packetCount > 0)
            {
                rawStream.Seek(equipPacketLoc, SeekOrigin.Begin);
                equipPacket = rawReader.ReadBytes(packetCount * 8);
                //parsedPackets = new EquipPacket[packetCount];
                for (int i = 0; i < packetCount; i++)
                {
                    EquipPacket tempPacket = new EquipPacket();
                    tempPacket.Value1 = BitConverter.ToInt16(equipPacket, i * 8 + 0);
                    tempPacket.Value2 = BitConverter.ToInt16(equipPacket, i * 8 + 2);
                    tempPacket.Value3 = BitConverter.ToInt16(equipPacket, i * 8 + 4);
                    tempPacket.Value4 = BitConverter.ToInt16(equipPacket, i * 8 + 6);
                    parsedPackets.Add(tempPacket);
                }
            }
            parsedWeapons = new Weapon[4][];
            for (int j = 0; j < 4; j++)
            {
                if (isPsp1 && dataLocs[j] != 0)
                    parsedWeapons[j] = new Weapon[weaponCounts[j] - ((sbyte)firstFour[j])];
                else if (isPsp1)
                    parsedWeapons[j] = new Weapon[0];
                else
                    parsedWeapons[j] = new Weapon[numEntries];
                if (dataLocs[j] != 0)
                {
                    for (int i = 0; i < parsedWeapons[j].Length; i++)
                    {
                        parsedWeapons[j][i] = new Weapon();
                        parsedWeapons[j][i].IsValid = false;
                        if ((isPsp1 && (i + ((sbyte)firstFour[j]) >= 0)) || (!isPsp1 && validWeapons[j][i] != 0xFF))
                        {
                            if(isPsp1)
                                rawStream.Seek(dataLocs[j] + (i + ((sbyte)firstFour[j])) * 38, SeekOrigin.Begin);
                            else
                                rawStream.Seek(dataLocs[j] + validWeapons[j][i] * 38, SeekOrigin.Begin);
                            parsedWeapons[j][i].IsValid = true;
                            parsedWeapons[j][i].AttackName = rawReader.ReadByte();
                            parsedWeapons[j][i].ShootEffect = rawReader.ReadByte();
                            parsedWeapons[j][i].WeaponModel = rawReader.ReadByte();
                            parsedWeapons[j][i].SelectedRange = rawReader.ReadByte();
                            parsedWeapons[j][i].MinATP = rawReader.ReadInt16();
                            parsedWeapons[j][i].MaxATP = rawReader.ReadInt16();
                            parsedWeapons[j][i].Ata = rawReader.ReadInt16();
                            parsedWeapons[j][i].StatusEffect = rawReader.ReadByte();
                            parsedWeapons[j][i].StatusLevel = rawReader.ReadByte();
                            parsedWeapons[j][i].MaxTargets = rawReader.ReadByte();
                            parsedWeapons[j][i].AvailableRaces = rawReader.ReadByte();
                            parsedWeapons[j][i].ReqATP = rawReader.ReadInt16();
                            byte rankEle = rawReader.ReadByte();
                            parsedWeapons[j][i].Rank = (byte)(rankEle & 0xF);
                            parsedWeapons[j][i].DropElement = (byte)(rankEle >> 4);
                            parsedWeapons[j][i].DropPercent = rawReader.ReadByte();
                            parsedWeapons[j][i].SoundEffect = rawReader.ReadByte();
                            parsedWeapons[j][i].SetID = rawReader.ReadByte();
                            parsedWeapons[j][i].BasePP = rawReader.ReadInt16();
                            short tempPP = rawReader.ReadInt16();
                            if (tempPP != 0)
                                parsedWeapons[j][i].RegenPP = (short)((5 * parsedWeapons[j][i].BasePP) / tempPP);
                            else
                                parsedWeapons[j][i].RegenPP = 0;
                            parsedWeapons[j][i].AtpMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].DfpMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].AtaMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].EvpMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].StaMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].TpMod = rawReader.ReadInt16();
                            parsedWeapons[j][i].MstMod = rawReader.ReadInt16();
                            if (isPsp1)
                            {
                                parsedWeapons[j][i].MaxTargets = (byte)(parsedWeapons[j][i].StatusLevel & 0xF);
                                if((parsedWeapons[j][i].AvailableRaces & 0x40) != 0)
                                    parsedWeapons[j][i].DropElement |= 0x8;
                                parsedWeapons[j][i].AvailableRaces &= 0x3F;
                                parsedWeapons[j][i].StatusLevel >>= 4;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < parsedWeapons[j].Length; i++)
                    {
                        parsedWeapons[j][i] = new Weapon();
                    }
                }
            }

        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(Encoding.ASCII.GetBytes("NXR\0"));
            outWriter.Write((int)0);    //Size to be.
            outWriter.Write((int)0);    //Header location.
            outWriter.Write((int)0);    //I don't even know if this stuff does anything...
            outWriter.Write((int)1);
            if (dataType != 2)
                outWriter.Write((int)-1);
            else
                outWriter.Write((int)0x61726761);
            outWriter.Write((int)-1);
            int equipPacketLoc = -1;
            if (equipPacket != null)
            {
                equipPacketLoc = (int)outStream.Position;
                for (int i = 0; i < parsedPackets.Count; i++)
                {
                    outWriter.Write(((EquipPacket)parsedPackets[i]).Value1);
                    outWriter.Write(((EquipPacket)parsedPackets[i]).Value2);
                    outWriter.Write(((EquipPacket)parsedPackets[i]).Value3);
                    outWriter.Write(((EquipPacket)parsedPackets[i]).Value4);
                }
                //outWriter.Write(equipPacket);
            }
            int numEntries = 14;
            if (dataType == 2)
                numEntries = 80;
            packetCount = (byte)parsedPackets.Count;
            int[] dataLocs = new int[4];
            byte[] validWeapons = new byte[4 * numEntries];
            int numPointers = 1; //1 pointer for equip packet!
            for (int i = 0; i < 4; i++)
            {
                dataLocs[i] = -1;
                byte currWeapon = 0;
                for (int j = 0; j < numEntries; j++)
                {
                    if (parsedWeapons[i][j].IsValid)
                    {
                        if (currWeapon == 0)
                        {
                            dataLocs[i] = (int)outStream.Position;
                            numPointers++;
                        }
                        validWeapons[i * numEntries + j] = currWeapon++;
                        outWriter.Write(parsedWeapons[i][j].AttackName);
                        outWriter.Write(parsedWeapons[i][j].ShootEffect);
                        outWriter.Write(parsedWeapons[i][j].WeaponModel);
                        outWriter.Write(parsedWeapons[i][j].SelectedRange);
                        outWriter.Write(parsedWeapons[i][j].MinATP);
                        outWriter.Write(parsedWeapons[i][j].MaxATP);
                        outWriter.Write(parsedWeapons[i][j].Ata);
                        outWriter.Write(parsedWeapons[i][j].StatusEffect);
                        outWriter.Write(parsedWeapons[i][j].StatusLevel);
                        outWriter.Write(parsedWeapons[i][j].MaxTargets);
                        outWriter.Write(parsedWeapons[i][j].AvailableRaces);
                        outWriter.Write(parsedWeapons[i][j].ReqATP);
                        byte rankElement = (byte)((parsedWeapons[i][j].DropElement << 4) | parsedWeapons[i][j].Rank);
                        outWriter.Write(rankElement);
                        outWriter.Write(parsedWeapons[i][j].DropPercent);
                        outWriter.Write(parsedWeapons[i][j].SoundEffect);
                        outWriter.Write(parsedWeapons[i][j].SetID);
                        outWriter.Write(parsedWeapons[i][j].BasePP);
                        short ppMod = 0;
                        if(parsedWeapons[i][j].RegenPP != 0)
                            ppMod = (short)((parsedWeapons[i][j].BasePP * 5) / parsedWeapons[i][j].RegenPP);
                        outWriter.Write(ppMod);
                        outWriter.Write(parsedWeapons[i][j].AtpMod);
                        outWriter.Write(parsedWeapons[i][j].DfpMod);
                        outWriter.Write(parsedWeapons[i][j].AtaMod);
                        outWriter.Write(parsedWeapons[i][j].EvpMod);
                        outWriter.Write(parsedWeapons[i][j].StaMod);
                        outWriter.Write(parsedWeapons[i][j].TpMod);
                        outWriter.Write(parsedWeapons[i][j].MstMod);
                    }
                    else
                        validWeapons[i * numEntries + j] = 0xFF;
                }
            }
            if(outStream.Position % 4 != 0)
                outStream.Seek(outStream.Position % 4, SeekOrigin.Current);
            int headerLoc = (int)outStream.Position;
            if (equipPacketLoc == -1)
            {
                equipPacketLoc = headerLoc + 0x54;
            }
            calculatedPointers = new int[numPointers];  //This is where they start showing up!
            calculatedPointers[0] = (int)outStream.Position;
            int currentPointerIndex = 1;
            outWriter.Write(equipPacketLoc);
            outWriter.Write(weaponClassValue);
            for (int i = 0; i < 4; i++)
            {
                if (dataLocs[i] != -1)
                {
                    calculatedPointers[currentPointerIndex++] = (int)outStream.Position;
                    outWriter.Write(dataLocs[i]);
                }
                else
                    outWriter.Write((int)0);
            }
            if (dataType == 2)
            {
                outWriter.Write(Math.Max(packetCount, (byte)1));
                outWriter.Write(weaponType);
                outWriter.Write(equipStat);
                outWriter.Write(validWeapons);
            }
            else
            {
                outWriter.Write(validWeapons);
                outWriter.Write(Math.Max(packetCount, (byte)1));
                outWriter.Write(weaponType);
                outWriter.Write(equipStat);
            }
            int padSize = (int)outStream.Position;//(int)((outStream.Position + 0xF) & 0xFFFFFFF0);
            int normSize = (int)outStream.Position;
            if ((padSize & 0xF) == 8)
            {
                padSize = (int)((outStream.Position + 0xF) & 0xFFFFFFF0);
            }
            else if((padSize & 0xF) > 8)
            {
                padSize = (int)((outStream.Position + 0x1F) & 0xFFFFFFF0);
                normSize = (int)((outStream.Position + 0xF) & 0xFFFFFFF0);
            }
            else if((padSize & 0xF) > 0)
            {
                padSize = (int)((outStream.Position + 0xF) & 0xFFFFFFF0);
                normSize = (int)((outStream.Position) & 0xFFFFFFF0);
            }
            else
            {
                normSize = (int)(outStream.Position - 8);
            }
            outWriter.Write(new byte[padSize - outStream.Position]);
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(normSize);
            outWriter.Write(headerLoc);
            byte[] toReturn = outStream.ToArray();
            outWriter.Close();
            return toReturn;
        }

        public void expandFile()
        {
            if (dataType != 0)
            {
                return;
            }
            dataType = 2;
            for (int i = 0; i < 4; i++)
            {
                Array.Resize(ref parsedWeapons[i], 80);
                for (int j = 14; j < 80; j++)
                {
                    parsedWeapons[i][j] = new Weapon();
                    parsedWeapons[i][j].IsValid = false;
                }
            }
        }

        public void saveTextFile(Stream outFile)
        {
            StreamWriter textOut = new StreamWriter(outFile);
            textOut.WriteLine("//mf:wn\tattname\teffect\tmodel\trange\tminATP\tmaxATP\tATA\tSE\tSE lv\ttargets\traces\treq\trank\telement\tpercent\tsound\tset\tPP\tregen\tATPmod\tDFPmod\tATAmod\tEVPmod\tunk 1\tunk2\tMSTmod");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < parsedWeapons[i].Length; j++)
                {
                    textOut.Write(i + ":" + j.ToString("X2") + "\t");
                    textOut.Write(parsedWeapons[i][j].AttackName + "\t");
                    textOut.Write(parsedWeapons[i][j].ShootEffect + "\t");
                    textOut.Write(parsedWeapons[i][j].WeaponModel + "\t");
                    textOut.Write(parsedWeapons[i][j].SelectedRange + "\t");
                    textOut.Write(parsedWeapons[i][j].MinATP + "\t");
                    textOut.Write(parsedWeapons[i][j].MaxATP + "\t");
                    textOut.Write(parsedWeapons[i][j].Ata + "\t");
                    textOut.Write(parsedWeapons[i][j].StatusEffect + "\t");
                    textOut.Write(parsedWeapons[i][j].StatusLevel + "\t");
                    textOut.Write(parsedWeapons[i][j].MaxTargets + "\t");
                    textOut.Write(parsedWeapons[i][j].AvailableRaces + "\t");
                    textOut.Write(parsedWeapons[i][j].ReqATP + "\t");
                    textOut.Write(parsedWeapons[i][j].Rank + "\t");
                    textOut.Write(parsedWeapons[i][j].DropElement + "\t");
                    textOut.Write(parsedWeapons[i][j].DropPercent + "\t");
                    textOut.Write(parsedWeapons[i][j].SoundEffect + "\t");
                    textOut.Write(parsedWeapons[i][j].SetID + "\t");
                    textOut.Write(parsedWeapons[i][j].BasePP + "\t");
                    textOut.Write(parsedWeapons[i][j].RegenPP + "\t");
                    textOut.Write(parsedWeapons[i][j].AtpMod + "\t");
                    textOut.Write(parsedWeapons[i][j].DfpMod + "\t");
                    textOut.Write(parsedWeapons[i][j].AtaMod + "\t");
                    textOut.Write(parsedWeapons[i][j].EvpMod + "\t");
                    textOut.Write(parsedWeapons[i][j].StaMod + "\t");
                    textOut.Write(parsedWeapons[i][j].TpMod + "\t");
                    textOut.WriteLine(parsedWeapons[i][j].MstMod);
                }
                textOut.WriteLine();
            }
            textOut.WriteLine("//'R'\tIndex\tValue 1\tValue 2\tValue 3\tValue 4");
            for (int i = 0; i < parsedPackets.Count; i++)
            {
                textOut.WriteLine("R\t" + i + "\t" + parsedPackets[i].Value1 + "\t" + parsedPackets[i].Value2 + "\t" + parsedPackets[i].Value3 + "\t" + parsedPackets[i].Value4);
            }
            textOut.Close();
        }

        public void loadTextFile(Stream inFile)
        {
            StreamReader fileReader = new StreamReader(inFile);
            while (!fileReader.EndOfStream)
            {
                string currLine = fileReader.ReadLine();
                string croppedLine = currLine.Trim();
                if (!currLine.StartsWith("//") && croppedLine != "")
                {
                    string[] splitLine = currLine.Split('\t');
                    if (splitLine[0] == "R")    //Range
                    {
                        int index = Convert.ToInt32(splitLine[1]);
                        short val1 = Convert.ToInt16(splitLine[2]);
                        short val2 = Convert.ToInt16(splitLine[3]);
                        short val3 = Convert.ToInt16(splitLine[4]);
                        short val4 = Convert.ToInt16(splitLine[5]);
                        EquipPacket temp = new EquipPacket();
                        temp.Value1 = val1;
                        temp.Value2 = val2;
                        temp.Value3 = val3;
                        temp.Value4 = val4;
                        if (index >= parsedPackets.Count)
                            parsedPackets.Add(temp);
                        else
                            parsedPackets[index] = temp;
                    }
                    else
                    {
                        string[] indices = splitLine[0].Split(':');
                        int manufacture = Convert.ToByte(indices[0]);
                        int index = Convert.ToByte(indices[1], 16);
                        parsedWeapons[manufacture][index].IsValid = false;
                        for (int j = 1; j < splitLine.Length; j++)
                        {
                            if (splitLine[j] != "0")
                                parsedWeapons[manufacture][index].IsValid = true;
                        }

                        parsedWeapons[manufacture][index].AttackName = Convert.ToByte(splitLine[1]);
                        parsedWeapons[manufacture][index].ShootEffect = Convert.ToByte(splitLine[2]);
                        parsedWeapons[manufacture][index].WeaponModel = Convert.ToByte(splitLine[3]);
                        parsedWeapons[manufacture][index].SelectedRange = Convert.ToByte(splitLine[4]);
                        parsedWeapons[manufacture][index].MinATP = Convert.ToInt16(splitLine[5]);
                        parsedWeapons[manufacture][index].MaxATP = Convert.ToInt16(splitLine[6]);
                        parsedWeapons[manufacture][index].Ata = Convert.ToInt16(splitLine[7]);
                        parsedWeapons[manufacture][index].StatusEffect = Convert.ToByte(splitLine[8]);
                        parsedWeapons[manufacture][index].StatusLevel = Convert.ToByte(splitLine[9]);
                        parsedWeapons[manufacture][index].MaxTargets = Convert.ToByte(splitLine[10]);
                        parsedWeapons[manufacture][index].AvailableRaces = Convert.ToByte(splitLine[11]);
                        parsedWeapons[manufacture][index].ReqATP = Convert.ToInt16(splitLine[12]);
                        parsedWeapons[manufacture][index].Rank = Convert.ToByte(splitLine[13]);
                        parsedWeapons[manufacture][index].DropElement = Convert.ToByte(splitLine[14]);
                        parsedWeapons[manufacture][index].DropPercent = Convert.ToByte(splitLine[15]);
                        parsedWeapons[manufacture][index].SoundEffect = Convert.ToByte(splitLine[16]);
                        parsedWeapons[manufacture][index].SetID = Convert.ToByte(splitLine[17]);
                        parsedWeapons[manufacture][index].BasePP = Convert.ToInt16(splitLine[18]);
                        parsedWeapons[manufacture][index].RegenPP = Convert.ToInt16(splitLine[19]);
                        parsedWeapons[manufacture][index].AtpMod = Convert.ToInt16(splitLine[20]);
                        parsedWeapons[manufacture][index].DfpMod = Convert.ToInt16(splitLine[21]);
                        parsedWeapons[manufacture][index].AtaMod = Convert.ToInt16(splitLine[22]);
                        parsedWeapons[manufacture][index].EvpMod = Convert.ToInt16(splitLine[23]);
                        parsedWeapons[manufacture][index].StaMod = Convert.ToInt16(splitLine[24]);
                        parsedWeapons[manufacture][index].TpMod = Convert.ToInt16(splitLine[25]);
                        parsedWeapons[manufacture][index].MstMod = Convert.ToInt16(splitLine[26]);
                    }
                }
            }
        }
    }
}
