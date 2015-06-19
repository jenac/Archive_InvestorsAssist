using InvestorsAssist.Configuration.Objects;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Configuration
{
    public class SystemSettings
    {
        private SecureString _passKey;
        public Email EmailSetting { get; private set; }
        public Ibd IbdSetting { get; private set; }
        private static readonly Lazy<SystemSettings> lazy =
        new Lazy<SystemSettings>(() => new SystemSettings());

        public static SystemSettings Instance { get { return lazy.Value; } }

        private SystemSettings()
        {
            //Key from one drive
            string keyFile = Path.Combine(FileSystem.GetOneDriveFolder(), "InvestorAssist.key");
            _passKey = File.ReadAllText(keyFile).ToSecureString();

            //Setting from google drive
            string emailFile = Path.Combine(FileSystem.GetGoogleDriveFolder(), "Email.xml");
            string emailXml = File.ReadAllText(emailFile);
            this.EmailSetting = Serializer.DeserializeFromXml<Email>(emailXml);
            this.EmailSetting.SecurePassword = Encryption.Decrypt(
                this.EmailSetting.Password,
                _passKey.ToPlainString()
                ).ToSecureString();
            this.EmailSetting.Password = string.Empty;

            string ibdFile = Path.Combine(FileSystem.GetGoogleDriveFolder(), "Ibd.xml");
            string ibdXml = File.ReadAllText(ibdFile);
            this.IbdSetting = Serializer.DeserializeFromXml<Ibd>(ibdXml);
            this.IbdSetting.SecurePassword = Encryption.Decrypt(
                this.IbdSetting.Password, 
                _passKey.ToPlainString()
                ).ToSecureString();
            this.IbdSetting.Password = string.Empty;

        }
    }
}
