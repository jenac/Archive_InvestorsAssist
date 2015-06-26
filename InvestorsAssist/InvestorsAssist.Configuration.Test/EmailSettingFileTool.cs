using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Configuration.Objects;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;
using System.Collections.Generic;

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
                To = @"lihe.chen@gmail.com",
                Cc = new List<string> { "10327187@qq.com" },
            };
            var setting = Serializer.SerializeToXml(value);
        }
    }
}
