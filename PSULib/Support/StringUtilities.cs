using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    public class StringUtilities
    {
        public static byte[] encodePaddedSjisString(string toEncode, int encodedLength)
        {
            byte[] unpaddedString = Encoding.GetEncoding("shift-jis").GetBytes(toEncode);
            byte[] paddedString;
            if (unpaddedString.Length == encodedLength)
            {
                paddedString = unpaddedString;
            }
            else
            {
                paddedString = new byte[encodedLength];
                Array.Copy(unpaddedString, paddedString, Math.Min(encodedLength, unpaddedString.Length));
            }
            return paddedString;
        }
    }
}
