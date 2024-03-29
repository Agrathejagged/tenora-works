﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    class XvrRasterDecoders
    {
        public static XvrRasterDecoder GetDecoder(XvrImageParameters parameters)
        {
            switch (PsuTextureTypeMethods.FromByte(parameters.pixelFlags))
            {
                case PsuTextureType.Raster: return new XvrSimpleFormatDecoder(parameters.PixelFormat);
                case PsuTextureType.Dxt1: return new Dxt1Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt2: return new Dxt2Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt3: return new Dxt3Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt4: return new Dxt4Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt5: return new Dxt5Decoder(parameters.PixelFormat);
                default: throw new Exception("Invalid texture format " + parameters.pixelFlags);
            }
        }
    }
}
