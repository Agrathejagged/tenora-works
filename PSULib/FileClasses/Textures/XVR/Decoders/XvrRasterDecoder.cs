using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PSULib.FileClasses.Textures.XVR
{
    /// <summary>
    /// This class is for common code for all decoders.
    /// That includes basic setup and unswizzling.
    /// </summary>
    abstract class XvrRasterDecoder
    {
        public Bitmap[] DecodeImage(byte[] rasterData, XvrImageParameters parameters, bool hasMips = false)
        {
            MemoryStream imageStream = new MemoryStream(rasterData);
            BinaryReader imageReader = new BinaryReader(imageStream);
            imageStream.Seek(parameters.startingOffset, SeekOrigin.Begin);
            int maxMips = hasMips ? parameters.maximumMips : 1;
            Bitmap[] mips = new Bitmap[maxMips];
            for (int i = 0; i < maxMips; i++)
            {
                mips[i] = readMip(imageReader, parameters.width >> i, parameters.height >> i);
            }
            imageReader.Close();
            return mips;
        }

        protected abstract Bitmap readMip(BinaryReader imageReader, int currWidth, int currHeight);
        

    }
}
