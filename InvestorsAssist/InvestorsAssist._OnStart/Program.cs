using InvestorsAssist.Configuration;
using InvestorsAssist.Utility.Internet;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                            string.Empty);
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
    }
}
