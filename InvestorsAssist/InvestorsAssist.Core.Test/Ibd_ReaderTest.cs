using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Core.Ibd;

namespace InvestorsAssist.Core.Test
{
    [TestClass]
    public class Ibd_ReaderTest
    {
        [TestMethod]
        public void Ibd_Reader_Should_Download()
        {
            var reader = new Reader();
            var ret = reader.DownloadIbd50List();
            Assert.IsTrue(ret.Count == 50);
        }
    }
}
