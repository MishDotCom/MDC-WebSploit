using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Open_Port_Scanner
{
    class TcpPortScanner
    {
        public static int[] ScanPorts(string ip, int[] ports)
        {
            List<int> openPorts = new List<int>();
            ParallelOptions po = new ParallelOptions();
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            po.MaxDegreeOfParallelism = Environment.ProcessorCount * 100000;
            Parallel.ForEach(ports, port =>  
            {
                using(TcpClient client = new TcpClient())
                {
                    try
                    {
                        var res = client.BeginConnect(ip, port, null, null);
                        bool connected = res.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
                        if(connected)
                        {
                            openPorts.Add(port);
                            client.EndConnect(res);
                        }
                    }
                    catch(Exception ex)
                    {
                        Controller.ShowError(ex.Message);
                    }
                }
            });
            return openPorts.ToArray();
        }

        public static void ScanSinglePort(string ip, int port)
        {
            using(TcpClient client = new TcpClient())
            {
                try
                    {
                    var res = client.BeginConnect(ip, port, null, null);
                    bool connected = res.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
                    if(connected)
                    {
                        Console.WriteLine($"prtsc> Port {port} is open on {ip}.\n");
                        client.EndConnect(res);
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"prtsc> Port {port} is closed on {ip}.\n");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    Controller.ShowError(ex.Message);
                }
            }
        }

        public static int[] _20_ports = {21,22,23,80,111,135,139,443,445,1723,3306,3389,5900,8080};
        public static int[] _200_ports_tcp = {200,1,3,7,9,13,17,19,21,23,26,37,53,79,82,88,100,106,111,113,119,135,139,144,179,199,254,255,280,311,389,427,443,445,464,497,513,515,543,544,548,554,593,625,631,636,646,787,808,873,902,990,1000,1022,1024,1033,1035,1041,1044,1048,1050,1053,1054,1056,1058,1059,1064,1066,1069,1071,1074,1080,1110,1234,1433,1494,1521,1720,1723,1755,1761,1801,1900,1935,1998,2000,2003,2005,2049,2103,2105,2107,2121,2161,2301,2383,2401,2601,2717,2869,2967,3000,3001,3128,3268,3306,3389,3689,3690,3703,3986,4000,4001,4045,4899,5000,5001,5003,5009,5050,5051,5060,5101,5120,5190,5357,5432,5555,5631,5666,5800,5900,5901,6000,6002,6004,6112,6646,6666,7000,7070,7937,7938,8000,8002,8008,8010,8031,8080,8081,8443,8888,9000,9001,9090,9100,9102,9999,10001,10010,32768,32771,49152,49157,50000};
    }

    class Controller
    {
        public static void MainTCPPortScan(string ip, int[] ports)
        {
            int[] open_ports = TcpPortScanner.ScanPorts(ip, ports);
            ReturnResult(open_ports, ip, "tcp");
        }
        private static void ReturnResult(int[] ports, string ip, string protocol)
        {
            bool displayedRes = false;
            Ports ports_ = new Ports();
            if(ports.Length != 0)
            {
                Console.WriteLine($"prtsc> Open ports on {ip} >");
                Console.WriteLine(" _________________________");
                Console.WriteLine("|  Ports  |  Description  |");
                Console.WriteLine("--------------------------");
                foreach(int port in ports)
                {
                    displayedRes = false;
                    foreach(Port prt in ports_.common_20)
                    {
                        if(prt.port == port){
                            Console.WriteLine($"| {port} >> {prt.desc} >> {protocol.ToUpper()}");
                            displayedRes = true;
                        }
                    }
                    if(!displayedRes)
                        Console.WriteLine($"| {port} >> N/A >> N/A");
                }
                Console.WriteLine("--------------------------\n");
            }
            else
            {
                Console.WriteLine($"prts> No open ports found on {ip}.\nTry '--p -200' for more ports!\n");
            }
        }

        public static void ShowError(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[!]-[websploit/prtsc/err_logger] > An error occurred : {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    class Ports
    {
        public List<Port> common_20 = new List<Port>();
        Port _21 = new Port(21, "ftp");
        Port _22 = new Port(22, "ssh");
        Port _23 = new Port(23, "telnet");
        Port _25 = new Port(25, "smtp");
        Port _53 = new Port(53, "domain name system");
        Port _80 = new Port(80, "http");
        Port _110 = new Port(110, "pop3");
        Port _111 = new Port(111, "rpcbind");
        Port _135 = new Port(135, "msrpc");
        Port _139 = new Port(139, "netbios-ssn");
        Port _143 = new Port(143, "imap");
        Port _443 = new Port(443, "https");
        Port _445 = new Port(445, "microsoft-ds");
        Port _993 = new Port(993, "imaps");
        Port _995 = new Port(995, "pop3s");
        Port _1723 = new Port(1723, "pptp");
        Port _3306 = new Port(3306, "mysql");
        Port _3389 = new Port(3389, "ms-wbt-server");
        Port _5900 = new Port(5900, "vnc");
        Port _8080 = new Port(8080, "http-proxy");

        public Ports()
        {
            common_20.Add(_21);
            common_20.Add(_22);
            common_20.Add(_23);
            common_20.Add(_25);
            common_20.Add(_53);
            common_20.Add(_80);
            common_20.Add(_110);
            common_20.Add(_111);
            common_20.Add(_135);
            common_20.Add(_139);
            common_20.Add(_143);
            common_20.Add(_443);
            common_20.Add(_445);
            common_20.Add(_993);
            common_20.Add(_995);
            common_20.Add(_1723);
            common_20.Add(_3306);
            common_20.Add(_3389);
            common_20.Add(_5900);
            common_20.Add(_8080);
        }
    }

    class Port
    {
        public int port;
        public string desc;
        public Port(int port, string desc)
        {
            this.port = port;
            this.desc = desc;
        }
    }
}