using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using PSULib.FileClasses.General;
using PSULib.Support;

namespace PSULib.FileClasses.Missions
{
    [DataContract]
    public class SetFile : PsuFile
    {

        [DataMember]
        public short areaID;
        [DataMember]
        public MapListing[] mapData;

        [DataContract]
        public class ObjectEntry
        {
            [DataMember]
            public int headerInt1;
            [DataMember]
            public int headerInt2;
            [DataMember]
            public int headerInt3;
            [DataMember]
            public short headerShort1;
            [DataMember]
            public short objID;
            [DataMember]
            public int unkInt1;
            [DataMember]
            public float objX;
            [DataMember]
            public float objY;
            [DataMember]
            public float objZ;
            [DataMember]
            public float objRotX;
            [DataMember]
            public float objRotY;
            [DataMember]
            public float objRotZ;
            [DataMember]
            public byte[] metadata;
        }

        [DataContract]
        public class ListHeader
        {
            [DataMember]
            public int unusedInt1 = -1; //Always -1
            [DataMember]
            public float unusedBoundSphereValue1 = 0; //Suspect these are a bound sphere thing. I have seen a value here, it's a float.
            [DataMember]
            public float unusedBoundSphereValue2 = 0; //Suspect these are a bound sphere thing. Gambling that this is a float, I haven't seen one.
            [DataMember]
            public float unusedBoundSphereValue3 = 0; //Suspect these are a bound sphere thing. I have seen a value here, it's a float.
            [DataMember]
            public float unusedBoundSphereValue4 = 0; //Suspect these are a bound sphere thing. I have seen a value here, it's a float.
            [DataMember]
            public short unusedShort1 = 0; //Doesn't appear to be read, may be an editor hint. Only seen 0.
            [DataMember]
            public short unknownShort1 = -1; //This one's important, if it's not -1, the game acts up.
            [DataMember]
            public int unusedInt2 = -1; //Always -1
            [DataMember]
            public short listIndex = 0; //This is the index in the list, but it doesn't appear to actually matter. Just in case, though...
            [DataMember]
            public short unknownPairedShort1 = 0; //This one and the next are read together, but they're generally 0.
            [DataMember]
            public short unknownPairedShort2 = 0; //This one and the previous are read together, but they're generally 0.

            [DataMember]
            public ObjectEntry[] objects;
        }

        [DataContract]
        public class MapListing
        {
            [DataMember]
            public short mapNumber;
            [DataMember]
            public ListHeader[] headers;
        }

