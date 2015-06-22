using InvestorsAssist.Configuration;
using InvestorsAssist.Utility.Internet;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestorsAssist.Utility.Security;
using InvestorsAssist.Entities;
using HtmlAgilityPack;
using System.Threading;

namespace InvestorsAssist.Core.Ibd
{
    public class Reader
    {

        private readonly CookieAwareHttpClient _client;

        public Reader()
        {
            _client = new CookieAwareHttpClient();
        }
        public List<Stock> DownloadIbd50List()
        {
            List<Stock> value = new List<Stock>();
            for (int i = 0; i < SystemSettings.Instance.IbdSetting.MaxRetries; i++)
            {
                if (i!=0)
                {
                    //Sleep before retry.
                    Thread.Sleep(1000 * SystemSettings.Instance.IbdSetting.RetryInterval);
                }
                try
                {
                    var text = DownloadIbd50AsText();
                    if (string.IsNullOrEmpty(text))
                        continue;
                    return TextParser.Parse(text);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WarnFormat("Download Ibd 50 fail @{0}: {1}", i, ex.Message);
                    Logger.Instance.Warn(ex.StackTrace);
                    continue;
                }
            }
            return value;
        }
        private string DownloadIbd50AsText()
        {
            var loginUrl = @"https://www.investors.com/Services/SiteAjaxService.asmx/MemberSingIn";

            string loginData = Serializer.SerializeToJson<Object>(new
            {
                strEmail = SystemSettings.Instance.IbdSetting.Username,
                strPassword = SystemSettings.Instance.IbdSetting.SecurePassword.ToPlainString(),
                blnRemember = false
            });
            
            var response = _client.DownloadString(
                loginUrl, 
                new Dictionary<string, string>
                {
                    { "Content-Type", "application/json;charset=utf-8"},
                    { "If-Modified-Since", "1970-01-01"},
                    { "Cache-Control", "no-cache"}
                },
                loginData);

            //Check login response.
            dynamic loginReturn = Serializer.DeserializeFromJson<dynamic>(response);
            if (loginReturn.d.ToString() != "SOK")
            {
                Logger.Instance.ErrorFormat("Fail to login investors.com. Server says {0}", response);
                return string.Empty;
            }

            //skip any Ads.
            response = _client.DownloadString(@"http://www.investors.com/");

            response = _client.DownloadString(@"http://research.investors.com/screen-center/?start=ibd");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);
            string symbols = doc.GetElementbyId("screencenter-hidden-symbols").Attributes["value"].Value;
            string textUrl = string.Format(@"http://research.investors.com/screencenter/export.aspx?ScreenID=19&Type=1&exportType=text&Dir=Ascending&Exp=IBD 50 Rank&RandomSymbols={0}", symbols);

            return _client.DownloadString(textUrl);
        }

        
    }
}
