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
    class GainLossAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            double[] gainLoss = data.Select(d => d.Close - d.Open).Reverse().ToArray();
            var calculator = new ContinuousCalculator(gainLoss, gl => gl > 0);
            GainLoss value = new GainLoss();
            value.Symbol = data.Last().Symbol;
            value.Date = data.Last().Date;
            value.MaxContGainDays = calculator.TrueMax;
            value.AvgContGainDays = calculator.TrueAvg;
            value.MaxContLossDays = calculator.FalseMax;
            value.AvgContLossDays = calculator.FalseAvg;
            value.LastGLContDays = calculator.LastCont;
            return new List<Indicator> {value.ToIndicator()};
        }
    }
}
