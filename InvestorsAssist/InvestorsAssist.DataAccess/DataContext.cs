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

        public IEnumerable<string> GetFollowingList()
        {
            return this._objectCtx.ExecuteStoreQuery<string>("EXEC Proc_IbdPick_Following_Get");
            
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

        public void DeleteEod(string symbol)
        {
            this.Database.ExecuteSqlCommand("EXEC Proc_Eod_Delete {0}", symbol);
        }

        public IEnumerable<string> GetDistinctSymbolFromCompany()
        {
            return this._objectCtx.ExecuteStoreQuery<string>("EXEC Proc_DistinctSymbolFromCompany_Get");
        }

        public IEnumerable<Eod> GetEod(string symbol)
        {
            return this._objectCtx.ExecuteStoreQuery<Eod>("EXEC Proc_Eod_Get {0}", symbol);
        }

        public void SaveIndicator(Indicator value)
        {
            this.Database.ExecuteSqlCommand("EXEC Proc_Indicator_Upsert {0}, {1}, {2}, {3}", 
                value.Symbol, value.Name, value.Date, value.Data);
        }

        public IEnumerable<Watchlist> GetActiveWatchlist()
        {
            return this._objectCtx.ExecuteStoreQuery<Watchlist>("EXEC Proc_Watchlist_GetActive");
        }
    }
}
