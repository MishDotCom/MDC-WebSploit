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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSploit.Open_Port_Scanner
{
    class Program
    {
        public static void CommandInterpreter(string command)
        {
            string target = "";
            string[] command_words = command.Split(" "); 
            if(command_words.Length >= 2)
            {
                if(IPAddress.TryParse(command_words[1], out IPAddress addr))
                {
                    target = command_words[1];
                    if(command_words.Length >= 3)
                    {
                        if(command_words[2] == "-p")
                        {
                            if(command_words.Length >= 4)
                            {
                                if(Int32.TryParse(command_words[3], out int port) && port > 0)
                                {
                                    Console.WriteLine($"prtsc> Initiating port-scan on {target}...");
                                    Task task = Task.Factory.StartNew(() => {
                                        TcpPortScanner.ScanSinglePort(target, port);
                                    });
                                    task.Wait();
                                }
                                else
                                    InvalidSyntax(command);
                            }
                            else
                                InvalidSyntax(command);
                        }
                        else if(command_words[2] == "-P")
                        {
                            if(command_words.Length >= 4)
                            {
                                string[] str_ports = command_words[3].Split(',');
                                List<int> ports_lst = new List<int>();
                                if(str_ports.Length > 1)
                                {
                                    foreach(string str_prt in str_ports)
                                    {
                                        if(Int32.TryParse(str_prt, out int prt) && prt < 65335)
                                        {
                                            ports_lst.Add(prt);
                                        }
                                    }
                                    if(ports_lst.Count >= 1)
                                    {
                                        Console.WriteLine($"prtsc> Initiating port-scan on {target}...");
                                        Task task = Task.Factory.StartNew(() => {
                                            Controller.MainTCPPortScan(target, ports_lst.ToArray());
                                        });
                                        task.Wait();
                                    }
                                    else
                                        InvalidSyntax(command);
                                }
                                else
                                    InvalidSyntax(command);
                            }
                            else
                                InvalidSyntax(command);
                        }
                        else if(command_words[2] == "--p")
                        {
                            if(command_words.Length >= 4)
                            {
                                if(command_words[3] == "-20")
                                {
                                    Console.WriteLine($"prtsc> Initiating port-scan on {target}...");
                                    Task task = Task.Factory.StartNew(() => {
                                        Controller.MainTCPPortScan(target, TcpPortScanner._20_ports);
                                    });
                                    task.Wait();
                                }
                                else if(command_words[3] == "-200")
                                {
                                    Console.WriteLine($"prtsc> Initiating port-scan on {target}...");
                                    Task task = Task.Factory.StartNew(() => {
                                        Controller.MainTCPPortScan(target, TcpPortScanner._200_ports_tcp);
                                    });
                                    task.Wait();
                                }
                                else
                                    InvalidSyntax(command);
                            }
                        }
                        else
                            InvalidSyntax(command);
                    }
                    else
                        InvalidSyntax(command);
                }
                else if(command_words[1].ToLower() == "-help")
                {
                    Help();
                }
                else
                    InvalidSyntax(command);
            }
            else
                InvalidSyntax(command);
        }

        static void InvalidSyntax(string cmd)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[!]-[websploit/prtsc/err_logger] > Invalid command ['{cmd}']. Type '-prtsc -help'\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SYNTAX] > -prtsc <TARGET>(IP address) <SUBTASK>\n");
            Console.WriteLine("> Subtask commands:");
            Console.WriteLine("[CMD] : '-p' Scans an individual port (SYNTAX : -p <PORT> ex: -p 21)");
            Console.WriteLine("[CMD] : '-P' Scans a given list of ports (SYNTAX : -P <PORT,PORT,PORT> ex: -P 21,22,25)");
            Console.WriteLine("[CMD] : '--p' Scans a list of common ports (SYNTAX : --p <LIST (Already built-in): -20 (20 common ports), -200 (200 common ports)>\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