        public SetFile(string inFilename, byte[] rawData, byte[] inHeader, int[] ptrs, int baseAddr, bool bigEndian = false)
        {
            header = inHeader;
            filename = inFilename;  //Assuming they'll always base 0.
            MemoryStream transFile = new MemoryStream(rawData);
            BinaryReader fileReader = BigEndianBinaryReader.GetEndianSpecificBinaryReader(transFile, bigEndian);
            transFile.Seek(8, SeekOrigin.Begin);
            int headerLoc = fileReader.ReadInt32();
            transFile.Seek(headerLoc, SeekOrigin.Begin);
            areaID = fileReader.ReadInt16();
            short mapCount = fileReader.ReadInt16();
            int mainListPointer = fileReader.ReadInt32() - baseAddr;
            mapData = new MapListing[mapCount];
            for (int i = 0; i < mapCount; i++)
            {
                transFile.Seek(mainListPointer + i * 12, SeekOrigin.Begin);
                mapData[i] = new MapListing();
                mapData[i].mapNumber = fileReader.ReadInt16();
                short listCount = fileReader.ReadInt16();
                mapData[i].headers = new ListHeader[listCount];
                int listPtr = fileReader.ReadInt32() - baseAddr;
                for (int j = 0; j < listCount; j++)
                {
                    transFile.Seek(listPtr + j * 0x28, SeekOrigin.Begin);
                    mapData[i].headers[j] = new ListHeader();
                    mapData[i].headers[j].unusedInt1 = fileReader.ReadInt32();
                    mapData[i].headers[j].unusedBoundSphereValue1 = fileReader.ReadSingle();
                    mapData[i].headers[j].unusedBoundSphereValue2 = fileReader.ReadSingle();
                    mapData[i].headers[j].unusedBoundSphereValue3 = fileReader.ReadSingle();
                    mapData[i].headers[j].unusedBoundSphereValue4 = fileReader.ReadSingle();
                    mapData[i].headers[j].unusedShort1 = fileReader.ReadInt16();
                    mapData[i].headers[j].unknownShort1 = fileReader.ReadInt16();
                    mapData[i].headers[j].unusedInt2 = fileReader.ReadInt32();
                    mapData[i].headers[j].listIndex = fileReader.ReadInt16();
                    mapData[i].headers[j].unknownPairedShort1 = fileReader.ReadInt16();
                    mapData[i].headers[j].unknownPairedShort2 = fileReader.ReadInt16();
                    short listEntryCount = fileReader.ReadInt16();
                    mapData[i].headers[j].objects = new ObjectEntry[listEntryCount];
                    int objectListLoc = fileReader.ReadInt32() - baseAddr;
                    for (int k = 0; k < listEntryCount; k++)
                    {
                        transFile.Seek(objectListLoc + k * 0x34, SeekOrigin.Begin);
                        ObjectEntry temp = new ObjectEntry();
                        temp.headerInt1 = fileReader.ReadInt32();
                        temp.headerInt2 = fileReader.ReadInt32();
                        temp.headerInt3 = fileReader.ReadInt32();
                        temp.headerShort1 = fileReader.ReadInt16();
                        temp.objID = fileReader.ReadInt16();
                        temp.unkInt1 = fileReader.ReadInt32();
                        temp.objX = fileReader.ReadSingle();
                        temp.objY = fileReader.ReadSingle();
                        temp.objZ = fileReader.ReadSingle();
                        temp.objRotX = fileReader.ReadSingle();
                        temp.objRotY = fileReader.ReadSingle();
                        temp.objRotZ = fileReader.ReadSingle();
                        int metadataLength = fileReader.ReadInt32();
                        int metadataLoc = fileReader.ReadInt32() - baseAddr;
                        transFile.Seek(metadataLoc, SeekOrigin.Begin);
                        temp.metadata = fileReader.ReadBytes(metadataLength);
                        mapData[i].headers[j].objects[k] = temp;
                    }
                }
            }
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            List<int> ptrList = new List<int>();
            outWriter.Seek(0x10, SeekOrigin.Begin);
            int[][][] metaPtrs = new int[mapData.Length][][];
            int[][] listPtrs = new int[mapData.Length][];
            int[] mapPtrs = new int[mapData.Length];
            for (int i = 0; i < mapData.Length; i++)
            {
                metaPtrs[i] = new int[mapData[i].headers.Length][];
                listPtrs[i] = new int[mapData[i].headers.Length];
                for (int j = 0; j < mapData[i].headers.Length; j++)
                {
                    metaPtrs[i][j] = new int[mapData[i].headers[j].objects.Length];
                    for (int k = 0; k < mapData[i].headers[j].objects.Length; k++)
                    {
                        metaPtrs[i][j][k] = (int)outStream.Position;
                        outWriter.Write(mapData[i].headers[j].objects[k].metadata);
                    }
                }
            }
            for (int i = 0; i < mapData.Length; i++)
            {
                for (int j = 0; j < mapData[i].headers.Length; j++)
                {
                    listPtrs[i][j] = (int)outStream.Position;
                    for (int k = 0; k < mapData[i].headers[j].objects.Length; k++)
                    {
                        ObjectEntry tempObj = mapData[i].headers[j].objects[k];
                        outWriter.Write(tempObj.headerInt1);
                        outWriter.Write(tempObj.headerInt2);
                        outWriter.Write(tempObj.headerInt3);
                        outWriter.Write(tempObj.headerShort1);
                        outWriter.Write(tempObj.objID);
                        outWriter.Write(tempObj.unkInt1);
                        outWriter.Write(tempObj.objX);
                        outWriter.Write(tempObj.objY);
                        outWriter.Write(tempObj.objZ);
                        outWriter.Write(tempObj.objRotX);
                        outWriter.Write(tempObj.objRotY);
                        outWriter.Write(tempObj.objRotZ);
                        outWriter.Write(tempObj.metadata.Length);
                        ptrList.Add((int)outStream.Position);
                        outWriter.Write(metaPtrs[i][j][k]);
                    }
                }
            }
            for (int i = 0; i < mapData.Length; i++)
            {
                mapPtrs[i] = (int)outStream.Position;
                for (int j = 0; j < mapData[i].headers.Length; j++)
                {
                    outWriter.Write(mapData[i].headers[j].unusedInt1);
                    outWriter.Write(mapData[i].headers[j].unusedBoundSphereValue1);
                    outWriter.Write(mapData[i].headers[j].unusedBoundSphereValue2);
                    outWriter.Write(mapData[i].headers[j].unusedBoundSphereValue3);
                    outWriter.Write(mapData[i].headers[j].unusedBoundSphereValue4);
                    outWriter.Write(mapData[i].headers[j].unusedShort1);
                    outWriter.Write(mapData[i].headers[j].unknownShort1);
                    outWriter.Write(mapData[i].headers[j].unusedInt2);
                    outWriter.Write(mapData[i].headers[j].listIndex);
                    outWriter.Write(mapData[i].headers[j].unknownPairedShort1);
                    outWriter.Write(mapData[i].headers[j].unknownPairedShort2);
                    outWriter.Write((short)mapData[i].headers[j].objects.Length);
                    ptrList.Add((int)outStream.Position);
                    outWriter.Write(listPtrs[i][j]);
                }
            }
            int mapListPtr = (int)outStream.Position;
            for (int i = 0; i < mapData.Length; i++)
            {
                outWriter.Write(mapData[i].mapNumber);
                outWriter.Write((short)mapData[i].headers.Length);
                ptrList.Add((int)outStream.Position);
                outWriter.Write(mapPtrs[i]);
                outWriter.Write(0);
            }
            int mainPtr = (int)outStream.Position;
            outWriter.Write(areaID);
            outWriter.Write((short)mapData.Length);
            ptrList.Add((int)outStream.Position);
            outWriter.Write(mapListPtr);
            int fileLength = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(fileLength);
            outWriter.Write(mainPtr);
            outWriter.Write(0);
            calculatedPointers = ptrList.ToArray();
            return outStream.ToArray();
        }
    }
}
