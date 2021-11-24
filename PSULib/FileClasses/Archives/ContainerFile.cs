using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PSULib.FileClasses.Archives
{
    public interface ContainerFile
    {
        //Loads the filenames from the file (no other parsing)
        List<string> getFilenames();
        //Gets a parsed file based on its name.
        PsuFile getFileParsed(string filename);
        //Check if file exists in container. Obviously more meaningful on files with unique names (NBLs, not AFSes).
        bool doesFileExist(string filename);
        bool isParsedFileCached(string filename);
        //Gets a raw file based on its name. Maybe don't attempt on NBL section?
        RawFile getFileRaw(string filename);
        //Gets a raw file based on its name.
        PsuFile getFileParsed(int fileIndex);
        bool isParsedFileCached(int fileIndex);
        //Gets a raw file based on its index. Maybe don't attempt on NBL section?
        RawFile getFileRaw(int fileIndex);
        //Replaces a file based on filename.
        void replaceFile(string filename, RawFile toReplace);
        void replaceFile(int index, RawFile toReplace);
        void addFile(int index, RawFile toAdd);
        void saveFile(Stream outStream);
        void removeFile(int index);
        void removeFile(string filename);
        void renameFile(string filename, string newFilename);
        void renameFile(int index, string newFilename);
        bool Encrypted { get; set; }
        bool Compressed { get; set; }
        byte[] ToRaw();
    }
}
