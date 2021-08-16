using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib;

namespace psu_generic_parser
{
    public class AfsLoader : PsuFile, ContainerFile
    {
        public static string[] languages = { "_j", "_ae", "_be", "_g", "_f", "_s", "_i", "_k", "_cs", "_ct" };
        public uint headerLoc;
        public uint headerSize;
        public class AfsFileEntry
        {
            public uint location;
            public uint fileSize;
            public string fileName;
            public ushort year;
            public ushort month;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public uint garbageInt;
            public PsuFile fileContents;
            public byte[] rawContents;
        };

        public List<AfsFileEntry> afsList;

        public bool Encrypted { get => false; set { } }
        public bool Compressed { get => false; set { }  }

        //Initialize empty file.
        public AfsLoader() 
        {
            afsList = new List<AfsFileEntry>();

        }

        public AfsLoader(Stream fileToLoad)
        {
            BinaryReader fileLoader = new BinaryReader(fileToLoad);
            fileToLoad.Seek(0, SeekOrigin.Begin);
            if(new string(Encoding.ASCII.GetChars(fileLoader.ReadBytes(4))) != "AFS\0")
                return;
            fileToLoad.Seek(4, SeekOrigin.Begin);
            int fileCount = fileLoader.ReadInt32();
            afsList = new List<AfsFileEntry>(fileCount);

            for(int i = 0; i < fileCount; i++)
            {
                AfsFileEntry temp = new AfsFileEntry();
                temp.location = fileLoader.ReadUInt32();
                temp.fileSize = fileLoader.ReadUInt32();
                afsList.Add(temp);
            }
            headerLoc = fileLoader.ReadUInt32();
            headerSize = fileLoader.ReadUInt32();

            fileToLoad.Seek(headerLoc, SeekOrigin.Begin);

            for (int i = 0; i < fileCount; i++)
            {
                afsList[i].fileName = Encoding.GetEncoding("shift-jis").GetString(fileLoader.ReadBytes(0x20)).TrimEnd('\0');
                afsList[i].year = fileLoader.ReadUInt16();
                afsList[i].month = fileLoader.ReadUInt16();
                afsList[i].day = fileLoader.ReadUInt16();
                afsList[i].hour = fileLoader.ReadUInt16();
                afsList[i].minute = fileLoader.ReadUInt16();
                afsList[i].second = fileLoader.ReadUInt16();
                afsList[i].garbageInt = fileLoader.ReadUInt32();
            }
            
            for (int i = 0; i < fileCount; i++)
            {
                fileToLoad.Seek(afsList[i].location, SeekOrigin.Begin);
                afsList[i].rawContents = fileLoader.ReadBytes((int)afsList[i].fileSize);
            }
            //subPaths = new AfsLoader[fileCount];
            //nblContents = new NblLoader[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                populateFile(afsList[i]);
            }
            fileLoader.Close();
            fileLoader.Dispose();
            return;
        }

        private void populateFile(AfsFileEntry afsFileEntry)
        {
            if (afsFileEntry.rawContents.Length > 3)
            {
                string headerName = ASCIIEncoding.GetEncoding("ASCII").GetString(afsFileEntry.rawContents, 0, 4);
                if (headerName.StartsWith("AFS"))
                {
                    afsFileEntry.fileContents = new AfsLoader(new MemoryStream(afsFileEntry.rawContents));
                    afsFileEntry.fileContents.filename = afsFileEntry.fileName;
                }
                else if (headerName == "NMLL" || headerName == "NMLB")
                {
                    afsFileEntry.fileContents = new NblLoader(new MemoryStream(afsFileEntry.rawContents));
                    afsFileEntry.fileContents.filename = afsFileEntry.fileName;
                }
            }

            //Handle AFS archive file entries that aren't nbl or are null
            if (afsFileEntry.fileContents == null)
            {
                if (afsFileEntry.rawContents.Length > 0)
                {
                    afsFileEntry.fileContents = new UnpointeredFile(afsFileEntry.fileName, afsFileEntry.rawContents, null);
                    afsFileEntry.fileContents.filename = afsFileEntry.fileName;
                }
                else
                {
                    afsFileEntry.fileContents = new UnpointeredFile(afsFileEntry.fileName, new byte[0], null);
                    afsFileEntry.fileContents.filename = afsFileEntry.fileName;
                }
            }
        }

        public AfsLoader(string inFilename, Stream toLoad) : this(toLoad)
        {
            filename = inFilename;
        }

