using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Archives
{
    /// <summary>
    /// Represents container files that care about filename uniqueness (mostly just NBLs at the moment).
    /// </summary>
    public interface FilenameAwareContainerFile
    {
        /// <summary>
        /// Check if a filename can be added to the archive's contents.
        /// </summary>
        /// <param name="rawFile"></param>
        /// <returns></returns>
        bool ValidateFilename(string newFilename);
    }
}
