using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    public class QuestListFile : PsuFile
    {
        public class QuestListing
        {
            public int QuestNumber { get; set; }
            public string FileName { get; set; }
        }

        public BindingList<QuestListing> questList = new BindingList<QuestListing>();

        public QuestListFile(string inFilename, byte[] rawData, byte[] subHeader, int[] ptrs, int baseAddr)
        {
            header = subHeader;
            filename = inFilename;
            
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(0x8, SeekOrigin.Begin);
            int headerLoc = inReader.ReadInt32();

            inStream.Seek(headerLoc, SeekOrigin.Begin);
            int listLoc = inReader.ReadInt32();
            int listCount = inReader.ReadInt32();
            
            for(int i = 0; i < listCount; i++)
            {
                inStream.Seek(listLoc - baseAddr + i * 8, SeekOrigin.Begin);
                QuestListing temp = new QuestListing();
                temp.QuestNumber = inReader.ReadInt32();
                int stringLoc = inReader.ReadInt32();
                inStream.Seek(stringLoc - baseAddr, SeekOrigin.Begin);
                StringBuilder filenameBuilder = new StringBuilder();
                byte nextChar = inReader.ReadByte();
                while(nextChar != 0)
                {
                    filenameBuilder.Append(Convert.ToChar(nextChar));
                    nextChar = inReader.ReadByte();
                }
                temp.FileName = filenameBuilder.ToString();
                questList.Add(temp);
            }
        }

        public override byte[] ToRaw()
        {
            calculatedPointers = new int[questList.Count + 1];
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);

            outStream.Seek(0x10, SeekOrigin.Begin);
            int[] questFilenameLocs = new int[questList.Count];
            for(int i = 0; i < questList.Count; i++)
            {
                questFilenameLocs[i] = (int)outStream.Position;
                outWriter.Write(ASCIIEncoding.ASCII.GetBytes(questList[i].FileName + "\0"));
                outStream.Seek((outStream.Position + 3) & 0xFFFFFFFC, SeekOrigin.Begin);
            }
            int questListLoc = (int)outStream.Position;
            for(int i = 0; i < questList.Count; i++)
            {
                outWriter.Write(questList[i].QuestNumber);
                calculatedPointers[i] = (int)outStream.Position;
                outWriter.Write(questFilenameLocs[i]);
            }

            calculatedPointers[questList.Count] = (int)outStream.Position;
            int headerLoc = (int)outStream.Position;
            outWriter.Write(questListLoc);
            outWriter.Write(questList.Count);
            outStream.Seek((outStream.Position + 0x1F) & 0xFFFFFFE0, SeekOrigin.Begin);
            int fileLength = (int)outStream.Position;
            outStream.Seek(0, SeekOrigin.Begin);
            outWriter.Write(new byte[] { 0x4E, 0x58, 0x52, 0 });
            outWriter.Write(fileLength);
            outWriter.Write(headerLoc);
            outWriter.Write((int)0);
            return outStream.ToArray();
        }

        public void LoadTextFile(string[] lines)
        {
            BindingList<QuestListing> newQuestList = new BindingList<QuestListing>();
            foreach(string line in lines)
            {
                if (line != "")
                {
                    QuestListing temp = new QuestListing();
                    string[] splitLine = line.Split('\t');
                    temp.QuestNumber = int.Parse(splitLine[0]);
                    temp.FileName = splitLine[1];
                    newQuestList.Add(temp);
                }
            }
            questList = newQuestList;
        }
    }
}
