using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Textures.XVR
{
    class XvrRasterEncoders
    {
        public static XvrRasterEncoder GetEncoder(XvrImageParameters parameters)
        {
            switch (PsuTextureTypeMethods.FromByte(parameters.pixelFlags))
            {
                case PsuTextureType.Raster: return new XvrSimpleFormatEncoder(parameters.PixelFormat);
                    /*
                case PsuTextureType.Dxt1: return new DxtnDecoder(parameters.PixelFormat);
                case PsuTextureType.Dxt2: return new Dxt2Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt3: return new Dxt3Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt4: return new Dxt4Decoder(parameters.PixelFormat);
                case PsuTextureType.Dxt5: return new Dxt5Decoder(parameters.PixelFormat);*/
                default: throw new Exception("Invalid texture format " + parameters.pixelFlags);
            }
        }

        public static XvrRasterEncoder GetEncoder(PsuTextureType texType, PsuTexturePixelFormat pixelFormat)
        {
            switch (texType)
            {
                case PsuTextureType.Raster: return new XvrSimpleFormatEncoder(pixelFormat);
                /*
            case PsuTextureType.Dxt1: return new DxtnDecoder(parameters.PixelFormat);
            case PsuTextureType.Dxt2: return new Dxt2Decoder(parameters.PixelFormat);
            case PsuTextureType.Dxt3: return new Dxt3Decoder(parameters.PixelFormat);
            case PsuTextureType.Dxt4: return new Dxt4Decoder(parameters.PixelFormat);
            case PsuTextureType.Dxt5: return new Dxt5Decoder(parameters.PixelFormat);*/
                default: throw new Exception("Invalid texture format " + texType);
            }
        }
    }
}
