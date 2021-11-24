using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PSULib.Support;

namespace PSULib.FileClasses.General
{
    public class RawFile
    {
        public byte[] subHeader;
        public byte[] fileContents;
        public List<int> pointers = new List<int>();
        public uint chunkSize = 0x60;
        public uint fileOffset;
        public string filename = "";
        public string fileheader = "";

        public RawFile()
        {

        }

        //Creates a RawFile from my custom container format.
        public RawFile(Stream inStream, string initialFilename)
        {
            BinaryReader inReader = new BinaryReader(inStream);
            string initialHeader = Encoding.ASCII.GetString(inReader.ReadBytes(4));
            uint testChunkSize = inReader.ReadUInt32();
            inStream.Seek(0x30, SeekOrigin.Begin);
            uint testFileOffset = inReader.ReadUInt32();
            int fileLength = inReader.ReadInt32();
            inReader.ReadInt32();
            int pointerCount = inReader.ReadInt32();

            //Okay, now check if these actually make sense...
            bool success = true;
            if(testChunkSize > 1024) //never actually seen this above like 0x60
            {
                success = false;
            }
            //Pointers and file contents must fit in the saved data.
            //Being _very_ generous with buffer length
            if(pointerCount < 0 || fileLength < 0 || (fileLength + 0x60 + pointerCount * 4) > inStream.Length || (fileLength + 0x60 + pointerCount * 4 + 0x200) < inStream.Length)
            {
                success = false;
            }
            //If the file validates, load it as normal.
            if(success)
            {
                fileheader = initialHeader;
                chunkSize = testChunkSize;
                inStream.Seek(0x10, SeekOrigin.Begin);
                filename = Encoding.GetEncoding("shift-jis").GetString(inReader.ReadBytes(0x20));
                filename = filename.TrimEnd('\0');
                fileOffset = testFileOffset;
                inStream.Seek(0x40, SeekOrigin.Begin);
                subHeader = inReader.ReadBytes(0x20);
                fileContents = inReader.ReadBytes(fileLength);
                inStream.Seek(inStream.Position + 0x7F & 0xFFFFFF80, SeekOrigin.Begin);

                for (int i = 0; i < pointerCount; i++)
                {
                    pointers.Add(inReader.ReadInt32());
                }
            }
            else
            {
                //Otherwise, treat it as an arbitrary file--don't build pointers or anything.

                filename = initialFilename;
                fileOffset = 0;
                inStream.Seek(0, SeekOrigin.Begin);
                subHeader = new byte[0x20];
                fileContents = inReader.ReadBytes((int)inStream.Length);
                //Do fix the header for scripts, though.
                if (initialHeader == "NMLL" || initialHeader == "AFS\0")
                {
                    fileheader = initialHeader;
                }
                else
                {
                    fileheader = "STD\0";
                }
            }
            inReader.Close();
        }

        public void RebaseFile(uint newBaseLocation)
        {
            uint difference = newBaseLocation - fileOffset;
            //fileOffset = 
            for (int i = 0; i < pointers.Count; i++)
            {
                int pointerLoc = (int)(pointers[i] - fileOffset);
                int newPointer = (int)(BitConverter.ToInt32(fileContents, pointerLoc) + difference);
                byte[] newPointerBytes = BitConverter.GetBytes(newPointer);
                Array.Copy(newPointerBytes, 0, fileContents, pointerLoc, 4);
                pointers[i] += (int)difference;
            }
            fileOffset = newBaseLocation;
        }
        public void WriteToStream(Stream outStream)
        {
            byte[] toSave = fileContents;
            BinaryWriter outWriter = new BinaryWriter(outStream);
            if (pointers == null)
                outWriter.Write(Encoding.ASCII.GetBytes("STD\0"));
            else
                outWriter.Write(Encoding.ASCII.GetBytes(filename.ToUpper().ToCharArray(filename.Length - 3, 3)));
            outWriter.Seek(0x4, SeekOrigin.Begin);
            outWriter.Write(0x60);
            outStream.Seek(0x10, SeekOrigin.Begin);
            outWriter.Write(StringUtilities.encodePaddedSjisString(filename, 0x20));
            outWriter.Write(0);
            outWriter.Write(toSave.Length);
            outWriter.Write(0);
            if (pointers != null)
                outWriter.Write(pointers.Count);
            else
                outWriter.Write(0);
            if (subHeader != null)
                outWriter.Write(subHeader);
            else
                outStream.Seek(0x60, SeekOrigin.Begin);
            outWriter.Write(toSave);
            outStream.Seek(outStream.Position + 0x7F & 0xFFFFFF80, SeekOrigin.Begin);
            if (pointers != null)
            {
                for (int i = 0; i < pointers.Count; i++)
                    outWriter.Write(pointers[i]);
            }
            outWriter.Close();
        }

        public byte[] WriteToBytes(bool writeMetaData = false)
        {
            List<byte> output = new List<byte>();
            byte[] toSave = fileContents;
            string fileNameSansPath = Path.GetFileName(filename);

            if (writeMetaData == true)
            {
                if (pointers == null || pointers.Count == 0)
                {
                    output.AddRange(Encoding.ASCII.GetBytes("STD\0"));
                }
                else
                {
                    output.AddRange(Encoding.ASCII.GetBytes(fileNameSansPath.ToUpper().ToCharArray(fileNameSansPath.Length - 3, 3)));
                    output.Add(0);
                }
                output.AddRange(BitConverter.GetBytes(chunkSize));

                output.AddRange(new byte[0x8]);
                output.AddRange(StringUtilities.encodePaddedSjisString(fileNameSansPath, 0x20));
                output.AddRange(new byte[0x4]);
                output.AddRange(BitConverter.GetBytes(toSave.Length));
                output.AddRange(new byte[0x4]);

                if (pointers != null)
                {
                    output.AddRange(BitConverter.GetBytes(pointers.Count));
                }
                else
                {
                    output.AddRange(new byte[0x4]);
                }
                if (subHeader != null)
                {
                    output.AddRange(subHeader);
                }
                else
                {
                    output.AddRange(new byte[0x20]);
                }
            }
            output.AddRange(toSave);

            if (writeMetaData == true)
            {
                if (pointers != null)
                {
                    //Calc padding to get to pointer start
                    long pointerPadding = (output.Count + 0x7F & 0xFFFFFF80) - output.Count;
                    output.AddRange(new byte[pointerPadding]);
                    for (int i = 0; i < pointers.Count; i++)
                    {
                        output.AddRange(BitConverter.GetBytes(pointers[i]));
                    }
                }
            }

            return output.ToArray();
        }
    }
}
