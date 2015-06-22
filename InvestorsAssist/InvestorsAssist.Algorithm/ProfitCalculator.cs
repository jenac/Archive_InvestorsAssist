
namespace InvestorsAssist.Algorithm
{
	public static class ProfitCalculator
	{
		public static double CalculateProfit(int days, double[] prices)
		{
			if (prices.Length < days) return 0;
			double end = prices[0];
			double start = prices[days - 1];
			return (end-start) / start * 100;
		}
	}
}

