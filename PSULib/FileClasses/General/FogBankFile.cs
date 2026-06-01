using PSULib.FileClasses.Maps.Common;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.General
{
    public class FogBankFile : PsuFile
    {
        private const int ENTRY_SIZE = 28;
        public List<LndFog> FogEntries { get; } = new List<LndFog>();


        public FogBankFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            List<int> pointerList = new List<int>(ptrs.Length);
            for (int i = 0; i < ptrs.Length; i++)
            {
                pointerList.Add(ptrs[i] - baseAddr);
            }
            calculatedPointers = pointerList.ToArray();
            byte[] headers = inReader.ReadBytes(4);
            int fileLength = inReader.ReadInt32();
            int headerLoc = inReader.ReadInt32();
            //I've never seen this not be 32 entries, but I've also never seen it be anything other than header-data-table of contents
            //so we can dynamically calculate this!
            int entryCount = (headerLoc - 0x10) / ENTRY_SIZE;
            //assuming there's gonna be 32 of these...

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int[] locations = new int[entryCount];
            for(int i = 0; i < entryCount; i++)
            {
                locations[i] = inReader.ReadInt32() - baseAddr;
            }
            for(int i = 0; i < entryCount; i++)
            {
                inStream.Seek(locations[i], SeekOrigin.Begin);
                FogEntries.Add(new LndFog(inReader));
            }
        }

        public override byte[] ToRaw()
        {
            calculatedPointers = new int[FogEntries.Count];
            //each entry is 28 bytes of data plus 4 bytes of pointer
            //base header is 0x10 bytes
            //this file may still be effectively hardcoded in terms of length in the .exe--not verified
            MemoryStream outStream = new MemoryStream(FogEntries.Count * (ENTRY_SIZE + 4) + 0x10);
            BinaryWriter outWriter = new BinaryWriter(outStream);
            //could just recalculate this afterward, but this is small.
            int[] destinations = new int[FogEntries.Count];
            for(int i = 0; i < FogEntries.Count; i++)
            {
                destinations[i] = (int)outStream.Position;
                FogEntries[i].Write(outWriter);
            }

            int headerLoc = (int)outStream.Position;
            for (int i = 0; i < FogEntries.Count; i++)
            {
                calculatedPointers[i] = (int)outStream.Position;
                outWriter.Write(destinations[i]);
            }
            int fileSize = (int)outStream.Position;

            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(fileSize);
            outWriter.Write(headerLoc);
            outWriter.Write(0);

            header = buildSubheader((int)outStream.Length);
            return outStream.ToArray();
        }
    }
}
