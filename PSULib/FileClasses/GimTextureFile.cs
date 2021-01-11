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
    public class GimTextureFile : PsuFile, TextureFile
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
