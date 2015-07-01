using InvestorsAssist.Configuration;
using InvestorsAssist.Utility.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestorsAssist.Utility.Security;

namespace InvestorsAssist.Core
{
    public static class ExceptionHandler
    {
        public static void AlertViaEmail(string program, Exception e)
        {
            using (var email = new EmailClient(
                    SystemSettings.Instance.EmailSetting.Server,
                    SystemSettings.Instance.EmailSetting.Port,
                    SystemSettings.Instance.EmailSetting.Username,
                    SystemSettings.Instance.EmailSetting.SecurePassword.ToPlainString()))
            {
                email.SendHtmlEmail(SystemSettings.Instance.EmailSetting.To,
                    new List<string>(),
                    string.Format("IA: {0} has an Exception!", program),
                    string.Format("Details: <b>{0}</b><br/>{1}", e.Message, e.StackTrace));
            }
        }
    }
}
