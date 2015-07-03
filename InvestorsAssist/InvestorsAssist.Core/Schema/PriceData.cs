using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestorsAssist.Utility.Text;

namespace InvestorsAssist.Core.Schema
{
    //Todo: refactor it with Eod
    public class PriceData
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public decimal Volume { get; set; }
    }

    public static class PriceDataParser
    {
        public static PriceData Parse(string line)
        {
            if (line.StartsWith("Date", StringComparison.InvariantCultureIgnoreCase))
                return null;
            string[] sa = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (sa.Count() == 6)
            {
                return new PriceData
                {
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
