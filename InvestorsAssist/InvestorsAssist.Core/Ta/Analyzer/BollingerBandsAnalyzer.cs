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
    class BollingerBandsAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            double[] closePrices = data.Select(d=>d.Close).ToArray();
            List<BollingerBandsData> bBDataValues = BollingerBandsCalculator.CalculateBollingerBands(
                BollingerBandsCalculator.Period,
                BollingerBandsCalculator.DeviationUp,
                BollingerBandsCalculator.DeviationDown,
                closePrices);
            if (bBDataValues.Count == 0) return null;
            var lastBB = AlgorithmHelper.TakeLast<BollingerBandsData>(bBDataValues, 1).FirstOrDefault();
            if (lastBB == null) return null;
            BollingerBands value = new BollingerBands();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.Upper = lastBB.Upper;
            value.Middle = lastBB.Middle;
            value.Lower = lastBB.Lower;
            value.ChannelHight = value.Upper - value.Lower;
            value.ChannelPercent = value.ChannelHight * 100 / value.Middle;
            return new List<Indicator> {value.ToIndicator()};
        }
    }
}
