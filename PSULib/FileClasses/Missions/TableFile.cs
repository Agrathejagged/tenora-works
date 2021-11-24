using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.FileClasses.General;

namespace PSULib.FileClasses.Missions
{
    public class TableFile : PsuFile
    {
        public class QuestGroup
        {
            public List<int> questNums = new List<int>();
            public short nameIndex = 0;
            public short descIndex = 0;
            public short unkShort1 = 0;    //0000
            public short unkShort2 = 0x3FF;    //03FF
            public byte[] unkBytes = new byte[0x14];    //0x14 of them
        }

        public List<QuestGroup> categories = new List<QuestGroup>();

        public int cityReturnQuest = -1;
        public short cityReturnZone = -1;
        public short cityReturnMap = -1;
        public int cityReturnEntry = -1;
        public short unkShort1 = 0;
        public short unkShort2 = 0;
        public int timestamp = 0;
        public short nameIndex = 0;
        public short descIndex = 1;
        public short startX = 0;
        public short startY = 0;
        public short enableCustomBackground = 0;
        public short backgroundNumber = 0;
        public short defaultIcon = 0;
        public short returnToCityIndex = 0;

        public TableFile(string filename)
        {
            this.filename = filename;
        }

        public TableFile(string inFilename, byte[] rawData, byte[] inHeader, int[] ptrs, int baseAddr)
        {
            header = inHeader;
            filename = inFilename;  //Assuming they'll always base 0.
            MemoryStream transFile = new MemoryStream(rawData);
            BinaryReader fileReader = new BinaryReader(transFile);
            transFile.Seek(8, SeekOrigin.Begin);
            int headerLoc = fileReader.ReadInt32();
            transFile.Seek(headerLoc, SeekOrigin.Begin);
            unkShort1 = fileReader.ReadInt16();
            unkShort2 = fileReader.ReadInt16();
            timestamp = fileReader.ReadInt32();
            int categoryAddr = fileReader.ReadInt32();
            int questListAddr = fileReader.ReadInt32();
            int catCount = fileReader.ReadInt16();
            int questCount = fileReader.ReadInt16();
            nameIndex = fileReader.ReadInt16();
            descIndex = fileReader.ReadInt16();
            startX = fileReader.ReadInt16();
            startY = fileReader.ReadInt16();
            enableCustomBackground = fileReader.ReadInt16();
            backgroundNumber = fileReader.ReadInt16();
            defaultIcon = fileReader.ReadInt16();
            returnToCityIndex = fileReader.ReadInt16();
            int returnAddr = fileReader.ReadInt32();

            if (returnAddr != 0)
            {
                transFile.Seek(returnAddr - baseAddr, SeekOrigin.Begin);
                cityReturnQuest = fileReader.ReadInt32();
                cityReturnZone = fileReader.ReadInt16();
                cityReturnMap = fileReader.ReadInt16();
                cityReturnEntry = fileReader.ReadInt32();
            }

            int[] questList = new int[questCount];
            transFile.Seek(questListAddr - baseAddr, SeekOrigin.Begin);
            for (int i = 0; i < questCount; i++)
                questList[i] = fileReader.ReadInt32();

            transFile.Seek(categoryAddr - baseAddr, SeekOrigin.Begin);
            for (int i = 0; i < catCount; i++)
            {
                QuestGroup grp = new QuestGroup();
                grp.questNums = new List<int>();
                short firstQuest = fileReader.ReadInt16();
                short currentListQuestCount = fileReader.ReadInt16();
                for (int k = 0; k < currentListQuestCount; k++)
                {
                    grp.questNums.Add(questList[k + firstQuest]);
                }
                grp.nameIndex = fileReader.ReadInt16();
                grp.descIndex = fileReader.ReadInt16();
                grp.unkShort1 = fileReader.ReadInt16();
                grp.unkShort2 = fileReader.ReadInt16();
                grp.unkBytes = fileReader.ReadBytes(0x14);
                categories.Add(grp);
            }

        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(Encoding.ASCII.GetBytes("NXR\0"));
            outWriter.Seek(0x10, SeekOrigin.Begin);
            short[] categoryStartNums = new short[categories.Count];
            short[] categoryQuestCounts = new short[categories.Count];
            int questCount = 0;
            for (int i = 0; i < categories.Count; i++)
            {
                categoryStartNums[i] = (short)questCount;
                for (int j = 0; j < categories[i].questNums.Count; j++)
                    outWriter.Write(categories[i].questNums[j]);
                questCount += categories[i].questNums.Count;
            }
            int catOffset = (int)outStream.Position;
            for (int i = 0; i < categories.Count; i++)
            {
                outWriter.Write(categoryStartNums[i]);
                outWriter.Write((short)categories[i].questNums.Count);
                outWriter.Write(categories[i].nameIndex);
                outWriter.Write(categories[i].descIndex);
                outWriter.Write(categories[i].unkShort1);
                outWriter.Write(categories[i].unkShort2);
                outWriter.Write(categories[i].unkBytes);
            }
            int retOffset = (int)outStream.Position;
            if (cityReturnQuest != -1)
            {
                outWriter.Write(cityReturnQuest);
                outWriter.Write(cityReturnZone);
                outWriter.Write(cityReturnMap);
                outWriter.Write(cityReturnEntry);
            }
            int headerLoc = (int)outStream.Position;
            outWriter.Write(unkShort1);
            outWriter.Write(unkShort2);
            outWriter.Write(timestamp);
            outWriter.Write(catOffset);
            outWriter.Write(0x10);
            outWriter.Write((short)categories.Count);
            outWriter.Write((short)questCount);
            outWriter.Write(nameIndex);
            outWriter.Write(descIndex);
            outWriter.Write(startX);
            outWriter.Write(startY);
            outWriter.Write(enableCustomBackground);
            outWriter.Write(backgroundNumber);
            outWriter.Write(defaultIcon);
            outWriter.Write(returnToCityIndex);
            if (cityReturnQuest != -1)
                outWriter.Write(retOffset);
            else
                outWriter.Write(0);
            int fileLength = (int)outStream.Position;
            outStream.Seek(4, SeekOrigin.Begin);
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            if (cityReturnQuest != -1)
            {
                calculatedPointers = new int[3];
                calculatedPointers[2] = headerLoc + 0x24;
            }
            else
            {
                calculatedPointers = new int[2];
            }

            calculatedPointers[0] = headerLoc + 0x8;
            calculatedPointers[1] = headerLoc + 0xC;

            byte[] length = BitConverter.GetBytes(fileLength);
            byte[] lengthPlusTwenty = BitConverter.GetBytes(fileLength + 0x20);

            if (header == null)
            {
                header = new byte[] { 0x4E, 0x58, 0x49, 0x46, 0x18, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x84, 0x02, 0x00, 0x00, 0xA4, 0x02, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
            }
            Array.Copy(length, 0, header, 0x10, 4);
            Array.Copy(lengthPlusTwenty, 0, header, 0x14, 4);

            return outStream.ToArray();
        }
    }
}
