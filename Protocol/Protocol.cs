using System;

namespace PROTOCOL
{
    public class CONSTANTS
    {
        public const byte FAS_ServoEnable = 0x2A;
        public const byte FAS_MoveToLimit = 0x36;
        public const byte FAS_GetActualPos = 0x53;
        public const byte FAS_MovePause = 0x58;
        public const byte FAS_ClearPosition = 0x56;
    }

    public interface ISerializable
    {
        byte[] GetBytes();
        int GetSize();
    }
    public class Protocol : ISerializable
    {
        public Header Header { get; set; }
        public ISerializable Data { get; set; }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[GetSize()];

            Header.GetBytes().CopyTo(bytes, 0);
            Data.GetBytes().CopyTo(bytes, Header.GetSize());

            return bytes;
        }

        public int GetSize()
        {
            return Header.GetSize() + Data.GetSize();
        }
    }
}
