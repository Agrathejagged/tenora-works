using PSULib.FileClasses.General;
using PSULib.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counter_Editor.Files
{
    /// <summary>
    /// Class for performing limited operations on a quest.xnr file. Not a full parsed file!
    /// </summary>
    internal class QuestDataExtractor
    {
        public static int GetQuestNumber(RawFile questFile)
        {
            bool bigEndian = questFile.filename.EndsWith(".ynr");
            BinaryReader reader = BigEndianBinaryReader.GetEndianSpecificBinaryReader(new MemoryStream(questFile.fileContents), bigEndian);
            reader.BaseStream.Position = 8;
            int headerLoc = reader.ReadInt32();
            reader.BaseStream.Position = headerLoc + 8;
            return reader.ReadInt32();

        }
    }
}
