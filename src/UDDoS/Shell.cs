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
using System.Net;
using System.Web;
using System.Threading.Tasks;

namespace UDDoS
{
    class Shell
    {
        public static bool env = false;

        public static void CommandInterpreter(string command)
        {
            string[] command_words = command.Split(" ");
            if(command_words.Length >= 2)
            {
                if(command_words[1] == "-help")
                {
                    Help();
                }
                else
                {
                    if(command_words.Length == 5)
                    {
                        if(command_words[1] == "--http")
                        {
                            if(command_words[2] == "-d")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPDownload(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;
                            }
                            else if(command_words[2] == "-g")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPReqGET(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;
                            }
                            else if(command_words[2] == "-b")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPBoth(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;  
                            }
                            else
                                InvalidSyntaxErr(command);
                        }
                        else if(command_words[1] == "--https")
                        {
                            if(command_words[2] == "-d")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPSDownload(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;
                            }
                            else if(command_words[2] == "-g")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPSReqGET(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;
                            }
                            else if(command_words[2] == "-b")
                            {
                                if(isUrlValid(command_words[3]))
                                {
                                    if(Int32.TryParse(command_words[4], out int threads))
                                    {
                                        Task t = Task.Factory.StartNew(() => {
                                            DDoS.ProtocolHTTPSBoth(command_words[3], threads);
                                        });
                                        t.Wait();
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                }
                                else
                                    InvalidSyntaxErr(command);;
                            }
                            else
                                InvalidSyntaxErr(command);
                        }
                        else if(command_words[1] == "--tcp")
                        {
                            if(command_words[2] == "-h")
                            {
                                string[] target = command_words[3].Split(':');
                                if(target.Length == 2)
                                {
                                    if(isIpValid(target[0]))
                                    {
                                        if(Int32.TryParse(target[1], out int port) && port < 45505){
                                            if(Int32.TryParse(command_words[4], out int threads))
                                            {
                                                Task t = Task.Factory.StartNew(() => {
                                                    DDoS.ProtocolTCPHEAVY(target[0], port, threads);
                                                });
                                                t.Wait();
                                            }
                                            else
                                                InvalidSyntaxErr(command);
                                        }
                                        else
                                            InvalidSyntaxErr(command);
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                } 
                                else
                                    InvalidSyntaxErr(command);
                            }
                            else if(command_words[2] == "-e")
                            {
                                string[] target = command_words[3].Split(':');
                                if(target.Length == 2)
                                {
                                    if(isIpValid(target[0]))
                                    {
                                        if(Int32.TryParse(target[1], out int port) && port < 45505){
                                            if(Int32.TryParse(command_words[4], out int threads))
                                            {
                                                Task t = Task.Factory.StartNew(() => {
                                                    DDoS.ProtocolTCPEZ(target[0], port, threads);
                                                });
                                                t.Wait();
                                            }
                                            else
                                                InvalidSyntaxErr(command);
                                        }
                                        else
                                            InvalidSyntaxErr(command);
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                } 
                                else
                                    InvalidSyntaxErr(command);
                            }
                            else
                                InvalidSyntaxErr(command);
                        }
                        else if(command_words[1] == "--udp")
                        {
                            if(command_words[2] == "-h")
                            {
                                string[] target = command_words[3].Split(':');
                                if(target.Length == 2)
                                {
                                    if(isIpValid(target[0]))
                                    {
                                        if(Int32.TryParse(target[1], out int port) && port < 45505){
                                            if(Int32.TryParse(command_words[4], out int threads))
                                            {
                                                Task t = Task.Factory.StartNew(() => {
                                                    UDDoS.UdpDDoS.SendPackage(target[0], port, threads);
                                                });
                                                t.Wait();
                                            }
                                            else
                                                InvalidSyntaxErr(command);
                                        }
                                        else
                                            InvalidSyntaxErr(command);
                                    }
                                    else
                                        InvalidSyntaxErr(command);
                                } 
                                else
                                    InvalidSyntaxErr(command);
                            }
                            else
                                InvalidSyntaxErr(command);
                        }
                        else
                            InvalidSyntaxErr(command);
                    }
                    else
                        InvalidSyntaxErr(command);
                }
            }
            else
                InvalidSyntaxErr(command);
        }

        static bool isUrlValid(string url)
        {
            using(WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadString(url);
                    return true;
                }
                catch(WebException)
                {
                    return false;
                }
            }
        }

        static bool isIpValid(string ip)
        {
            return IPAddress.TryParse(ip, out IPAddress addr);
        }

        static void InvalidSyntaxErr(string cmd)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[!]-[websploit@uddos] ~$ Invalid syntax [{cmd}]! Type '-uddos -help'\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //envcl

        public static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SYNTAX] : -uddos <PROTOCOL> <PROTOCOL_TASK> <TARGET>(ex: [TCP/UDP 127.0.0.1:80] [http/s : http://url.xyz/]) <THREADS>");
            Console.WriteLine();
            Console.WriteLine("[CMD] For http protocols.");
            Console.WriteLine("'--http -d' DoS through multiple denied download req. [WEB]");
            Console.WriteLine("'--http -g' DoS through multiple denied GET req. [WEB]");
            Console.WriteLine("'--http -b' DoS through multiple denied download and GET req.[MOST POWERFUL / SLOWEST] [WEB]");
            Console.WriteLine("[CMD] For https protocols.");
            Console.WriteLine("'--https -d' DoS through multiple denied download req. [WEB]");
            Console.WriteLine("'--https -g' DoS through multiple denied GET req. [WEB]");
            Console.WriteLine("'--https -b' DoS through multiple denied download and GET req.[MOST POWERFUL / SLOWEST] [WEB]");
            Console.WriteLine("[CMD] For tcp protocols.");
            Console.WriteLine("'--tcp -h' DoS through flooding with large packets. [SERVERS] ");
            Console.WriteLine("'--tcp -e' DoS through flooding with denied connections. [LIGHTER] [SERVERS] ");
            Console.WriteLine("[CMD] For udp protocols.");
            Console.WriteLine("'--udp -p' DoS through flooding with big packets through UDP (rquires udp port). [SERVERS]\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
