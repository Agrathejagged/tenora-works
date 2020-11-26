using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    public class ThinkDragonFile : PsuFile
    {
        private PointeredFile backupRaw;
        public class GroupTwoLeaf
        {
            public List<Tuple<float, int>> tupleList = new List<Tuple<float, int>>();
            public float unkFloat1, unkFloat2;
            public byte unkByte1, unkByte2, unkByte3, unkByte4, unkByte5, unkByte6, unkByte7;
        }

        public class GroupTwoEntry
        {
            public List<List<Tuple<float, int>>> branchOne, branchTwo, branchThree, branchFour;
            public List<List<GroupTwoLeaf>> branchFive;
            public byte byte1, byte2, byte3;
            public int int1;
        }

        public class GroupOneEntry
        {
            public float floatOne, floatTwo, floatThree, floatFour;
            public List<Tuple<float, float>> groupOneChildOne;
            public List<Tuple<float, float, float>> groupOneChildTwo;
            public byte byteOne, byteTwo;
        }

        public GroupOneEntry groupOne = new GroupOneEntry();
        public GroupTwoEntry groupTwo = new GroupTwoEntry();

        public ThinkDragonFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            backupRaw = new PointeredFile(inFilename, rawData, subHeader, ptrs, baseAddr, false);

            filename = inFilename;
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);

            inStream.Seek(8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int groupOneLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoLoc = inReader.ReadInt32() - baseAddr;
            byte groupOneCount = inReader.ReadByte();
            byte groupTwoCount = inReader.ReadByte();
            //In De Ragan, groups 1 and 2 are each only 1 entry long.

            //Group 1 is much easier than group 2.
            inStream.Seek(groupOneLoc, SeekOrigin.Begin);
            groupOne.floatOne = inReader.ReadSingle();
            groupOne.floatTwo = inReader.ReadSingle();
            groupOne.floatThree = inReader.ReadSingle();
            groupOne.floatFour = inReader.ReadSingle();
            int groupOneChildOneLoc = inReader.ReadInt32() - baseAddr;
            int groupOneChildTwoLoc = inReader.ReadInt32() - baseAddr;
            byte groupOneChildOneCount = inReader.ReadByte();
            byte groupOneChildTwoCount = inReader.ReadByte();
            groupOne.byteOne = inReader.ReadByte();
            groupOne.byteTwo = inReader.ReadByte();
            groupOne.groupOneChildOne = new List<Tuple<float, float>>();
            groupOne.groupOneChildTwo = new List<Tuple<float, float, float>>();
            inStream.Seek(groupOneChildOneLoc, SeekOrigin.Begin);
            for (int i = 0; i < groupOneChildOneCount; i++)
            {
                float a = inReader.ReadSingle();
                float b = inReader.ReadSingle();
                groupOne.groupOneChildOne.Add(new Tuple<float, float>(a, b));
            }
            inStream.Seek(groupOneChildTwoLoc, SeekOrigin.Begin);
            for (int i = 0; i < groupOneChildTwoCount; i++)
            {
                float a = inReader.ReadSingle();
                float b = inReader.ReadSingle();
                float c = inReader.ReadSingle();
                groupOne.groupOneChildTwo.Add(new Tuple<float, float, float>(a, b, c));
            }

            //Group 2...
            //Five pointers, five counts, and then mystery values!
            //First four appear to be to lists of float/int pairs.
            inStream.Seek(groupTwoLoc, SeekOrigin.Begin);
            int groupTwoBranchOneLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoBranchTwoLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoBranchThreeLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoBranchFourLoc = inReader.ReadInt32() - baseAddr;
            int groupTwoBranchFiveLoc = inReader.ReadInt32() - baseAddr;
            byte groupTwoBranchOneCount = inReader.ReadByte();
            byte groupTwoBranchTwoCount = inReader.ReadByte();
            byte groupTwoBranchThreeCount = inReader.ReadByte();
            byte groupTwoBranchFourCount = inReader.ReadByte();
            byte groupTwoBranchFiveCount = inReader.ReadByte();
            groupTwo.byte1 = inReader.ReadByte();
            groupTwo.byte2 = inReader.ReadByte();
            groupTwo.byte3 = inReader.ReadByte();
            groupTwo.int1 = inReader.ReadInt32();
            groupTwo.branchFive = new List<List<GroupTwoLeaf>>();

            groupTwo.branchOne = readFloatIntList(groupTwoBranchOneLoc, groupTwoBranchOneCount, baseAddr, inStream, inReader);
            groupTwo.branchTwo = readFloatIntList(groupTwoBranchTwoLoc, groupTwoBranchTwoCount, baseAddr, inStream, inReader);
            groupTwo.branchThree = readFloatIntList(groupTwoBranchThreeLoc, groupTwoBranchThreeCount, baseAddr, inStream, inReader);
            groupTwo.branchFour = readFloatIntList(groupTwoBranchFourLoc, groupTwoBranchFourCount, baseAddr, inStream, inReader);

            for (int i = 0; i < groupTwoBranchFiveCount; i++)
            {
                inStream.Seek(groupTwoBranchFiveLoc + i * 8, SeekOrigin.Begin);
                int currentPointer = inReader.ReadInt32() - baseAddr;
                int currentCount = inReader.ReadInt32();

                List<GroupTwoLeaf> leaves = new List<GroupTwoLeaf>();
                for (int j = 0; j < currentCount; j++)
                {
                    inStream.Seek(currentPointer + j * 20, SeekOrigin.Begin);
                    GroupTwoLeaf leaf = new GroupTwoLeaf();
                    int tupleListPtr = inReader.ReadInt32() - baseAddr;
                    leaf.unkFloat1 = inReader.ReadSingle();
                    leaf.unkFloat2 = inReader.ReadSingle();
                    byte tupleCount = inReader.ReadByte();
                    leaf.unkByte1 = inReader.ReadByte();
                    leaf.unkByte2 = inReader.ReadByte();
                    leaf.unkByte3 = inReader.ReadByte();
                    leaf.unkByte4 = inReader.ReadByte();
                    leaf.unkByte5 = inReader.ReadByte();
                    leaf.unkByte6 = inReader.ReadByte();
                    leaf.unkByte7 = inReader.ReadByte();

                    inStream.Seek(tupleListPtr, SeekOrigin.Begin);
                    for (int k = 0; k < tupleCount; k++)
                    {
                        float a = inReader.ReadSingle();
                        int b = inReader.ReadInt32();
                        leaf.tupleList.Add(new Tuple<float, int>(a, b));
                    }
                    leaves.Add(leaf);
                }
                groupTwo.branchFive.Add(leaves);
            }
        }

        private List<List<Tuple<float, int>>> readFloatIntList(int startAddr, byte count, int baseAddr, Stream inStream, BinaryReader inReader)
        {
            var toReturn = new List<List<Tuple<float, int>>>();
            for (int i = 0; i < count; i++)
            {
                inStream.Seek(startAddr + i * 8, SeekOrigin.Begin);
                int currentPointer = inReader.ReadInt32() - baseAddr;
                int currentCount = inReader.ReadInt32();

                List<Tuple<float, int>> currentList = new List<Tuple<float, int>>();
                inStream.Seek(currentPointer, SeekOrigin.Begin);
                for (int j = 0; j < currentCount; j++)
                {
                    float a = inReader.ReadSingle();
                    int b = inReader.ReadInt32();
                    currentList.Add(new Tuple<float, int>(a, b));
                }
                toReturn.Add(currentList);
            }
            return toReturn;
        }

        
        public override byte[] ToRaw()
        {
            //Return this until a proper implementation is made
            return backupRaw.ToRaw();
        }
    }
}
