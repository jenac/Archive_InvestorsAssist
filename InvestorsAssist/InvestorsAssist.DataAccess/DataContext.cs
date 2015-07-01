using InvestorsAssist.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.DataAccess
{
    public class DataContext : DbContext
    {
        protected ObjectContext _objectCtx;
        
        private DataContext()
            : base()
        {

        }

        public DataContext(string connectionString)
            : base(connectionString)
        {
            _objectCtx = (this as IObjectContextAdapter).ObjectContext;
            _objectCtx.CommandTimeout = 300;
        }

        public void SaveIbdPick(IbdPick value)
        {
            this.Database.ExecuteSqlCommand("EXEC Proc_IbdPick_Upsert {0}, {1}, {2}, {3}, {4}",
                value.Symbol, value.Date, value.Ibd50Rank, value.Data, value.Following);
        }

        public DateTime? GetIbdPickLastUpdate()
        {
            return this._objectCtx.ExecuteStoreQuery<DateTime>("EXEC Proc_IbdPick_LastUpdate_Get").FirstOrDefault();
        }

        public IEnumerable<string> GetFollowingList()
        {
            return this._objectCtx.ExecuteStoreQuery<string>("EXEC Proc_IbdPick_Following_Get");
            
        }

        public IEnumerable<DateTime> GetLast2IbdDate()
        {
            return this._objectCtx.ExecuteStoreQuery<DateTime>("EXEC Proc_IbdPick_Last2_Dates_Get");
        }


        public IEnumerable<string> GetIbd50ByDate(DateTime latest)
        {
            return this._objectCtx.ExecuteStoreQuery<string>("EXEC Proc_IbdPick_Ibd50_Symbol_By_Date_Get {0}", latest);
        }

        public IEnumerable<string> GetCompanySymbols()
        {
            return this._objectCtx.ExecuteStoreQuery<string>("EXEC Proc_Company_Symbol_Get");
        }

        public void SaveCompany(Company value)
        {
            this.Database.ExecuteSqlCommand("EXEC Proc_Company_Upsert {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                value.Symbol, value.Exchange, value.Name, value.LastSale, value.MarketCap, value.Sector, value.Industry);
        }

        public IEnumerable<DataState> GetEodState()
        {
            return this._objectCtx.ExecuteStoreQuery<DataState>("EXEC Proc_EodState_Get");
        }

        //Bulk Insert
        public int LoadData(string file, string table)
        {
            const string _BULK_INSERT = @"BULK INSERT {0} FROM '{1}' WITH ( 
FIELDTERMINATOR = ',',
ROWTERMINATOR = '\n'
); SELECT @@ROWCOUNT";
            int rowCount = _objectCtx.ExecuteStoreQuery<int>
                (string.Format(_BULK_INSERT, table, file)).Single();
            return rowCount;
        }

        public IEnumerable<Eod> GetLast3Eod(string symbol)
        {
            return this._objectCtx.ExecuteStoreQuery<Eod>("EXEC Proc_Last3Eod_Get {0}", symbol);
        }
    }
}
