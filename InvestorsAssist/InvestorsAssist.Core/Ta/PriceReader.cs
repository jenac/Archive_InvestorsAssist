using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InvestorsAssist.Core.Ta
{
    static class PriceReader
    {
        //Read Eod from Google, it handles the merge/split
        private static readonly string _EOD_URL_FMT =
            @"http://www.google.com/finance/historical?q={0}&histperiod=daily&startdate={1}&enddate={2}&output=csv";

        //SPY is used to figure out last trade date
        private static readonly string _INDEX_SYMBOL_FOR_LAST_TRADE = @"SPY";

        private static readonly string _CURPRICE_URL_FMT =
            @"http://download.finance.yahoo.com/d/quotes.csv?s={0}&f=l1&e=.csv";

        static PriceReader()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
        }
        private static string WebGet(string url)
        {
            try
            {
                /*
				using (var client = new WebClient())
				{
					client.Proxy = null;
					return client.DownloadString(url);
				}*/
                HttpWebRequest req =
                    (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                req.KeepAlive = true;
                req.UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; WIndows NT 9.0; en-US))";
                req.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                req.Proxy = null;

                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Logger.Instance.WarnFormat("{0} : {1}", url, e.Message);
                return string.Empty;
            }
        }

        public static DateTime? GetLastTradeDate()
        {
            DateTime start = DateTime.Today.AddDays(-30);
            DateTime end = DateTime.Today;
            string csv = WebGet(
                string.Format(_EOD_URL_FMT,
                    HttpUtility.UrlEncode(_INDEX_SYMBOL_FOR_LAST_TRADE),
                    start.ToString("yyyy-MM-dd"),
                    end.ToString("yyyy-MM-dd")));
            if (string.IsNullOrEmpty(csv))
                return null;
            string[] lines =
                csv.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<PriceData> indexPriceDataLast30Days =
                lines.Select(l => PriceDataParser.Parse(l)).Where(e => e != null).ToList();
            return indexPriceDataLast30Days.Max(e => e.Date);
        }


        public static List<PriceData> ReadPriceDataBySymbol(string symbol)
        {
            string csv = WebGet(
                string.Format(_EOD_URL_FMT,
                    HttpUtility.UrlEncode(symbol),
                    DateTime.Today.AddYears(-10).ToString("yyyy-MM-dd"),
                    DateTime.Today.ToString("yyyy-MM-dd")));
            string[] lines =
                csv.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines
                .Select(l => PriceDataParser.Parse(l)).ToList();
        }

        public static double ReadCurrentPrice(string symbol)
        {
            string csv = WebGet(
                string.Format(_CURPRICE_URL_FMT, symbol));
            double value;
            if (double.TryParse(csv.Trim(), out value))
                return value;
            return -1;
        }
    }
}
