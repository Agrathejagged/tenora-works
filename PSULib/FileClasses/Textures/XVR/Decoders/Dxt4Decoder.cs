using PSULib.FileClasses.Textures.XVR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    /// <summary>
    /// DXT4 is DXT5, but with premultiplied alpha values, so we'll just make it the same for now.
    /// It's supposed to be a hint to the renderer somehow? We don't really do that here...
    /// </summary>
    class Dxt4Decoder : Dxt5Decoder
    {
        public Dxt4Decoder(PsuTexturePixelFormat pixelFormat) : base(pixelFormat)
        {
        }
    }
}
