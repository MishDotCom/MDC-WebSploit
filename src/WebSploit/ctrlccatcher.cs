using System;
using System.Threading;

namespace WebSploit
{
    class ExitBraker
    {
        public static void CatchExit()
        {
            Thread catcherTh = new Thread(() => {
                Console.CancelKeyPress += delegate {
                    Console.WriteLine("att");
                };
            });
            catcherTh.Start();
        }
    }
}