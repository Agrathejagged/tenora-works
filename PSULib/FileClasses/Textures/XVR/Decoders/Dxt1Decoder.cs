using PSULib.FileClasses.Textures.XVR;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace PSULib.FileClasses.Textures.XVR
{
    /// <summary>
    /// DXT1 is premultiplied alpha. This should probably be a hint to the renderer, but that's not relevant here.
    /// </summary>
    class Dxt1Decoder : DxtnDecoder
    {
        public Dxt1Decoder(PsuTexturePixelFormat format) : base(format)
        {
        }

        protected override Bitmap readMip(BinaryReader imageReader, int currWidth, int currHeight)
        {
            Bitmap bmp = new Bitmap(currWidth, currHeight, PixelFormat.Format32bppArgb);
            BitmapData currMip = bmp.LockBits(new Rectangle(0, 0, currWidth, currHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte[] currImage = imageReader.ReadBytes(Math.Max(8, currWidth * currHeight / 2));
            byte[] imgArr = new byte[currWidth * currHeight * 4];
            for (int k = 0; k * 64 < imgArr.Length; k++)  //i represents the current block
            {
                ushort color0 = BitConverter.ToUInt16(currImage, k * 8);
                ushort color1 = BitConverter.ToUInt16(currImage, k * 8 + 2);
                int pixelData = BitConverter.ToInt32(currImage, k * 8 + 4);

                byte color0A, color1A, color0R, color1R, color0G, color1G, color0B, color1B = 0;
                if (pixelFormat == PsuTexturePixelFormat.Argb1555)
                {
                    color0A = 255;
                    color1A = 255;
                    color0R = (byte)((color0 & 0xF800) >> 8);
                    color0G = (byte)((color0 & 0x7E0) >> 3);
                    color0B = (byte)((color0 & 0x1F) << 3);
                    color1R = (byte)((color1 & 0xF800) >> 8);
                    color1G = (byte)((color1 & 0x7E0) >> 3);
                    /*
                color0A = (byte)(((color0 & 0x8000) >> 15) * 255);
                color1A = (byte)(((color1 & 0x8000) >> 15) * 255);
                color0R = (byte)((color0 & 0x7C00) >> 8);
                color0G = (byte)((color0 & 0x3E0) >> 8);
                color1R = (byte)((color1 & 0x7C00) >> 8);
                color1G = (byte)((color1 & 0x3E0) >> 8);*/
                }
                else if (pixelFormat == PsuTexturePixelFormat.Rgb565)//Only other option is RGB565...?
                {
                    color0A = 255;
                    color1A = 255;
                    color0R = (byte)((color0 & 0xF800) >> 8);
                    color0G = (byte)((color0 & 0x7E0) >> 3);
                    color0B = (byte)((color0 & 0x1F) << 3);
                    color1R = (byte)((color1 & 0xF800) >> 8);
                    color1G = (byte)((color1 & 0x7E0) >> 3);
                }
                else
                {
                    throw new Exception("Unexpected DXT1 pixel format " + pixelFormat.ToString());
                }
                color0B = (byte)((color0 & 0x1F) << 3); //Blue doesn't change.
                color1B = (byte)((color1 & 0x1F) << 3);
                for (int j = 0; j < 16; j++)
                {
                    int currPixel = pixelData & 3;
                    pixelData >>= 2;
                    int xCoord = k % (Math.Max(currWidth, 4) / 4) * 4 + j % 4;
                    int yCoord = k / (Math.Max(currWidth, 4) / 4) * 4 + j / 4;
                    int destinationAddress = (xCoord + yCoord * Math.Max(currWidth, 4)) * 4;
                    if (destinationAddress + 1 < imgArr.Length)
                    {
                        imgArr[destinationAddress] = getColorComponent(color0, color1, color0B, color1B, currPixel);
                        imgArr[destinationAddress + 1] = getColorComponent(color0, color1, color0G, color1G, currPixel);
                        imgArr[destinationAddress + 2] = getColorComponent(color0, color1, color0R, color1R, currPixel);
                        imgArr[destinationAddress + 3] = (byte)(pixelFormat == PsuTexturePixelFormat.Argb1555 ? getColorComponent(color0, color1, color0A, color1A, currPixel) : 255);
                    }
                }
            }
            Marshal.Copy(imgArr, 0, currMip.Scan0, imgArr.Length);
            bmp.UnlockBits(currMip);
            return bmp;

        }

        private byte getColorComponent(ushort color0, ushort color1, byte component0, byte component1, int index)
        {
            if (color0 > color1)
            {
                switch (index)
                {
                    case 0: return component0;
                    case 1: return component1;
                    case 2: return (byte)((2 * component0 + component1) / 3);
                    case 3: return (byte)((component0 + 2 * component1) / 3);
                    default: return 0;
                }
            }
            else
            {
                switch (index)
                {
                    case 0: return component0;
                    case 1: return component1;
                    case 2: return (byte)((component0 + component1) / 2);
                    case 3: return 0;
                    default: return 0;
                }
            }
        }
    }
}
