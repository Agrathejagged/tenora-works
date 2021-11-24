using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.Support
{
    /// <summary>
    /// I couldn't figure out what the actual CRC32 calculation is in the game, so I just ripped out the calculation here.
    /// </summary>
    public class PsuCrc32
    {
        /// <summary>
        /// Computes the game's version of the CRC32 for a given filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="truncate">Remove the last 4 bytes of the filename</param>
        /// <returns>game-compliant CRC of filename</returns>
        public static int ComputeHash(string filename, bool truncate)
        {
            int hashValue = -1;
            byte[] filenameBytes = Encoding.GetEncoding("Shift-JIS").GetBytes(filename);
            int filenameLength = truncate ? filenameBytes.Length - 4 : filenameBytes.Length;

            for(int i = 0; i < filenameLength; i++)
            {
                hashValue ^= (int)filenameBytes[i];
                for(int j = 0; j < 8; j++)
                {
                    if((hashValue & 0x1) != 0)
                    {
                        hashValue ^= -613349823; // 0xDB710641
                    }
                    hashValue >>= 1;
                }
            }
            hashValue = ~hashValue;
            return hashValue;
        }
    }
}
