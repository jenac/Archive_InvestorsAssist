using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;

namespace InvestorsAssist.Core.Schema
{
	public class GainLoss : IndicatorDTO
	{
		public static string Name = "GainLoss";
		public int MaxContGainDays { get; set; }
		public double AvgContGainDays { get; set; }
		public int MaxContLossDays { get; set; }
		public double AvgContLossDays { get; set; }
		public int LastGLContDays { get; set; }

		public override Indicator ToIndicator()
		{
			return new Indicator
			{
				Symbol = this.Symbol,
				Name = GainLoss.Name,
				Date = this.Date,
                Data = Serializer.SerializeToXml<GainLoss>(this)
			};
		}
	}
}

