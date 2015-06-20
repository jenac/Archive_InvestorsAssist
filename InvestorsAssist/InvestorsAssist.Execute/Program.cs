using InvestorsAssist.Core;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Execute
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance.Info("InvestorsAssist.Execute started ...");
            try
            {
                using (var context = new DataContext("InvestorsAssist"))
                {
                    var reader = new IbdReader();
                    var stocks = reader.DownloadIbd50List();
                    Logger.Instance.InfoFormat("{0} stocks downloaded from investors.com", stocks.Count);
                    foreach (var stock in stocks)
                    {
                        context.SaveStock(stock);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.InfoFormat("Exception while downloading stocksfrom investors.com", ex.Message);
                Logger.Instance.Info(ex.StackTrace);
            }
            Logger.Instance.Info("InvestorsAssist.Execute done.");
        }
    }
}
