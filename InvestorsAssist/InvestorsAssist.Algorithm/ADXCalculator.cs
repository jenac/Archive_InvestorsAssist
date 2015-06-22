using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;

namespace InvestorsAssist.Algorithm
{
    public class ADXCalculator
    {
        public static readonly int Period = 14;
        public static double[] CalculateADX(int period, double[] highPrices, double[] lowPrices, double[] closePrices)
        {
            
            int size = closePrices.Length;
            double[] adxValues = new double[size];
            double[] adxrValues = new double[size];
            int adxBegin;
            int adxEnd;
            Core.Adx(0, closePrices.Length - 1, highPrices, lowPrices, closePrices, period, out adxBegin, out adxEnd, adxValues);
            Core.Adxr(0, closePrices.Length - 1, highPrices, lowPrices, closePrices, period, out adxBegin, out adxEnd, adxrValues);
            return adxValues.Take(adxEnd).ToArray();
        }
    }
}
