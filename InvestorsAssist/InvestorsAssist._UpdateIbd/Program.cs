using InvestorsAssist.Core;
using InvestorsAssist.Core.Ibd;
using InvestorsAssist.Core.Interface;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist._UpdateIbd
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Instance.Info("InvestorsAssist._UpdateIbd started ...");
            try
            {
                using (var context = new DataContext("InvestorsAssist"))
                {
                    List<IWorker> workers = new List<IWorker> {
                        new IbdUpdateWorker(context),
                    };
                    foreach (var worker in workers)
                    {
                        Logger.Instance.InfoFormat("Worker '{0}' is on duty", worker.Name);
                        worker.DoWork();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.InfoFormat("Exception: {0}", ex.Message);
                Logger.Instance.Info(ex.StackTrace);
                ExceptionHandler.AlertViaEmail("InvestorsAssist._UpdateIbd", ex);
            }
            Logger.Instance.Info("InvestorsAssist._UpdateIbd done.");
        }
    }
}
