using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestorsAssist.Utility.IO;

namespace InvestorsAssist.Utility.Test
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void Serializer_SerializeToJson_should_serialize_dynamic()
        {
            string json = @"{""d"":""SOK""}";
            dynamic d = Serializer.DeserializeFromJson<dynamic>(json);
            Assert.IsTrue(d.d.ToString() == "SOK");
        }
    }
}
