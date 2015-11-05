using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Entities
{
    public class Watchlist
    {
        public string Name { get; set; }
        public string CSV { get; set; }

        public bool Active { get; set; }

    }
}
