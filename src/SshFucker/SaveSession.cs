using System;
using System.IO;

namespace SshAttacker
{
    class SaveSession
    {
        public static void SaveWrittenSession(bool result, string target, string user, string pass)
        {
            string data = "";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var seed = new Random();
            string file_name = "SshFuckerSession" + seed.Next(100000,999999) + ".txt";
            path = path + "/" + file_name;
            if(result)
            {
                string title = "SshFucker Session";
                string sub_title = "Result -> Credentials Found Successfully.";
                string host = "Target : " + target;
                string userD = "Username : " + user;
                string passD = "Password : " + pass;
                data = title +"\n"+ sub_title +"\n"+ host +"\n"+ userD +"\n"+ passD;
            }
            else
            {
                string title = "SshFucker Session";
                string sub_title = "Result -> Credentials Not Found";
                string host = "Target : " + target;
                string userD = "Username : NOT FOUND";
                string passD = "Password : NOT FOUND";
                data = title +"\n"+ sub_title +"\n"+ host +"\n"+ userD +"\n"+ passD;
            }
            if(!File.Exists(path))
            {
                File.Create(path).Close();
                using(StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(data);
                }
                Console.WriteLine($"[INFO] Session saved to path {path}.\n\n");
                Environment.Exit(0);
            }
            else
            {
                SaveWrittenSession(result, target, user, pass);
            }
        }
    }
}