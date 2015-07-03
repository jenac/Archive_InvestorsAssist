using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;


namespace InvestorsAssist.Core.Schema
{
	public class Profit : IndicatorDTO
	{
		public static string Name = "Profit";
		public double R20Day { get; set; }
		public double R50Day { get; set; }
		public double R100Day { get; set; }
		public double R150Day { get; set; }
		public double R200Day { get; set; }

		public override Indicator ToIndicator()
		{
			return new Indicator
			{
				Symbol = this.Symbol,
				Name = Profit.Name,
				Date = this.Date,
                Data = Serializer.SerializeToXml<Profit>(this)
			};
		}
	}
}

