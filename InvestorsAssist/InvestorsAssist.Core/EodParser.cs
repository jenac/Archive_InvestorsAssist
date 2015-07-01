using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core
{
    static class EodParser
    {
        public static Eod ParseEod(string line, string symbol)
        {
            if (line.StartsWith("Date", StringComparison.InvariantCultureIgnoreCase))
                return null;
            string[] sa = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (sa.Count() == 6)
            {
                return new Eod
                {
                    Symbol = symbol,
                    Date = DateTime.Parse(sa[0]),
                    Open = TextParser.ParseDouble(sa[1]),
                    High = TextParser.ParseDouble(sa[2]),
                    Low = TextParser.ParseDouble(sa[3]),
                    Close = TextParser.ParseDouble(sa[4]),
                    Volume = TextParser.ParseDecimal(sa[5].Replace(",", string.Empty))
                };
            }
            return null;
        }
    }
}
