using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace psu_generic_parser
{
    public class NblLoader : PsuFile, ContainerFile
    {
        //REL/XNJ/etc headers
        public struct FileHeader
        {
            public string identifier;
            public uint chunkSize; //Always 0xwhatever, but still tracking it... because?
            public uint unknown1; // Always 0?
            public uint unknown2; // Always 0?
            // ENCRYPTED SECTION
            public string fileName;
            public uint filePosition;
            public uint fileSize;
            public uint pointerPosition;   //Still defined even if none exist for file, have to look at size.
            public uint pointerSize;   //0 if none used
            // END ENCRYPTED SECTION
            public byte[] subHeader;
            /*
            public string nxifHeader;  //Only there if calculated pointers exist
            public uint unknown3;  // Always 0x18
            public uint unknown4;  // Always 01
            public uint unknown5;  // Always 0x20
            public uint nxifFilesize;  // Matches above 
            public uint nxifFilepad;   // Filesize + 0x20
            public uint nxifPointpad;  // (Pointer length + 0x1F) & 0xFFFFFFF0
            public uint unknown9;  //Gaaaaaah this stuff is file dependent~
             */
        }

        public List<NblChunk> chunks = new List<NblChunk>();
        public bool isCompressed { get { return chunks.Count > 0 && chunks[0].compressed; } }
                
        public bool isBigEndian = false;

        //Calculated data.
        uint tmllHeaderLoc = 0;
        BlewFish decryptor;
        uint decryptKey = 0;

        //Filedata
        byte[] decompressedFiles;

        public NblLoader(string filePath)
        {
            //Just in here for future use.
        }

        public NblLoader()
        {
            NblChunk newChunk = new NblChunk();
            newChunk.encryptionKey = decryptKey;
            newChunk.encrypted = decryptKey != 0;
            newChunk.compressed = false;
            newChunk.chunkID = "NMLL";
            newChunk.versionNumber = (short)2;
            chunks.Insert(0, newChunk);
        }

        public NblLoader(Stream fileToLoad)
        {
            fileToLoad.Seek(0x3, SeekOrigin.Begin);
            int endian = fileToLoad.ReadByte();
            isBigEndian = (endian == 0x42);
            BinaryReader fileLoader = getBinaryReader(fileToLoad);
            fileToLoad.Seek(0, SeekOrigin.Begin);
            NblChunk firstChunk = loadGroup(fileToLoad, fileLoader);
            chunks.Add(firstChunk);
            if (tmllHeaderLoc != 0)
            {
                fileToLoad.Seek(tmllHeaderLoc, SeekOrigin.Begin);
                NblChunk tmllChunk = loadGroup(fileToLoad, fileLoader);
                chunks.Add(tmllChunk);
            }
            fileLoader.Close();
            fileLoader.Dispose();
        }

        public void addNmllFile(RawFile file)
        {
            chunks[0].addFile(file);
        }

        public NblLoader(List<RawFile> nmllFiles, List<RawFile> tmllFiles)
        {
            if (nmllFiles != null && nmllFiles.Count > 0)
            {
                NblChunk nmllChunk = new NblChunk { chunkID = "NMLL", versionNumber = 2, fileContents = nmllFiles };
                chunks.Add(nmllChunk);
            }

            if (tmllFiles != null && tmllFiles.Count > 0)
            {
                NblChunk tmllChunk = new NblChunk { chunkID = "TMLL", versionNumber = 2, fileContents = tmllFiles };
                chunks.Add(tmllChunk);
            }
        }
        public void addTmllFile(RawFile file)
        {
            if (chunks.Count < 2 || chunks[1].chunkID != "TMLL")
            {
                NblChunk newChunk = new NblChunk();
                newChunk.encryptionKey = 0;
                newChunk.encrypted = false;
                newChunk.compressed = false;
                newChunk.chunkID = "TMLL";
                newChunk.versionNumber = (short)2;
                chunks.Insert(1, newChunk);
            }
            chunks[1].addFile(file);
        }

        public NblLoader(List<RawFile> nmllFiles) : this(nmllFiles, null)
        {

        }

        private NblChunk loadGroup(Stream fileToLoad, BinaryReader fileLoader)
        {
            NblChunk toRet = new NblChunk();
            toRet.bigEndian = isBigEndian;
            long offset = fileToLoad.Position;

            string formatName = new String(fileLoader.ReadChars(4));
            ushort fileVersion = fileLoader.ReadUInt16();
            int paddingAmount = fileVersion == 0x1002 ? 0x3F : 0x7FF;
            uint mask = fileVersion == 0x1002 ? 0xFFFFFFC0 : 0xFFFFF800;
            ushort chunkFilenameLength = fileLoader.ReadUInt16();
            int headerSize = fileLoader.ReadInt32();
            int numFiles = fileLoader.ReadInt32();
            uint uncompressedSize = fileLoader.ReadUInt32();
            uint compressedSize = fileLoader.ReadUInt32();
            uint pointerLength = fileLoader.ReadUInt32() / 4;
            if (formatName.StartsWith("NML"))
                decryptKey = fileLoader.ReadUInt32();
            else
                fileLoader.ReadUInt32();
            uint size = compressedSize == 0 ? uncompressedSize : compressedSize;

            uint nmllDataLoc = (uint)((headerSize + paddingAmount) & mask);
            uint pointerLoc = (uint)(nmllDataLoc + size + paddingAmount) & mask;
            uint mainHeaderSize = 0x20;

            if (formatName.StartsWith("NML"))
            {
                mainHeaderSize = 0x30;
                uint tmllHeaderSize = fileLoader.ReadUInt32();
                uint tmllDataSizeUncomp = fileLoader.ReadUInt32();
                uint tmllDataSizeComp = fileLoader.ReadUInt32();
                uint tmllCount = fileLoader.ReadUInt32();
                if (tmllCount > 0)
                    tmllHeaderLoc = (uint)(pointerLoc + pointerLength * 4 + paddingAmount) & mask;
            }

            decryptor = new BlewFish(decryptKey, isBigEndian);
            FileHeader[] groupHeaders = new FileHeader[numFiles];

            for (int i = 0; i < numFiles; i++)
            {
                fileToLoad.Seek(mainHeaderSize + 0x60 * i + offset, SeekOrigin.Begin);
                FileHeader currentHeader = readHeader(fileLoader.ReadBytes(0x60));
                groupHeaders[i] = currentHeader;
            }

            fileToLoad.Seek(nmllDataLoc + offset, SeekOrigin.Begin);
            int encryptedSectionSize;
            if (fileVersion == 0x1002)
            {
                //Obfuscation stuff from sega.
                int rawEncryptedSectionSize = (int)((((compressedSize >> 0xB) ^ compressedSize) & 0xE0) + 0x20);
                encryptedSectionSize = Math.Min(rawEncryptedSectionSize, (int)compressedSize);
            }
            else
            {
                encryptedSectionSize = (int)size;
            }

            if (encryptedSectionSize % 8 != 0)
            {
                encryptedSectionSize -= (encryptedSectionSize % 8);
            }

            byte[] decryptedFiles;
            if (decryptKey != 0 && formatName.StartsWith("NML"))
            {
                byte[] encryptedFiles = fileLoader.ReadBytes((int)size);
                decryptedFiles = decryptor.decryptBlock(encryptedFiles, encryptedSectionSize);
            }
            else
                decryptedFiles = fileLoader.ReadBytes((int)size + 7);

            if (compressedSize != 0)
            {
                if (fileVersion == 0x1002)
                {
                    DeflateStream ds = new DeflateStream(new MemoryStream(decryptedFiles), CompressionMode.Decompress);
                    MemoryStream decompressedStream = new MemoryStream((int)uncompressedSize);
                    ds.CopyTo(decompressedStream);
                    decompressedFiles = decompressedStream.ToArray();
                }
                else
                {
                    decompressedFiles = PrsCompDecomp.Decompress(decryptedFiles, uncompressedSize);
                }
            }
            else
                decompressedFiles = decryptedFiles;

            List<int> pointers = new List<int>((int)pointerLength);

            if (pointerLength > 0)
            {
                fileToLoad.Seek(pointerLoc + offset, SeekOrigin.Begin);
                for (int i = 0; i < pointerLength; i++)
                    pointers.Add(fileLoader.ReadInt32());
            }

            List<RawFile> files = new List<RawFile>(numFiles);
            for (int i = 0; i < numFiles; i++)
            {
                RawFile tempFile = new RawFile();
                tempFile.filename = groupHeaders[i].fileName;
                tempFile.subHeader = groupHeaders[i].subHeader;
                tempFile.chunkSize = groupHeaders[i].chunkSize;
                tempFile.fileOffset = groupHeaders[i].filePosition;

                tempFile.fileContents = new byte[groupHeaders[i].fileSize];
                Array.Copy(decompressedFiles, groupHeaders[i].filePosition, tempFile.fileContents, 0, groupHeaders[i].fileSize);
                if (tempFile.fileContents.Length > 0)
                {
                    tempFile.fileheader = new String(ASCIIEncoding.ASCII.GetChars(tempFile.fileContents, 0, 4));
                }
                if (groupHeaders[i].pointerSize > 0)
                    tempFile.pointers = pointers.GetRange((int)groupHeaders[i].pointerPosition / 4, (int)groupHeaders[i].pointerSize / 4);
                files.Add(tempFile);
            }
            toRet.encryptionKey = decryptKey;
            toRet.encrypted = decryptKey != 0;
            toRet.compressed = compressedSize != 0;
            toRet.chunkID = formatName;
            toRet.versionNumber = (short)fileVersion;
            toRet.fileContents = files;
            return toRet;
        }

        public void exportDataBlob(string filePath)
        {
            string fullPath = filePath + @"\exported.dat";
            File.WriteAllBytes(fullPath, decompressedFiles);
        }

        //TODO: Allow saving files in PSP2i format (using partial encrypt and deflate instead of PRS)
        public void saveFile(Stream fileToWrite, bool compressNmll, bool compressTmll, bool saveUnmodified)
        {
            BinaryWriter beta;
            if (isBigEndian)
                beta = new BigEndianBinaryWriter(fileToWrite);
            else
                beta = new BinaryWriter(fileToWrite);
            //A bit of logic just in case we're loading naked TMLL chunks for some reason.
            bool startsWithNmll = (chunks[0].chunkID.StartsWith("NML"));
            uint tmllHeaderLength = 0;
            uint tmllLength = 0;
            uint tmllCompressed = 0;
            int tmllFiles = 0;
            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].chunkID.StartsWith("NML"))
                    chunks[i].compressed = compressNmll;
                if (chunks[i].chunkID.StartsWith("TML"))
                {
                    chunks[i].compressed = compressTmll;
                }
                byte[] currentChunkData = chunks[i].SaveFile(saveUnmodified);
                //Fill in TMLL info for NMLL header (yep)
                //Ignoring the case where you have multiple TMLLs (this is a damaged file, anyway)
                if (chunks[i].chunkID.StartsWith("TML"))
                {
                    tmllHeaderLength = BitConverter.ToUInt32(currentChunkData, 0x8);
                    tmllLength = BitConverter.ToUInt32(currentChunkData, 0x10);
                    tmllCompressed = BitConverter.ToUInt32(currentChunkData, 0x14);
                    tmllFiles = chunks[i].fileContents.Count;
                }
                beta.Write(currentChunkData);
            }

            if (startsWithNmll)
            {
                fileToWrite.Seek(0x20, SeekOrigin.Begin);
                beta.Write(tmllHeaderLength);
                beta.Write(tmllLength);
                beta.Write(tmllCompressed);
                beta.Write(tmllFiles);
            }
            beta.Close();
        }

        private BinaryReader getBinaryReader(Stream s)
        {
            return BigEndianBinaryReader.GetEndianSpecificBinaryReader(s, isBigEndian);
        }

        private FileHeader readHeader(byte[] headerData)
        {
            FileHeader toReturn = new FileHeader();
            MemoryStream headerStream = new MemoryStream(headerData);
            BinaryReader headerReader = getBinaryReader(headerStream);
            toReturn.identifier = new string(headerReader.ReadChars(4));
            toReturn.chunkSize = headerReader.ReadUInt32();
            toReturn.unknown1 = headerReader.ReadUInt32();
            toReturn.unknown2 = headerReader.ReadUInt32();
            byte[] decryptHeader = new byte[0x30];
            if (decryptKey != 0)
            {
                decryptHeader = decryptor.decryptBlock(headerReader.ReadBytes(0x30));
            }
            else
            {
                decryptHeader = headerReader.ReadBytes(0x30);
            }
            MemoryStream alpha = new MemoryStream(decryptHeader);
            BinaryReader beta = getBinaryReader(alpha);
            toReturn.fileName = new string(beta.ReadChars(0x20)).TrimEnd('\0');
            toReturn.filePosition = beta.ReadUInt32();
            toReturn.fileSize = beta.ReadUInt32();
            toReturn.pointerPosition = beta.ReadUInt32();
            toReturn.pointerSize = beta.ReadUInt32();
            toReturn.subHeader = headerReader.ReadBytes(0x20);
            return toReturn;
        }

        public void replaceFile(string toReplace, Stream importFile)
        {

            BinaryReader inReader = getBinaryReader(importFile);
            string identifier = ASCIIEncoding.ASCII.GetString(inReader.ReadBytes(4));
            importFile.Seek(0x10, SeekOrigin.Begin);
            string actualName = ASCIIEncoding.ASCII.GetString(inReader.ReadBytes(0x20));
            actualName.TrimEnd('\0');
            int baseAddr = inReader.ReadInt32();
            int fileLength = inReader.ReadInt32();
            int chunkSize = inReader.ReadInt32();
            int pointerCount = inReader.ReadInt32();
            byte[] subHeader = inReader.ReadBytes(0x20);
            byte[] rawFile = inReader.ReadBytes(fileLength);
            importFile.Seek((importFile.Position + 0x7F) & 0xFFFFFF80, SeekOrigin.Begin);
            List<int> pointers = new List<int>();
            for (int i = 0; i < pointerCount; i++)
            {
                pointers.Add(inReader.ReadInt32());
            }
            RawFile tempFile = new RawFile();
            tempFile.pointers = pointers;
            tempFile.subHeader = subHeader;
            tempFile.fileOffset = 0;
            tempFile.chunkSize = (uint)chunkSize;
            tempFile.filename = filename;
            tempFile.fileheader = identifier;
            tempFile.fileContents = rawFile;
            replaceFile(filename, tempFile);
        }

        public override byte[] ToRaw()
        {
            MemoryStream toOutput = new MemoryStream();
            saveFile(toOutput, false, false, false);
            return toOutput.ToArray();
        }

        #region ContainerFile Members

        public List<string> getFilenames()
        {
            return chunks.Select(chunk => chunk.chunkID + " chunk").ToList();
        }

        public PsuFile getFileParsed(string filename)
        {
            return chunks.First(chunk => chunk.chunkID + " chunk" == filename);
        }

        public RawFile getFileRaw(string filename)
        {
            throw new NotImplementedException();
        }

        public PsuFile getFileParsed(int fileIndex)
        {
            return chunks[fileIndex];
        }

        public RawFile getFileRaw(int fileIndex)
        {
            throw new NotImplementedException();
        }

        public void replaceFile(string filename, RawFile toReplace)
        {
            foreach (NblChunk chunk in chunks)
            {
                if (chunk.getFilenames().Contains(filename))
                {
                    chunk.replaceFile(filename, toReplace);
                }
            }
        }

        public void addFile(int index, RawFile toAdd)
        {
            throw new NotImplementedException();
        }
        /*
public override bool MatchesFile(string filename, byte[] fileContents, int[] pointers)
{
   if (filename.EndsWith(".nbl") || ASCIIEncoding.ASCII.GetString(fileContents, 0, 4) == "NMLL")
       return true;
   return false;
}*/

        #endregion
    }

    public class NblChunk : PsuFile, ContainerFile
    {
        public string chunkID;
        public short versionNumber;
        public bool compressed = false;
        public bool encrypted = false;
        public bool bigEndian = false;
        public uint encryptionKey;
        public List<RawFile> fileContents = new List<RawFile>();
        Dictionary<string, PsuFile> loadedFileCache = new Dictionary<string, PsuFile>();

        public byte[] SaveFile(bool discardChanges)
        {
            //First, combine all the files.
            MemoryStream groupFileStream = new MemoryStream();
            MemoryStream groupHeaderStream = new MemoryStream();
            BinaryWriter groupHeaderWriter;
            if (bigEndian)
                groupHeaderWriter = new BigEndianBinaryWriter(groupHeaderStream);
            else
                groupHeaderWriter = new BinaryWriter(groupHeaderStream);
            int paddingAmount = versionNumber == 0x1002 ? 0x3F : 0x7FF;
            uint mask = versionNumber == 0x1002 ? 0xFFFFFFC0 : 0xFFFFF800;

            if (this.chunkID.StartsWith("NML"))
                groupHeaderStream.Seek(0x30, SeekOrigin.Begin);
            else if (this.chunkID.StartsWith("TML"))
                groupHeaderStream.Seek(0x20, SeekOrigin.Begin);

            NblLoader.FileHeader[] headers = new NblLoader.FileHeader[this.fileContents.Count];
            List<RawFile> savedFiles = new List<RawFile>(this.fileContents);
            List<int> pointers = new List<int>();
            //Annoying, this one has to be a running tally, can't do it any other way.
            ushort filenamelength = 0;
            for (int i = 0; i < fileContents.Count; i++)
            {
                //Figure out whether to take the cached copy or the original
                if (this.loadedFileCache.ContainsKey(fileContents[i].filename) && !discardChanges)
                {
                    savedFiles[i] = loadedFileCache[fileContents[i].filename].ToRawFile((uint)groupFileStream.Position);
                    savedFiles[i].chunkSize = fileContents[i].chunkSize;
                }
                else if (savedFiles[i].fileOffset != (uint)groupFileStream.Position)
                {
                    savedFiles[i].RebaseFile((uint)groupFileStream.Position);
                }
                //Let's not use a FileHeader any more. Just put all the data straight into the file.
                //Guessing on identifier--this SHOULD be true, generally, but...?
                //This needs to be stored in the file class.
                string identifier = "STD\0";
                if (this.chunkID.StartsWith("TML"))
                    identifier = "NNVR";
                else if (savedFiles[i].subHeader != null && savedFiles[i].subHeader[0] == 0x4E) //'N'--so hopefully just NXIF or NUIF
                    identifier = Path.GetExtension(savedFiles[i].filename).Substring(1).ToUpper().PadRight(4, '\0');
                groupHeaderWriter.Write(ASCIIEncoding.ASCII.GetBytes(identifier));
                groupHeaderWriter.Write(savedFiles[i].chunkSize);
                groupHeaderWriter.Write((int)0); //"unknown1"
                groupHeaderWriter.Write((int)0); //"unknown2"
                groupHeaderWriter.Write(ASCIIEncoding.ASCII.GetBytes(savedFiles[i].filename.PadRight(0x20, '\0')));
                groupHeaderWriter.Write(savedFiles[i].fileOffset);
                groupHeaderWriter.Write(savedFiles[i].fileContents.Length);
                groupHeaderWriter.Write(pointers.Count * 4);
                groupHeaderWriter.Write(savedFiles[i].pointers.Count * 4);
                if (savedFiles[i].subHeader == null)
                    groupHeaderWriter.Write(new byte[0x20]);
                else
                    groupHeaderWriter.Write(savedFiles[i].subHeader);

                //Update filename length (include \0 terminator).
                if (this.chunkID.StartsWith("NML"))
                    filenamelength += (ushort)(savedFiles[i].filename.Length + 1);

                //Now put the data into the file pieces.
                groupFileStream.Write(savedFiles[i].fileContents, 0, savedFiles[i].fileContents.Length);
                //Padding out to nearest 0x10
                groupFileStream.Seek((int)(groupFileStream.Position + 0x1F) & 0xFFFFFFE0, SeekOrigin.Begin);
                pointers.AddRange(savedFiles[i].pointers);
            }
            int headerLength = (int)groupHeaderStream.Position;
            groupHeaderStream.Seek((groupHeaderStream.Position + paddingAmount) & mask, SeekOrigin.Begin);
            int uncompressedSize = (int)groupFileStream.Position;
            byte[] rawData;
            if (compressed)
            {
                if(versionNumber == 0x1002) // PSP2 files use Deflate.
                {
                    groupFileStream.Seek(0, SeekOrigin.Begin);
                    MemoryStream compressedStream = new MemoryStream();
                    using(DeflateStream ds = new DeflateStream(compressedStream, CompressionMode.Compress))
                    {
                        groupFileStream.CopyTo(ds);
                    }
                    rawData = compressedStream.ToArray();
                }
                else //PSU uses PRS.
                {
                    rawData = PrsCompDecomp.compress(groupFileStream.ToArray());
                }
            }
            else
            {
                rawData = groupFileStream.ToArray();
            }
            int fileLength = rawData.Length;
            groupHeaderWriter.Write(rawData);
            //Write out pointers (if applicable)
            groupHeaderStream.Seek((groupHeaderStream.Position + paddingAmount) & mask, SeekOrigin.Begin);
            for (int i = 0; i < pointers.Count; i++)
                groupHeaderWriter.Write(pointers[i]);
            groupHeaderWriter.Write(new byte[((groupHeaderStream.Position + paddingAmount) & mask) - groupHeaderStream.Position]);

            //Now fill in the header (leaving the space if necessary).
            groupHeaderStream.Seek(0, SeekOrigin.Begin);
            groupHeaderWriter.Write(ASCIIEncoding.ASCII.GetBytes(this.chunkID));
            groupHeaderWriter.Write(this.versionNumber);
            groupHeaderWriter.Write(filenamelength);
            groupHeaderWriter.Write(headerLength);
            groupHeaderWriter.Write(this.fileContents.Count);
            groupHeaderWriter.Write(uncompressedSize);
            if (compressed)
                groupHeaderWriter.Write(rawData.Length);
            else
                groupHeaderWriter.Write((int)0);
            groupHeaderWriter.Write(pointers.Count * 4);
            groupHeaderWriter.Write((int)0); //Still enforcing no encryption.

            return groupHeaderStream.ToArray();

        }

        public override byte[] ToRaw()
        {
            return SaveFile(false);
        }

        #region ContainerFile Members

        public List<string> getFilenames()
        {
            return fileContents.Select(file => file.filename).ToList();
        }

        public PsuFile getFileParsed(int index)
        {
            return getFileParsed(fileContents[index].filename);
        }

        public PsuFile getFileParsed(string filename)
        {
            if (!loadedFileCache.ContainsKey(filename))
            {
                int index = fileContents.FindIndex(p => p.filename == filename);
                loadedFileCache.Add(filename, PsuFiles.FromRaw(filename, fileContents[index].fileContents, fileContents[index].subHeader, fileContents[index].pointers.ToArray(), (int)fileContents[index].fileOffset, bigEndian));
            }
            return loadedFileCache[filename];
        }

        public RawFile getFileRaw(string filename)
        {
            return fileContents.First(file => file.filename == filename);
        }

        public RawFile getFileRaw(int fileIndex)
        {
            return fileContents[fileIndex];
        }

        public void replaceFile(string filename, RawFile toReplace)
        {
            if (loadedFileCache.ContainsKey(filename))
            {
                loadedFileCache.Remove(filename);
            }
            int index = fileContents.Select(file => file.filename).ToList().IndexOf(filename);
            fileContents[index] = toReplace;
        }

        public void addFile(int index, RawFile toAdd)
        {
            fileContents.Insert(index, toAdd);
        }

        public void removeFile(string filename)
        {
            if (loadedFileCache.ContainsKey(filename))
            {
                loadedFileCache.Remove(filename);
            }
            int index = fileContents.Select(file => file.filename).ToList().IndexOf(filename);
            fileContents.RemoveAt(index);
        }

        public void removeFile(int index)
        {
            string filename = fileContents[index].filename;
            if (loadedFileCache.ContainsKey(filename))
            {
                loadedFileCache.Remove(filename);
            }
            fileContents.RemoveAt(index);
        }

        public void insertFile(RawFile toInsert, int index)
        {
            fileContents.Insert(index, toInsert);
        }

        public void addFile(RawFile toAdd)
        {
            fileContents.Add(toAdd);
        }

        #endregion
    }
}