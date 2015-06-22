using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ibd
{
    public class UpdatesWorker : IWorker
    {
        private readonly DataContext _context;

        public UpdatesWorker(DataContext context)
        {
            _context = context;
        }
        public void DoWork()
        {
            var reader = new Reader();
            var stocks = reader.DownloadIbd50List();
            Logger.Instance.InfoFormat("{0} stocks downloaded from investors.com", stocks.Count);
            foreach (var stock in stocks)
            {
                _context.SaveStock(stock);
            }
        }

        public string Name
        {
            get { return "IBD updates"; }
        }
    }
}
