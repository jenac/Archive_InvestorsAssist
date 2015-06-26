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
    public class Email
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        [XmlIgnore]
        public SecureString SecurePassword { get; set; }
        public string Password { get; set; }

        public string To { get; set; }

        public List<string> Cc { get; set; }
    }
}
