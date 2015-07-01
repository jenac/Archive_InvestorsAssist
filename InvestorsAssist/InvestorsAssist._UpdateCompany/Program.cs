using InvestorsAssist.Core;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist._UpdateCompany
{
    class Program
    {
        /// <summary>
        /// Update companies/ETF List Weekly, Send Email Notify new companies
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Logger.Instance.Info("InvestorsAssist._UpdateCompany started ...");
            try
            {
                using (var context = new DataContext("InvestorsAssist"))
                {
                    List<IWorker> workers = new List<IWorker> {
                        new CompanyUpdateWorker(context),
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
                ExceptionHandler.AlertViaEmail("InvestorsAssist._UpdateCompany", ex);
            }
            Logger.Instance.Info("InvestorsAssist._UpdateCompany done.");
        }
    }
}
