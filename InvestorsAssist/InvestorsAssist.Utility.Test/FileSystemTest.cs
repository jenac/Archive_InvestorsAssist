using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Utility.IO;

namespace InvestorsAssist.Utility.Test
{
    [TestClass]
    public class FileSystemTest
    {
        [TestMethod]
        public void FileSystem_GetSettingsFolder_Should_ReturnCorrectPath()
        {
            var path = FileSystem.GetSettingsFolder();
        }
    }
}
