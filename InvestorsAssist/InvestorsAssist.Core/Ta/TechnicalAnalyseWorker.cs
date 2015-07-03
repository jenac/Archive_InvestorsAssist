using InvestorsAssist.Configuration;
using InvestorsAssist.Core.Interface;
using InvestorsAssist.Core.Ta.Analyzer;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Internet;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ta
{
    public class TechnicalAnalyseWorker : IWorker
    {
        private readonly DataContext _context;
        private readonly List<IAnalyzer> _analyzers;

        public TechnicalAnalyseWorker(DataContext context)
        {
            _context = context;
            _analyzers = new List<IAnalyzer>
            {
                new ADXAnalyzer(),
                new BollingerBandsAnalyzer(),
                new GainLossAnalyzer(),
                new MACDAnalyzer(),
                new ProfitAnalyzer(),
                new RSIAnalyzer(),
                new SMAAnalyzer(),
            };
        }
        public void DoWork()
        {
            var symbols = _context.GetDistinctSymbolFromCompany().ToList();
            var buffer = new List<Indicator>();
            foreach(var symbol in symbols)
            {

                var data = _context.GetEod(symbol).ToList();
                if (data.Count < 50)
                    continue;
                foreach(var analyzer in _analyzers)
                {
                    try
                    {
                        var indicators = analyzer.AnalyzeData(data).Where(i => i!= null);
                        if (indicators != null && indicators.Count() > 0)
                        {
                            buffer.AddRange(indicators);
                        }
                    }
                    catch (Exception ex)
                    {
                        //will continue after handling.
                        Logger.Instance.ErrorFormat("Exception while TA Analyze {0}", ex.Message);
                        Logger.Instance.Error(ex.StackTrace);
                        ExceptionHandler.AlertViaEmail("TA", ex);
                    }
                }
                if (buffer.Count > 256)
                {
                    foreach(var indicator in buffer)
                    {
                        _context.SaveIndicator(indicator);
                    }
                    buffer.Clear();
                }
            }
            
            
            /*
            var reader = new CompanyReader();
            var companies = reader.ReadCompaniesFromInternet();
            Logger.Instance.InfoFormat("{0} companies found", companies.Count);

            var existingCompanySymbols = _context.GetCompanySymbols().ToList();
            var newCompanies = companies
                .Where(c => !existingCompanySymbols.Contains(c.Symbol)
                    && !c.Symbol.Contains('/')
                    && !c.Symbol.Contains('^')).ToList();
            if (newCompanies.Count > 0)
            {
                Logger.Instance.InfoFormat("Saving {0} new companies.", newCompanies.Count);
                foreach(var company in newCompanies)
                {
                    _context.SaveCompany(company);
                }
                using (var email = new EmailClient(
                SystemSettings.Instance.EmailSetting.Server,
                SystemSettings.Instance.EmailSetting.Port,
                SystemSettings.Instance.EmailSetting.Username,
                SystemSettings.Instance.EmailSetting.SecurePassword.ToPlainString()))
                {
                    email.SendHtmlEmail(SystemSettings.Instance.EmailSetting.To,
                        SystemSettings.Instance.EmailSetting.Cc,
                        "IA: Update Company",
                        string.Format("{0} new companies: {1}", 
                            newCompanies.Count, 
                            string.Join(", ", newCompanies.Select(c => c.Symbol).ToArray())));
                }
            }
            else
            {
                Logger.Instance.Info("Companies up to date.");
            }*/
        }

        public string Name
        {
            get { return "Technical Analysis"; }
        }
    }
}
