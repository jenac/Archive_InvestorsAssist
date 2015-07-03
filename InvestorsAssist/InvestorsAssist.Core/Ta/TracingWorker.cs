using InvestorsAssist.Algorithm;
using InvestorsAssist.Algorithm.Helper;
using InvestorsAssist.Algorithm.Schema;
using InvestorsAssist.Configuration;
using InvestorsAssist.Core.Interface;
using InvestorsAssist.Core.Schema;
using InvestorsAssist.DataAccess;
using InvestorsAssist.Entities;
using InvestorsAssist.Utility.Internet;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ta
{
    public class TracingWorker : IWorker
    {
        private readonly DataContext _context;

        public TracingWorker(DataContext context)
        {
            _context = context;
        }
        public string Name
        {
            get { return "Technical Analysis"; }
        }

        public void DoWork()
        {
            
            DateTime? lastTradingDate = GetLastTradingDate();
            if (lastTradingDate == null)
            {
                Logger.Instance.Error("Cannot get last trading date with retries.");
                return;
            }

            //if (lastTradingDate != DateTime.Today)
            //{
            //    Logger.Instance.InfoFormat("No trading today: {0}", DateTime.Today.ToString("yyyy-MM-dd"));
            //    return;
            //}

            //get ibd50 last
            // get ibd50 late prev list
            //
            DailySummary summary = new DailySummary(lastTradingDate.Value);
            List<DateTime> dates = _context.GetLast2IbdDate().ToList();

            if (dates.Count != 2)
            {
                Logger.Instance.Error("No history for daily summary yet. Need at least 2 days of history");
                return;
            }
            DateTime latest = dates[0];
            DateTime previous = dates[1];
            List<string> latestList = _context.GetIbd50ByDate(latest).ToList();
            List<string> previousList = _context.GetIbd50ByDate(previous).ToList();
            summary.LatestIdb50 = latestList;
            summary.NewInIdb50 = latestList.Except(previousList).ToList();
            summary.JustOutIdb50 = previousList.Except(latestList).ToList();

            List<string> following = _context.GetFollowingList().ToList();
            summary.StillFollowing = following.Except(latestList).ToList();

            //process following list:
            List<StockSummuary> detailedSummaries = new List<StockSummuary>();
            foreach (var symbol in following)
            {
                var prices = GetPriceData(symbol);
                if (prices.Count == 0)
                {
                    Logger.Instance.ErrorFormat("Cannot get prices for {0}", symbol);
                    continue;
                }
                else if (prices.Count < 50)
                {
                    Logger.Instance.ErrorFormat("History is too short for {0}", symbol);
                    continue;
                }
                detailedSummaries.Add(Analyse(symbol, prices));
            }
            summary.DetailedSummaries = detailedSummaries;

            using (var email = new EmailClient(
                SystemSettings.Instance.EmailSetting.Server, 
                SystemSettings.Instance.EmailSetting.Port,
                SystemSettings.Instance.EmailSetting.Username,
                SystemSettings.Instance.EmailSetting.SecurePassword.ToPlainString()))
            {
                email.SendHtmlEmail(SystemSettings.Instance.EmailSetting.To, 
                    SystemSettings.Instance.EmailSetting.Cc, 
                    "IA: Daily Summary", 
                    summary.ToHtmlPresentation());
            }
            //Load template and create/send email
            //Logger.Instance.Info()
        }

        //Get last trading date with retries
        private DateTime? GetLastTradingDate()
        {
            DateTime? lastTradingDate = null;
            for (int i = 0; i < 10; i++)
            {
                lastTradingDate = PriceReader.GetLastTradeDate();
                if (lastTradingDate != null)
                {
                    break;
                }
                Thread.Sleep(6 * 1000);
            }
            return lastTradingDate;
        }

        private List<PriceData> GetPriceData(string symbol)
        {
            List<PriceData> prices = new List<PriceData>();
            for (int i = 0; i < 10; i++)
            {
                prices = PriceReader.ReadPriceDataBySymbol(symbol);
                if (prices.Count != 0)
                {
                    break;
                }
                Thread.Sleep(6 * 1000);
            }
            return prices.Where(e => e != null).OrderBy(e => e.Date).ToList();
        }

        private StockSummuary Analyse(string symbol, List<PriceData> prices)
        {
            StockSummuary value = new StockSummuary { Symbol = symbol };
            value.LastClose = prices.Last().Close;
            value.LastHigh = prices.Last().High;
            value.LastLow = prices.Last().Low;

            List<double> closePrices = prices.Select(p => p.Close).ToList();
            List<double> highPrices = prices.Select(p => p.High).ToList();
            List<double> lowPrices = prices.Select(p => p.Low).ToList();

            double[] closePricesArray = closePrices.ToArray();
            double[] closePricesArrayDesc = closePrices.ToArray();
            Array.Reverse(closePricesArrayDesc);

            value.R100 = ProfitCalculator.CalculateProfit(100, closePricesArrayDesc);
            value.R200 = ProfitCalculator.CalculateProfit(200, closePricesArrayDesc);

            value.Rsi = RSICalculator.CalculateLastRsi(RSICalculator.Period, closePricesArray);

            value.R30Price = RSICalculator.PredictPrice(RSICalculator.Period, 30, closePrices);
            value.R50Price = RSICalculator.PredictPrice(RSICalculator.Period, 50, closePrices);
            value.R70Price = RSICalculator.PredictPrice(RSICalculator.Period, 70, closePrices);

            if (closePricesArray.Length > 55)
            {
                double[] sma50 = AlgorithmHelper.TakeLast<double>(SMACalculator.CalculateSMA(50, closePricesArray), 2).ToArray();
                value.PrevSMA50 = sma50[0];
                value.SMA50 = sma50[1];
            }

            if (closePricesArray.Length > 205)
            {
                double[] sma200 = AlgorithmHelper.TakeLast<double>(SMACalculator.CalculateSMA(200, closePricesArray), 2).ToArray();
                value.PrevSMA200 = sma200[0];
                value.SMA200 = sma200[1];
            }

            double[] sma9 = AlgorithmHelper.TakeLast<double>(SMACalculator.CalculateSMA(9, closePricesArray), 2).ToArray();
            value.PrevSMA9 = sma9[0];
            value.SMA9 = sma9[1];

            double[] sma21 = AlgorithmHelper.TakeLast<double>(SMACalculator.CalculateSMA(21, closePricesArray), 2).ToArray();
            value.PrevSMA21 = sma21[0];
            value.SMA21 = sma21[1];


            value.BB = BollingerBandsCalculator.CalculateBollingerBands(
                BollingerBandsCalculator.Period,
                BollingerBandsCalculator.DeviationUp,
                BollingerBandsCalculator.DeviationDown,
                closePricesArray).LastOrDefault();

            MacdData[] macds = AlgorithmHelper.TakeLast<MacdData>(
                MACDCalculator.CalculateMACD(
                    MACDCalculator.FastPeriod,
                    MACDCalculator.SlowPeriod,
                    MACDCalculator.SignalPeriod,
                    closePricesArray), 2).ToArray();

            value.PrevMacd = macds[0];
            value.Macd = macds[1];

            value.Adx = AlgorithmHelper.GetLast(
                ADXCalculator.CalculateADX(
                    ADXCalculator.Period,
                    highPrices.ToArray(),
                    lowPrices.ToArray(),
                    closePricesArray));

            return value;
        }
    }
}
