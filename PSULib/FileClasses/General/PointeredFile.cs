﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PSULib.Support;

namespace PSULib.FileClasses.General
{
    //For generic unknown files with pointers.
    public class PointeredFile : PsuFile
    {
        public class FileChunk
        {
            public int chunkLocation;
            public byte[] contents;
        }
        public List<FileChunk> splitData;
        public Dictionary<int, int> pointerToDestination = new Dictionary<int, int>();
        public Dictionary<int, HashSet<int>> destinationToPointers = new Dictionary<int, HashSet<int>>();
        int fileLength;

        public bool isBigEndian = false; //Store so we can output back to the original on saving
        public bool isBrokenFile = false; //Used for certain files, which appear broken.
        protected byte[] brokenRaw; //Used for certain files, which appear broken.
        protected int[] brokenPtrs; //Used for certain files, which appear broken.

        protected PointeredFile()
        {

        }

        //Takes the file, splits it based upon which pointer accesses it.
        //Also modifies all pointers to be 0-based!
        public PointeredFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr, bool useAsBigEndian)
        {
            isBigEndian = useAsBigEndian;

            header = subHeader;
            filename = inFilename;
            fileLength = BitConverter.ToInt32(rawData, 4);
            if (ptrs == null)
                ptrs = new int[0];

            MemoryStream initialStream = new MemoryStream(rawData);
            BinaryReader initialReader = BigEndianBinaryReader.GetEndianSpecificBinaryReader(initialStream, useAsBigEndian);

            //Check if the file pointers make sense, update the raw data to be relative to 0 rather than relative to baseAddr
            //Also some files like to point at stuff in the padding space, so account for that.
            byte[] modifiedData = modifiedData = (byte[])rawData.Clone();
            /*if(rawData.Length % 0x20 != 0)
            {
                modifiedData = new byte[((rawData.Length + 0x20) & 0xFFFFFFE0)];
                Array.Copy(rawData, modifiedData, rawData.Length);
            }*/
            /*
            else
            {
                modifiedData = (byte[])rawData.Clone();
            }*/
            int[] rebasedPointers = new int[ptrs.Length];

            MemoryStream memStream = new MemoryStream(modifiedData);
            var fileReader = BigEndianBinaryReader.GetEndianSpecificBinaryReader(memStream, useAsBigEndian);
            BinaryWriter modifiedWriter = BigEndianBinaryWriter.GetEndianSpecificBinaryWriter(memStream, useAsBigEndian);

            for (int i = 0; i < rebasedPointers.Length; i++)
            {
                rebasedPointers[i] = ptrs[i] - baseAddr;
                if (rebasedPointers[i] < 0 || rebasedPointers[i] > modifiedData.Length)
                {
                    markFileBroken(rawData, ptrs, baseAddr);
                }
                memStream.Seek(rebasedPointers[i], SeekOrigin.Begin);
                int pointerDestination = fileReader.ReadInt32() - baseAddr;
                if (pointerDestination < 0 || pointerDestination > modifiedData.Length)
                {
                    markFileBroken(rawData, ptrs, baseAddr);
                }
                memStream.Seek(rebasedPointers[i], SeekOrigin.Begin);
                modifiedWriter.Write(pointerDestination);
            }

            //+1 because the base pointer in the file isn't a calculated value
            List<int> pointerDestinations = new List<int>(rebasedPointers.Length + 1);
            memStream.Seek(8, SeekOrigin.Begin);

            int rootPtr = fileReader.ReadInt32();
            if (rootPtr < 0 || rootPtr > modifiedData.Length)
            {
                markFileBroken(rawData, ptrs, baseAddr);
                return;
            }

            addToMaps(8, rootPtr);

            for (int i = 0; i < rebasedPointers.Length; i++)
            {
                int effectivePtr = rebasedPointers[i];
                memStream.Seek(effectivePtr, SeekOrigin.Begin);
                int tempPtr = fileReader.ReadInt32();
                addToMaps(effectivePtr, tempPtr);
            }

            int startAddr = 0;
            splitData = new List<FileChunk>(destinationToPointers.Count + 1);
            foreach (int dest in destinationToPointers.Keys.Union(new int[] { modifiedData.Length }).OrderBy(key => key))
            {
                memStream.Seek(startAddr, SeekOrigin.Begin);
                byte[] bytes = fileReader.ReadBytes(dest - startAddr);
                splitData.Add(new FileChunk { chunkLocation = startAddr, contents = bytes });
                startAddr = dest;
            }
        }

        private void markFileBroken(byte[] rawData, int[] ptrs, int baseAddr)
        {
            isBrokenFile = true;
            brokenRaw = rawData;
            brokenPtrs = new int[ptrs.Length];
            for (int j = 0; j < ptrs.Length; j++)
                brokenPtrs[j] = ptrs[j] - baseAddr;
        }

        private void addToMaps(int pointerSource, int pointerDest)
        {
            pointerToDestination.Add(pointerSource, pointerDest);
            if (!destinationToPointers.ContainsKey(pointerDest))
            {
                destinationToPointers.Add(pointerDest, new HashSet<int>());
            }
            destinationToPointers[pointerDest].Add(pointerSource);
        }

        public static uint ReverseBytes(uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static ushort ReverseBytes(ushort value)
        {
            return (ushort)((value & 0x00FFu) << 8 | (value & 0xFF00u) >> 8);
        }

        public override byte[] ToRaw()
        {
            if (!isBrokenFile)
            {
                MemoryStream outputStream = new MemoryStream();
                BinaryWriter outputWriter = new BinaryWriter(outputStream);

                for (int i = 0; i < splitData.Count; i++)
                {
                    outputWriter.Write(splitData[i].contents);
                }

                calculatedPointers = new int[pointerToDestination.Keys.Count - 1];
                Array.Copy(pointerToDestination.Keys.ToArray(), 1, calculatedPointers, 0, calculatedPointers.Length);
                byte[] toOut = outputStream.ToArray();
                return toOut;
            }
            else
            {
                calculatedPointers = brokenPtrs;
                return brokenRaw;
            }
        }
    }
}
