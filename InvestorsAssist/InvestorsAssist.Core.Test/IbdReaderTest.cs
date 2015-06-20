using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InvestorsAssist.Core.Test
{
    [TestClass]
    public class IbdReaderTest
    {
        [TestMethod]
        public void IbdReader_Should_Download()
        {
            var reader = new IbdReader();
            var ret = reader.DownloadIbd50List();
            Assert.IsTrue(ret.Count == 50);
        }
    }
}
