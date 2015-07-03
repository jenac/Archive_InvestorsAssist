using InvestorsAssist.Algorithm;
using InvestorsAssist.Algorithm.Helper;
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
    class SMAAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            double[] closePrices = data.Select(d=>d.Close).ToArray();

            double[] sma5Values = SMACalculator.CalculateSMA(5, closePrices);
            double[] sma10Values = SMACalculator.CalculateSMA(10, closePrices);
            double[] sma20Values = SMACalculator.CalculateSMA(20, closePrices);
            double[] sma50Values = SMACalculator.CalculateSMA(50, closePrices);
            double[] sma200Values = SMACalculator.CalculateSMA(200, closePrices);

            SMA value = new SMA();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.SMA5 = AlgorithmHelper.GetLast(sma5Values);
            value.SMA10 = AlgorithmHelper.GetLast(sma10Values);
            value.SMA20 = AlgorithmHelper.GetLast(sma20Values);
            value.SMA50 = AlgorithmHelper.GetLast(sma50Values);
            value.SMA200 = AlgorithmHelper.GetLast(sma200Values);

            return new List<Indicator> {value.ToIndicator()};
        }
    }
}
