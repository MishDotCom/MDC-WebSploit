using System;

namespace IpGrabber
{
    class Shell
    {
        public static void CommandInterpreter(string command)
        {
            Main.GetWebsiteIp(command.Split(" ")[1]);
        }
    }
}