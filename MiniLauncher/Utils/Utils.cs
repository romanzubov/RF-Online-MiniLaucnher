using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MiniLauncher.Utils
{
    public class Utils
    {
        public static string DownloadDataFromFile(string uri)
        {
            var result = string.Empty;
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 5.1;) Firefox/2.0.0.7");
                    webClient.Proxy = null;
                    result = webClient.DownloadString(new Uri(uri));
                }
                catch (Exception exception)
                {
                    SimpleLogger.GetInstance.Error(exception.ToString());
                }
            }
            return result;
        }
    }
}
