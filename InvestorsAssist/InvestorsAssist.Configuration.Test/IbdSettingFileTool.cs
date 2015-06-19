using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Configuration.Objects;
using InvestorsAssist.Utility.IO;
using InvestorsAssist.Utility.Security;

namespace InvestorsAssist.Configuration.Test
{
    [TestClass]
    public class IbdSettingFileTool
    {
        [TestMethod]
        public void CreateIbdSetting_Test()
        {
            var value = new Ibd
            {
                Username = @"htxx2009@gmail.com",
                Password = Encryption.Encrypt("****", "****"),
            };
            var setting = Serializer.SerializeToXml(value);
        }
    }
}
