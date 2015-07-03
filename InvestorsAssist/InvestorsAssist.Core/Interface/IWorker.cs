using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Interface
{
    public interface IWorker
    {
        string Name { get; }
        void DoWork();
    }
}