        public void replaceFile(int index, Stream toImport, string filename)
        {
            BinaryReader beta = new BinaryReader(toImport);
            toImport.Seek(0, SeekOrigin.Begin);
            afsList[index].rawContents = beta.ReadBytes((int)toImport.Length);
            afsList[index].fileName = filename;
            if (ASCIIEncoding.GetEncoding("ASCII").GetString(afsList[index].rawContents, 0, 3) == "AFS")
            {
                afsList[index].fileContents = new AfsLoader(new MemoryStream(afsList[index].rawContents));
            }
            else if (ASCIIEncoding.GetEncoding("ASCII").GetString(afsList[index].rawContents, 0, 4) == "NMLL")
            {
                afsList[index].fileContents = new NblLoader(new MemoryStream(afsList[index].rawContents));
            }
            afsList[index].fileSize = (uint)toImport.Length;
        }

        public void addFile(string filename, Stream toImport)
        {
            AfsFileEntry temp = new AfsFileEntry();
            temp.fileName = filename;
            BinaryReader beta = new BinaryReader(toImport);
            toImport.Seek(0, SeekOrigin.Begin);

            temp.rawContents = beta.ReadBytes((int)toImport.Length);
            if (ASCIIEncoding.GetEncoding("ASCII").GetString(temp.rawContents, 0, 3) == "AFS")
            {
                temp.fileContents = new AfsLoader(new MemoryStream(temp.rawContents));
            }
            else if (ASCIIEncoding.GetEncoding("ASCII").GetString(temp.rawContents, 0, 4) == "NMLL")
            {
                temp.fileContents = new NblLoader(new MemoryStream(temp.rawContents));
            }
            afsList.Add(temp);
        }

        public void saveFile(Stream saveFile)
        {
            BinaryWriter beta = new BinaryWriter(saveFile);
            int fileCount = afsList.Count;
            for (int i = 0; i < fileCount; i++)
            {
                if (afsList[i].fileContents != null)
                {
                    afsList[i].rawContents = afsList[i].fileContents.ToRaw();
                }
            }
            beta.Write(Encoding.ASCII.GetBytes("AFS\0"));
            beta.Write(fileCount);
            uint[] fileLocs = new uint[fileCount + 1];
            fileLocs[0] = (uint)(((fileCount + 1) * 8 + 0x7FF) & 0xFFFF800);
            saveFile.Seek(fileLocs[0], SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                fileLocs[i] = (uint)saveFile.Position;
                beta.Write(afsList[i].rawContents);
            }
            fileLocs[fileCount] = (uint)saveFile.Position;
            for (int i = 0; i < fileCount; i++)
            {
                beta.Write(ContainerUtilities.encodePaddedSjisString(afsList[i].fileName, 0x20));
                beta.Write(new byte[0x10]);
            }
            beta.Write(new byte[((fileCount * 0x30) + 0x7FF) & 0x800]);
            saveFile.Seek(8, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                beta.Write(fileLocs[i]);
                beta.Write(afsList[i].rawContents.Length);
            }
            beta.Write(fileLocs[fileCount]);
            beta.Write(fileCount * 0x30);
            beta.Close();
        }

        public override byte[] ToRaw()
        {
            MemoryStream temp = new MemoryStream();
            saveFile(temp);
            return temp.ToArray();
        }

        public void addZone(int zoneNum, Stream zoneToAdd)
        {
            BinaryReader zoneLoader = new BinaryReader(zoneToAdd);
            byte[] loadedZone = zoneLoader.ReadBytes((int)zoneToAdd.Length);
            for (int i = 0; i < languages.Length; i++)
            {
                AfsFileEntry temp = new AfsFileEntry();
                temp.rawContents = loadedZone;
                temp.fileName = "zone" + zoneNum.ToString("D2") + languages[i] + ".nbl";
                temp.fileSize = (uint)loadedZone.Length;
                temp.fileContents = new NblLoader(new MemoryStream(temp.rawContents));
                temp.fileContents.filename = temp.fileName;
                afsList.Add(temp);
            }
        }

        public void setQuest(Stream questFile)
        {
            BinaryReader questLoader = new BinaryReader(questFile);
            byte[] loadedQuest = questLoader.ReadBytes((int) questFile.Length);
            for (int i = 0; i < afsList.Count; i++)
            {
                if (afsList[i].fileName.Contains("quest"))
                {
                    afsList[i].fileSize = (uint)loadedQuest.Length;
                    afsList[i].rawContents = loadedQuest;
                    afsList[i].fileContents = new NblLoader(new MemoryStream(loadedQuest));
                    afsList[i].fileContents.filename = afsList[i].fileName;
                }
            }
        }

        public void setZone(int zoneNum, Stream zoneFile)
        {
            BinaryReader zoneLoader = new BinaryReader(zoneFile);
            byte[] loadedZone = zoneLoader.ReadBytes((int)zoneFile.Length);
            for(int i = 0; i < afsList.Count; i++)
            {
                if(afsList[i].fileName.Contains("zone" + zoneNum.ToString("D2")))
                {
                    afsList[i].fileSize = (uint)loadedZone.Length;
                    afsList[i].rawContents = loadedZone;
                    afsList[i].fileContents = new NblLoader(new MemoryStream(loadedZone));
                    afsList[i].fileContents.filename = afsList[i].fileName;
                }
            }
        }

