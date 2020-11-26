using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using psu_generic_parser;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;

namespace psu_generic_parser
{
    [DataContract]
    public class SetFile : PsuFile
    {
        [DataMember]
        public short areaID;
        [DataMember]
        public MapListing[] mapData;

        [DataContract]
        public struct ObjectEntry
        {
            [DataMember]
            public byte[] startBytes; //0x14 length
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
            public int metadataLength;
            [DataMember]
            public byte[] metadata;
        }

        [DataContract]
        public struct ListHeader
        {
            [DataMember]
            public byte[] headerBytes; //36 bytes
            [DataMember]
            public ObjectEntry[] objects;
        }

        [DataContract]
        public struct MapListing
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
                    mapData[i].headers[j].headerBytes = fileReader.ReadBytes(34);
                    short listEntryCount = fileReader.ReadInt16();
                    mapData[i].headers[j].objects = new ObjectEntry[listEntryCount];
                    int objectListLoc = fileReader.ReadInt32() - baseAddr;
                    for (int k = 0; k < listEntryCount; k++)
                    {
                        transFile.Seek(objectListLoc + k * 0x34, SeekOrigin.Begin);
                        ObjectEntry temp = new ObjectEntry();
                        temp.startBytes = fileReader.ReadBytes(14);
                        temp.objID = fileReader.ReadInt16();
                        temp.unkInt1 = fileReader.ReadInt32();
                        temp.objX = fileReader.ReadSingle();
                        temp.objY = fileReader.ReadSingle();
                        temp.objZ = fileReader.ReadSingle();
                        temp.objRotX = fileReader.ReadSingle();
                        temp.objRotY = fileReader.ReadSingle();
                        temp.objRotZ = fileReader.ReadSingle();
                        temp.metadataLength = fileReader.ReadInt32();
                        int metadataLoc = fileReader.ReadInt32() - baseAddr;
                        transFile.Seek(metadataLoc, SeekOrigin.Begin);
                        temp.metadata = fileReader.ReadBytes(temp.metadataLength);
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
                        outWriter.Write(tempObj.startBytes);
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
                    outWriter.Write(mapData[i].headers[j].headerBytes);
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
                outWriter.Write((int)0);
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
            outWriter.Write((int)0);
            calculatedPointers = ptrList.ToArray();
            return (outStream.ToArray());
        }
    }
}
