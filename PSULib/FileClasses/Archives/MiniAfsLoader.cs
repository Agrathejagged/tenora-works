using PSULib.FileClasses;
using PSULib.FileClasses.General;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Archives
{
    public class MiniAfsLoader : PsuFile, ContainerFile
    {
        public static string[] languages = { "_j", "_ae", "_be", "_g", "_f", "_s", "_i", "_k", "_cs", "_ct" };
        public class MiniAfsFileEntry
        {
            public int location;
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

        public List<MiniAfsFileEntry> afsList;

        public bool Encrypted { get => false; set { } }
        public bool Compressed { get => false; set { } }

        //Initialize empty file.
        public MiniAfsLoader()
        {
            afsList = new List<MiniAfsFileEntry>();

        }

        public MiniAfsLoader(Stream fileToLoad)
        {
            BinaryReader fileLoader = new BinaryReader(fileToLoad);
            fileToLoad.Seek(0, SeekOrigin.Begin);
            if (fileLoader.ReadInt16() != 0x50AF)
                Console.Out.WriteLine("Invalid file.");
            int fileCount = fileLoader.ReadInt16();
            fileLoader.ReadInt32(); //Discarding the unknown.
            afsList = new List<MiniAfsFileEntry>(fileCount);

            for (int i = 0; i < fileCount; i++)
            {
                MiniAfsFileEntry temp = new MiniAfsFileEntry();
                temp.location = fileLoader.ReadUInt16() * 0x800;
                afsList.Add(temp);
            }
            int headerLoc = fileLoader.ReadUInt16() * 0x800;

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
                int nextLocation = i + 1 < fileCount ? afsList[i + 1].location : headerLoc;
                int fileLength = nextLocation - afsList[i].location;
                afsList[i].rawContents = fileLoader.ReadBytes(fileLength);
            }
            for (int i = 0; i < fileCount; i++)
            {
                if (afsList[i].rawContents.Length > 3)
                {
                    string headerName = Encoding.GetEncoding("ASCII").GetString(afsList[i].rawContents, 0, 4);
                    if (headerName.StartsWith("AFS"))
                    {
                        afsList[i].fileContents = new AfsLoader(new MemoryStream(afsList[i].rawContents));
                    }
                    else if (headerName == "NMLL" || headerName == "NMLB")
                    {
                        afsList[i].fileContents = new NblLoader(new MemoryStream(afsList[i].rawContents));
                    }
                }
            }
            fileLoader.Close();
            fileLoader.Dispose();
            return;
        }

        public MiniAfsLoader(string inFilename, Stream toLoad) : this(toLoad)
        {
            filename = inFilename;
        }

        public void replaceFile(int index, Stream toImport)
        {
            BinaryReader beta = new BinaryReader(toImport);
            toImport.Seek(0, SeekOrigin.Begin);
            afsList[index].rawContents = beta.ReadBytes((int)toImport.Length);
            if (Encoding.GetEncoding("ASCII").GetString(afsList[index].rawContents, 0, 3) == "AFS")
            {
                afsList[index].fileContents = new AfsLoader(new MemoryStream(afsList[index].rawContents));
            }
            else if (Encoding.GetEncoding("ASCII").GetString(afsList[index].rawContents, 0, 4) == "NMLL")
            {
                afsList[index].fileContents = new NblLoader(new MemoryStream(afsList[index].rawContents));
            }
        }

        public void addFile(string filename, Stream toImport)
        {
            MiniAfsFileEntry temp = new MiniAfsFileEntry();
            temp.fileName = filename;
            BinaryReader beta = new BinaryReader(toImport);
            toImport.Seek(0, SeekOrigin.Begin);

            temp.rawContents = beta.ReadBytes((int)toImport.Length);
            if (Encoding.GetEncoding("ASCII").GetString(temp.rawContents, 0, 3) == "AFS")
            {
                temp.fileContents = new AfsLoader(new MemoryStream(temp.rawContents));
            }
            else if (Encoding.GetEncoding("ASCII").GetString(temp.rawContents, 0, 4) == "NMLL")
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
                    afsList[i].rawContents = afsList[i].fileContents.ToRaw();
            }
            beta.Write((short)0x50AF);
            beta.Write((short)fileCount);
            uint[] fileLocs = new uint[fileCount + 1];
            fileLocs[0] = (uint)((fileCount + 1) * 8 + 0x7FF & 0xFFFF800);
            saveFile.Seek(fileLocs[0], SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                fileLocs[i] = (uint)saveFile.Position;
                beta.Write(afsList[i].rawContents);
            }
            fileLocs[fileCount] = (uint)saveFile.Position;
            for (int i = 0; i < fileCount; i++)
            {
                beta.Write(StringUtilities.encodePaddedSjisString(afsList[i].fileName, 0x20));
                beta.Write(new byte[0x10]);
            }
            beta.Write(new byte[fileCount * 0x30 + 0x7FF & 0x800]);
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
            outStream.Seek((fileCount + 1) * 8 + 0x7FF & 0xFFFF800, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                byte[] currFile = File.ReadAllBytes(filenames[i]);
                fileLocs[i] = (int)outStream.Position;
                fileSizes[i] = currFile.Length;
                outWriter.Write(currFile);
                outStream.Seek(outStream.Position + 0x7FF & 0xFFFF800, SeekOrigin.Begin);
            }
            metadataLoc = (int)outStream.Position;
            for (int i = 0; i < fileCount; i++)
            {
                outWriter.Write(StringUtilities.encodePaddedSjisString(Path.GetFileName(filenames[i]), 0x20));
                outWriter.Write(0);
                outWriter.Write(0);
                outWriter.Write(0);
                outWriter.Write(0);
            }
            outWriter.Write(new byte[outStream.Position + 0x7FF & 0x800]);
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
            replaceFile(index, new MemoryStream(toReplace.fileContents));
        }

        public void replaceFile(int index, RawFile toReplace)
        {
            replaceFile(index, new MemoryStream(toReplace.fileContents));
        }

        #endregion

        public void addFile(int index, RawFile toAdd)
        {
            MiniAfsFileEntry miniAfsFileEntry = new MiniAfsFileEntry();
            miniAfsFileEntry.fileName = toAdd.filename;
            miniAfsFileEntry.rawContents = toAdd.WriteToBytes();
            afsList.Insert(index, miniAfsFileEntry);
            if (Encoding.GetEncoding("ASCII").GetString(miniAfsFileEntry.rawContents, 0, 3) == "AFS")
            {
                miniAfsFileEntry.fileContents = new AfsLoader(new MemoryStream(miniAfsFileEntry.rawContents));
            }
            else if (Encoding.GetEncoding("ASCII").GetString(miniAfsFileEntry.rawContents, 0, 4) == "NMLL")
            {
                miniAfsFileEntry.fileContents = new NblLoader(new MemoryStream(miniAfsFileEntry.rawContents));
            }
        }

        public bool doesFileExist(string filename)
        {
            throw new NotImplementedException();
        }

        public bool isParsedFileCached(string filename)
        {
            throw new NotImplementedException();
        }

        public bool isParsedFileCached(int fileIndex)
        {
            throw new NotImplementedException();
        }

        public void removeFile(int index)
        {
            afsList.RemoveAt(index);
        }

        public void removeFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void renameFile(string filename, string newFilename)
        {
            throw new NotImplementedException();
        }

        public void renameFile(int index, string newFilename)
        {
            throw new NotImplementedException();
        }
    }
}
