
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
namespace InvestorsAssist.Core.Schema
{
    public class ADX : IndicatorDTO
    {
        public const string Name = "ADX";
        
        public double ADX14 { get; set; }
        public override Indicator ToIndicator()
        {
            return new Indicator
            {
                Symbol = this.Symbol,
                Name = ADX.Name,
                Date = this.Date,
                Data = Serializer.SerializeToXml<ADX>(this)
            };
        }
    }

    
		
		
}
