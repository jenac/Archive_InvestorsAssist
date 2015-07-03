using InvestorsAssist.Algorithm;
using InvestorsAssist.Algorithm.Helper;
using InvestorsAssist.Algorithm.Schema;
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
    class MACDAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            double[] closePrices = data.Select(d=>d.Close).ToArray();
            List<MacdData> macdValues = MACDCalculator.CalculateMACD(
                MACDCalculator.FastPeriod,
                MACDCalculator.SlowPeriod,
                MACDCalculator.SignalPeriod,
                closePrices);
            if (macdValues.Count == 0) return null;
            var lastMacd = AlgorithmHelper.TakeLast<MacdData>(macdValues, 1).FirstOrDefault();
            if (lastMacd == null) return null;
            MACD value = new MACD();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.MacdValue = lastMacd.MacdValue;
            value.MacdSingal = lastMacd.MacdSingal;
            value.MacdHIST = lastMacd.MacdHIST;
            return new List<Indicator> {value.ToIndicator()};
        }
    }
}
