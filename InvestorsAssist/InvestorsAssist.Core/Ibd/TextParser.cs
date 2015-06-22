using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ibd
{
    public static class TextParser
    {
        public static List<Stock> Parse(string text)
        {
            string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int index = Array.FindIndex(lines, l => l.StartsWith("------"));
            string dateLine = lines.Skip(index + 51).Take(1).First();
            DateTime? date = ParseDateLine(dateLine);
            if (date == null)
            {
                //log
                return null;
            }
            string guideLine = lines.Skip(index).Take(1).First();
            List<string> stockLines = lines.Skip(index+1).Take(50).ToList();
            var stocks = stockLines.Select(s => ParseStockLine(guideLine, s)).ToList();
            stocks.ForEach(s => s.Date = date.Value);
            return stocks;

        }


        internal static DateTime? ParseDateLine(string dateLine)
        {
            string date = dateLine.Trim().Substring(dateLine.IndexOf(',')+1).Trim();
            DateTime value;
            if (DateTime.TryParse(date, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        internal static Stock ParseStockLine(string guidLine, string stockLine)
        {
            List<Tuple<int, int>> marks = new List<Tuple<int, int>>();
            
            int start = 0;
            for (int i = 1; i < guidLine.Length; i++)
            {
                if (guidLine[i] == '-' && guidLine[i - 1] == ' ')
                {
                    start = i;
                }
                if (guidLine[i] == ' ' && guidLine[i-1] == '-')        
                {
                    marks.Add(Tuple.Create<int,int>(start, i-start));
                }
            }

            string [] sa = marks.Select(m => stockLine.Substring(m.Item1, m.Item2).Trim()).ToArray();

            IbdData data = new IbdData
            {
                Symbol = sa[0],
                CompanyName = sa[1],
                Ibd50Rank = sa[2],
                CurrentPrice = sa[3],
                PriceChange = sa[4],
                PriceChangePercent = sa[5],
                OffHighPercent = sa[6],
                VolumeInK = sa[7],
                VolumeChangePercent = sa[8],
                CompositeRating = sa[9],
                EPSRating = sa[10],
                RSRating = sa[11],
                SMRRating = sa[12],
                AccDisRating = sa[13],
                GroupRelStrRating = sa[14],
                LastQtrEPSChangePercent = sa[15],
                PrevQtrEPSChangePercent = sa[16],
                CurrentQtrEPSEstChangePercent = sa[17],
                CurrentYearEPSEstChangePercent = sa[18],
                LastQtrSalesChangePercent = sa[19],
                LatestYearAnnualROE = sa[20],
                LatestYearAnnualProfitMargin = sa[21],
                MgmtOwnPercent = sa[22],
                QtrsofRisingSponsorship = sa[23],
            };

            return new Stock
            {
                Data = Serializer.SerializeToJson<IbdData>(data),
                Ibd50Rank = int.Parse(data.Ibd50Rank),
                Symbol = data.Symbol,
                Following = true
            };
        }
    }
}
