using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace psu_generic_parser
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
