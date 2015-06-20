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

        public void SaveStock(Stock value)
        {
            this.Database.ExecuteSqlCommand("EXEC Proc_Stock_Upsert {0}, {1}, {2}, {3}",
                value.Symbol, value.Date, value.Ibd50Rank, value.Data);
        }

        public DateTime? GetStockLastUpdate()
        {
            return this._objectCtx.ExecuteStoreQuery<DateTime>("EXEC Proc_Stock_LastUpdate_Get").FirstOrDefault();
        }
        
    }
}
