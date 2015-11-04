using InvestorsAssist.Core;
using InvestorsAssist.Core.Interface;
using InvestorsAssist.Core.Ta;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;

namespace InvestorsAssist._TechnicalAnalysis
{
    class Program
    {
        /// <summary>
        /// TA daily
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Logger.Instance.Info("InvestorsAssist._TechnicalAnalysis started ...");
            try
            {
                using (var context = new DataContext("InvestorsAssist"))
                {
                    List<IWorker> workers = new List<IWorker> {
                        new TechnicalAnalyseWorker(context),
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
                ExceptionHandler.AlertViaEmail("InvestorsAssist._TechnicalAnalysis", ex);
            }
            Logger.Instance.Info("InvestorsAssist._TechnicalAnalysis done.");
        }
    }
}