        public static void createFromDirectory(string directoryName, string outputName)
        {
            string[] filenames = Directory.GetFiles(directoryName);
            Array.Sort(filenames);
            int fileCount = filenames.Length;
            FileStream outStream = new FileStream(outputName, FileMode.Create);
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write(Encoding.ASCII.GetBytes("AFS\0"));
            outWriter.Write(fileCount);
            int[] fileLocs = new int[fileCount];
            int[] fileSizes = new int[fileCount];
            int metadataLoc = 0;
            int metadataLength = fileCount * 0x30;
            outStream.Seek(((fileCount + 1) * 8 + 0x7FF) & 0xFFFF800, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                byte[] currFile = File.ReadAllBytes(filenames[i]);
                fileLocs[i] = (int)outStream.Position;
                fileSizes[i] = currFile.Length;
                outWriter.Write(currFile);
                outStream.Seek((outStream.Position + 0x7FF) & 0xFFFF800, SeekOrigin.Begin);
            }
            metadataLoc = (int)outStream.Position;
            for (int i = 0; i < fileCount; i++)
            {
                outWriter.Write(ContainerUtilities.encodePaddedSjisString(Path.GetFileName(filenames[i]), 0x20));
                outWriter.Write((int)0);
                outWriter.Write((int)0);
                outWriter.Write((int)0);
                outWriter.Write((int)0);
            }
            outWriter.Write(new byte[(outStream.Position + 0x7FF) & 0x800]);
            outStream.Seek(8, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                outWriter.Write(fileLocs[i]);
                outWriter.Write(fileSizes[i]);
            }
            outWriter.Write(metadataLoc);
            outWriter.Write(metadataLength);
            outWriter.Close();
        }

        #region ContainerFile Members

        public List<string> getFilenames()
        {
            return afsList.Select(file => file.fileName).ToList();
        }

        public PsuFile getFileParsed(string filename)
        {
            return afsList.First(file => file.fileName == filename).fileContents;
        }

        public RawFile getFileRaw(string filename)
        {
            throw new NotImplementedException("Not ready yet");
        }

        public PsuFile getFileParsed(int fileIndex)
        {
            return afsList[fileIndex].fileContents;
        }

        public RawFile getFileRaw(int fileIndex)
        {
            throw new NotImplementedException("Not ready yet");
        }

        public void replaceFile(string filename, RawFile toReplace)
        {
            int index = afsList.FindIndex(file => file.fileName == filename);
            replaceFile(index, new MemoryStream(toReplace.fileContents), toReplace.filename);
        }

        public void replaceFile(int index, RawFile toReplace)
        {
            replaceFile(index, new MemoryStream(toReplace.fileContents), toReplace.filename);
        }

        #endregion

        public static AfsLoader CreateMissionAfs(RawFile questNbl, Dictionary<int, RawFile> zoneNbls, bool createAllLanguages)
        {
            AfsLoader toReturn = new AfsLoader();
            int highestZone = zoneNbls.Keys.Max();
            AddLanguages(toReturn, questNbl, "quest", createAllLanguages);

            //Alright, so.
            //0-1-10-11-...2-20-21...-
            for (int i = 0; i < 10; i++)
            {
                if (zoneNbls.ContainsKey(i))
                    AddLanguages(toReturn, zoneNbls[i], "zone" + i.ToString("D2"), createAllLanguages);
                if (i > 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (zoneNbls.ContainsKey(i*10 + j))
                            AddLanguages(toReturn, zoneNbls[i], "zone" + (i * 10 + j).ToString("D2"), createAllLanguages);
                    }
                }
            }
            return toReturn;
        }

        private static void AddLanguages(AfsLoader containingFile, RawFile fileToAdd, string filenamePrefix, bool createAllLanguages)
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (createAllLanguages || languages[i] == "_ae")
                    containingFile.afsList.Add(new AfsFileEntry { rawContents = fileToAdd.fileContents, fileName = filenamePrefix + languages[i] + ".nbl" });
                else
                    containingFile.afsList.Add(new AfsFileEntry { fileName = filenamePrefix + languages[i] + ".nbl" });
            }
        }

        public void addFile(int index, RawFile toAdd)
        {
            afsList.Insert(index, new AfsFileEntry { rawContents = toAdd.fileContents, fileName = toAdd.filename });
        }

        public bool doesFileExist(string filename)
        {
            return afsList.Any(afsFile => afsFile.fileName == filename);
        }

        public bool isParsedFileCached(string filename)
        {
            return afsList.Any(afsFile => afsFile.fileName == filename && afsFile.fileContents != null);
        }

        public bool isParsedFileCached(int fileIndex)
        {
            return afsList.Count > fileIndex && afsList[fileIndex].fileContents != null;
        }
    }
}
