using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Models
{
    /// <summary>
    /// D3D filter for minify and mipmaps. Technically also includes max anisotropy, but the value's always 1. Naming convention based on en_Kyozoress.xto from Sonic 06.
    /// </summary>
    public enum MinifyMipFilter
    {
        MIN_POINT_MIPMAP_NEAREST = 0,
        MIN_LINEAR_MIPMAP_NEAREST = 1,
        MIN_POINT_MIPMAP_POINT = 2,
        MIN_POINT_MIPMAP_LINEAR = 3,
        MIN_LINEAR_MIPMAP_POINT = 4,
        MIN_LINEAR_MIPMAP_LINEAR = 5,
        MIN_ANISOTROPIC_MIPMAP_NEAREST = 6,
        MIN_ANISOTROPIC_MIPMAP_POINT = 7,
        MIN_ANISOTROPIC_MIPMAP_LINEAR = 8,
    }

    public enum MagnifyFilter
    {
        POINT = 0,
        LINEAR = 1,
        ANISOTROPIC = 2
    }
}
