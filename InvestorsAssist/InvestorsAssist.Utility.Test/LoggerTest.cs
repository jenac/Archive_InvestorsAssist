using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Utility.IO;
using System.IO;
namespace InvestorsAssist.Utility.Test
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void Logger_Should_Log()
        {
            string logFile =
                Path.Combine(FileSystem.GetLogsFolder(), "LoggerTest.log");
            if (File.Exists(logFile)) File.Delete(logFile);
            Logger.Instance.Info("Test Logger. ");
            Assert.IsTrue(File.Exists(logFile));
        }
    }
}
