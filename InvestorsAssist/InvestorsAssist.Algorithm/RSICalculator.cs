using InvestorsAssist.Algorithm.Schema;
using InvestorsAssist.Algorithm.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using TicTacTec.TA.Library;

namespace InvestorsAssist.Algorithm
{
	public static class RSICalculator
	{
		public static readonly int Period = 14;
		public static double[] CalculateRsi(int period, double[] prices)
		{
			int size = prices.Length;
			double[] rsiValues = new double[size];
			int rsiBegin;
			int rsiEnd;
			Core.Rsi(0, prices.Length - 1, prices, period, out rsiBegin, out rsiEnd, rsiValues);
			return rsiValues.Take(rsiEnd).ToArray();
		}

		public static double CalculateLastRsi(int period, double[] prices)
		{
			return AlgorithmHelper.GetLast(CalculateRsi(period, prices));
		}

		public static double PredictPrice(int period, double targetRSI, List<double> prices)
		{
			double currentPrice = prices.Last();
			double currentRSI = RSICalculator.CalculateLastRsi(period, prices.ToArray());
			if (currentRSI == 0)
				return 0;
			if (targetRSI == currentRSI) return currentPrice;

			double targetRS = GetRS(targetRSI);
			AvgGainLossPair aglp = GetAGL(period, prices.Count() - 1, prices);
			double delta = (targetRSI > currentRSI) ? targetRS * aglp.AvgLoss * 13 - aglp.AvgGain * 13 :
				(aglp.AvgGain * 13 / targetRS - aglp.AvgLoss * 13) * -1;
			return currentPrice + delta;

		}

		public static double GetRS(double RSI)
		{
			return 100 / (100 - RSI) - 1;
		}

		public static AvgGainLossPair GetAGL(int period, int index, List<double> prices)
		{
			if (index < period) throw new Exception("no enough data");
			AvgGainLossPair agl = GetFirstAGL(period, prices);
			if (index == period) return agl;
			for (int i = period + 1; i <= index; i++)
			{
				double prevPrice = prices[i - 1];
				double curPrice = prices[i];
				double delta = curPrice - prevPrice;
				double gain = delta > 0? delta: 0;
				double loss = delta < 0? Math.Abs(delta): 0;

				agl.AvgGain = (agl.AvgGain * 13 + gain)/ 14;
				agl.AvgLoss = (agl.AvgLoss * 13 + loss) / 14;
			}
			return agl;
		}

		public static AvgGainLossPair GetFirstAGL(int period, List<double> prices)
		{
			List<double> firstPeridPrices = prices.Take(period).ToList();
			var deltas = firstPeridPrices.Zip(firstPeridPrices.Skip(1), (current, next) => next - current).ToList();
			return new AvgGainLossPair
			{
				AvgGain = deltas.Where(d => d > 0).Sum() / period,
				AvgLoss = Math.Abs(deltas.Where(d => d < 0).Sum()) / period
			};
		}

		public static RSIRangeData GetRSIRange(int period, List<double> prices)
		{
			List<double> sortedRSI = RSICalculator.CalculateRsi(period, prices.ToArray()).OrderBy(e=>e).ToList();
			int count = sortedRSI.Count();
			if (count < 100) return null;

			RSIRangeData value = new RSIRangeData();
			value.Min = sortedRSI.First();
			value.Max = sortedRSI.Last();
			value.L5 = sortedRSI.Skip(count / 20).First();
			value.L10 = sortedRSI.Skip(count / 10).First();
			value.L15 = sortedRSI.Skip(count / 20 * 3).First();
			value.H5 = sortedRSI.Skip(count / 20 * 19).First();
			value.H10 = sortedRSI.Skip(count / 10 * 9).First();
			value.H15 = sortedRSI.Skip(count / 20 * 17).First();
			return value;
		}
	}
}

