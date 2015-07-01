using InvestorsAssist.Entities;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.DataAccess
{
    public static class UpdateHelper
    {
        public static string ToCsv(this Eod value)
        {
            return string.Format("{0},{1},{2:0.00},{3:0.00},{4:0.00},{5:0.00},{6}",
                value.Symbol,
                value.Date.ToString("yyyy-MM-dd"),
                value.Open,
                value.High,
                value.Low,
                value.Close,
                value.Volume);
        }

        public static void BulkSave(DataContext context, string tableName, string[] lines)
        {
            string file = Path.Combine(FileSystem.GetTempFolder());
            FileSystem.EnsureFolder(file);
            file = Path.Combine(file, string.Format("{0}.csv", tableName));
            File.WriteAllLines(file, lines);
            try
            {
                context.LoadData(file, tableName);
            }
            finally
            {
                File.Delete(file);
            }
        }
    }
}
