/*
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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace WebSploit.Nscn
{
    class Main
    {
        static int host_count = 0;
        public static void GetWebsiteIp(string web)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(web);
                foreach(IPAddress addr in hostEntry.AddressList)
                    Console.WriteLine($"nscn> Found IP address {addr.ToString()} on {web}.");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!]-[websploit@nscn] > {ex.Message}\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static string[] pingHosts(string input)
        {
            return new string[]{"s", "a"};
        }

        public static void pingLan(string ip, bool verbose)
        {
            Console.WriteLine($"nscn/pLan> Starting NSCN LAN host scan at {DateTime.Now}...\n");
            int title_lng = $"| Open Hosts On {ip} |".Length;
            Console.WriteLine(new string('_', title_lng));
            Console.WriteLine($"| Open Hosts On {ip} |");
            Console.WriteLine(new string('-', title_lng));

            List<string> open_hosts = new List<string>();
            string[] ipItems = ip.Split('.');
            string ipSubstring = "";
            for(int i = 0; i < ipItems.Length - 1; i++)
                ipSubstring = ipSubstring + ipItems[i] + ".";
            Parallel.For(1, 257, i => {
                string current_ip = ipSubstring + i.ToString();
                var host = pingHostToSeeIfUp(current_ip);
                if(host != null)
                {
                    open_hosts.Add(current_ip);
                    host_count++;
                    Console.WriteLine($"| Host {current_ip} is up.");
                    if(verbose)
                    {
                        Console.WriteLine($"| Found host at {host.time_discovered}.");
                        Console.WriteLine($"| Replied in {host.response_time} ms at the {host.pingResponded} ping.");
                        Console.WriteLine("|"+new string('-',title_lng-1));
                    }
                }
            });
            if(!verbose)
                Console.WriteLine("|"+new string('-',title_lng-1));
            Console.WriteLine($"\nnscn/pLan> Found a total of {host_count} open hosts of 256 on {ip}.\n");
            host_count = 0;
        }

        public static void pingHost(string host, int bytes)
        {
            if(bytes > 65500)
                bytes = 65500;
            IPAddress address = IPAddress.Parse(host);
            PingOptions options = new PingOptions(128, true);
            Ping ping = new Ping();
            byte[] buffer = new byte[bytes];
            for(int i = 0; i < 4; i++)
            {
                try
                {
                    var reply = ping.Send(address, 1000, buffer, options);
                    if(reply != null)
                    {
                        if(reply.Status == IPStatus.Success)
                        {
                            Console.WriteLine($"nscn/pingHost> Reply from {reply.Address}: bytes={reply.Buffer.Length} time={reply.RoundtripTime}, TTL={reply.Options.Ttl}");
                        }
                        else if(reply.Status == IPStatus.TimedOut)
                        {
                            Console.WriteLine("nscn/pingHost> Connection timed out...");
                        }
                        else
                        {
                            Console.WriteLine($"nscn/pingHost> Connection failed: {reply.Status.ToString()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("nscn/pingHost> Ping reply was null for some reason...");
                    }
                }
                catch(PingException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[!]-[nscn/errLogger] > An error occured : {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch(SocketException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[!]-[nscn/errLogger] > An error occured : {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        static Host pingHostToSeeIfUp(string host)
        {
            if(IPAddress.TryParse(host, out IPAddress addr))
            {
                PingOptions options = new PingOptions(128, true);
                Ping ping = new Ping();
                byte[] buffer = new byte[32];
                for(int i = 0; i < 4; i++)
                {
                    try
                    {
                        var reply = ping.Send(addr, 1000, buffer, options);
                        if(reply != null)
                        {
                            if(reply.Status == IPStatus.Success)
                            {
                                Host foundHost = new Host(host, DateTime.Now, Int32.Parse(reply.RoundtripTime.ToString()), i+1);
                                return foundHost;
                            }
                        }
                        else
                        {
                            Console.WriteLine("nscn/pingHost> Ping reply was null for some reason...");
                        }
                    }
                    catch(PingException)
                    {
                        return null;
                    }
                    catch(SocketException)
                    {
                        return null;
                    }
                }
                return null;
            }
            else
                return null;
        }

        static string getMacAddress(string ipv4)
        {
            if(IPAddress.TryParse(ipv4, out IPAddress adr))
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(ipv4);
                foreach(IPAddress addr in host.AddressList)
                {
                    if(addr.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        return addr.ToString();
                    }
                }
                return "";
            }
            else
            {
                return null;
            }
        }
    }

    class Host
    {
        public string ip;
        public int response_time;
        public DateTime time_discovered;
        public int pingResponded;

        public Host(string ip, DateTime time, int response_time, int pingResponded)
        {
            this.ip = ip;
            this.time_discovered = time;
            this.response_time = response_time;
            this.pingResponded = pingResponded;
        }
    }
}