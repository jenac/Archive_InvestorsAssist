using System.Linq;
using TicTacTec.TA.Library;
namespace InvestorsAssist.Algorithm
{
	public static class SMACalculator
	{
		public static double[] CalculateSMA(int period, double[] closePrices)
		{
			int size = closePrices.Length;
			double[] smaValues = new double[size];
			int smaBegin;
			int smaEnd;
			Core.Sma(0, closePrices.Length - 1, closePrices, period, out smaBegin, out smaEnd, smaValues);
			return smaValues.Take(smaEnd).ToArray();
		}
	}
}

