using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    abstract class DxtnDecoder : XvrRasterDecoder
    {
        public PsuTexturePixelFormat pixelFormat { get; }
        public DxtnDecoder(PsuTexturePixelFormat format)
        {
            pixelFormat = format;
        }
    }
}
