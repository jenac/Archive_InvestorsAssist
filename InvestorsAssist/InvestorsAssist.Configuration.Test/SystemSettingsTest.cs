using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Utility;
namespace InvestorsAssist.Configuration.Test
{
    [TestClass]
    public class SystemSettingsTest
    {
        [TestMethod]
        public void SystemSettingsTest_Should_Initalize()
        {
            var setting = SystemSettings.Instance;
            Assert.IsTrue(setting.IbdSetting != null);
            Assert.IsTrue(setting.EmailSetting != null);
        }
    }
}
