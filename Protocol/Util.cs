using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PROTOCOL
{
    public class Util
    {
        public static void Send(Stream writer, Protocol msg)
        {
            writer.Write(msg.GetBytes(), 0, msg.GetSize());
        }

        public static Protocol Receive(Stream reader)
        {
            int totalRecv = 0;
            int sizeToRead = 5;
            byte[] hBuffer = new byte[sizeToRead];
            
            while(sizeToRead > 0)
            {
                byte[] buffer = new byte[sizeToRead];
                int recv = reader.Read(buffer, 0, sizeToRead);
                if (recv == 0)
                    return null;

                buffer.CopyTo(hBuffer, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
            }

            Console.WriteLine("헤더추출");
            Header header = new Header(hBuffer);

            totalRecv = 0;
            byte[] bBuffer = new byte[header.LENGTH-3];
            sizeToRead = (int)header.LENGTH-3;

            while (sizeToRead > 0)
            {
                byte[] buffer = new byte[sizeToRead];
                int recv = reader.Read(buffer, 0, sizeToRead);
                if (recv == 0)
                    return null;

                buffer.CopyTo(bBuffer, totalRecv);
                totalRecv += recv;
                sizeToRead -= recv;
            }

            ISerializable body = null;

            switch (header.TYPE)
            {
                case CONSTANTS.FAS_ServoEnable:
                    if (bBuffer[0] == 1)
                        Console.WriteLine("통신상태 : 정상");
                    else
                        Console.WriteLine("통신상태 : 에러");
                    break;
                case CONSTANTS.FAS_MoveToLimit:
                    if (bBuffer[0] == 1)
                        Console.WriteLine("통신상태 : 정상");
                    else
                        Console.WriteLine("통신상태 : 에러");
                    break;
                case CONSTANTS.FAS_GetActualPos:
                    if (bBuffer[0] == 1)
                    {
                        Console.WriteLine("통신상태 : 정상");
                        Console.WriteLine($"실제 위치 : {BitConverter.ToInt32(bBuffer,1)}");
                    }
                    else
                        Console.WriteLine("통신상태 : 에러");
                    break;
                case CONSTANTS.FAS_MovePause:
                    if (bBuffer[0] == 1)
                        Console.WriteLine("통신상태 : 정상");
                    else
                        Console.WriteLine("통신상태 : 에러");
                    break;
                case CONSTANTS.FAS_ClearPosition:
                    if (bBuffer[0] == 1)
                        Console.WriteLine("통신상태 : 정상");
                    else
                        Console.WriteLine("통신상태 : 에러");
                    break;
                default:
                    break;
            }

            return new Protocol() { Header = header, Data = body };



        }
    }
}
