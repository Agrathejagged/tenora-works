using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSULib.FileClasses.General
{
    public class UnpointeredFile : PsuFile
    {
        public byte[] theData;
        public UnpointeredFile(string inFilename, byte[] rawData, byte[] subHeader)
        {
            header = subHeader;
            theData = rawData;
            filename = inFilename;
        }

        public override byte[] ToRaw()
        {
            return theData;
        }
    }
}
