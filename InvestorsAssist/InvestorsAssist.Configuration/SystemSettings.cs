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

        private const string _keyFilename = "InvestorAssist.key";
        private const string _emailFilename = "Email.xml";
        private const string _ibdFilename = "Ibd.xml";
        private SystemSettings()
        {
            string keyFile = Path.Combine(FileSystem.GetSettingsFolder(), _keyFilename);
            _passKey = File.ReadAllText(keyFile).ToSecureString();

            string emailFile = Path.Combine(FileSystem.GetSettingsFolder(), _emailFilename);
            string emailXml = File.ReadAllText(emailFile);
            this.EmailSetting = Serializer.DeserializeFromXml<Email>(emailXml);
            this.EmailSetting.SecurePassword = Encryption.Decrypt(
                this.EmailSetting.Password,
                _passKey.ToPlainString()
                ).ToSecureString();
            this.EmailSetting.Password = string.Empty;

            string ibdFile = Path.Combine(FileSystem.GetSettingsFolder(), _ibdFilename);
            string ibdXml = File.ReadAllText(ibdFile);
            this.IbdSetting = Serializer.DeserializeFromXml<Ibd>(ibdXml);
            this.IbdSetting.SecurePassword = Encryption.Decrypt(
                this.IbdSetting.Password, 
                _passKey.ToPlainString()
                ).ToSecureString();
            this.IbdSetting.Password = string.Empty;

        }

        public void SetIbdAccount(string username, string password)
        {
            string keyFile = Path.Combine(FileSystem.GetSettingsFolder(), _keyFilename);
            _passKey = File.ReadAllText(keyFile).ToSecureString();
            Ibd value = new Ibd
            {
                Username = username, 
                Password = Encryption.Encrypt(password, _passKey.ToPlainString()),
                MaxRetries = this.IbdSetting.MaxRetries,
                RetryInterval = this.IbdSetting.RetryInterval
            };
            string ibdFile = Path.Combine(FileSystem.GetSettingsFolder(), _ibdFilename);
            File.WriteAllText(ibdFile, Serializer.SerializeToXml<Ibd>(value));
        }

        public void SetEmailAccount(string username, string password)
        {
            string keyFile = Path.Combine(FileSystem.GetSettingsFolder(), _keyFilename);
            _passKey = File.ReadAllText(keyFile).ToSecureString();
            Email value = new Email
            {
                Server = this.EmailSetting.Server,
                Port = this.EmailSetting.Port,
                Username = username,
                Password = Encryption.Encrypt(password, _passKey.ToPlainString()),
                To = this.EmailSetting.To,
                Cc = this.EmailSetting.Cc
            };
            string emailFile = Path.Combine(FileSystem.GetSettingsFolder(), _emailFilename);
            File.WriteAllText(emailFile, Serializer.SerializeToXml<Email>(value));
        }
    }
}
