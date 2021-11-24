using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    class XvrImageParameters
    {
        public byte pixelFormat, pixelFlags;
        public int width, height;
        public int maximumMips;
        public int maxU, maxV;
        public int startingOffset;

        public Boolean HasMips { get { return pixelFlags == 0x71 || (pixelFlags > 0x72 && pixelFlags % 2 == 0); } }

        public PsuTextureType TextureType { get { return PsuTextureTypeMethods.FromByte(pixelFlags); } }

        public XvrImageParameters(byte[] header, byte[] rawData)
        {
            pixelFormat = header[0x18];
            pixelFlags = header[0x19];
            width = BitConverter.ToInt16(header, 0x1C);
            height = BitConverter.ToInt16(header, 0x1E);
            maxU = 1;   //Which bit is the highest!
            maxV = 1;
            if (BitConverter.ToInt32(rawData, 0) == 0x840001)
            {
                if (BitConverter.ToInt32(rawData, 0x14) == 0)
                {
                    startingOffset = 0x7E0;
                }
                else
                {
                    startingOffset = 0x20;
                }
                short temp = BitConverter.ToInt16(rawData, 0xE);
                maximumMips = temp & 0xF;
                maxU = (temp & 0xF00) >> 8;
                maxV = (temp & 0xF0) >> 4;
            }
            else
            {
                startingOffset = 0;
                maxV = (int)(Math.Log(width - 1, 2));
                maxU = (int)(Math.Log(height - 1, 2));
                maximumMips = Math.Min(maxU, maxV);
            }
        }

        public XvrImageParameters(int width, int height, PsuTextureType texType, bool hasMips = false, PsuTexturePixelFormat format = PsuTexturePixelFormat.Argb8888)
        {
            startingOffset = 0x20;
            this.width = width;
            this.height = height;

            pixelFormat = format.ToRawValue();
            pixelFlags = (byte)(texType.ToRawValue() | (hasMips ? 1 : 0));
            this.width = width;
            this.height = height;
            maxV = (int)(Math.Log(width - 1, 2));
            maxU = (int)(Math.Log(height - 1, 2));
            maximumMips = Math.Min(maxU, maxV);
        }
        
        public PsuTexturePixelFormat PixelFormat
        {
            get {
                switch (pixelFormat)
                {
                    case 0x0: return PsuTexturePixelFormat.Palette8;
                    case 0x1: return PsuTexturePixelFormat.Palette8;
                    case 0x2: return PsuTexturePixelFormat.Argb1555;
                    case 0x3: return PsuTexturePixelFormat.Rgb555;
                    case 0x4: return PsuTexturePixelFormat.Argb4444;
                    case 0x5: return PsuTexturePixelFormat.Rgb565;
                    case 0x6: return PsuTexturePixelFormat.Argb8888;
                    case 0x7: return PsuTexturePixelFormat.Xrgb8888;
                    case 0x8: return PsuTexturePixelFormat.Mystery32;
                    case 0x9: return PsuTexturePixelFormat.Palette8;
                    case 0xA: return PsuTexturePixelFormat.Palette8;
                    case 0xB: return PsuTexturePixelFormat.Ax88;
                    case 0xC: return PsuTexturePixelFormat.Rgb655;
                    case 0xD: return PsuTexturePixelFormat.Mystery16;
                    case 0xE: return PsuTexturePixelFormat.Mystery16;
                    case 0xF: return PsuTexturePixelFormat.Gb88;
                    case 0x10: return PsuTexturePixelFormat.Rb88;
                    case 0x11: return PsuTexturePixelFormat.Mystery16;
                    case 0x12: return PsuTexturePixelFormat.Mystery32;
                    case 0x13: return PsuTexturePixelFormat.Rgb565;
                    case 0x14: return PsuTexturePixelFormat.Rgba8888;
                    case 0x15: return PsuTexturePixelFormat.Abgr8888;
                    case 0x16: return PsuTexturePixelFormat.Mystery32;
                    case 0x17: return PsuTexturePixelFormat.Abgr8888;
                    case 0x18: return PsuTexturePixelFormat.Abgr8888;
                    case 0x19: return PsuTexturePixelFormat.Mystery16;
                    case 0x1A: return PsuTexturePixelFormat.Mystery32;
                    default: return PsuTexturePixelFormat.Invalid;
                }
            }
        }

        public string getFormatString()
        {
            switch (pixelFlags)
            {
                case 0x70:
                    return PixelFormat.ToString();
                case 0x71:
                    return PixelFormat.ToString() + " w/mips";
                case 0x73: return "DXT1";
                case 0x74: return "DXT1 w/mips";
                case 0x75: return "DXT2";
                case 0x76: return "DXT2 w/mips";
                case 0x77: return "DXT3";
                case 0x78: return "DXT3 w/mips";
                case 0x79: return "DXT4";
                case 0x7A: return "DXT4 w/mips";
                case 0x7B: return "DXT5";
                case 0x7C: return "DXT5 w/mips";
                default: return "unknown format";
            }
        }
    }
}
