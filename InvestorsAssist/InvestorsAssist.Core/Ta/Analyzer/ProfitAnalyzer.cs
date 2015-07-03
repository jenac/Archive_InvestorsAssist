using InvestorsAssist.Algorithm;
using InvestorsAssist.Core.Interface;
using InvestorsAssist.Core.Schema;
using InvestorsAssist.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ta.Analyzer
{
    class ProfitAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            //profit need sort by date desc
            double[] closePrices = data.Select(d=>d.Close).Reverse().ToArray();
            Profit value = new Profit();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.R20Day = ProfitCalculator.CalculateProfit(20, closePrices);
            value.R50Day = ProfitCalculator.CalculateProfit(50, closePrices);
            value.R100Day = ProfitCalculator.CalculateProfit(100, closePrices);
            value.R150Day = ProfitCalculator.CalculateProfit(150, closePrices);
            value.R200Day = ProfitCalculator.CalculateProfit(200, closePrices);
            return new List<Indicator> { value.ToIndicator() };
        }
    }
}
