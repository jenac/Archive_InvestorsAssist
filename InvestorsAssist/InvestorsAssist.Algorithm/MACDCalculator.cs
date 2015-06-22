using InvestorsAssist.Algorithm.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;

namespace InvestorsAssist.Algorithm
{
    public class MACDCalculator
    {
        public static readonly int FastPeriod = 12;
        public static readonly int SlowPeriod = 26;
        public static readonly int SignalPeriod = 9;

        public static List<MacdData> CalculateMACD(int fastPeriod, int slowPeriod, int signalPeriod, double[] closePrices)
        {

            int size = closePrices.Length;
            double[] macdValues = new double[size];
            double[] macdSingalValues = new double[size];
            double[] macdHISTValues = new double[size];
            int beginIndex;
            int endIndex;
            Core.Macd(0, size-1 , closePrices, FastPeriod,SlowPeriod,SignalPeriod,
                out beginIndex, out endIndex, 
                macdValues, macdSingalValues, macdHISTValues);
            List<MacdData> value = new List<MacdData>();
            for (int i = 0; i < endIndex; i++)
            {
                value.Add(new MacdData
                {
                    MacdValue = macdValues[i],
                    MacdSingal = macdSingalValues[i],
                    MacdHIST = macdHISTValues[i]
                });
            }
            return value;
        }
    }
}
