using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    public class BigEndianBinaryReader : BinaryReader
    {
        public static BinaryReader GetEndianSpecificBinaryReader(Stream stream, bool bigEndian)
        {
            if (bigEndian)
                return new BigEndianBinaryReader(stream);
            return new BinaryReader(stream);
        }

        public BigEndianBinaryReader(Stream inStream) : base(inStream)
        {

        }

        public override short ReadInt16()
        {
            return unchecked ((short)(ReverseBytes(base.ReadUInt16())));
        }

        public override ushort ReadUInt16()
        {
            return ReverseBytes(base.ReadUInt16());
        }

        public override int ReadInt32()
        {
            return unchecked ((int)(ReverseBytes(base.ReadUInt32())));
        }
        public int ReadLittleEndianInt32()
        {
            return base.ReadInt32();
        }

        public override float ReadSingle()
        {
            return BitConverter.ToSingle(base.ReadBytes(4).Reverse().ToArray(), 0);
        }

        public override uint ReadUInt32()
        {
            return ReverseBytes(base.ReadUInt32());
        }

        private static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        private static UInt16 ReverseBytes(UInt16 value)
        {
            return (ushort)((value & 0x00FFu) << 8 | (value & 0xFF00u) >> 8);
        }
    }

    public static class BinaryReaderAddition
    {
        public static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            return BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);
        }

        public static string ReadAsciiString(this BinaryReader inReader, int location)
        {
            inReader.BaseStream.Seek(location, SeekOrigin.Begin);
            StringBuilder sb = new StringBuilder();
            char c = (char)inReader.ReadByte();
            while (c != '\0')
            {
                sb.Append(c);
                c = (char)inReader.ReadByte();
            }
            return sb.ToString();
        }
    }
}
