using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.Internet
{
    public class CookieAwareHttpClient
    {
        private readonly CookieContainer _cookieContainer;

        public CookieAwareHttpClient()
        {
            _cookieContainer = new CookieContainer();
        }

        public string DownloadString(string url, Dictionary<string, string> headers = null, string data = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            if (headers != null && headers.Count > 0)
            {
                foreach (string key in headers.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "content-type":
                            httpWebRequest.ContentType = headers[key];
                            break;
                        case "if-modified-since":
                            httpWebRequest.IfModifiedSince = new DateTime(1970, 1, 1);
                            DateTime value;
                            if (DateTime.TryParse(headers[key], out value))
                            {
                                httpWebRequest.IfModifiedSince = value;
                            }
                            break;
                        case "accept":
                            httpWebRequest.Accept = headers[key];
                            break;
                        case "user-agent":
                            httpWebRequest.UserAgent = headers[key];
                            break;
                        default:
                            httpWebRequest.Headers.Add(key, headers[key]);
                            break;
                    }

                }
            }
            if (!string.IsNullOrEmpty(data))
            {
                httpWebRequest.Method = "POST";
                var buffer = Encoding.ASCII.GetBytes(data);
                httpWebRequest.ContentLength = buffer.Length;
                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                httpWebRequest.Method = "GET";
            }
            httpWebRequest.CookieContainer = _cookieContainer;
            using (var response = httpWebRequest.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
