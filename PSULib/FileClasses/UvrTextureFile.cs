using GimSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    class UvrTextureFile : PsuFile, TextureFile
    {
        private UnpointeredFile backupRaw;
        public GimPaletteFormat palFormat { get; set; }
        PixelFormat imageFormat;
        public Bitmap[] mipMaps;
        public int mipCount = 1;
        byte[] rawData;
        int width = 0;
        int height = 0;
        int globalIndex = 0;
        int unknown1 = 0;
        string superFormat;
        string subFormat;
        private byte[] inHeader;

        public UvrTextureFile(byte[] inHeader, byte[] rawData, string filename)
        {
            backupRaw = new UnpointeredFile(filename, rawData, inHeader);

            this.inHeader = inHeader;
            this.rawData = rawData;
            this.filename = filename;
            parseData();
        }

        Bitmap[] TextureFile.mipMaps
        {
            get
            {
                return mipMaps;
            }

            set
            {
                mipMaps = value;
            }
        }

        string TextureFile.filename
        {
            get
            {
                return this.filename;
            }

            set
            {
                this.filename = value;
            }
        }

        public bool allowMips()
        {
            return true;
        }

        public void loadImage(Bitmap toImport, bool rebuildMips)
        {
            throw new NotImplementedException();
        }

        public void loadXvrFile(byte[] fileToRead)
        {
            throw new NotImplementedException();
        }

        public void ReplaceMip(Bitmap toImport, int mipToReplace)
        {
            throw new NotImplementedException();
        }

        void parseData()
        {
            MemoryStream imageStream = new MemoryStream(rawData);
            BinaryReader imageReader = new BinaryReader(imageStream);
            superFormat = new string(ASCIIEncoding.ASCII.GetChars(imageReader.ReadBytes(4), 0, 4));
            globalIndex = imageReader.ReadInt32();
            unknown1 = imageReader.ReadInt32();
            imageReader.ReadInt32();
            subFormat = new string(ASCIIEncoding.ASCII.GetChars(imageReader.ReadBytes(4), 0, 4));
            int fileSize = imageReader.ReadInt32();
            //ignoring "filesize" value; if it's not accurate, TOO BAD
            byte colorDepth = imageReader.ReadByte();
            //Assuming all of these are palettized. They may not be!
            
            byte flags = imageReader.ReadByte();
            int paletteSize = 16;
            int bpp = 4;// flags & 0xE;mipCount = 1;

            palFormat = (GimPaletteFormat)colorDepth;
            imageReader.ReadInt16();
            width = imageReader.ReadInt16();
            height = imageReader.ReadInt16();
            if ((flags & 0xF) == 0) //I think this is unpalettized data?
            {
                readRawImage(imageReader, imageStream);
            }
            else
            {
                int maxU = (int)(Math.Log(width, 2));
                int maxV = (int)(Math.Log(height, 2));
                imageFormat = PixelFormat.Format4bppIndexed;
                if ((flags & 2) == 2)
                {
                    bpp = flags & 0xC;
                    if (bpp == 8)
                    {
                        paletteSize = 256;
                        imageFormat = PixelFormat.Format8bppIndexed;
                    }
                    else if (bpp == 4)
                    {
                        paletteSize = 16;
                        imageFormat = PixelFormat.Format4bppIndexed;
                    }
                    else
                        throw new Exception("Invalid palette size " + (flags & 0xC));
                }
                else
                {
                    int tempBpp = flags & 0x4;
                    if (tempBpp != 0)
                    {
                        bpp = 8;
                        paletteSize = 256;
                        imageFormat = PixelFormat.Format8bppIndexed;
                    }
                    else
                    {
                        bpp = 4;
                        paletteSize = 16;
                        imageFormat = PixelFormat.Format4bppIndexed;
                    }
                }

                Bitmap parsedImage = new Bitmap(width, height, imageFormat);
                ColorPalette pal = parsedImage.Palette;

                for (int i = 0; i < paletteSize; i++)
                {
                    int alpha = 255;
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    switch (palFormat)
                    {
                        case GimPaletteFormat.Argb1555: //ACTUALLY 565
                            ushort currentColor = imageReader.ReadUInt16();
                            red = ((currentColor >> 11) & 0x1F) << 3;
                            green = ((currentColor >> 5) & 0x3F) << 2;
                            blue = ((currentColor >> 0) & 0x1F) << 3;
                            break;
                        case GimPaletteFormat.Argb4444:
                            currentColor = imageReader.ReadUInt16();
                            blue = ((currentColor & 0xF000) >> 8);
                            green = (currentColor & 0xF00) >> 4;
                            red = (currentColor & 0xF0) >> 0;
                            alpha = (currentColor & 0xF) << 4;
                            break;
                        case GimPaletteFormat.Argb8888:
                            blue = imageReader.ReadByte();
                            green = imageReader.ReadByte();
                            red = imageReader.ReadByte();
                            alpha = imageReader.ReadByte();
                            break;
                        case GimPaletteFormat.Rgb565: //ACTUALLY 1555
                            currentColor = imageReader.ReadUInt16();
                            alpha = 255 * ((currentColor >> 15) & 0x1) >> 0;
                            red = ((currentColor >> 10) & 0x1F) << 3;
                            green = ((currentColor >> 5) & 0x1F) << 3;
                            blue = ((currentColor >> 0) & 0x1F) << 3;
                            break;
                        case GimPaletteFormat.Unknown: throw new Exception("Bad format: " + colorDepth);
                    }
                    pal.Entries[i] = Color.FromArgb((alpha << 24) | (red << 0) | (blue << 16) | (green << 8));
                }

                byte[] pixelData = imageReader.ReadBytes(bpp * width * height / 8);

                byte[] untwiddledPixels = UnSwizzle(pixelData, 0, width, height, bpp);
                if (paletteSize == 16)
                {
                    for (int i = 0; i < untwiddledPixels.Length; i++)
                    {
                        byte left = (byte)(untwiddledPixels[i] >> 4);
                        byte right = (byte)(untwiddledPixels[i] & 0xF);
                        untwiddledPixels[i] = (byte)((right << 4) | left);
                    }
                }
                /*new byte[width * height / 2];

                int rowWidth = width / 2;
                int rowblocks = (rowWidth / 16);

                for (int y = 0; y < height; y++)
                {
                    for(int x = 0; x < rowWidth; x++)
                    {
                        int blockX = x / 16;
                        int blockY = y / 8;

                        int blockIndex = blockX + ((blockY) * rowblocks);
                        int blockAddress = blockIndex * 16 * 8;
                        byte pixel = pixelData[blockAddress + (x - blockX * 16) + ((y - blockY * 8) * 16)];
                        untwiddledPixels[y * rowWidth + x] = pixelData[blockAddress + (x - blockX * 16) + ((y - blockY * 8) * 16)];
                    }
                }*/

                BitmapData temp = parsedImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, imageFormat);
                Marshal.Copy(untwiddledPixels, 0, temp.Scan0, pixelData.Length);
                parsedImage.UnlockBits(temp);
                parsedImage.Palette = pal;
                mipMaps = new Bitmap[1];
                mipMaps[0] = parsedImage;
                return;
            }
        }

        private void readRawImage(BinaryReader imageReader, Stream imageStream)
        {

            int bpp = 4;
            switch(palFormat)
            {
                case GimPaletteFormat.Argb1555: imageFormat = PixelFormat.Format16bppRgb565; bpp = 16; break;
                case GimPaletteFormat.Rgb565: imageFormat = PixelFormat.Format16bppArgb1555; bpp = 16; break;
                //hoping this one never appears...
                //case GimPaletteFormat.Argb4444: imageFormat = PixelFormat.Format.Format16bppRgb565; bpp = 4; break;
                case GimPaletteFormat.Argb8888: imageFormat = PixelFormat.Format32bppArgb; bpp = 32; break;
                    //This'll make it crash quick.
                default: imageFormat = PixelFormat.Format32bppArgb; bpp = 8; break;
            }
            byte[] pixelData = imageReader.ReadBytes(bpp * width * height / 8);

            byte[] untwiddledPixels = UnSwizzle(pixelData, 0, width, height, bpp);
            Bitmap parsedImage = new Bitmap(width, height, imageFormat);
            BitmapData temp = parsedImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, imageFormat);
            Marshal.Copy(untwiddledPixels, 0, temp.Scan0, pixelData.Length);
            parsedImage.UnlockBits(temp);
            mipMaps = new Bitmap[1];
            mipMaps[0] = parsedImage;
        }

        private void setPixel(byte[] rasterData, int x, int y, byte value)
        {
            int byteOffset = y * (width / 2) + (x >> 1);
            int numBits = 4 * (x % 2);

            rasterData[byteOffset] |= (byte)(value << numBits);
        }

        private byte getPixel(byte[] rasterData, int x, int y)
        {
            int byteOffset = y * (width / 2) + (x >> 1);
            int numBits = 4 * (x % 2);

            return (byte)(rasterData[byteOffset] >> numBits);
        }

        public override byte[] ToRaw()
        {
            //Return this until a proper implementation is made
            return backupRaw.ToRaw();
        }

        public static byte[] UnSwizzle(byte[] source, int offset, int width, int height, int bpp)
        {
            int destinationOffset = 0;

            // Incorperate the bpp into the width
            width = (width * bpp) >> 3;

            byte[] destination = new byte[width * height];

            int rowblocks = (width / 16);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockX = x / 16;
                    int blockY = y / 8;

                    int blockIndex = blockX + ((blockY) * rowblocks);
                    int blockAddress = blockIndex * 16 * 8;

                    destination[destinationOffset] = source[offset + blockAddress + (x - blockX * 16) + ((y - blockY * 8) * 16)];
                    destinationOffset++;
                }
            }

            return destination;
        }

        public Bitmap getPreviewMip(int mipIndex)
        {
            return mipMaps[mipIndex];
        }
    }
}
