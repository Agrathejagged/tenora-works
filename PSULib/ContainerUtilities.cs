﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib
{
    public class ContainerUtilities
    {
        public static byte[] encodePaddedSjisString(string toEncode, int encodedLength)
        {
            byte[] unpaddedString = Encoding.GetEncoding("shift-jis").GetBytes(toEncode);
            byte[] paddedString;
            if (unpaddedString.Length == 0x20)
            {
                paddedString = unpaddedString;
            }
            else
            {
                paddedString = new byte[0x20];
                Array.Copy(unpaddedString, paddedString, Math.Min(0x20, unpaddedString.Length));
            }
            return paddedString;
        }
    }
}
