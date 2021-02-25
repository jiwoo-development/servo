using System;
using System.Net.Sockets;
using System.Threading;

using TCPIP;
using PROTOCOL;

namespace MainPC
{
    public class MainApp
    {
        public static void Main(String[] args)
        {
            Server server = new Server("127.0.0.1", 5425);
            Socket socket = null;
            server.ServerStart(ref socket);
            while (socket == null) ;

            Thread receive = new Thread(() =>
            {
                Protocol protocol = new Protocol();
                while (true)
                {
                    Client.receiveDone.Reset();
                    Client.Receive(socket);
                    Console.WriteLine("데이터 수신");
                    Client.receiveDone.WaitOne();
                }
            });
            receive.Start();
        }
    }
}