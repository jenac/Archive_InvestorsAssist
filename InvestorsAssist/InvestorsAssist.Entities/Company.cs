using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Entities
{
    public class Company
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string Name { get; set; }
        public float LastSale { get; set; }
        public decimal MarketCap { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
    }
}
