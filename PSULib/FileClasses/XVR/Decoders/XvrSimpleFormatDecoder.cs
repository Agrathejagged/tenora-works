using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser.FileClasses.XVR
{
    /// <summary>
    /// For non DXTc pixel formats.
    /// </summary>
    class XvrSimpleFormatDecoder : XvrRasterDecoder
    {
        public PsuTexturePixelFormat PixelFormat { get; }
        public XvrSimpleFormatDecoder(PsuTexturePixelFormat format)
        {
            PixelFormat = format;
        }

        protected override Bitmap readMip(BinaryReader imageReader, int currWidth, int currHeight)
        {
            Bitmap currentMip = new Bitmap(currWidth, currHeight, PixelFormat.ToPixelFormat());
            byte[] swizzledData = imageReader.ReadBytes(currWidth * currHeight * PixelFormat.GetDestinationBytesPerPixel());
            byte[] unswizzledData = unswizzleAndConvert(swizzledData, currWidth, currHeight);
            BitmapData data = currentMip.LockBits(new Rectangle(0, 0, currWidth, currHeight), ImageLockMode.WriteOnly, PixelFormat.ToPixelFormat());
            Marshal.Copy(unswizzledData, 0, data.Scan0, unswizzledData.Length);
            currentMip.UnlockBits(data);
            return currentMip;
        }

        protected byte[] unswizzleAndConvert(byte[] swizzledData, int width, int height)
        {
            int maxV = (int)(Math.Log(width, 2));
            int maxU = (int)(Math.Log(height, 2));
            
            byte[] unswizzledData = new byte[width * height * PixelFormat.GetDestinationBytesPerPixel()];
            int sourceBytesPerPixel = PixelFormat.GetSourceBytesPerPixel();

            for (int j = 0; (j < width * height) && (j * sourceBytesPerPixel < swizzledData.Length); j++)
            {
                int u = 0, v = 0;
                int origCoord = j;
                for (int k = 0; k < maxU || k < maxV; k++)
                {
                    if (k < maxV)   //Transpose!
                    {
                        v |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }
                    if (k < maxU)   //Transpose!
                    {
                        u |= (origCoord & 1) << k;
                        origCoord >>= 1;
                    }
                }
                if (u < height && v < width)
                {
                    PixelFormat.DecodePixel(swizzledData, j, unswizzledData, (u * width + v));
                }
            }
            return unswizzledData;
        }

    }
}
