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
    //including RSI, RSIRange, RSIPredict
    class RSIAnalyzer : IAnalyzer
    {
        public List<Indicator> AnalyzeData(List<Eod> data)
        {
            List<Indicator> rsiIndicators = new List<Indicator>();
            double[] closePrices = data.Select(d=>d.Close).ToArray();
            double[] RsiValues = RSICalculator.CalculateRsi(RSICalculator.Period, closePrices);
            if (RsiValues.Length == 0) return rsiIndicators;

            //RSI
            var cont = new ContinuousCalculator(RsiValues, r => r > 50.0);
            RSI rsiValue = new RSI();
            rsiValue.Symbol = data.Last().Symbol;
            rsiValue.Date = data.Last().Date;
            rsiValue.Avg = RsiValues.Average();
            rsiValue.LastRSI = AlgorithmHelper.GetLast(RsiValues);
            rsiValue.PercentGT50 = RsiValues.Where(r => r > 50).Count() * 100 / RsiValues.Count();
            rsiValue.TotalDays = RsiValues.Count();
            rsiValue.MaxContGT50Days = cont.TrueMax;
            rsiValue.MaxContLT50Days = cont.FalseMax;
            rsiValue.AvgContGT50Days = cont.TrueAvg;
            rsiValue.AvgContLT50Days = cont.FalseAvg;
            rsiValue.LastContDays = cont.LastCont;

            rsiIndicators.Add(rsiValue.ToIndicator());

            //RSIPredict
            List<double> closePriceList = data.Select(d => d.Close).ToList();
            RSIPredict predictValue = new RSIPredict();
            predictValue.Symbol = data.Last().Symbol;
            predictValue.Date = data.Last().Date;
            predictValue.PredictRsi30Price = RSICalculator.PredictPrice(RSICalculator.Period, 30, closePriceList);
            predictValue.PredictRsi50Price = RSICalculator.PredictPrice(RSICalculator.Period, 50, closePriceList);
            predictValue.PredictRsi70Price = RSICalculator.PredictPrice(RSICalculator.Period, 70, closePriceList);

            rsiIndicators.Add(predictValue.ToIndicator());
            return rsiIndicators;
            
        }
    }
}
