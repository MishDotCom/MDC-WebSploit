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

namespace IpGrabber
{
    class Shell
    {
        public static void CommandInterpreter(string command)
        {
            string[] words = command.Split(" ");
            if(words.Length >= 2)
            {
                if(words[1] == "-ph")
                {
                    int bytes = 32;
                    if(words.Length == 5)
                    {
                        if(words[3].ToLower() == "-b")
                        {
                            if(Int32.TryParse(words[4], out int bytesRaw))
                                bytes = bytesRaw;
                        }
                    }
                    Main.pingHost(words[2], bytes);
                }
                else if(words[1] == "-gh")
                {
                    Main.GetWebsiteIp(words[2]);
                }
                else if(words[1] == "-pLan")
                {
                    if(words.Length > 2)
                    {
                        bool verbose = false;
                        if(words.Length > 3)
                        {
                            if(words[3] == "--V")
                                verbose = true;
                        }
                        Main.pingLan(words[2], verbose);
                    }
                    else
                        throwErr($"Invalid syntax {command}.");
                }
                else if(words[1] == "-help")
                {
                    Help();
                }
                else
                    throwErr($"Invalid syntax {command}.");
            }
            else
                throwErr($"Invalid syntax {command}.");
        }

        static void throwErr(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"nscn/errLogger> {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SYNTAX] > -nscn <TASK>\n");
            Console.WriteLine("'-ph' Pings a single host 4 times.");
            Console.WriteLine("'-pH' Pings a list of hosts 4 times each.");
            Console.WriteLine("'-pLan' Finds all avalilable hosts on a LAN. Options:");
            Console.WriteLine("'--V' Verbose displays more information about the host dicovered.");
            Console.WriteLine("'-gh' Returns the ip of a known website.\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}