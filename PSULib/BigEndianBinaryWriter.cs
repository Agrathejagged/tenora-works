using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psu_generic_parser
{
    public class BigEndianBinaryWriter : BinaryWriter
    {
        public BigEndianBinaryWriter(Stream output) : base(output)
        {
        }

        public override void Write(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value).Reverse().ToArray();
            base.Write(valueBytes);
        }

        public override void Write(uint value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value).Reverse().ToArray();
            base.Write(valueBytes);
        }

        public override void Write(short value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value).Reverse().ToArray();
            base.Write(valueBytes);
        }

        public override void Write(ushort value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value).Reverse().ToArray();
            base.Write(valueBytes);
        }
    }
}
