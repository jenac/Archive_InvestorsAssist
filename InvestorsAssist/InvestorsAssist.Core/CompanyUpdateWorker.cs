using InvestorsAssist.Configuration;
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

namespace InvestorsAssist.Core
{
    public class CompanyUpdateWorker : IWorker
    {
        private readonly DataContext _context;

        public CompanyUpdateWorker(DataContext context)
        {
            _context = context;
        }
        public void DoWork()
        {
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
            }
        }

        public string Name
        {
            get { return "Company updates"; }
        }
    }
}
