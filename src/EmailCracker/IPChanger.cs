using System;
using System.Management;
using System.Threading;
using System.Net;

namespace EmailCracker
{
    class IPChanger
    {
        public static void ChangeIP(bool vrebose)
        {
            Random seed = new Random();
            string[] ipItems = GetCurrentIP().Split('.');
            string ipSubstring = "";
            for(int i = 0; i < ipItems.Length - 1; i++)
                ipSubstring = ipSubstring + ipItems[i] + ".";
            string subnetMask = "255.255.255.0";
            string newIp = ipSubstring + seed.Next(1,255).ToString();
            setIP(newIp, subnetMask, vrebose);
        }

        static void setIP(string ip_address, string subnet_mask, bool verbose)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        ManagementBaseObject setIP;
                        ManagementBaseObject newIP =
                            objMO.GetMethodParameters("EnableStatic");

                        newIP["IPAddress"] = new string[] { ip_address };
                        newIP["SubnetMask"] = new string[] { subnet_mask };

                        setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
                        if(verbose)
                            Console.WriteLine($"[VERBOSE] : Static Ip was changed to {ip_address}.");
                    }
                    catch 
                    {
                    }
                }
            }
        }

        static string GetCurrentIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress addr in  host.AddressList)
            {
                if(addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return addr.ToString();
            }
            return "127.0.0.1";
        }
    }
}