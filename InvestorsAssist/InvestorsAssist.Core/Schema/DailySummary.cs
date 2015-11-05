using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Schema
{
    public class DailySummary
    {
        public DateTime TradingDate { get; private set; }
        public List<TraceSummuary> DetailedSummaries { get; set; }
        public DailySummary(DateTime tradingDate)
        {
            this.TradingDate = tradingDate;
            
        }
    }
}
