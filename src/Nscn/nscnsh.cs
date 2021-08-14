using System;

namespace IpGrabber
{
    class Shell
    {
        public static void CommandInterpreter(string command)
        {
            string[] words = command.Split(" ");
            if(words.Length >= 2)
            {
                if(words[1] == "-ph")
                {
                    int bytes = 32;
                    if(words.Length == 5)
                    {
                        if(words[3].ToLower() == "-b")
                        {
                            if(Int32.TryParse(words[4], out int bytesRaw))
                                bytes = bytesRaw;
                        }
                    }
                    Main.pingHost(words[2], bytes);
                }
                else if(words[1] == "-gh")
                {
                    Main.GetWebsiteIp(words[2]);
                }
                else if(words[1] == "-pLan")
                {
                    if(words.Length > 2)
                    {
                        bool verbose = false;
                        if(words.Length > 3)
                        {
                            if(words[3] == "--V")
                                verbose = true;
                        }
                        Main.pingLan(words[2], verbose);
                    }
                    else
                        throwErr($"Invalid syntax {command}.");
                }
                else if(words[1] == "-help")
                {
                    Help();
                }
                else
                    throwErr($"Invalid syntax {command}.");
            }
            else
                throwErr($"Invalid syntax {command}.");
        }

        static void throwErr(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"nscn/errLogger> {err}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SYNTAX] > -nscn <TASK>\n");
            Console.WriteLine("'-ph' Pings a single host 4 times.");
            Console.WriteLine("'-pH' Pings a list of hosts 4 times each.");
            Console.WriteLine("'-pLan' Finds all avalilable hosts on a LAN. Options:");
            Console.WriteLine("'--V' Verbose displays more information about the host dicovered.");
            Console.WriteLine("'-gh' Returns the ip of a known website.\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}