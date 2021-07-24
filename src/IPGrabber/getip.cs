using System;
using System.Collections.Generic;
using System.Net;

namespace IpGrabber
{
    class Main
    {
        public static void GetWebsiteIp(string web)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(web);
                foreach(IPAddress addr in hostEntry.AddressList)
                    Console.WriteLine($"ipgr> Found IP address {addr.ToString()} on {web}.");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[!]-[websploit@ipgr] > {ex.Message}\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}