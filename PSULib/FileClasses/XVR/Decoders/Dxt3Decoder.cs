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
    class Dxt3Decoder : DxtnDecoder
    {
        public Dxt3Decoder(PsuTexturePixelFormat pixelFormat) : base(pixelFormat)
        {

        }

        protected override Bitmap readMip(BinaryReader imageReader, int currWidth, int currHeight)
        {
            Bitmap bmp = new Bitmap(currWidth, currHeight, PixelFormat.Format32bppArgb);
            BitmapData currMip = bmp.LockBits(new Rectangle(0, 0, currWidth, currHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte[] currImage = imageReader.ReadBytes(currWidth * currHeight / 2);
            byte[] imgArr = new byte[currWidth * currHeight * 2];
            for (int k = 0; (k * 64) < imgArr.Length; k++)  //i represents the current block
            {
                long alphaVals = BitConverter.ToInt64(currImage, k * 16);
                ushort color0 = BitConverter.ToUInt16(currImage, k * 16 + 8);
                ushort color1 = BitConverter.ToUInt16(currImage, k * 16 + 10);
                int pixelData = BitConverter.ToInt32(currImage, k * 16 + 12);
                int color0R = (color0 & 0xF800) >> 8;
                int color0G = (color0 & 0x7E0) >> 3;
                int color0B = (color0 & 0x1F) << 3;
                int color1R = (color0 & 0xF800) >> 8;
                int color1G = (color1 & 0x7E0) >> 3;
                int color1B = (color1 & 0x1F) << 3;

                for (int j = 0; j < 16; j++)
                {
                    byte currAlpha = (byte)(alphaVals & 0xF);
                    alphaVals >>= 4;
                    int currPixel = pixelData & 3;
                    pixelData >>= 2;
                    int pixelOut = 0;
                    switch (currPixel)
                    {
                        case 0: pixelOut = color0; break;
                        case 1: pixelOut = color1; break;
                        case 2: pixelOut = (currAlpha << 28) | (((color0R + color1R) / 2) << 16) | (((color0G + color1G) / 2) << 8) | (((color0B + color1B) / 2)); break;
                        case 3: pixelOut = 0; break;
                    }
                    byte[] pixelArr = BitConverter.GetBytes(pixelOut);
                    int xCoord = (k % (currWidth / 4)) * 4 + j % 4;
                    int yCoord = (k / (currWidth / 4)) * 4 + j / 4;
                    int destinationAddress = (xCoord + yCoord * currWidth) * 4;
                    imgArr[destinationAddress] = pixelArr[0];
                    imgArr[destinationAddress + 1] = pixelArr[1];
                    imgArr[destinationAddress + 2] = pixelArr[2];
                    imgArr[destinationAddress + 3] = pixelArr[3];
                }
            }
            Marshal.Copy(imgArr, 0, currMip.Scan0, imgArr.Length);
            bmp.UnlockBits(currMip);
            return bmp;
        }
    }
}
