using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;

namespace InvestorsAssist.Core.Schema
{
    public class BollingerBands : IndicatorDTO
    {
        public const string Name = "BollingerBands";
        public double Upper { get; set; }
        public double Middle { get; set; }
        public double Lower { get; set; }
        public double ChannelHight { get; set; }
        public double ChannelPercent { get; set; }

        public override Indicator ToIndicator()
        {
            return new Indicator
            {
                Symbol = this.Symbol,
                Name = BollingerBands.Name,
                Date = this.Date,
                Data = Serializer.SerializeToXml<BollingerBands>(this)
            };
        }
    }
}
