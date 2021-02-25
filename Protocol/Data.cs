using System;

namespace PROTOCOL
{
    public class REQUEST : ISerializable
    {
        public byte[] DATA;

        public REQUEST() { }
        public REQUEST(byte[] bytes)
        {
            DATA = new byte[bytes.Length];
            bytes.CopyTo(DATA, 0);
        }
        public byte[] GetBytes()
        {
            return DATA;
        }

        public int GetSize()
        {
            return DATA.Length;
        }
    }
}
