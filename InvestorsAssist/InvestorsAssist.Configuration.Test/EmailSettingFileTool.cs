using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Configuration.Objects;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;

namespace InvestorsAssist.Configuration.Test
{
    [TestClass]
    public class EmailSettingFileTool
    {
        [TestMethod]
        public void CreateEmailSetting_Test()
        {
            var value = new Email
            {
                Server = "smtp.gmail.com",
                Port = 587,
                Username = @"htxx2009@gmail.com",
                Password = Encryption.Encrypt("*****", "*****"),
            };
            var setting = Serializer.SerializeToXml(value);
        }
    }
}
