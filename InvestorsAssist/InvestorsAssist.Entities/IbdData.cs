using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Entities
{
    public class IbdData
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Ibd50Rank { get; set; }
        public string CurrentPrice { get; set; }
        public string PriceChange { get; set; }
        public string PriceChangePercent { get; set; }
        public string OffHighPercent { get; set; }
        public string VolumeInK { get; set; }
        public string VolumeChangePercent { get; set; }
        public string CompositeRating { get; set; }
        public string EPSRating { get; set; }
        public string RSRating { get; set; }
        public string SMRRating { get; set; }
        public string AccDisRating { get; set; }
        public string GroupRelStrRating { get; set; }
        public string LastQtrEPSChangePercent { get; set; }
        public string PrevQtrEPSChangePercent { get; set; }
        public string CurrentQtrEPSEstChangePercent { get; set; }
        public string CurrentYearEPSEstChangePercent { get; set; }
        public string LastQtrSalesChangePercent { get; set; }
        public string LatestYearAnnualROE { get; set; }
        public string LatestYearAnnualProfitMargin { get; set; }
        public string MgmtOwnPercent { get; set; }
        public string QtrsofRisingSponsorship { get; set; }
    }
}
