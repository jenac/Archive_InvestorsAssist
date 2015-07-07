using InvestorsAssist._WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace InvestorsAssist._WebApi.Controller
{
    public class SharpChartController : ApiController
    {
        // GET api/sharpchart/AAPL
        public SharpChartJS Get(string id)
        {
            try
            {
                SharpChartJS value = new SharpChartJS
                {
                    DailyChartBase64 = GetStockChartsImage("D", id),
                    WeeklyChartBase64 = GetStockChartsImage("W", id),
                };
                return value;
            }
            catch
            {
                return new SharpChartJS();
            }
        }


        //period:
        //D - daily
        //W - weekly
        private string GetStockChartsImage(string period, string symbol)
        {
            byte[] imageAsByteArray;
            using (var webClient = new WebClient())
            {
                imageAsByteArray = webClient.DownloadData(
                    string.Format("http://stockcharts.com/c-sc/sc?s={0}&p={1}&b=5&g=0&i=0", symbol, period));
                return Convert.ToBase64String(imageAsByteArray);
            }

        }
    }
}
