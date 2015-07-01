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
    public class EodUpdateWorker : IWorker
    {
        private readonly DataContext _context;
        private readonly EodReader _reader;

        public EodUpdateWorker(DataContext context)
        {
            _context = context;
            _reader = new EodReader();
        }
        public string Name
        {
            get { return "Eod Updates"; }
        }

        public void DoWork()
        {
            DateTime? lastTradeDate = _reader.GetLastTradeDate();
			if (!lastTradeDate.HasValue)
			{
				Logger.Instance.Error("Fail to get last trade date.");
				return;
			}
			else
			{
				Logger.Instance.InfoFormat("Last trade date is: {0}", lastTradeDate.Value.ToString("yyyy-MM-dd"));
			}

			var states = _context
                .GetEodState()
                .GroupBy(s =>s.Symbol)
                .Select( g => new DataState { Symbol = g.Key, Last = g.Min(l => l.Last)});

			List<EodParam> parameters = states.Select(e => new EodParam
				{
					Symbol = e.Symbol,
					//For symbol without since, we need get 30 years of data
					Start = (e.Last.HasValue ? e.Last.Value : DateTime.Today.AddYears(-30)),
					End = lastTradeDate.Value
				}).Where(p => p.Start < lastTradeDate.Value).ToList();

			Logger.Instance.InfoFormat("{0} symbols to be processed", parameters.Count);
			int i = 0;
			const int batchSize = 16;
			while (true)
			{
				List<EodParam> processing = parameters.Skip(i * batchSize).Take(batchSize).ToList();
				if (processing.Count == 0)
					break;
				List<Eod> eods = ReadEods(processing);

				if (eods.Count > 0)
				{
					Logger.Instance.InfoFormat("Saving {0} eods ", eods.Count);
					UpdateHelper.BulkSave (_context, "Eod",
						eods.Select (e => e.ToCsv ()).ToArray());
				}
				i++;
			}
		}

		private List<Eod> ReadEods(List<EodParam> parameters)
		{
			List<Eod> seed = new List<Eod> ();
			List<Eod> value =
				parameters.AsParallel()
					.Select(p => _reader.ReadEodBySymbol(p))
					.Aggregate(seed, (i, j) => i.Union(j).ToList());
			return value;
		}
    }
}
