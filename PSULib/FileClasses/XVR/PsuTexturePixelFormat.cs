using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser.FileClasses.XVR
{
    public enum PsuTexturePixelFormat
    {
        Argb1555,
        Rgb555,
        Argb4444,
        Rgb565,
        Argb8888,
        Xrgb8888,
        Mystery32,
        Palette8,
        Ax88,
        Rgb655,
        Mystery16,
        Gb88,
        Rb88,
        Rgba8888,
        Abgr8888,
        Invalid
    }

    public enum PsuTextureType
    {
        Raster,
        Dxt1,
        Dxt2,
        Dxt3,
        Dxt4,
        Dxt5
    }

    static class PsuTextureTypeMethods
    {
        public static PsuTextureType FromByte(byte internalRepresentation)
        {
            switch (internalRepresentation)
            {
                case 0x70:
                case 0x71: return PsuTextureType.Raster;
                case 0x73:
                case 0x74: return PsuTextureType.Dxt1;
                case 0x75:
                case 0x76: return PsuTextureType.Dxt2;
                case 0x77:
                case 0x78: return PsuTextureType.Dxt3;
                case 0x79:
                case 0x7A: return PsuTextureType.Dxt4;
                case 0x7B:
                case 0x7C: return PsuTextureType.Dxt5;
                default: return PsuTextureType.Raster;
            }
        }
        public static byte ToRawValue(this PsuTextureType format)
        {
            switch (format)
            {
                case PsuTextureType.Raster: return 0x70;
                case PsuTextureType.Dxt1: return 0x73;
                case PsuTextureType.Dxt2: return 0x75;
                case PsuTextureType.Dxt3: return 0x77;
                case PsuTextureType.Dxt4: return 0x79;
                case PsuTextureType.Dxt5: return 0x7B;
            }
            return 0x70;
        }
    }

    static class PsuPixelFormatMethods
    {
        public static PsuTexturePixelFormat FromByte(byte internalRepresentation)
        {
            switch (internalRepresentation)
            {
                default: return PsuTexturePixelFormat.Invalid;
            }
        }

        public static PixelFormat ToPixelFormat(this PsuTexturePixelFormat format)
        {
            switch (format)
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
                default: return PixelFormat.Undefined;
            }
        }

        public static byte ToRawValue(this PsuTexturePixelFormat format)
        {
            switch (format)
            {
                case PsuTexturePixelFormat.Argb1555: return 2;
                case PsuTexturePixelFormat.Rgb555: return 3;
                case PsuTexturePixelFormat.Argb4444: return 4;
                case PsuTexturePixelFormat.Rgb565: return 5;
                case PsuTexturePixelFormat.Argb8888: return 6;
                case PsuTexturePixelFormat.Xrgb8888: return 7;
                case PsuTexturePixelFormat.Mystery32: return 8;
                case PsuTexturePixelFormat.Palette8: return 0;
                case PsuTexturePixelFormat.Ax88: return 11;
                case PsuTexturePixelFormat.Rgb655: return 12;
                case PsuTexturePixelFormat.Mystery16: return 13;
                case PsuTexturePixelFormat.Gb88: return 15;
                case PsuTexturePixelFormat.Rb88: return 16;
                case PsuTexturePixelFormat.Rgba8888: return 20;
                case PsuTexturePixelFormat.Abgr8888: return 21;
                default: return 8;
            }
        }

        public static int GetSourceBytesPerPixel(this PsuTexturePixelFormat format)
        {
            switch (format)
            {
                case PsuTexturePixelFormat.Palette8: return 1;
                case PsuTexturePixelFormat.Argb1555: 
                case PsuTexturePixelFormat.Rgb555: 
                case PsuTexturePixelFormat.Argb4444: 
                case PsuTexturePixelFormat.Rgb565: 
                case PsuTexturePixelFormat.Ax88: 
                case PsuTexturePixelFormat.Rgb655: 
                case PsuTexturePixelFormat.Mystery16:
                case PsuTexturePixelFormat.Gb88:
                case PsuTexturePixelFormat.Rb88: return 2;
                case PsuTexturePixelFormat.Argb8888:
                case PsuTexturePixelFormat.Xrgb8888:
                case PsuTexturePixelFormat.Mystery32:
                case PsuTexturePixelFormat.Rgba8888:
                case PsuTexturePixelFormat.Abgr8888: return 4;
                default: return -1; //let's force this one to fail as quickly and as frequently as possible
            }
        }

        public static int GetDestinationBytesPerPixel(this PsuTexturePixelFormat format)
        {
            switch (format)
            {
                case PsuTexturePixelFormat.Argb1555: return 2;
                case PsuTexturePixelFormat.Rgb555: return 2;
                case PsuTexturePixelFormat.Argb4444: return 4;
                case PsuTexturePixelFormat.Rgb565: return 2;
                case PsuTexturePixelFormat.Argb8888: return 4;
                case PsuTexturePixelFormat.Xrgb8888: return 3;
                case PsuTexturePixelFormat.Mystery32: return 4;
                case PsuTexturePixelFormat.Palette8: return 1;
                case PsuTexturePixelFormat.Ax88: return 2;
                case PsuTexturePixelFormat.Rgb655: return 3;
                case PsuTexturePixelFormat.Mystery16: return 2;
                case PsuTexturePixelFormat.Gb88: return 3;
                case PsuTexturePixelFormat.Rb88: return 3;
                case PsuTexturePixelFormat.Rgba8888: return 4;
                case PsuTexturePixelFormat.Abgr8888: return 4;
                default: return -1;
            }
        }

        public static void DecodePixel(this PsuTexturePixelFormat format, byte[] sourcePixels, int sourceAddress, byte[] destinationPixels, int destinationAddress)
        {
            switch (format)
            {
                //These are direct copies.
                case PsuTexturePixelFormat.Palette8: 
                case PsuTexturePixelFormat.Argb1555: 
                case PsuTexturePixelFormat.Rgb555: 
                case PsuTexturePixelFormat.Rgb565:
                case PsuTexturePixelFormat.Argb8888:
                    Array.Copy(sourcePixels, sourceAddress * format.GetSourceBytesPerPixel(), destinationPixels, destinationAddress * format.GetDestinationBytesPerPixel(), format.GetSourceBytesPerPixel());
                    break;
                //Promote all 4 colors from 4 to 8 bits.
                case PsuTexturePixelFormat.Argb4444:
                    byte gb = sourcePixels[sourceAddress * 2];
                    byte ar = sourcePixels[sourceAddress * 2 + 1];
                    byte a = (byte)(ar & 0xF0);
                    byte r = (byte)((ar & 0xF) << 4);
                    byte g = (byte)(gb & 0xF0);
                    byte b = (byte)((gb & 0xF) << 4);
                    destinationPixels[destinationAddress * 4] = b;
                    destinationPixels[destinationAddress * 4 + 1] = g;
                    destinationPixels[destinationAddress * 4 + 2] = r;
                    destinationPixels[destinationAddress * 4 + 3] = a;
                    break;
                //Copy 3 bytes, discard the next, copy the result into 3.
                case PsuTexturePixelFormat.Xrgb8888:
                    destinationPixels[destinationAddress * 3] = sourcePixels[sourceAddress * 4 + 0]; // B
                    destinationPixels[destinationAddress * 3 + 1] = sourcePixels[sourceAddress * 4 + 1]; // G
                    destinationPixels[destinationAddress * 3 + 2] = sourcePixels[sourceAddress * 4 + 2]; // R
                    break;
                //Promote first byte to upper byte of destination
                case PsuTexturePixelFormat.Ax88:
                    destinationPixels[destinationAddress * 2 + 1] = sourcePixels[sourceAddress * 2];
                    break;
                    //Promote all 3 colors to 8 bits
                case PsuTexturePixelFormat.Rgb655:
                    ushort combinedColor = (ushort)((sourcePixels[sourceAddress * 2 + 1] << 8) | sourcePixels[sourceAddress * 2]);
                    b = (byte)((combinedColor & 0x1F) << 3);
                    g = (byte)((combinedColor & 0x3E0) >> 2);
                    r = (byte)((combinedColor & 0xFC00) >> 8);
                    destinationPixels[destinationAddress * 3] = b; // B
                    destinationPixels[destinationAddress * 3 + 1] = g; // G
                    destinationPixels[destinationAddress * 3 + 2] = r; // R
                    break;
                //Copy values, add dummy third value
                case PsuTexturePixelFormat.Gb88:
                    g = sourcePixels[sourceAddress * 2 + 1];
                    b = sourcePixels[sourceAddress * 2 + 0];
                    destinationPixels[destinationAddress * 3] = b;
                    destinationPixels[destinationAddress * 3 + 1] = g;
                    break;
                case PsuTexturePixelFormat.Rb88:
                    r = sourcePixels[sourceAddress * 2 + 1];
                    b = sourcePixels[sourceAddress * 2 + 0];
                    destinationPixels[destinationAddress * 3] = b;
                    destinationPixels[destinationAddress * 3 + 1] = r;
                    break;
                //Reorder values
                case PsuTexturePixelFormat.Rgba8888:
                    destinationPixels[destinationAddress * 4] = sourcePixels[sourceAddress * 4 + 1]; // B
                    destinationPixels[destinationAddress * 4 + 1] = sourcePixels[sourceAddress * 4 + 2]; // G
                    destinationPixels[destinationAddress * 4 + 2] = sourcePixels[sourceAddress * 4 + 3]; // R
                    destinationPixels[destinationAddress * 4 + 3] = sourcePixels[sourceAddress * 4]; // A
                    break;
                case PsuTexturePixelFormat.Abgr8888:
                    destinationPixels[destinationAddress * 4] = sourcePixels[sourceAddress * 4 + 2]; // B
                    destinationPixels[destinationAddress * 4 + 1] = sourcePixels[sourceAddress * 4 + 1]; // G
                    destinationPixels[destinationAddress * 4 + 2] = sourcePixels[sourceAddress * 4 + 0]; // R
                    destinationPixels[destinationAddress * 4 + 3] = sourcePixels[sourceAddress * 4 + 3]; // A
                    break;
                    //These values have no bitmasks, so nothing gets copied.
                case PsuTexturePixelFormat.Mystery16:
                case PsuTexturePixelFormat.Mystery32:
                default: break;
            }
        }

        public static void EncodePixel(this PsuTexturePixelFormat format, byte[] sourcePixels, int sourceAddress, byte[] destinationPixels, int destinationAddress)
        {
            switch (format)
            {
                //These are direct copies.
                case PsuTexturePixelFormat.Palette8:
                case PsuTexturePixelFormat.Argb1555:
                case PsuTexturePixelFormat.Rgb555:
                case PsuTexturePixelFormat.Rgb565:
                case PsuTexturePixelFormat.Argb8888:
                    Array.Copy(sourcePixels, sourceAddress * format.GetSourceBytesPerPixel(), destinationPixels, destinationAddress * format.GetDestinationBytesPerPixel(), format.GetSourceBytesPerPixel());
                    break;
                //Promote all 4 colors from 4 to 8 bits.
                case PsuTexturePixelFormat.Argb4444:
                    byte a = sourcePixels[sourceAddress * 4 + 3];
                    byte r = sourcePixels[sourceAddress * 4 + 2];
                    byte g = sourcePixels[sourceAddress * 4 + 1];
                    byte b = sourcePixels[sourceAddress * 4];
                    byte ar = (byte)((a & 0xF0) | (r >> 4));
                    byte gb = (byte)((g & 0xF0) | (b >> 4));
                    destinationPixels[destinationAddress * 2] = gb;
                    destinationPixels[destinationAddress * 2 + 1] = ar;
                    break;
                //Copy 3 bytes, discard the next, copy the result into 3.
                case PsuTexturePixelFormat.Xrgb8888:
                    destinationPixels[destinationAddress * 4] = sourcePixels[sourceAddress * 3 + 0]; // B
                    destinationPixels[destinationAddress * 4 + 1] = sourcePixels[sourceAddress * 3 + 1]; // G
                    destinationPixels[destinationAddress * 4 + 2] = sourcePixels[sourceAddress * 3 + 2]; // R
                    break;
                //Promote first byte to upper byte of destination
                case PsuTexturePixelFormat.Ax88:
                    destinationPixels[destinationAddress * 2] = sourcePixels[sourceAddress * 2 + 1];
                    break;
                //Promote all 3 colors to 8 bits
                case PsuTexturePixelFormat.Rgb655:
                    b = sourcePixels[sourceAddress * 3];
                    g = sourcePixels[sourceAddress * 3 + 1];
                    r = sourcePixels[sourceAddress * 3 + 2];
                    byte lowerByte = (byte)((b >> 5) | ((g & 0x38) << 2));
                    byte upperByte = (byte)((r & 0xFC) | ((g & 0xC0) >> 6));
                    destinationPixels[destinationAddress * 2] = lowerByte;
                    destinationPixels[destinationAddress * 2 + 1] = upperByte;
                    break;
                //Copy values, add dummy third value
                case PsuTexturePixelFormat.Gb88:
                    g = sourcePixels[sourceAddress * 3 + 1];
                    b = sourcePixels[sourceAddress * 3 + 0];
                    destinationPixels[destinationAddress * 2] = b;
                    destinationPixels[destinationAddress * 2 + 1] = g;
                    break;
                case PsuTexturePixelFormat.Rb88:
                    r = sourcePixels[sourceAddress * 3 + 1];
                    b = sourcePixels[sourceAddress * 3 + 0];
                    destinationPixels[destinationAddress * 2] = b;
                    destinationPixels[destinationAddress * 2 + 1] = r;
                    break;
                //Reorder values
                case PsuTexturePixelFormat.Rgba8888:
                    destinationPixels[destinationAddress * 4 + 1] = sourcePixels[sourceAddress * 4]; // B
                    destinationPixels[destinationAddress * 4 + 2] = sourcePixels[sourceAddress * 4 + 1]; // G
                    destinationPixels[destinationAddress * 4 + 3] = sourcePixels[sourceAddress * 4 + 2]; // R
                    destinationPixels[destinationAddress * 4] = sourcePixels[sourceAddress * 4 + 3]; // A
                    break;
                case PsuTexturePixelFormat.Abgr8888:
                    destinationPixels[destinationAddress * 4 + 2] = sourcePixels[sourceAddress * 4]; // B
                    destinationPixels[destinationAddress * 4 + 1] = sourcePixels[sourceAddress * 4 + 1]; // G
                    destinationPixels[destinationAddress * 4 + 0] = sourcePixels[sourceAddress * 4 + 2]; // R
                    destinationPixels[destinationAddress * 4 + 3] = sourcePixels[sourceAddress * 4 + 3]; // A
                    break;
                //These values have no bitmasks, so nothing gets copied.
                case PsuTexturePixelFormat.Mystery16:
                case PsuTexturePixelFormat.Mystery32:
                default: break;
            }
        }
    }
}
