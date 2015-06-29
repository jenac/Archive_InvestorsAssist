using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Text;

namespace InvestorsAssist.Core
{
    static class CompanyParser
    {
        public static Company ParseCompany(string line, string exchange)
        {
            char[] trimCompany = new char[] { '"' };
            if (line.StartsWith("\"Symbol\"", StringComparison.InvariantCultureIgnoreCase))
                return null;
            string[] sa = line.Split(new string[] { "\"," }, StringSplitOptions.RemoveEmptyEntries);
            if (sa.Count() >= 8)
            {
                return new Company
                {
                    Symbol = sa[0].Trim(trimCompany),
                    Name = sa[1].Trim(trimCompany).Replace(',', ' '),
                    LastSale = TextParser.ParseFloat(sa[2].Trim(trimCompany)),
                    MarketCap = TextParser.ParseDecimal(sa[3].Trim(trimCompany)),
                    Sector = sa[6].Trim(trimCompany).Replace(',', ' '),
                    Industry = sa[7].Trim(trimCompany).Replace(',', ' '),
                    Exchange = exchange
                };
            }
            return null;
        }
    }
}
