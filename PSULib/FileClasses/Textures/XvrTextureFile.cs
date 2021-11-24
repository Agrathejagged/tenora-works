using PSULib.FileClasses.Textures.XVR;
using PSULib.FileClasses.General;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PSULib.FileClasses.Textures
{
    public class XvrTextureFile : PsuFile, ITextureFile
    {
        byte[] rawData;
        string superFormat = "GBIX";
        string subFormat = "PVRT";
        int globalIndex = 8; //Always?
        int unknown1 = 0; //Think 0 generally works here...
        public string textureFormat = "unknown";
        Bitmap[] mips;
        bool[] mipDirty;
        public PsuTexturePixelFormat OriginalPixelFormat { get; set; }
        public PsuTextureType OriginalTextureType { get; set; }
        public PsuTexturePixelFormat SavePixelFormat { get; set; }
        public PsuTextureType SaveTextureType { get; set; }

        public Bitmap[] mipMaps
        {
            get
            {
                return mips;
            }

            set
            {
                mips = value;
            }
        }

        string ITextureFile.filename
        {
            get
            {
                return filename;
            }

            set
            {
                filename = value;
            }
        }

        byte logTwo(int toCheck)
        {
            for (byte i = 0; i < 32; i++)
            {
                if (toCheck >> i == 1 && toCheck >> i + 1 == 0)
                    return i;
            }
            return 0;
        }

        public XvrTextureFile(byte[] theHeader, byte[] theFile, string inFilename)
        {
            header = theHeader;
            rawData = theFile;
            filename = inFilename;
            parseData();
        }

        public XvrTextureFile(List<Bitmap> mips, PsuTexturePixelFormat pixelFormat, string inFilename) : this(mips.ToArray(), pixelFormat, inFilename)
        {

        }

        public XvrTextureFile(Bitmap[] mips, PsuTexturePixelFormat pixelFormat, string inFilename)
        {
            filename = inFilename;
            rawData = new byte[0];
            this.mips = (Bitmap[])mips.Clone();

            unknown1 = 0;
            subFormat = "PVRT";

            OriginalPixelFormat = pixelFormat;
            SavePixelFormat = pixelFormat;
            OriginalTextureType = PsuTextureType.Raster;
            SaveTextureType = PsuTextureType.Raster;


            mipDirty = new bool[mips.Length];
            for (int i = 0; i < mipDirty.Length; i++)
            {
                mipDirty[i] = true;
            }
        }

        void parseData()
        {
            MemoryStream imageStream = new MemoryStream(rawData);
            BinaryReader imageReader = new BinaryReader(imageStream);
            string superFormat = new string(Encoding.ASCII.GetChars(header, 0, 4));
            int globalIndex = BitConverter.ToInt32(header, 4);
            unknown1 = BitConverter.ToInt32(header, 8);
            subFormat = new string(Encoding.ASCII.GetChars(header, 0x10, 4));
            if (subFormat != "PVRT")
                throw new Exception("Invalid format");
            //ignoring "filesize" value; if it's not accurate, TOO BAD
            XvrImageParameters imageParams = new XvrImageParameters(header, rawData);
            textureFormat = imageParams.getFormatString();
            OriginalPixelFormat = imageParams.PixelFormat;
            OriginalTextureType = imageParams.TextureType;
            SavePixelFormat = imageParams.PixelFormat;
            SaveTextureType = imageParams.TextureType;
            mips = XvrRasterDecoders.GetDecoder(imageParams).DecodeImage(rawData, imageParams, imageParams.HasMips);
            mipDirty = new bool[mips.Length];
        }

        public override byte[] ToRaw()
        {
            if (SaveTextureType != PsuTextureType.Raster)
            {
                if (!Array.TrueForAll(mipDirty, val => false))
                {
                    throw new InvalidOperationException("Can't save DXT textures yet");
                }
                else
                {
                    return rawData;
                }
            }
            int width = mips[0].Width;
            int height = mips[0].Height;

            XvrImageParameters parameters = new XvrImageParameters(width, height, SaveTextureType, mips.Length > 1, SavePixelFormat);
            byte[] imageData = XvrRasterEncoders.GetEncoder(parameters).EncodeImage(mips, parameters);

            MemoryStream headerStream = new MemoryStream();
            BinaryWriter headerWriter = new BinaryWriter(headerStream);
            headerWriter.Write(Encoding.ASCII.GetBytes(superFormat));
            headerWriter.Write(globalIndex);
            headerWriter.Write(unknown1);
            headerWriter.Write(0);
            headerWriter.Write(Encoding.ASCII.GetBytes(subFormat));
            headerWriter.Write(imageData.Length + 8);
            headerWriter.Write(parameters.pixelFormat);
            headerWriter.Write(parameters.pixelFlags);
            headerWriter.Write((short)0);
            headerWriter.Write((short)width);
            headerWriter.Write((short)height);
            header = headerStream.ToArray();

            MemoryStream outStream = new MemoryStream();
            BinaryWriter outWriter = new BinaryWriter(outStream);
            outWriter.Write((short)1);
            outWriter.Write((short)0x84);
            outWriter.Write(0);
            outWriter.Write(0);
            outWriter.Write((short)0x2A); //I don't know what this is!
            short dimensionsMips = 0;
            dimensionsMips |= (short)mips.Length;
            byte widthPower = logTwo(width);     //Make sure make sure MAKE SURE dimensions are a power of 2!
            byte heightPower = logTwo(height);
            dimensionsMips |= (short)(widthPower << 8);
            dimensionsMips |= (short)(heightPower << 4);
            //I don't think those mip count values are ever used, outside the lowest 4 bits.
            outWriter.Write(dimensionsMips);
            outWriter.Write(0);
            outWriter.Write(1);
            outWriter.Write(0);
            outWriter.Write(imageData.Length);
            outWriter.Write(imageData);
            return outStream.ToArray();
        }

        public bool allowMips()
        {
            return true;
        }

        public void loadImage(Bitmap toImport, bool rebuildMips)
        {
            List<Bitmap> newMips = new List<Bitmap>();
            newMips.Add(toImport);
            if (rebuildMips)
            {
                for (int k = 1; 1 << k <= toImport.Width && 1 << k <= toImport.Height; k++)
                {
                    Bitmap tempImage = new Bitmap(toImport, new Size(toImport.Width >> k, toImport.Height >> k));
                    newMips.Add(tempImage);
                }
            }
            mips = newMips.ToArray();
            mipDirty = new bool[mips.Length];
            for (int i = 0; i < mipDirty.Length; i++)
            {
                mipDirty[i] = true;
            }
        }

        public void ReplaceMip(Bitmap toImport, int mipToReplace)
        {
            mips[mipToReplace] = toImport;
            mipDirty[mipToReplace] = true;
        }

        public void loadXvrFile(byte[] fileToRead)
        {
            throw new NotImplementedException();
        }

        public Bitmap getPreviewMip(int mipIndex)
        {
            //this is a hack because DXT textures get converted to argb32 in case of rounding issues
            //it should probably be handled differently?
            if (!(mipDirty[mipIndex] && mips[mipIndex].PixelFormat == SavePixelFormat.ToPixelFormat() && mips[mipIndex].Width == mips[0].Width >> mipIndex && mips[mipIndex].Height == mips[0].Height >> mipIndex) && SavePixelFormat == OriginalPixelFormat)
            {
                return mips[mipIndex];
            }
            else return XvrRasterEncoders.GetEncoder(SaveTextureType, SavePixelFormat).getTranscodedMip(mips[mipIndex], mips[0].Width >> mipIndex, mips[0].Height >> mipIndex);
        }
    }
}
