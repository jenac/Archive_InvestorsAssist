using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InvestorsAssist.Core
{
    public class EodParam
    {
        public string Symbol { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    class EodReader
    {
        private readonly CookieAwareHttpClient _client;

        private readonly string _EOD_URL_FMT =
            @"http://www.google.com/finance/historical?q={0}&histperiod=daily&startdate={1}&enddate={2}&output=csv";

        //SPY is used to figure out last trade date
        private readonly string _INDEX_SYMBOL_FOR_LAST_TRADE = @"SPY";

        public EodReader()
        {
            _client = new CookieAwareHttpClient();
        }
        
        public DateTime? GetLastTradeDate()
        {
            DateTime start = DateTime.Today.AddDays(-30);
            DateTime end = DateTime.Today;
            string csv = _client.WebGet(
                string.Format(_EOD_URL_FMT,
                    HttpUtility.UrlEncode(_INDEX_SYMBOL_FOR_LAST_TRADE),
                    start.ToString("yyyy-MM-dd"),
                    end.ToString("yyyy-MM-dd")));
            if (string.IsNullOrEmpty(csv))
                return null;
            string[] lines =
                csv.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<Eod> indexEodLast30Days =
                lines.Select(l => EodParser.ParseEod(l, _INDEX_SYMBOL_FOR_LAST_TRADE)).Where(e => e != null).ToList();
            return indexEodLast30Days.Max(e => e.Date);
        }


        public List<Eod> ReadEodBySymbol(EodParam param)
        {
            if (param.Start >= param.End) return new List<Eod>();
            string csv = _client.WebGet(
                string.Format(_EOD_URL_FMT,
                    HttpUtility.UrlEncode(param.Symbol),
                    param.Start.ToString("yyyy-MM-dd"),
                    param.End.ToString("yyyy-MM-dd")));
            string[] lines =
                csv.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines
                .Select(l => EodParser.ParseEod(l, param.Symbol))
                .Where(e => (e != null && e.Date != param.Start)).ToList();
        }

        
    }
}
