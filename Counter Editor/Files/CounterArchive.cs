using PSULib.FileClasses.Archives;
using PSULib.FileClasses.General;
using PSULib.FileClasses.Missions;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Counter_Editor.Files
{
    /// <summary>
    /// Wrapper for archives containing counter data. Agnostic to base type--supports NBLs and pack files.
    /// </summary>
    public class CounterArchive
    {
        public class QuestDefinition
        {
            public int QuestNumber { get; set; } = -1;
            public NblLoader QuestNbl { get; set; } = null;
            public RawFile QuestRaw { get; set; } = null;

            public QuestDefinition(int num, RawFile rawFile, NblLoader nblFile)
            {
                QuestNumber = num;
                QuestNbl = nblFile;
                QuestRaw = rawFile;
            }
        }

        public class CategoryInfo
        {
            public string categoryName = "";
            public string categoryDescription = "";
            public List<QuestDefinition> categoryNumbersNbls = new List<QuestDefinition>();
            public short unknownShort1 = -1;
            public short unknownShort2 = -1;
            public byte[] miscBytes;
        }
        public List<CategoryInfo> categories = new List<CategoryInfo>();
        public TableFile table;
        public TextFile textFile;

        public string counterName = "";
        public string counterDescription = "";

        private Dictionary<int, QuestDefinition> QuestDefinitions { get; set; } = new Dictionary<int, QuestDefinition>();

        public int TotalQuests
        {
            get
            {
                int totalCount = 0;
                foreach (CategoryInfo cat in categories)
                    totalCount += cat.categoryNumbersNbls.Count;
                return totalCount;
            }
        }
        public int CategoryCount
        {
            get { return categories.Count; }
        }

        public int CityReturnQuest { get; set; }

        public short CityReturnZone { get; set; }

        public short CityReturnMap { get; set; }

        public int CityReturnEntry { get; set; }

        public short UnkShort1 { get; set; }

        public short UnkShort2 { get; set; }

        public int Timestamp { get; set; }

        public short StartX { get; set; }

        public short StartY { get; set; }

        public short EnableCustomBackground { get; set; }

        public short BackgroundNumber { get; set; }

        public short DefaultIcon { get; set; }

        public string ReturnToCityText { get; set; } = "";

        public CounterArchive()
        {
            table = new TableFile("table.rel");
            textFile = new TextFile("text.bin");
        }

        public CounterArchive(PackFile pack)
        {
            NblLoader tableNbl = (NblLoader)pack.getFileParsed(0);
            table = (TableFile)((NblChunk)tableNbl.getFileParsed(0)).getFileParsed("table.rel");
            textFile = (TextFile)((NblChunk)tableNbl.getFileParsed(0)).getFileParsed("text.bin");
            LoadMissionNbls(pack);
            LoadData(pack, 1);
        }

        public CounterArchive(NblLoader nbl)
        {
            table = (TableFile)((NblChunk)nbl.getFileParsed(0)).getFileParsed("table.rel");
            textFile = (TextFile)((NblChunk)nbl.getFileParsed(0)).getFileParsed("text.bin");
            LoadMissionNbls((NblChunk)nbl.getFileParsed(0));
            LoadData((NblChunk)nbl.getFileParsed(0), 2);
        }

        public CounterArchive(AfsLoader afs)
        {
            foreach(AfsLoader.AfsFileEntry file in afs.afsList)
            {
                if(file.fileName.StartsWith("table") && file.fileName.EndsWith("_ae.nbl"))
                {
                    NblLoader nbl = (NblLoader)file.fileContents;
                    table = (TableFile)((NblChunk)nbl.getFileParsed(0)).getFileParsed("table.rel");
                    textFile = (TextFile)((NblChunk)nbl.getFileParsed(0)).getFileParsed("text.bin");
                    LoadMissionNbls((NblChunk)nbl.getFileParsed(0));
                    LoadData((NblChunk)nbl.getFileParsed(0), 2);
                    return;
                }
            }
            throw new InvalidOperationException("Attempted to load non-counter AFS file.");
        }

        private void LoadMissionNbls(ContainerFile owner)
        {
            //afs files do not support this. Fortunately, this doesn't matter--we're inside the NBL at this point.
            List<string> filenames = owner.getFilenames();
            for(int i = 0; i < filenames.Count; i++) {
                if (filenames[i].EndsWith(".nbl"))
                {
                    RawFile rawFile = owner.getFileRaw(filenames[i]);
                    NblLoader nbl = (NblLoader)owner.getFileParsed(filenames[i]);
                    NblChunk nblChunk = (NblChunk)nbl.getFileParsed(0);
                    string questFilename = nblChunk.getFilenames().Find(filename => Regex.IsMatch(filename, "quest\\.[usxy]nr"));
                    if (questFilename != null)
                    {
                        RawFile questFile = nblChunk.getFileRaw(questFilename);
                        int questNumber = QuestDataExtractor.GetQuestNumber(questFile);
                        QuestDefinitions[questNumber] = new QuestDefinition(questNumber, rawFile, nbl);
                    }
                }
            }
        }

        private void LoadData(ContainerFile containingFile, int offset)
        {

            int currentNblNum = 0;
            for (int i = 0; i < table.categories.Count; i++)
            {
                CategoryInfo temp = new CategoryInfo();
                if (table.categories[i].nameIndex != -1)
                    temp.categoryName = textFile.strings[0][0][table.categories[i].nameIndex];
                if (table.categories[i].descIndex != -1)
                    temp.categoryDescription = textFile.strings[0][0][table.categories[i].descIndex];
                temp.unknownShort1 = table.categories[i].unkShort1;
                temp.unknownShort2 = table.categories[i].unkShort2;
                temp.miscBytes = table.categories[i].unkBytes;
                for (int j = 0; j < table.categories[i].questNums.Count; j++)
                {
                    //temp.categoryNumbersNbls.Add(new QuestDefinition(table.categories[i].questNums[j], containingFile.getFileRaw(currentNblNum + j + offset), (NblLoader)containingFile.getFileParsed(currentNblNum + j + offset)));
                    if (QuestDefinitions.ContainsKey(table.categories[i].questNums[j]))
                    {
                        temp.categoryNumbersNbls.Add(QuestDefinitions[table.categories[i].questNums[j]]);
                    } else
                    {
                        temp.categoryNumbersNbls.Add(new QuestDefinition(table.categories[i].questNums[j], null, null));
                    }
                }
                currentNblNum += table.categories[i].questNums.Count;
                categories.Add(temp);
            }
            counterName = textFile.strings[0][0][table.nameIndex];
            counterDescription = textFile.strings[0][0][table.descIndex];
            CityReturnEntry = table.cityReturnEntry;
            CityReturnQuest = table.cityReturnQuest;
            CityReturnZone = table.cityReturnZone;
            CityReturnMap = table.cityReturnMap;
            CityReturnEntry = table.cityReturnEntry;
            UnkShort1 = table.unkShort1;
            UnkShort2 = table.unkShort2;
            Timestamp = table.timestamp;
            StartX = table.startX;
            StartY = table.startY;
            EnableCustomBackground = table.enableCustomBackground;
            BackgroundNumber = table.backgroundNumber;
            DefaultIcon = table.defaultIcon;
            if(table.returnToCityIndex != -1)
            {
                ReturnToCityText = textFile.strings[0][0][table.returnToCityIndex];
            }
        }

        public void WriteAsPack(Stream outStream)
        {
            PackFile forExport = new PackFile("temp.pack");
            UpdateTable();
            forExport.containedFiles.Add(0, new NblLoader(new List<RawFile> { table.ToRawFile(0), textFile.ToRawFile(0) }));
            int currentNbl = 0;
            for(int i = 0; i < categories.Count; i++)
            {
                for(int j = 0; j < categories[i].categoryNumbersNbls.Count; j++)
                {
                    NblLoader currNblFile = categories[i].categoryNumbersNbls[j].QuestNbl;
                    forExport.containedFiles.Add(currentNbl + j + 1, currNblFile);
                    forExport.containedRawFiles.Add(currentNbl + j + 1, categories[i].categoryNumbersNbls[j].QuestRaw);
                }
                currentNbl += categories[i].categoryNumbersNbls.Count;
            }

            byte[] packOut = forExport.ToRaw();
            outStream.Write(packOut, 0, packOut.Length);
        }

        public void WriteAsNbl(Stream outStream)
        {
            
            UpdateTable();
            List<RawFile> files = new List<RawFile>
            {
                table.ToRawFile(0),
                textFile.ToRawFile(0)
            };

            int currentNbl = 0;
            for (int i = 0; i < categories.Count; i++)
            {
                for (int j = 0; j < categories[i].categoryNumbersNbls.Count; j++)
                {
                    NblLoader currNblFile = categories[i].categoryNumbersNbls[j].QuestNbl;
                    if(categories[i].categoryNumbersNbls[j].QuestRaw != null)
                    {
                        RawFile temp = new RawFile();
                        temp.chunkSize = categories[i].categoryNumbersNbls[j].QuestRaw.chunkSize;
                        temp.filename = currentNbl.ToString("D4") + ".nbl";
                        temp.fileContents = categories[i].categoryNumbersNbls[j].QuestRaw.fileContents;
                        temp.fileheader = categories[i].categoryNumbersNbls[j].QuestRaw.fileheader;
                        files.Add(temp);
                        currentNbl++;
                    }
                }
            }
            NblLoader forExport = new NblLoader(files);

            byte[] packOut = forExport.ToRaw();
            outStream.Write(packOut, 0, packOut.Length);
        }

        //Valid languages are J (0), AE (1), G (3), F (4)
        //Fun fact: this game doesn't actually care about filenames in AFS files, so unnumbered tables are fine.
        public void WriteAsAfs(Stream outStream)
        {
            MemoryStream nblStream = new MemoryStream();
            WriteAsNbl(nblStream);
            byte[] nblFile = nblStream.ToArray();
            AfsLoader afs = new AfsLoader();
            for(int i = 0; i < AfsLoader.languages.Length; i++)
            {
                afs.addFile("table" + AfsLoader.languages[i] + ".nbl", nblStream);
            }
            afs.saveFile(outStream);
        }

        private void UpdateTable()
        {
            table.categories.Clear();
            if (textFile.strings[0] != null)
                textFile.strings[0][0].Clear();
            else
            {
                textFile.strings[0] = new List<List<string>>();
                textFile.strings[0][0] = new List<string>();
            }
            if(ReturnToCityText != "")
            {
                table.returnToCityIndex = AddString(ReturnToCityText);
            }
            else
            {
                table.returnToCityIndex = -1;
            }
            table.nameIndex = AddString(counterName);
            table.descIndex = AddString(counterDescription);

            foreach (CategoryInfo cat in categories)
            {
                TableFile.QuestGroup grp = new TableFile.QuestGroup();
                grp.questNums.AddRange(cat.categoryNumbersNbls.Select(e=>e.QuestNumber));
                grp.nameIndex = AddString(cat.categoryName);
                grp.descIndex = AddString(cat.categoryDescription);
                table.categories.Add(grp);
            }

            table.cityReturnQuest = CityReturnQuest;
            table.cityReturnZone = CityReturnZone;
            table.cityReturnMap = CityReturnMap;
            table.cityReturnEntry = CityReturnEntry;
            table.unkShort1 = UnkShort1;
            table.unkShort2 = UnkShort2;
            table.timestamp = Timestamp;
            table.startX = StartX;
            table.startY = StartY;
            table.enableCustomBackground = EnableCustomBackground;
            table.backgroundNumber = BackgroundNumber;
            table.defaultIcon = DefaultIcon;
        }

        private short AddString(string textToAdd)
        {
            short stringId = -1;
            if (textFile.strings[0][0].Contains(textToAdd))
                stringId = (short)textFile.strings[0][0].IndexOf(textToAdd);
            else
            {
                stringId = (short)textFile.strings[0][0].Count;
                textFile.strings[0][0].Add(textToAdd);
            }
            return stringId;
        }

        public void PurgeAbsences()
        {
            for(int i = 0; i < categories.Count; i++)
            {
                CategoryInfo cat = categories[i];
                for (int j = 0; j < cat.categoryNumbersNbls.Count; j++) 
                {
                    QuestDefinition qpr = cat.categoryNumbersNbls[j];
                    if (cat.categoryNumbersNbls[j].QuestNbl == null)
                    {
                        cat.categoryNumbersNbls.Remove(qpr);
                        j--;
                    }
                }
                if (cat.categoryNumbersNbls.Count == 0)
                {
                    categories.Remove(cat);
                    i--;
                }
            }
            UpdateTable();
        }
    }
}
