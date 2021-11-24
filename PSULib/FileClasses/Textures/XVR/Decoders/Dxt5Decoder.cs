using PSULib.FileClasses.Textures.XVR;
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
    class Dxt5Decoder : DxtnDecoder
    {
        public Dxt5Decoder(PsuTexturePixelFormat pixelFormat) : base(pixelFormat)
        {
        }

        protected override Bitmap readMip(BinaryReader imageReader, int currWidth, int currHeight)
        {
            Bitmap toReturn = new Bitmap(currWidth, currHeight, PixelFormat.Format32bppArgb);
            BitmapData currMip = toReturn.LockBits(new Rectangle(0, 0, currWidth, currHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte[] currImage = imageReader.ReadBytes(Math.Max(8, currWidth * currHeight));
            byte[] imgArr = new byte[currWidth * currHeight * 4];
            for (int k = 0; k * 64 < imgArr.Length; k++)  //i represents the current block
            {
                byte alpha0 = currImage[k * 16];
                byte alpha1 = currImage[k * 16 + 1];
                long alphaData = BitConverter.ToInt64(currImage, k * 16);
                alphaData >>= 16;
                ushort color0 = BitConverter.ToUInt16(currImage, k * 16 + 8);
                ushort color1 = BitConverter.ToUInt16(currImage, k * 16 + 10);
                int pixelData = BitConverter.ToInt32(currImage, k * 16 + 12);
                byte color0R = (byte)((color0 & 0xF800) >> 8);
                byte color0G = (byte)((color0 & 0x7E0) >> 3);
                byte color0B = (byte)((color0 & 0x1F) << 3);
                byte color1R = (byte)((color1 & 0xF800) >> 8);
                byte color1G = (byte)((color1 & 0x7E0) >> 3);
                byte color1B = (byte)((color1 & 0x1F) << 3);

                for (int j = 0; j < 16; j++)
                {
                    int currAlphaCtrl = (int)alphaData & 0x7;
                    alphaData >>= 3;
                    int currPixel = pixelData & 3;
                    pixelData >>= 2;
                    int xCoord = k % (currWidth / 4) * 4 + j % 4;
                    int yCoord = k / (currWidth / 4) * 4 + j / 4;
                    int destinationAddress = (xCoord + yCoord * currWidth) * 4;
                    imgArr[destinationAddress] = getColorComponent(color0, color1, color0B, color1B, currPixel);
                    imgArr[destinationAddress + 1] = getColorComponent(color0, color1, color0G, color1G, currPixel);
                    imgArr[destinationAddress + 2] = getColorComponent(color0, color1, color0R, color1R, currPixel);
                    imgArr[destinationAddress + 3] = getAlphaComponent(alpha0, alpha1, currAlphaCtrl);
                }
            }
            Marshal.Copy(imgArr, 0, currMip.Scan0, imgArr.Length);
            toReturn.UnlockBits(currMip);
            return toReturn;
        }

        private byte getAlphaComponent(byte alpha0, byte alpha1, int alphaIndex)
        {
            if (alpha0 > alpha1)
            {
                switch (alphaIndex)
                {
                    case 0: return alpha0;
                    case 1: return alpha1;
                    case 2: return (byte)((6 * alpha0 + 1 * alpha1) / 7);
                    case 3: return (byte)((5 * alpha0 + 2 * alpha1) / 7);
                    case 4: return (byte)((4 * alpha0 + 3 * alpha1) / 7);
                    case 5: return (byte)((3 * alpha0 + 4 * alpha1) / 7);
                    case 6: return (byte)((2 * alpha0 + 5 * alpha1) / 7);
                    case 7: return (byte)((alpha0 + 6 * alpha1) / 7);
                }
            }
            else
            {
                switch (alphaIndex)
                {
                    case 0: return alpha0;
                    case 1: return alpha1;
                    case 2: return (byte)((4 * alpha0 + 1 * alpha1) / 5);
                    case 3: return (byte)((3 * alpha0 + 2 * alpha1) / 5);
                    case 4: return (byte)((2 * alpha0 + 3 * alpha1) / 5);
                    case 5: return (byte)((1 * alpha0 + 4 * alpha1) / 5);
                    case 6: return 0;
                    case 7: return 255;
                }
            }
            return 0;
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
                    case 3: return (byte)((component0 + component1) / 2);
                    default: return 0;
                }
            }
        }
    }
}
