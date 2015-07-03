using InvestorsAssist.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Interface
{
    interface IAnalyzer
    {
        List<Indicator> AnalyzeData(List<Eod> data);
    }
}
