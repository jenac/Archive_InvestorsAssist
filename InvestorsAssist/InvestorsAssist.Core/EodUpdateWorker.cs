using InvestorsAssist.DataAccess;
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Text;
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
            var corrected = parameters
                .Select(p => MergeSpiltCorrection(p))
                .Where(cp => cp!= null).ToList();
            //Merge Split
			List<Eod> seed = new List<Eod> ();
			List<Eod> value =
                corrected.AsParallel()
					.Select(p => _reader.ReadEodBySymbol(p))
					.Aggregate(seed, (i, j) => i.Union(j).ToList());
			return value;
		}

        private EodParam MergeSpiltCorrection(EodParam source)
        {
            if (source.Start == DateTime.Today.AddYears(-30))
                return source;
            var overlap = new EodParam {
                Symbol = source.Symbol,
                Start = source.Start.AddDays(-10),
                End = source.Start.AddDays(1)
            };
            var online = _reader.ReadEodBySymbol(overlap);
            if (online.Count == 0)
                return null;

            EodParam corrected = new EodParam {
                Symbol = source.Symbol,
                Start = DateTime.Today.AddYears(-30),
                End = source.End
            };
            var database = _context.GetLast3Eod(overlap.Symbol).ToList();
            if (database.Count == 0)
            {
                Logger.Instance.InfoFormat("{0} merge/split? will re-download", overlap.Symbol);
                return corrected;
            }
            foreach(var eod in database)
            {
                var found = online.Where(d => d.Symbol == eod.Symbol && d.Date == eod.Date).FirstOrDefault();
                if (found == null)
                {
                    continue;
                }
                if (!TextParser.AlmostEqual(found.Open, eod.Open) ||
                    !TextParser.AlmostEqual(found.High, eod.High) ||
                    !TextParser.AlmostEqual(found.Low, eod.Low) ||
                    !TextParser.AlmostEqual(found.Close, eod.Close))
                {
                    Logger.Instance.InfoFormat("{0} merge/split? will re-download", eod.Symbol);
                    return corrected;
                }
            }

            return source;
        }
    }
}
