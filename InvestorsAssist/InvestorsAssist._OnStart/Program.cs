using InvestorsAssist.Configuration;
using InvestorsAssist.Utility.Internet;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace InvestorsAssist._OnStart
{
    class Program
    {
        //executed while computer starts up
        static void Main(string[] args)
        {
            Logger.Instance.Info("InvestorsAssist._OnStart started");
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (var email = new EmailClient(
                    SystemSettings.Instance.EmailSetting.Server,
                    SystemSettings.Instance.EmailSetting.Port,
                    SystemSettings.Instance.EmailSetting.Username,
                    SystemSettings.Instance.EmailSetting.SecurePassword.ToPlainString()))
                    {
                        email.SendHtmlEmail("lihe.chen@gmail.com",
                            new List<string>(),
                            string.Format("IA: Computer {0} started @ {1}",
                                Environment.MachineName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            string.Format("IP: <b>{0}</b>", GetInternetIP()));
                    }
                }
                catch (Exception e)
                {
                    Logger.Instance.ErrorFormat("Send Email Failed: {0}", e.Message);
                    Logger.Instance.Error(e.StackTrace);
                    Thread.Sleep(30 * 1000);
                    continue;

                }
                break;
            }

            Logger.Instance.Info("InvestorsAssist._OnStart done.");
        }

        static string GetInternetIP()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var externalIP = client.DownloadString("http://checkip.dyndns.org/");
                    var reg = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    externalIP = reg.Matches(externalIP)[0].ToString();
                    return externalIP;
                }
            }
            catch { return string.Empty; }
        }
    }
}
