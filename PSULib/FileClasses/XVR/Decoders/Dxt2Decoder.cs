using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser.FileClasses.XVR
{
    /// <summary>
    /// DXT2 is DXT3, but with premultiplied alpha values, so we'll just make it the same for now.
    /// It's supposed to be a hint to the renderer somehow? We don't really do that here...
    /// </summary>
    class Dxt2Decoder : Dxt3Decoder
    {
        public Dxt2Decoder(PsuTexturePixelFormat pixelFormat) : base(pixelFormat)
        {
            
        }
    }
}
