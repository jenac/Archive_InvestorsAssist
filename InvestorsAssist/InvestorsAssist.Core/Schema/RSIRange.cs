using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
namespace InvestorsAssist.Core.Schema
{
	public class RSIRange : IndicatorDTO
	{
		/// <summary>
		/// Min: min RSI
		/// L5: 5% days in the low range
		/// H5: 5% days in the low range
		/// </summary>
		public static string Name = "RSI Range";
		public double Min { get; set; }
		public double Max { get; set; }
		public double L5 { get; set; }
		public double H5 { get; set; }
		public double L10 { get; set; }
		public double H10 { get; set; }
		public double L15 { get; set; }
		public double H15 { get; set; }


		public override Indicator ToIndicator()
		{
			return new Indicator
			{
				Symbol = this.Symbol,
				Name = RSIRange.Name,
				Date = this.Date,
                Data = Serializer.SerializeToXml<RSIRange>(this)
			};
		}
	}
}

