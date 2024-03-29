﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures
{
    public interface ITextureFile
    {
        bool allowMips();

        Bitmap[] mipMaps { get; set; }

        string filename { get; set; }

        void loadXvrFile(byte[] fileToRead);

        void loadImage(Bitmap toImport, bool rebuildMips);

        void ReplaceMip(Bitmap toImport, int mipToReplace);

        Bitmap getPreviewMip(int mipIndex);
    }
}
