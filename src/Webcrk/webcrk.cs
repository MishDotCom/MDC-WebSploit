/*
 *  Website Login Cracking Tool
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
 using System.Net;
 using System.Collections.Specialized;
 using System.Linq;
 using System.Threading;
 using System.Text;
 using System.IO;

 namespace WebSploit.WebCrk
 {
    public class CookieAwareWebClient : WebClient
    {
        public bool Login(string loginPageAddress, NameValueCollection loginData, bool verbose, bool randomHeader)
        {
            try
            {
                Random seed = new Random();
                CookieContainer container;
                NameValueCollection _formData = new NameValueCollection();
                _formData = loginData;

                var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if(randomHeader)
                    request.Headers.Set(HttpRequestHeader.UserAgent, MainWebCracker.headers.userAgents[seed.Next(0, MainWebCracker.headers.userAgents.Count-1)]);
                
                //var query = String.Join("&", _formData.Cast<string>().Select(key => $"{key}={_formData[key]}").ToList());
                var query = AssembleQuery(loginData);

                var buffer = Encoding.ASCII.GetBytes(query);
                request.ContentLength = buffer.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();
                request.AllowAutoRedirect = true;

                container = request.CookieContainer = new CookieContainer();

                var response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();
                    if(verbose)
                        Console.WriteLine($"[ VERBOSE ] Response Code : {response.StatusCode}, Response Length : {html.Length}");
                    if(html.Length > Data.denied_html_lenght + 50)
                    {
                        response.Close();
                        CookieContainer = container;
                        return true;
                    }
                    else if(response.StatusCode.ToString() == "302")
                    {
                        response.Close();
                        CookieContainer = container;
                        return true;
                    }
                    else
                    {
                        response.Close();
                        CookieContainer = container;
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                MainWebCracker.LogError(ex.Message);
                return false;
            }
        }

        public void FirstRunLogin(string loginPageAddress, NameValueCollection loginData)
        {
            Thread th = new Thread(() => {
                if(!Data.firstRun)
                {
                    CookieContainer container;

                    var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Headers.Set(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/532.1 (KHTML, like Gecko) Chrome/4.0.219.6 Safari/532.1");

                    var query = string.Join("&", 
                        loginData.Cast<string>().Select(key => $"{key}={loginData[key]}"));

                    var buffer = Encoding.ASCII.GetBytes(query);
                    request.ContentLength = buffer.Length;
                    var requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    request.AllowAutoRedirect = true;

                    container = request.CookieContainer = new CookieContainer();

                    var response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string html = reader.ReadToEnd();
                        Data.firstRun = true;
                        Data.denied_html_lenght = html.Length;
                    }
                }
            });
            th.Start();
        }
        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieAwareWebClient() : this(new CookieContainer())
        { }

        public CookieContainer CookieContainer { get; private set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            return request;
        }

        public string AssembleQuery(NameValueCollection nvc)
        {
            string query = "";
            for(int i = 0; i < nvc.Count; i++)
            {   
                string key = nvc.GetKey(i);
                if(i != nvc.Count - 1)
                    query = query + $"{key}={nvc[key]}" + "&";
                else
                    query = query + $"{key}={nvc[key]}";
            }
            //Console.WriteLine(query);
            return query;
        }
    }

    class Data
    {
        public static bool firstRun = false;
        public static int denied_html_lenght = 0;
    }
 }