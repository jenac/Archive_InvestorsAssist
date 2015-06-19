using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core
{
    //public class IbdReader
    //{
        
    //    private readonly Cookie
    //    public static string DownloadIbd50AsText()
    //    {
    //        var portalUrl = @"https://www.investors.com/secure/login.aspx?ns=personal";
    //        var loginUrl = @"https://www.investors.com/Services/SiteAjaxService.asmx/MemberSingIn";
    //        string loginData = @"{""strEmail"":""htxx2009@gmail.com"",""strPassword"":""Mn1234&*()"",""blnRemember"":false}";

    //        var client = new CookieAwareHttpClient();
    //        //var response = client.Request(
    //        //    new Request {
    //        //        Url= portalUrl,
    //        //        Headers = new Dictionary<string, string>
    //        //        {
    //        //            { "Accept",  "text/html, application/xhtml+xml, */*" },
    //        //            { "Accept-Language", "en-US,en;q=0.8,zh-Hans-CN;q=0.5,zh-Hans;q=0.3" },
    //        //            { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko" },
    //        //            { "Accept-Encoding", "gzip, deflate" },

    //        //        },

    //        //    });

    //        var response = client.Request(
    //            new Request
    //            {
    //                Url = loginUrl,
    //                Headers = new Dictionary<string, string>
    //                {
    //                    { "ContentType", "application/json;charset=utf-8"},
    //                    { "IfModifiedSince", "1970-01-01"},
    //                    { "Cache-Control", "no-cache"}
    //                },
    //                Data = loginData,

    //            });
    //        response = client.Request(
    //            new Request
    //            {
    //                Url = @"http://www.investors.com/",

    //            });



    //        response = client.Request(
    //            new Request
    //            {
    //                Url = @"http://research.investors.com/screen-center/?start=ibd",
                    

    //            });

    //        File.WriteAllText(@"Z:\11.html", response.Content);
    //    }
    //}
}
