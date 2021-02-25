using System;

namespace PROTOCOL
{
    public class Header : ISerializable
    {
        public byte HEADER { get; set; }
        public byte LENGTH { get; set; }
        public byte SYNCNO { get; set; }
        public byte RESERVED { get; set; }
        public byte TYPE { get; set; }

        public Header() { }
        public Header(byte[] bytes)
        {
            HEADER = bytes[0];
            LENGTH = bytes[1];
            SYNCNO = bytes[2];
            RESERVED = bytes[3];
            TYPE = bytes[4];
        }


        public byte[] GetBytes()
        {
            byte[] bytes = new byte[5];
            bytes[0] = HEADER;
            bytes[1] = LENGTH;
            bytes[2] = SYNCNO;
            bytes[3] = RESERVED;
            bytes[4] = TYPE;

            return bytes;
        }

        public int GetSize()
        {
            return 5;
        }
    }
}
