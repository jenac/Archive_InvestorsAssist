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
    class ADXAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            double[] highPrices = data.Select(d =>d.High).ToArray();
            double[] lowPrices = data.Select(d =>d.Low).ToArray();
            double[] closePrices = data.Select(d => d.Close).ToArray();
            double[] adxValues = ADXCalculator.CalculateADX(ADXCalculator.Period, highPrices, lowPrices, closePrices);
            if (adxValues.Length == 0) return null;

            ADX value = new ADX();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.ADX14 = AlgorithmHelper.GetLast(adxValues);
            return new List<Indicator> { value.ToIndicator() };
        }
    }
}
