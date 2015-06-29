using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Internet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core
{
    public class Etf
    {
        public string home_page { get; set; }
        public string symbol { get; set; }
    }

    class CompanyReader
    {
        private readonly CookieAwareHttpClient _client;

        private readonly List<string> _EXCHANGES = new List<string>
		{
			"NYSE",
			"NASDAQ",
			"AMEX"
		};
        private readonly string _COMPANY_URL_FMT =
            @"http://www.nasdaq.com/screening/companies-by-name.aspx?letter=&exchange={0}&render=download";

        private readonly string _ETF_URL =
            @"http://etfdb.com/screener-data/analysis.json/";

        public CompanyReader()
        {
            _client = new CookieAwareHttpClient();
        }

        public List<Company> ReadCompaniesFromInternet()
        {
            List<Company> seed = new List<Company>();
            var companies =
                _EXCHANGES.AsParallel()
                    .Select(c => ReadCompaniesByExchange(c))
                    .Aggregate(seed, (i, j) => i.Union(j).ToList());
            var etfs = ReadETFAsCompanies();
            companies.AddRange(etfs);
            return companies;
        }

        private List<Company> ReadCompaniesByExchange(string exchange)
        {
            string csv = _client.DownloadString(
                string.Format(_COMPANY_URL_FMT, exchange));
            string[] lines =
                csv.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines.Select(l => CompanyParser.ParseCompany(l, exchange)).Where(c => c != null).ToList();
        }

        private List<Company> ReadETFAsCompanies()
        {
            string json = _client.DownloadString(_ETF_URL);
            var etfs = JsonConvert.DeserializeObject<List<Etf>>(json);
            return etfs.Select(e => new Company
            {
                Symbol = e.symbol,
                Exchange = "ETF",
                Name = e.home_page
            }).ToList();
        }
    }
}
