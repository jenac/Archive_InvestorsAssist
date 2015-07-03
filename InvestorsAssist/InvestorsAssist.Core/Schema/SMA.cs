using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;

namespace InvestorsAssist.Core.Schema
{
	public class SMA : IndicatorDTO
	{
		public static string Name = "SMA";

        public double SMA5 { get; set; }
        public double SMA10 { get; set; }
        public double SMA20 { get; set; }
		public double SMA50 { get; set; }
		public double SMA200 { get; set; }

		public override Indicator ToIndicator()
		{
			return new Indicator
			{
				Symbol = this.Symbol,
				Name = SMA.Name,
				Date = this.Date,
                Data = Serializer.SerializeToXml<SMA>(this)
			};
		}
	}
}

