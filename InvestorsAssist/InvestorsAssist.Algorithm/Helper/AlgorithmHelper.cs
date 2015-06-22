using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Algorithm.Helper
{
    public static class AlgorithmHelper
    {
        public static double GetLast(double[] indicatorValues)
        {
            if (indicatorValues.Length == 0)
                return 0;
            return indicatorValues[indicatorValues.Length - 1];
        }

        public static IEnumerable<T> TakeLast<T>(IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}
