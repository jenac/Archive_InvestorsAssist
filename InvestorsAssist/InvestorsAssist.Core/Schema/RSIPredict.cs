using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;


namespace InvestorsAssist.Core.Schema
{
	public class RSIPredict : IndicatorDTO
	{
		public static string Name = "RSI Predict";
		public double PredictRsi30Price { get; set; }
		public double PredictRsi50Price { get; set; }
		public double PredictRsi70Price { get; set; }

		public override Indicator ToIndicator()
		{
			return new Indicator
			{
				Symbol = this.Symbol,
				Name = RSIPredict.Name,
				Date = this.Date,
                Data = Serializer.SerializeToXml<RSIPredict>(this)
			};
		}
	}
}

