using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser.FileClasses.XVR
{
    class PsuColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
        /// <summary>
        /// Ignored color component.
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        /// All colors can be treated as an int32, so just permitting that to keep control flow simple.
        /// For 8/16/24-bit colors, this will obviously ignore a lot of bits.
        /// </summary>
        /// <param name="inColor"></param>
        /// <param name="inFormat"></param>
        /*public PsuColor(uint inColor, PsuTexturePixelFormat inFormat)
        {
            switch (inFormat)
            {
                case PsuTexturePixelFormat.Argb1555: return PixelFormat.Format16bppArgb1555;
                case PsuTexturePixelFormat.Rgb555: return PixelFormat.Format16bppRgb555;
                case PsuTexturePixelFormat.Argb4444: return PixelFormat.Format32bppArgb;
                case PsuTexturePixelFormat.Rgb565: return PixelFormat.Format16bppRgb565;
                case PsuTexturePixelFormat.Argb8888: return PixelFormat.Format32bppArgb;
                case PsuTexturePixelFormat.Xrgb8888: return PixelFormat.Format24bppRgb;
                case PsuTexturePixelFormat.Mystery32: return PixelFormat.Format32bppArgb;
                case PsuTexturePixelFormat.Palette8: return PixelFormat.Format8bppIndexed;
                case PsuTexturePixelFormat.Ax88: return PixelFormat.Format16bppGrayScale;
                case PsuTexturePixelFormat.Rgb655: return PixelFormat.Format24bppRgb;
                case PsuTexturePixelFormat.Mystery16: return PixelFormat.Format16bppGrayScale;
                case PsuTexturePixelFormat.Gb88: return PixelFormat.Format24bppRgb;
                case PsuTexturePixelFormat.Rb88: return PixelFormat.Format24bppRgb;
                case PsuTexturePixelFormat.Rgba8888: return PixelFormat.Format32bppArgb;
                case PsuTexturePixelFormat.Abgr8888: return PixelFormat.Format32bppArgb;
            }
        }*/
    }
}
