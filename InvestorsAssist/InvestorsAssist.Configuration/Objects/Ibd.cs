using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvestorsAssist.Configuration.Objects
{
    [Serializable]
    public class Ibd
    {
        public string Username { get; set; }
        [XmlIgnore]
        public SecureString SecurePassword { get; set; }
        public string Password { get; set; }

        public int MaxRetries { get; set; }
    }
}
