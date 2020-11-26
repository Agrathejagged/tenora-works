using System;
using System.Collections.Generic;
using System.IO;

namespace psu_generic_parser
{
    public class NomFile : PsuFile
    {
        public class NomFrame 
        {
            public byte frame;
            public byte type;
            public byte type2;
            public List<float> data = new List<float>();

            public override string ToString()
            {
                string item = $"Frame {frame.ToString("D3")} ({type}{type2}):";
                //Add the float values to the string
                for (int j = 0; j < data.Count; j++)
                {
                    string temp = data[j].ToString();
                    item += " " + temp;

                    if (j != data.Count - 1)
                    {
                        item += ",";
                    }
                }
                return item;
            }
        }

        private byte[] fileContents;

        public List<List<NomFrame>> rotationFrameList = new List<List<NomFrame>>();
        public List<List<NomFrame>> positionFrameList = new List<List<NomFrame>>();
        public List<List<NomFrame>> list3FrameList = new List<List<NomFrame>>();
        public List<List<NomFrame>> list4FrameList = new List<List<NomFrame>>();
        public ushort frameCount;
        public float frameRate;

        public string[] boneNames = new string[] 
        {
            "Root",
            "Navel",
            "Pelvis",
            "L_thigh",
            "L_calf",
            "L_foot",
            "R_thigh",
            "R_calf",
            "R_foot",
            "Spine",
            "Spine1",
            "Neck_root",
            "Neck",
            "Head",
            "L_clavicle",
            "L_upperarm",
            "L_forearm",
            "L_hand",
            "L_weapon",
            "R_clavicle",
            "R_upperarm",
            "R_forearm",
            "R_hand",
            "R_weapon",
            "L_breast",
            "R_breast",
            "Belly",
            "Body"
        };

        //Takes the file, splits it based upon which pointer accesses it.
        //Also modifies all pointers to be 0-based!
        public NomFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            List<int> rotationOffsets = new List<int>();
            List<int> positionOffsets = new List<int>();
            List<int> list3Offsets = new List<int>();
            List<int> list4Offsets = new List<int>();

            header = subHeader;
            filename = inFilename;
            fileContents = rawData;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);

            //Skip some meta data looking bytes for now since we don't understand them.
            inStream.Seek(0x6, SeekOrigin.Begin);
            frameCount = inReader.ReadUInt16();
            frameRate = inReader.ReadSingle();

            //Skip past initial pointer data since it's redundant for our purposes
            inStream.Seek(0x40, SeekOrigin.Begin);

            //Populate offset lists. These should always have a set length and a set amount. All observed so far have anyways
            for (int i = 0; i < 28; i++) { rotationOffsets.Add(inReader.ReadInt32()); }
            for (int i = 0; i < 28; i++) { positionOffsets.Add(inReader.ReadInt32()); }
            for (int i = 0; i < 28; i++) { list3Offsets.Add(inReader.ReadInt32()); }
            for (int i = 0; i < 28; i++) { list4Offsets.Add(inReader.ReadInt32()); }

            //Populate the actual frame data lists

            //Read Rotation frame list
            ReadNomList(rotationOffsets, rotationFrameList, inReader, true);
            ReadNomList(positionOffsets, positionFrameList, inReader, false);
            ReadNomList(list3Offsets, list3FrameList, inReader, false);
            ReadNomList(list4Offsets, list4FrameList, inReader, false);
        }

        private void ReadNomList(List<int> frameOffsets, List<List<NomFrame>> frameList, BinaryReader inReader, bool isRotList = false)
        {
            for (int i = 0; i < frameOffsets.Count; i++)
            {
                if (frameOffsets[i] != 0)
                {
                    inReader.BaseStream.Seek(frameOffsets[i], SeekOrigin.Begin);
                    bool continueLoop = true;
                    int sanityCheck = 257; //Really shouldn't trigger, but in the case something is broken it's there.
                    List<NomFrame> frameValues = new List<NomFrame>();

                    //Read frames for the node until there aren't any
                    while (continueLoop)
                    {
                        NomFrame nomFrame = new NomFrame();
                        nomFrame.frame = inReader.ReadByte();
                        nomFrame.type = inReader.ReadByte();
                        nomFrame.type2 = (byte)(nomFrame.type % 0x10);
                        nomFrame.type /= 0x10;
                        byte examinedType = nomFrame.type;

                        //Check if we should exit
                        if (nomFrame.frame == frameCount)
                        {
                            if(isRotList && nomFrame.type2 != 0x8)
                            {
                                Console.WriteLine("Unexpected rot frame set end...");
                            } else if(!isRotList)
                            {
                                examinedType -= 0x2;
                            }
                            continueLoop = false;
                        }

                        //Handle different key types. Rotations and other data types handle this differently.
                        int typeCount = 0;
                        switch (examinedType)
                        {
                            case 0x0:
                            case 0x1:
                            case 0x2:
                                if (isRotList)
                                {
                                    typeCount = 0x4;
                                } else
                                {
                                    typeCount = 0x1;
                                }
                                break;
                            case 0x4:
                                typeCount = 0x3;
                                break;
                            case 0x5:
                            case 0x6:
                            case 0x7:
                                typeCount = 0x2;
                                break;
                            case 0x8:
                            case 0x9:
                            case 0xA:
                                break;
                            default:
                                Console.WriteLine("Unknown type " + examinedType + " detected at " + inReader.BaseStream.Position.ToString("X") + " in iteration " + i);
                                break;
                        }

                        //Read and store data
                        for (int j = 0; j < typeCount; j++)
                        {
                            nomFrame.data.Add(convertValue(inReader.ReadInt16(), isRotList));
                        }
                        frameValues.Add(nomFrame);

                        sanityCheck--;
                        if (sanityCheck < 0) { continueLoop = false; }
                    }

                    //Don't add it if it's garbage
                    if (sanityCheck >= 0)
                    {
                        frameList.Add(frameValues);
                    }
                    else
                    {
                        Console.WriteLine($"Bad frame count. Check node {i}, data address {frameOffsets[i].ToString("X")} in {filename} for more info.");
                        frameList.Add(null);
                    }
                }
                else
                {
                    frameList.Add(null);
                }
            }
        }

        //Deobfuscates animation frame data
        private float convertValue(short initialValue, bool isRotValue = false)
        {
            //This value is different for rotation frames
            int finalAddition = 0x37800000;
            if (isRotValue)
            {
                finalAddition = 0x30000000;
            }    

            int signum = Math.Sign(initialValue);
            int shifted = initialValue << 13;
            int initialValue1 = shifted & 0x0F800000;
            
            //Exit early if 0
            if (initialValue1 == 0)
            {
                return 0.0f;
            }

            int value2 = shifted & 0x0069C000;
            int finalValue1 = initialValue1 + finalAddition;
            int finalFloat = signum * (finalValue1 | value2);
            float result = BitConverter.ToSingle(BitConverter.GetBytes(finalFloat), 0);

            return result;
        }

        //This file's not editable yet, so just return the file contents.
        public override byte[] ToRaw()
        {
            return fileContents;
        }
    }
}