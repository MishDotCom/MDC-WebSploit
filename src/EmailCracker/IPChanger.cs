/*
 *  Static Internet Protocol Address Changer
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