/*
 *  Email Cracker Shell
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
using System.IO;
using System.Net;
using System.Web;
using System.Net.Security;
using System.Net.Sockets;
using System.Collections.Generic;

namespace EmailCracker
{
    class Shell
    {
        public static void CommandInterpreter(string cmd)
        {
            bool ssl = false;
            string[] cmd_words = cmd.Split(' ');
            if(cmd_words.Length >= 2)
            {
                if(cmd_words[1].ToLower() == "-help")
                {
                    Help();
                }
                else
                {
                    string smtp = cmd_words[1].ToLower();
                    if(cmd_words.Length >= 3)
                    {
                        if(cmd_words[2].ToLower() == "-s")
                        {
                            ssl = true;
                            if(Int32.TryParse(cmd_words[3], out int port) && port == 465 || port == 587 || port == 25 || port == 2525)
                            {
                                if(cmd_words.Length >= 5)
                                {
                                    if(IsValidEmail(cmd_words[4]))
                                    {
                                        string target = cmd_words[4];
                                        if(cmd_words.Length >= 6)
                                        {
                                            string sub_task = cmd_words[5];
                                            string[] passwords = new string[0];
                                            if(sub_task.ToUpper() == "-P")
                                            {
                                                if(cmd_words.Length >= 7)
                                                {
                                                    if(File.Exists(cmd_words[6]))
                                                    {
                                                        passwords = File.ReadAllLines(cmd_words[6]);
                                                        bool verbose = false;
                                                        if(cmd_words.Length >= 8)
                                                        {
                                                            if(cmd_words[7].ToLower() == "-v")
                                                                verbose = true;
                                                        }
                                                        MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose, ssl);
                                                    }
                                                    else
                                                        ThrowErr($"Invlid filepath [{cmd}]");
                                                }
                                                else
                                                    ThrowErr($"Invlid syntax [{cmd}]");
                                            }
                                            else if(sub_task.ToLower() == "--p")
                                            {
                                                passwords = WebSploit.Properties.Resources.rockyou.Split('\n');
                                                bool verbose = false;
                                                if(cmd_words.Length >= 7)
                                                {
                                                    if(cmd_words[6].ToLower() == "-v")
                                                        verbose = true;
                                                }
                                                MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose, ssl);
                                            }
                                            else
                                                ThrowErr($"Invlid syntax [{cmd}]");
                                        }
                                        else
                                            ThrowErr($"Invlid syntax [{cmd}]");
                                    }
                                    else
                                        ThrowErr($"Invlid email address [{cmd}]");
                                }
                                else
                                    ThrowErr($"Invlid syntax [{cmd}]");
                            }
                            else
                                ThrowErr($"Invlid SMTP port [{cmd}]");
                        }
                        else if(Int32.TryParse(cmd_words[2], out int port) && port == 465 || port == 587 || port == 25 || port == 2525)
                        {
                            if(cmd_words.Length >= 4)
                            {
                                if(IsValidEmail(cmd_words[3]))
                                {
                                    string target = cmd_words[3];
                                    if(cmd_words.Length >= 5)
                                    {
                                        string sub_task = cmd_words[4];
                                        string[] passwords = new string[0];
                                        if(sub_task.ToUpper() == "-P")
                                        {
                                            if(cmd_words.Length >= 6)
                                            {
                                                if(File.Exists(cmd_words[5]))
                                                {
                                                    passwords = File.ReadAllLines(cmd_words[5]);
                                                    bool verbose = false;
                                                    if(cmd_words.Length >= 7)
                                                    {
                                                        if(cmd_words[6].ToLower() == "-v")
                                                            verbose = true;
                                                    }
                                                    MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose, ssl);
                                                }
                                                else
                                                    ThrowErr($"Invlid filepath [{cmd}]");
                                            }
                                            else
                                                ThrowErr($"Invlid syntax [{cmd}]");
                                        }
                                        else if(sub_task.ToLower() == "--p")
                                        {
                                            passwords = WebSploit.Properties.Resources.rockyou.Split('\n');
                                            bool verbose = false;
                                            if(cmd_words.Length >= 6)
                                            {
                                                if(cmd_words[5].ToLower() == "-v")
                                                    verbose = true;
                                            }
                                            MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose, ssl);
                                        }
                                        else
                                            ThrowErr($"Invlid syntax [{cmd}]");
                                    }
                                    else
                                        ThrowErr($"Invlid syntax [{cmd}]");
                                }
                                else
                                    ThrowErr($"Invlid email address [{cmd}]");
                            }
                            else
                                ThrowErr($"Invlid syntax [{cmd}]");
                        }
                        else
                            ThrowErr($"Invlid SMTP port [{cmd}]");
                    }
                    else
                        ThrowErr($"Invlid syntax [{cmd}]");
                }
            }
            else
                ThrowErr($"Invlid syntax [{cmd}]");
        }

        static void ThrowErr(string err)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[!]-[websploit@email-cracker] : {err}. Type '-ecrk -help'.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[SYNTAX] > -ecrk <SMTP_SERVER> <SMTP_PORT> <TARGET> <-P <PATH TO WORDLIST>/--p for built-in (rockyou.txt)> <OPT: -v for verbose>\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}