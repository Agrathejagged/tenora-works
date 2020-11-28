using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FPB_Extractor
{
    public partial class FpbExtractForm : Form
    {
        private class FileMapping
        {
            public string listFilename { get; set; }
            public uint baseAddress { get; set; }
        }

        private FileMapping customFileMapping = new FileMapping { listFilename = "CUSTOM" };

        private Dictionary<RadioButton, FileMapping> buttonMap = new Dictionary<RadioButton, FileMapping>();

        public FpbExtractForm()
        {
            InitializeComponent();
            buttonMap[psp1FileNaRadioButton] = new FileMapping{listFilename = "list-psp1-na.bin", baseAddress = 0};
            buttonMap[psp1FileJRadioButton] = new FileMapping { listFilename = "list-psp1-j.bin", baseAddress = 0 };
            buttonMap[psp1FileEurRadioButton] = new FileMapping { listFilename = "list-psp1-eur.bin", baseAddress = 0 };

            buttonMap[psp2FileNaRadioButton] = new FileMapping { listFilename = "list-psp2-na.bin", baseAddress = 0 };
            buttonMap[psp2MediaNaRadioButton] = new FileMapping { listFilename = "list-psp2-na.bin", baseAddress = 0x80000000 };
            buttonMap[psp2FileEurRadioButton] = new FileMapping { listFilename = "list-psp2-eur.bin", baseAddress = 0 };
            buttonMap[psp2MediaEurRadioButton] = new FileMapping { listFilename = "list-psp2-eur.bin", baseAddress = 0x80000000 };
            buttonMap[psp2FileJRadioButton] = new FileMapping { listFilename = "list-psp2-j.bin", baseAddress = 0 };
            buttonMap[psp2MediaJRadioButton] = new FileMapping { listFilename = "list-psp2-j.bin", baseAddress = 0x80000000 };

            buttonMap[infinityFileRadioButton] = new FileMapping { listFilename = "list-psp2i-j.bin", baseAddress = 0 };
            buttonMap[infinityMediaRadioButton] = new FileMapping { listFilename = "list-psp2i-j.bin", baseAddress = 0x80000000 };
            buttonMap[customFpbRadioButton] = customFileMapping;

            checkListFiles();
        }

        private void checkListFiles()
        {
            foreach(var ent in buttonMap)
            {
                if(ent.Value.listFilename != "CUSTOM" && !File.Exists(ent.Value.listFilename))
                {
                    ent.Key.Enabled = false;
                }
            }
        }

        private void customFpbRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            browseListFileButton.Enabled = baseOffsetLabel.Enabled = listFileLabel.Enabled = listFileBox.Enabled = baseOffsetUpDown.Enabled = customFpbRadioButton.Checked;
            offsetExplanation.Visible = customFpbRadioButton.Checked;
        }

        private void extractFpbButton_Click(object sender, EventArgs e)
        {
            if (fpbFileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string readFilename = fpbFileBrowserDialog.FileName;
                string extractPath = destinationTextBox.Text;
                if(!Path.IsPathRooted(extractPath))
                {
                    extractPath = Path.Combine(Path.GetDirectoryName(readFilename), extractPath);
                }
                uint baseOffset = 0;
                string listFilename = "";
                foreach (var ent in buttonMap)
                {
                    if(ent.Key.Checked)
                    {
                        if (ent.Value.listFilename == "CUSTOM")
                        {
                            listFilename = listFileBox.Text;
                            baseOffset = Convert.ToUInt32(baseOffsetUpDown.Value);
                        }
                        else
                        { 
                            listFilename = ent.Value.listFilename;
                            baseOffset = ent.Value.baseAddress;
                        }
                    }
                }
                if(File.Exists(listFilename))
                {
                    extractFpb(readFilename, extractPath, useCrc32FilenameCheckbox.Checked, addExtensionCheckbox.Checked, listFilename, baseOffset);
                }
                else
                {
                    MessageBox.Show("Extraction Error", "The selected list file could not be found.");
                }
            }
        }

        private void extractFpb(string fpbFile, string extractPath, bool useCrc32Filename, bool addFileExtension, string listFile, long baseOffset)
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            int fileCount;
            uint[] crcs;
            uint[] fileLocs;
            using (Stream fileTableStream = new FileStream(listFile, FileMode.Open))
            {
                using(BinaryReader fileTableReader = new BinaryReader(fileTableStream))
                {
                    //There's a mystery file at offset 0. The count doesn't include it.
                    fileCount = fileTableReader.ReadInt32() + 1;
                    crcs = new uint[fileCount];
                    fileLocs = new uint[fileCount];
                    for (int i = 0; i < fileCount; i++)
                    {
                        crcs[i] = fileTableReader.ReadUInt32();
                        //There's something in the lower 11 bits, but I think it's just a hint to the game for how to load the file
                        fileLocs[i] = (uint)(fileTableReader.ReadInt32() & 0xFFFFF800);
                    }
                }
            }

            progressBar1.Value = 0;
            progressBar1.Step = 1;
            int fileNum = 0;

            using (Stream fileArchiveStream = new FileStream(fpbFile, FileMode.Open))
            {
                using (BinaryReader fileArchiveReader = new BinaryReader(fileArchiveStream))
                {
                    progressBar1.Maximum = fileLocs.Count(j => j > baseOffset && (j - baseOffset) < fileArchiveStream.Length);
                    for (int i = 0; i < fileCount; i++)
                    {
                        if (fileLocs[i] >= baseOffset && (fileLocs[i] - baseOffset) < fileArchiveStream.Length)
                        {
                            fileArchiveStream.Seek(fileLocs[i] - baseOffset, SeekOrigin.Begin);
                            byte[] fileIdentifier = fileArchiveReader.ReadBytes(4);
                            string fileExt = "";
                            if (addFileExtension)
                            {
                                fileExt = getFileExtension(fileIdentifier);
                            }

                            fileArchiveStream.Seek(fileLocs[i] - baseOffset, SeekOrigin.Begin);

                            uint nextFileOffset;
                            if(i < fileCount - 1 && fileLocs[i + 1] >= baseOffset)
                            {
                                //dangerous assumption: no bitmask changes (e.g if file 0 is above base offset, file 1 should not be below)
                                nextFileOffset = Math.Min((uint)fileArchiveStream.Length, (uint)(fileLocs[i + 1] - baseOffset));
                            }
                            else
                            {
                                nextFileOffset = (uint)fileArchiveStream.Length;
                            }
                            string outFilename;
                            if(useCrc32Filename)
                            {
                                outFilename = crcs[i].ToString("X8");
                            }
                            else
                            {
                                outFilename = "file-" + fileNum;
                            }
                            
                            File.WriteAllBytes(Path.Combine(extractPath, outFilename + fileExt), fileArchiveReader.ReadBytes((int)(nextFileOffset - (fileLocs[i] - baseOffset))));
                            progressBar1.PerformStep();
                            fileNum++;
                        }
                    }
                }
            }
        }

        private void browseListFileButton_Click(object sender, EventArgs e)
        {
            if(listFileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                listFileBox.Text = listFileBrowserDialog.FileName;
            }
        }

        private void browseDestinationButton_Click(object sender, EventArgs e)
        {
            if(destinationFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                destinationTextBox.Text = destinationFolderBrowserDialog.SelectedPath;
            }
        }

        private void ripFpbButton_Click(object sender, EventArgs e)
        {
            if (fpbFileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string readFilename = fpbFileBrowserDialog.FileName;
                string extractPath = destinationTextBox.Text;
                if (!Path.IsPathRooted(extractPath))
                {
                    extractPath = Path.Combine(Path.GetDirectoryName(readFilename), extractPath);
                }
                ripArchives(readFilename, extractPath, addExtensionCheckbox.Checked);
            }
        }

        private void ripArchives(string fpbFile, string extractPath, bool addFileExtension)
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            progressBar1.Value = 0;
            progressBar1.Step = 1;
            int fileNum = 0;

            using (Stream fileArchiveStream = new FileStream(fpbFile, FileMode.Open))
            {
                using (BinaryReader fileArchiveReader = new BinaryReader(fileArchiveStream))
                {
                    progressBar1.Maximum = (int)fileArchiveStream.Length;
                    while (fileArchiveStream.Position < fileArchiveStream.Length)
                    {
                        int baseOffset = (int)fileArchiveStream.Position;
                        int fileSize = 0;
                        string outFilename = "file-" + fileNum;
                        byte[] identifierBytes = fileArchiveReader.ReadBytes(4);
                        string identifierString = ASCIIEncoding.ASCII.GetString(identifierBytes);
                        if (identifierString == "NMLL")
                        {
                            //For NBLs, the file length is:
                            //1. NMLL header length + padding
                            //2. NMLL contents length + padding
                            //3. Pointer length + padding
                            //4. TMLL header length + padding
                            //5. TMLL contents length + padding
                            int nmllPadding = 0x800;
                            uint nmllPaddingEnd = 0xFFFFF800;
                            fileArchiveReader.ReadByte();
                            if (fileArchiveReader.ReadByte() == 0x10)
                            {
                                nmllPadding = 0x40;
                                nmllPaddingEnd = 0xFFFFFFC0;
                            }
                            fileArchiveReader.ReadInt16();
                            int nmllHeaderLength = fileArchiveReader.ReadInt32();
                            fileArchiveReader.ReadInt32();
                            int decompressedNmllDataLength = fileArchiveReader.ReadInt32();
                            int compressedNmllDataLength = fileArchiveReader.ReadInt32();
                            int nmllDataLength = compressedNmllDataLength != 0 ? compressedNmllDataLength : decompressedNmllDataLength;
                            int pointerDataLength = fileArchiveReader.ReadInt32();
                            fileArchiveStream.Seek(0x4, SeekOrigin.Current); //encryption key
                            int tmllHeaderLength = fileArchiveReader.ReadInt32();
                            int tmllDataLengthUncompressed = fileArchiveReader.ReadInt32();
                            int tmllDataLengthCompressed = fileArchiveReader.ReadInt32();
                            int tmllDataLength = tmllDataLengthCompressed != 0 ? tmllDataLengthCompressed : tmllDataLengthUncompressed;
                            int nmllHeaderTotal = (int)((nmllHeaderLength + nmllPadding - 1) & nmllPaddingEnd);
                            int pointerTotal = (int)((pointerDataLength + nmllPadding - 1) & nmllPaddingEnd);
                            int nmllFileTotal = (int)((nmllDataLength + nmllPadding - 1) & nmllPaddingEnd);
                            int tmllHeaderTotal = (int)((tmllHeaderLength + nmllPadding - 1) & nmllPaddingEnd);
                            int tmllDataTotal = (int)((tmllDataLength + nmllPadding - 1) & nmllPaddingEnd);
                            int minimumSize = nmllHeaderTotal + nmllFileTotal + pointerTotal + tmllHeaderTotal + tmllDataTotal;
                            fileArchiveStream.Seek(baseOffset + minimumSize, SeekOrigin.Begin);

                            int nextAddress = findIdentifier(fileArchiveStream, fileArchiveReader);
                            fileSize = nextAddress - baseOffset;
                            if (nmllPadding == 0x40)
                                outFilename += "-new-format";
                        }
                        else if (identifierString == "AFS\0")
                        {
                            //For AFS files, the file length is calculated by seeking to the filename chunk, then padding.
                            //So just take the count...
                            int fileCount = fileArchiveReader.ReadInt32();
                            //Skip that many file entries...
                            fileArchiveStream.Seek(fileCount * 8, SeekOrigin.Current);
                            int filenameChunkPos = fileArchiveReader.ReadInt32();
                            int filenameChunkLength = fileArchiveReader.ReadInt32();
                            //And then round up.
                            fileSize = (int)((filenameChunkPos + filenameChunkLength + 0x7FF) & 0xFFFFF800);
                        }
                        else if (BitConverter.ToInt16(identifierBytes, 0) == 0x50AF) // mini AFS is "af50" hex
                        {
                            //Same logic as non-mini-AFS, just smaller data values
                            int fileCount = BitConverter.ToInt16(identifierBytes, 2); //2 byte identifier, remainder's start of table.
                            fileArchiveReader.ReadInt32(); //mystery data
                            fileArchiveStream.Seek(fileCount * 2, SeekOrigin.Current);
                            int filenameChunkPos = fileArchiveReader.ReadUInt16() * 0x800;

                            //Filenames are still 0x30 bytes long, so we can calculate this.
                            int filenameChunkLength = 0x30 * fileCount;
                            int paddedFileLength = (int)((filenameChunkPos + filenameChunkLength + 0x7FF) & 0xFFFFF800);

                            fileSize = (int)((filenameChunkPos + filenameChunkLength + 0x7FF) & 0xFFFFF800);
                        }
                        else
                        {
                            int nextAddress = findIdentifier(fileArchiveStream, fileArchiveReader);
                            fileSize = nextAddress - baseOffset;
                        }
                        if (addFileExtension)
                        {
                            outFilename += getFileExtension(identifierBytes);
                        }
                        string filePath = Path.Combine(extractPath, outFilename);
                        fileArchiveStream.Seek(baseOffset, SeekOrigin.Begin);
                        File.WriteAllBytes(filePath, fileArchiveReader.ReadBytes(fileSize));
                        fileNum++;
                        progressBar1.Value = (int)fileArchiveStream.Position;
                    }
                }
            }

        }

        private string[] formatStrings = { "AFS\0", "NMLL", "BIND", "RIFF", "Pack" };

        private int findIdentifier(Stream stream, BinaryReader reader)
        {
            //We'll keep stuff aligned by 4 bytes, because that's how it goes.
            while(stream.Position < stream.Length - 4)
            {
                byte[] identifierBytes = reader.ReadBytes(4);
                string identifierString = ASCIIEncoding.ASCII.GetString(identifierBytes);
                if ((BitConverter.ToInt16(identifierBytes, 0) == 0x50AF && ((stream.Position - 4) % 0x800 == 0)) || formatStrings.Contains(identifierString))
                {
                    return (int)stream.Position - 4;
                }
                stream.Seek(Math.Min(stream.Length - 1, ((stream.Position + 0x20) & 0xFFFFFFE0)), SeekOrigin.Begin);
            }
            return (int)stream.Length;
        }

        private string getFileExtension(byte[] identifierBytes)
        {
            if(BitConverter.ToInt16(identifierBytes, 0) == 0x50AF)
            {
                return ".nafs";
            }
            else
            {
                switch(ASCIIEncoding.ASCII.GetString(identifierBytes))
                {
                    case "NMLL": return ".nbl"; 
                    case "AFS\0": return ".afs";
                    case "BIND": return ".seq.adx";
                    case "RIFF": return ".strm.adx";
                    case "Pack": return ".pac";
                    default: return "";
                }
            }
        }
    }
}
