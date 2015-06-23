using InvestorsAssist.Algorithm.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ta
{
    public class StockSummuary
    {
        public string Symbol { get; set; }
        public double LastClose { get; set; }
        public double LastHigh { get; set; }
        public double LastLow { get; set; }

        public double R100 { get; set; }
        public double R200 { get; set; }

        public double Rsi { get; set; }
        public double R30Price { get; set; }
        public double R70Price { get; set; }
        public double R50Price { get; set; }

        public double SMA50 { get; set; }
        public double SMA200 { get; set; }
        public double SMA9 { get; set; }
        public double SMA21 { get; set; }

        public double PrevSMA50 { get; set; }
        public double PrevSMA200 { get; set; }
        public double PrevSMA9 { get; set; }
        public double PrevSMA21 { get; set; }


        public BollingerBandsData BB { get; set; }

        public MacdData Macd { get; set; }

        public MacdData PrevMacd { get; set; }

        public double Adx { get; set; }
    }
}
