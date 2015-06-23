using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.Text
{
    public static class TextParser
    {
        public static float ParseFloat(string source)
        {
            float value;
            if (float.TryParse(source, out value))
                return value;
            return 0;
        }

        public static decimal ParseDecimal(string source)
        {
            decimal value;
            if (decimal.TryParse(source, out value))
                return value;
            return 0;
        }

        public static string MoneyFormat(this double value)
        {
            return string.Format("{0:0.00}", value);
        }

        public static bool AlmostEqual(this double left, double right)
        {
            double delta = Math.Abs(left - right);
            double baseLine = Math.Min(Math.Abs(left), Math.Abs(right));
            return delta / baseLine < 0.005 || delta < 0.005;
        }

        public static string Base64Encode(string plain)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(plain);
            return System.Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string encoded)
        {
            var bytes = System.Convert.FromBase64String(encoded);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
