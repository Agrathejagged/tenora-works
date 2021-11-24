using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    /// <summary>
    /// For non DXTc pixel formats.
    /// </summary>
    class XvrSimpleFormatEncoder : XvrRasterEncoder
    {
        public PsuTexturePixelFormat PixelFormat { get; }
        public XvrSimpleFormatEncoder(PsuTexturePixelFormat format)
        {
            PixelFormat = format;
        }

        protected override byte[] writeMip(Bitmap currentMip, int mipWidth, int mipHeight)
        {

            Bitmap resultBitmap = getTranscodedMip(currentMip, mipWidth, mipHeight);
            int currWidth = mipWidth;
            int currHeight = mipHeight;
            BitmapData data = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.ToPixelFormat());
            byte[] unswizzledData = new byte[currWidth * currHeight * PixelFormat.GetDestinationBytesPerPixel()];
            Marshal.Copy(data.Scan0, unswizzledData, 0, unswizzledData.Length);
            resultBitmap.UnlockBits(data);
            byte[] swizzledData = swizzleAndConvert(unswizzledData, currWidth, currHeight);
            return swizzledData;
        }

        public override Bitmap getTranscodedMip(Bitmap currentMip, int width, int height)
        {
            PixelFormat resultFormat = PixelFormat.ToPixelFormat();
            if (resultFormat != currentMip.PixelFormat)
            {
                Bitmap toReturn = new Bitmap(width, height, resultFormat);
                if (resultFormat == System.Drawing.Imaging.PixelFormat.Format16bppArgb1555)  //Goddammit GDI+, why you not work with argb1555.
                {
                    Bitmap temp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
                    if (width != currentMip.Width || height != currentMip.Height)
                    {
                        Graphics.FromImage(temp).DrawImage(currentMip, new Rectangle(0, 0, width, height));
                    }
                    else
                        Graphics.FromImage(temp).DrawImageUnscaledAndClipped(currentMip, new Rectangle(0, 0, width, height));
                    BitmapData tempData = temp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, resultFormat);
                    BitmapData realData = toReturn.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, resultFormat);
                    byte[] tempArr = new byte[height * width * 2];
                    Marshal.Copy(tempData.Scan0, tempArr, 0, tempArr.Length);
                    temp.UnlockBits(tempData);
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            if ((currentMip).GetPixel(i, j).A > 127)
                            {
                                tempArr[(i + j * width) * 2 + 1] |= 0x80;
                            }
                            else
                                tempArr[(i + j * width) * 2 + 1] &= 0x7F;
                        }
                    }
                    Marshal.Copy(tempArr, 0, realData.Scan0, tempArr.Length);
                    toReturn.UnlockBits(realData);
                }
                else
                {
                    Graphics g = Graphics.FromImage(toReturn);
                    if (width != currentMip.Width || height != currentMip.Height)
                    {
                        g.DrawImage(currentMip, new Rectangle(0, 0, width, height));
                    }
                    else
                        g.DrawImageUnscaledAndClipped(currentMip, new Rectangle(0, 0, width, height));
                }

                return toReturn;
            }
            return currentMip;
        }

        protected byte[] swizzleAndConvert(byte[] unswizzledData, int width, int height)
        {
            int maxV = (int)(Math.Log(width, 2));
            int maxU = (int)(Math.Log(height, 2));

            byte[] swizzledData = new byte[width * height * PixelFormat.GetSourceBytesPerPixel()];
            int sourceBytesPerPixel = PixelFormat.GetSourceBytesPerPixel();
            int destinationBytesPerPixel = PixelFormat.GetDestinationBytesPerPixel();

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
                    PixelFormat.EncodePixel(unswizzledData, (u * width + v), swizzledData, j);
                }
            }
            return swizzledData;
        }

    }
}
