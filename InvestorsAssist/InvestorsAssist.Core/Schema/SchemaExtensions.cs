using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Schema
{
    public static class SchemaExtensions
    {
        public static string ToHtmlPresentation(this DailySummary summary)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"<!DOCTYPE html>");
            sb.AppendLine(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">");
            sb.AppendLine(@"<head>");
            sb.AppendLine(@"<meta charset=""utf-8"" />");
            sb.AppendLine(@"<title></title>");
            sb.AppendLine(@"</head>");
            sb.AppendLine(@"<body>");
            sb.AppendFormat(@"<h1>Summary on {0}</h1>", summary.TradingDate.ToString("yyyy-MM-dd"));
            sb.AppendLine(@"<hr/>");
            sb.AppendLine("<ul>");
            foreach (var detail in summary.DetailedSummaries)
            {
                var messages = new List<string>();
                if (detail.Rsi <= 30)
                    messages.Add("RSI <= 30");
                else if (detail.Rsi >= 70)
                    messages.Add("RSI >= 70");

                if (detail.LastHigh >= detail.R70Price)
                    messages.Add("Last High >= R70 price");
                if (detail.LastLow <= detail.R30Price)
                    messages.Add("Last Low <= R30 price");

                if (detail.LastLow <= detail.SMA50)
                    messages.Add("Last Low <= SMA 50");

                if (detail.LastLow <= detail.SMA200)
                    messages.Add("Last Low <= SMA 200");

                if (detail.PrevSMA50 > detail.PrevSMA200 && detail.SMA50 < detail.SMA200)
                    messages.Add("SMA 50/200 bearish cross over");
                if (detail.PrevSMA50 < detail.PrevSMA200 && detail.SMA50 > detail.SMA200)
                    messages.Add("SMA 50/200 bullish cross over");

                if (detail.PrevSMA9 > detail.PrevSMA21 && detail.SMA9 < detail.SMA21)
                    messages.Add("SMA 9/21 bearish cross over");
                if (detail.PrevSMA9 < detail.PrevSMA21 && detail.SMA9 > detail.SMA21)
                    messages.Add("SMA 9/21 bullish cross over");

                if (detail.LastHigh >= detail.BB.Upper)
                    messages.Add("Last High >= BB Upper");

                if (detail.LastLow <= detail.BB.Lower)
                    messages.Add("Last Low >= BB Lower");

                if (detail.PrevMacd.MacdValue > detail.PrevMacd.MacdSingal
                    && detail.Macd.MacdValue < detail.Macd.MacdSingal)
                    messages.Add("MACD bearish cross over");
                if (detail.PrevMacd.MacdValue < detail.PrevMacd.MacdSingal
                    && detail.Macd.MacdValue > detail.Macd.MacdSingal)
                    messages.Add("MACD bullish cross over");
                if (messages.Count > 0)
                {
                    sb.AppendLine("<li>");
                    sb.AppendFormat("<b>{0}</b>: {1}", detail.Symbol, string.Join(" | ", messages.ToArray()));
                    sb.AppendLine("</li>");
                }
            }
            sb.AppendLine("</ul>");
            sb.AppendLine(@"</body>");
            sb.AppendLine(@"</html>");
            return sb.ToString();
        }
    }
}
