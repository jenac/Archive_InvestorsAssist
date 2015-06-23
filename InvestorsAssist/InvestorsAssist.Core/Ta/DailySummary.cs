using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ta
{
    public class DailySummary
    {
        public DateTime TradingDate { get; private set; }
        public List<string> LatestIdb50 { get; set; }
        public List<string> NewInIdb50 { get; set; }
        public List<string> JustOutIdb50 { get; set; }

        public List<string> StillFollowing { get; set; }

        public List<StockSummuary> DetailedSummaries { get; set; }

        public DailySummary(DateTime tradingDate)
        {
            this.TradingDate = tradingDate;
        }
    }
}
