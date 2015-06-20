using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.IO
{
    public class Logger
    {
        private static readonly Lazy<ILog> lazy =
        new Lazy<ILog>(() => {
            XmlConfigurator.Configure();
            return LogManager.GetLogger(Assembly.GetExecutingAssembly().FullName);
        });

        public static ILog Instance { get { return lazy.Value; } }

        private Logger()
        { }
        
    }
}
