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
    class GimTextureFile : PsuFile, TextureFile
    {
        byte[] rawData;
        Bitmap mainImage;
        public GimSharp.GimPaletteFormat palFormat { get; set; }
        public GimSharp.GimDataFormat dataFormat { get; set; }

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

        public Bitmap[] mipMaps
        {
            get
            {
                return new Bitmap[] { mainImage };
            }

            set
            {
                if (value != null && value.Length > 0)
                    mainImage = value[0];
            }
        }

        public GimTextureFile(byte[] theHeader, byte[] theFile, string inFilename)
        {
            header = theHeader;
            rawData = theFile;
            filename = inFilename;
            parseData();
        }

        private void parseData()
        {
            GimSharp.GimTexture tex = new GimSharp.GimTexture(rawData);
            mainImage = tex.ToBitmap();
            dataFormat = tex.DataFormat;
            palFormat = tex.PaletteFormat;
            /*
            MemoryStream inStream = new MemoryStream(rawData);
            BinaryReader inReader = new BinaryReader(inStream);
            inStream.Seek(0x10, SeekOrigin.Begin);
            int category = 0;
            int categoryLength = 0xC;
            while(category < 4)
            {
                inStream.Seek(categoryLength - 0xC, SeekOrigin.Current);
                category = inReader.ReadInt32();
                inReader.ReadInt32();
                categoryLength = inReader.ReadInt32();
            }
            byte[] imageBytes = new byte[0];
            short width = 0, height = 0;
            ColorPalette palette = palette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;

            if(category == 4)
            {
                inStream.Seek(0xC, SeekOrigin.Current);
                width = inReader.ReadInt16();
                height = inReader.ReadInt16();

                inStream.Seek(0x34, SeekOrigin.Current);
                imageBytes = new byte[width * height / 2];
                for(int i = 0; i < width*height/2; i++)
                {
                    byte currentByte = inReader.ReadByte();
                    imageBytes[i] = (byte)(((currentByte & 0xF0) >> 4) | ((currentByte & 0xF) << 4));
                }
            }
            category = inReader.ReadInt32();
            inReader.ReadInt32();
            categoryLength = inReader.ReadInt32();
            if(category == 5)
            {
                inStream.Seek(0x44, SeekOrigin.Current);

                for (int i = 0; i < 16; i++)
                {
                    palette.Entries[i] = Color.FromArgb(inReader.ReadInt32());
                }
            }
            PixelFormat toUse = PixelFormat.Format4bppIndexed;
            mainImage = new Bitmap(width, height, toUse);
            mainImage.Palette = palette;
            BitmapData temp = mainImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, toUse);
            Marshal.Copy(imageBytes, 0, temp.Scan0, imageBytes.Length);
            mainImage.UnlockBits(temp);*/
        }

        public override byte[] ToRaw()
        {
            return rawData;
        }

        public bool allowMips()
        {
            return false;
        }

        public void loadXvrFile(byte[] fileToRead)
        {
            throw new NotImplementedException();
        }

        public void loadImage(Bitmap toImport, bool rebuildMips)
        {
            throw new NotImplementedException();
        }

        public void ReplaceMip(Bitmap toImport, int mipToReplace)
        {
            throw new NotImplementedException();
        }

        public Bitmap getPreviewMip(int mipIndex)
        {
            return mipMaps[mipIndex];
        }
    }
}
