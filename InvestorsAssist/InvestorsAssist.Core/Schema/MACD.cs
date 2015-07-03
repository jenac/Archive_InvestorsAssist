using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;


namespace InvestorsAssist.Core.Schema
{
    public class MACD : IndicatorDTO
    {
        public const string Name = "MACD";
        public double MacdValue { get; set; }
        public double MacdSingal { get; set; }
        public double MacdHIST { get; set; }

        public override Indicator ToIndicator()
        {
            return new Indicator
            {
                Symbol = this.Symbol,
                Name = MACD.Name,
                Date = this.Date,
                Data = Serializer.SerializeToXml<MACD>(this)
            };
        }
    }
}
