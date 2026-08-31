using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Bosses
{
    /// <summary>
    /// Used for Fakis 1 and Mother Brain. Very similar to the format used for Fakis 2/Falz 2, but with a few added header floats.
    /// </summary>
    public class ThinkFakis1MotherBrainFile : PsuFile
    {
        private PointeredFile backupRaw;

        public class GroupOneEntry
        {
            public short byte1;
            public short byte2;
            public short byte3;
            public short byte4;
        }

        public class GroupTwoEntry
        {
            public short short1;
            public short short2;
        }

        public class GroupThreeEntry
        {
            public float float1;
            public float float2;
        }

        // Triplets always make me think XYZ.
        public float floatGroup1Entry1;
        public float floatGroup1Entry2;
        public float floatGroup1Entry3;

        public List<GroupOneEntry> groupOneEntries = new List<GroupOneEntry>();
        public List<GroupTwoEntry> groupTwoEntries = new List<GroupTwoEntry>();
        public List<GroupThreeEntry> groupThreeEntries = new List<GroupThreeEntry>();

        // Only present on Mother Brain.
        public float floatGroup2Entry1;
        public float floatGroup2Entry2;
        public float floatGroup2Entry3;

        public ThinkFakis1MotherBrainFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            backupRaw = new PointeredFile(inFilename, rawData, subHeader, ptrs, baseAddr, false);

            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);

            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            floatGroup1Entry1 = inReader.ReadSingle();
            floatGroup1Entry2 = inReader.ReadSingle();
            floatGroup1Entry3 = inReader.ReadSingle();
            int groupOneLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoLoc = inReader.ReadInt32() - baseAddr;
            int groupThreeLoc = inReader.ReadInt32() - baseAddr;
            byte groupOneCount = inReader.ReadByte();
            byte groupTwoCount = inReader.ReadByte();
            byte groupThreeCount = inReader.ReadByte();
            inReader.ReadByte(); //This is just a 0.
            floatGroup2Entry1 = inReader.ReadSingle();
            floatGroup2Entry2 = inReader.ReadSingle();
            floatGroup2Entry3 = inReader.ReadSingle();

            //Group 1 is much easier than group 2.
            inStream.Seek(groupOneLoc, SeekOrigin.Begin);
            for(int i = 0; i < groupOneCount; i++)
            {
                GroupOneEntry entry = new GroupOneEntry();
                entry.byte1 = inReader.ReadByte();
                entry.byte2 = inReader.ReadByte();
                entry.byte3 = inReader.ReadByte();
                entry.byte4 = inReader.ReadByte();
                groupOneEntries.Add(entry);
            }

            inStream.Seek(groupTwoLoc, SeekOrigin.Begin);
            for(int i = 0; i < groupTwoCount; i++)
            {
                GroupTwoEntry entry = new GroupTwoEntry();
                entry.short1 = inReader.ReadInt16();
                entry.short2 = inReader.ReadInt16();
                groupTwoEntries.Add(entry);
            }

            inStream.Seek(groupThreeLoc, SeekOrigin.Begin);
            for(int i = 0; i< groupThreeCount; i++)
            {
                GroupThreeEntry entry = new GroupThreeEntry();
                entry.float1 = inReader.ReadSingle();
                entry.float2 = inReader.ReadSingle();
                groupThreeEntries.Add(entry);
            }
        }


        public override byte[] ToRaw()
        {
            //Return this until a proper implementation is made
            return backupRaw.ToRaw();
        }
    }
}
