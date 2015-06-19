using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace InvestorsAssist.Utility.IO
{
    public static class Serializer
    {
        public static string SerializeToXml<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here
            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public static string SerializeToJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeFromJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
