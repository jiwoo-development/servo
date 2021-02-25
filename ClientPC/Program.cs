using System;
using System.Net.Sockets;
using System.Threading;

using TCPIP;
using PROTOCOL;

namespace ClientPC
{
    class TYPE
    {
        public const string ServoOn = "1";
        public const string ServoOff = "2";
        public const string MoveToLimit = "3";
        public const string MovePause = "4";
        public const string Move = "5";
        public const string GetActualPos = "6";
        public const string ClearPosition = "7";
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            Client client = new Client("192.168.0.2", 2002);
            Socket socket = null;
            client.ConnectServer(ref socket);
            while (socket == null) ;

            Thread send = new Thread(() =>
            {
                try
                {
                    byte sync = 0;
                    Protocol protocol = new Protocol
                    {
                        Header = new Header()
                        {
                            HEADER = 0xAA,
                            LENGTH = 0,
                            SYNCNO = sync++,
                            RESERVED = 0x00,
                            TYPE = 0x2A
                        }
                    };
                    while (true)
                    {
                        Client.sendDone.Reset();
                        switch (Console.ReadLine())
                        {
                            case TYPE.ServoOn:
                                protocol.Data = new REQUEST(new byte[1] { 1 });

                                protocol.Header.LENGTH = 4;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_ServoEnable;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.ServoOff:
                                protocol.Data = new REQUEST(new byte[1] { 0 });

                                protocol.Header.LENGTH = 4;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_ServoEnable;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.MoveToLimit:
                                byte[] pps = BitConverter.GetBytes(1000);
                                byte[] direction = new byte[1] { 1 };

                                byte[] data = new byte[5];
                                pps.CopyTo(data, 0);
                                direction.CopyTo(data, 4);

                                protocol.Data = new REQUEST(data);

                                protocol.Header.LENGTH = 3 + 5;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_MoveToLimit;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.MovePause:
                                protocol.Data = new REQUEST(new byte[1] { 1 });

                                protocol.Header.LENGTH = 4;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_MovePause;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.Move:
                                protocol.Data = new REQUEST(new byte[1] { 0 });

                                protocol.Header.LENGTH = 4;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_MovePause;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.GetActualPos:
                                protocol.Data = new REQUEST(new byte[0]);

                                protocol.Header.LENGTH = 3;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = CONSTANTS.FAS_GetActualPos;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            case TYPE.ClearPosition:
                                protocol.Data = new REQUEST(new byte[0]);

                                protocol.Header.LENGTH = 3;
                                protocol.Header.SYNCNO = sync++;
                                protocol.Header.TYPE = 0x56;

                                Client.Send(socket, protocol.GetBytes());
                                break;
                            default:
                                Console.WriteLine("1~4");
                                continue;
                        }
                        Console.WriteLine("데이터 송신");
                        /*
                        Console.WriteLine($"HEADER : {protocol.Header.HEADER}");
                        Console.WriteLine($"LENGTH : {protocol.Header.LENGTH}");
                        Console.WriteLine($"SYNCNO : {protocol.Header.SYNCNO}");
                        Console.WriteLine($"RESERVED : {protocol.Header.RESERVED}");
                        Console.WriteLine($"TYPE : {protocol.Header.TYPE}");
                        */
                        Client.sendDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {

                }
            });
            send.Start();

            Thread receive = new Thread(() =>
            {
                try
                {
                    Protocol protocol = new Protocol();
                    while (true)
                    {
                        Client.receiveDone.Reset();
                        Console.WriteLine("데이터 수신");
                        Client.Receive(socket);
                        Client.receiveDone.WaitOne();


                        if (receiveData.buffer[5] == 0)
                        {
                            Console.WriteLine($"통신상태 : 양호 {receiveData.buffer[5]}");
                            if (receiveData.buffer[4] == CONSTANTS.FAS_GetActualPos)
                            {
                                Console.WriteLine($"실제 위치 : {BitConverter.ToInt32(receiveData.buffer, 6)}");
                            }
                        }
                        else
                            Console.WriteLine($"통신상태 : 불량 {receiveData.buffer[5]}");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    socket.Close();
                }
            });
            receive.Start();
        }
    }
}

