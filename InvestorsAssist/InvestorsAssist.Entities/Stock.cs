using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Entities
{
    public class Stock
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public int Ibd50Rank { get; set; }

        public bool Following { get; set; }
        public string Data { get; set; }
    }
}
