using InvestorsAssist.Algorithm.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacTec.TA.Library;

namespace InvestorsAssist.Algorithm
{
    public class BollingerBandsCalculator
    {
        public static readonly int Period = 20;
        public static readonly int DeviationUp  = 2;
        public static readonly int DeviationDown = 2;

        public static List<BollingerBandsData> CalculateBollingerBands(int period, int deviationUp, int deviationDown, double[] closePrices)
        {
            int size = closePrices.Length;
            double[] uppperValues = new double[size];
            double[] middleValues = new double[size];
            double[] lowerValues = new double[size];
            int beginIndex;
            int endIndex;
            Core.Bbands(0, size-1, closePrices, period, deviationUp, deviationDown,
                Core.MAType.Sma, out beginIndex, out endIndex, uppperValues, middleValues, lowerValues);
            List<BollingerBandsData> value = new List<BollingerBandsData>();
            for (int i = 0; i < endIndex; i++)
            {
                value.Add(new BollingerBandsData
                {
                    Upper = uppperValues[i],
                    Middle = middleValues[i],
                    Lower = lowerValues[i]
                });
            }
            return value;
        }
    }
}
