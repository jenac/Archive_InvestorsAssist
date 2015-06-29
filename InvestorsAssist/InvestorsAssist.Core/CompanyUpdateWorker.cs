using InvestorsAssist.DataAccess;
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
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
