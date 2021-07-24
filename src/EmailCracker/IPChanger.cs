using System;
using System.Management;
using System.Threading;

namespace EmailCracker
{
    class IPChanger
        {
            public static void ChangeIP(bool vrebose)
            {
                Random seed = new Random();
                int num1 = seed.Next(100, 250);
                int num2 = seed.Next(100, 255);
                int num3 = seed.Next(1, 9);
                int num4 = seed.Next(150, 250);
                string newIp = num1.ToString() + "." + num2.ToString() + ".1." + num3.ToString();
                string subnetMask = "255.255.255.0";
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
        }
}