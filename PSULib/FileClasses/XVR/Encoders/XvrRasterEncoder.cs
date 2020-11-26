using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace psu_generic_parser.FileClasses.XVR
{
    /// <summary>
    /// This class is for common code for all encoders.
    /// That includes basic setup and swizzling.
    /// </summary>
    abstract class XvrRasterEncoder
    {
        public byte[] EncodeImage(Bitmap[] mipImages, XvrImageParameters parameters)
        {
            int maxMips = parameters.HasMips ? parameters.maximumMips : 1;
            byte[][] mips = new byte[mipImages.Length][];
            //byte[][] mips = new byte[maxMips][];
            //for (int i = 0; i < maxMips; i++)
            for (int i = 0; i < mipImages.Length; i++)
            {
                mips[i] = writeMip(mipImages[i], parameters.width >> i, parameters.height >> i);
            }
            return mips.SelectMany(mip=>mip).ToArray();
        }

        protected abstract byte[] writeMip(Bitmap currentMip, int mipWidth, int mipHeight);

        public abstract Bitmap getTranscodedMip(Bitmap currentMip, int width, int height);
    }
}
