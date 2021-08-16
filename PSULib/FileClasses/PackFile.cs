using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    //A pack file is a container only for NBLs, so it's much easier!
    public class PackFile : PsuFile, ContainerFile
    {
        public Dictionary<int, NblLoader> containedFiles = new Dictionary<int, NblLoader>();
        public Dictionary<int, RawFile> containedRawFiles = new Dictionary<int, RawFile>();
        public List<string> filenames = new List<string>();

        public PackFile(string inFilename)
        {
            filename = inFilename;
        }

        public PackFile(Stream fileToLoad)
        {
            BinaryReader fileLoader = new BinaryReader(fileToLoad);
            fileToLoad.Seek(0x400, SeekOrigin.Begin);
            int fileCount = fileLoader.ReadInt32();
            int[] fileLocs = new int[fileCount];
            int[] fileSizes = new int[fileCount];

            fileToLoad.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
                fileLocs[i] = fileLoader.ReadInt32();

            fileToLoad.Seek(0x200, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
                fileSizes[i] = fileLoader.ReadInt32();

            fileToLoad.Seek(0x404, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                if (fileSizes[i] > 0)
                {
                    byte[] currentFile = fileLoader.ReadBytes(fileSizes[i]);
                    MemoryStream nblStream = new MemoryStream(currentFile);
                    RawFile raw = new RawFile();
                    raw.fileContents = currentFile;
                    raw.filename = i > 0 ? "nbl" + i.ToString("d4") + ".nbl" : "table.nbl";
                    NblLoader currentNbl = new NblLoader(nblStream);
                    containedFiles.Add(i, currentNbl);
                    containedRawFiles.Add(i, raw);
                    filenames.Add(raw.filename);
                    nblStream.Close();
                }
            }
        }

        public bool Encrypted { get => false; set { } }
        public bool Compressed { get => false; set { } }

        public void addFile(int index, RawFile toAdd)
        {
            throw new NotImplementedException();
        }

        public bool doesFileExist(string filename)
        {
            return filenames.Contains(filename);
        }

        public List<string> getFilenames()
        {
            return filenames;
        }

        public PsuFile getFileParsed(int fileIndex)
        {
            if(containedFiles.ContainsKey(fileIndex))
                return containedFiles[fileIndex];
            return null;
        }

        public PsuFile getFileParsed(string filename)
        {
            if (filenames.Contains(filename))
                return (containedFiles[filenames.IndexOf(filename)]);
            return null;
        }

        public RawFile getFileRaw(int fileIndex)
        {
            if (containedRawFiles.ContainsKey(fileIndex))
                return containedRawFiles[fileIndex];
            return null;
        }

        public RawFile getFileRaw(string filename)
        {
            if (filenames.Contains(filename))
                return (containedRawFiles[filenames.IndexOf(filename)]);
            return null;
        }

        public bool isParsedFileCached(string filename)
        {
            return containedFiles.ContainsKey(filenames.IndexOf(filename));
        }

        public bool isParsedFileCached(int fileIndex)
        {
            return containedFiles.ContainsKey(fileIndex);
        }

        public void replaceFile(string filename, RawFile toReplace)
        {
            if (filenames.Contains(filename))
            { 
                containedFiles[filenames.IndexOf(filename)] = new NblLoader(new MemoryStream(toReplace.fileContents));
                containedRawFiles[filenames.IndexOf(filename)] = toReplace;
            }
        }

        public void replaceFile(int index, RawFile toReplace)
        {
            containedFiles[index] = new NblLoader(new MemoryStream(toReplace.fileContents));
            containedRawFiles[index] = toReplace;
        }

        public void saveFile(Stream outStream)
        {
            this.ToRawFile(0).WriteToStream(outStream);
        }

        public override byte[] ToRaw()
        {
            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outStream.Seek(0x404, SeekOrigin.Begin);

            int[] fileLocs = new int[containedFiles.Count];
            int[] fileSizes = new int[containedFiles.Count];

            byte[][] rawFiles = new byte[containedFiles.Count][];
            int writtenFiles = 0;

            for (int i = 0; i < containedFiles.Count; i++)
            {
                if (containedFiles.ContainsKey(i) && containedFiles[i] != null)
                {
                    if (containedFiles[i].dirty || !containedRawFiles.ContainsKey(i))
                        rawFiles[i] = containedFiles[i].ToRaw();
                    else
                        rawFiles[i] = containedRawFiles[i].fileContents;
                    fileLocs[i] = (int)outStream.Position - 0x404;
                    writtenFiles++;
                    outWriter.Write(rawFiles[i]);
                }
                else
                {
                    rawFiles[i] = new byte[0];
                    fileLocs[i] = 0;
                }
            }
            outStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < containedFiles.Count; i++)
            {
                outWriter.Write(fileLocs[i]);
            }
            outStream.Seek(0x200, SeekOrigin.Begin);
            for (int i = 0; i < containedFiles.Count; i++)
            {
                outWriter.Write(rawFiles[i].Length);
            }
            outStream.Seek(0x400, SeekOrigin.Begin);
            outWriter.Write(fileLocs.Length);
            return outStream.ToArray();
        }
    }
}
