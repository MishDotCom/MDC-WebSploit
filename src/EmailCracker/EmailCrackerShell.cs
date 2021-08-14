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
                        if(Int32.TryParse(cmd_words[2], out int port) && port == 465 || port == 587 || port == 25 || port == 2525)
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
                                                    MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose);
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
                                            MainAttacker.AttemptLogin(smtp, port, target, passwords, verbose);
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