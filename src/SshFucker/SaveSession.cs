/*
 *  Ssh Fucker Session Saver
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