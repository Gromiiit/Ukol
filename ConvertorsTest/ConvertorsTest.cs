using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notino.Homework.FormatConvertors;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Notino.Homework.ConvertorsTest
{
    public static class StringMod
    {
        public static string DeleteControls(this string input)
        {
            return new string(input.Where(c => !char.IsControl(c) && !char.IsWhiteSpace(c)).ToArray());
        }
    }
    [TestClass]
    public class ConvertorsTest
    {
        

        [TestMethod]
        public void FactoryJsonConvertor()
        {
            var convertor = ConvertorFactory.GetConvertor(Format.JSON);
            Assert.IsInstanceOfType(convertor, typeof(JSONConvertor));
        }

        [TestMethod]
        public void FactoryXmlConvertor()
        {
            var convertor = ConvertorFactory.GetConvertor(Format.XML);
            Assert.IsInstanceOfType(convertor, typeof(XMLConvertor));
        }

        [TestMethod]
        public void JsonDeserializeObject()
        {
            var seriallized = "{\"title\":\"title\",\"text\":\"text\"}";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new JSONConvertor();
            var deseriallized = convertor.Deserialize<Document>(seriallized).ToList().ElementAt(0);
            
            Assert.AreEqual(document.Title, deseriallized.Title);
            Assert.AreEqual(document.Text, deseriallized.Text);
        }

        [TestMethod]
        public void JsonDeserializeList()
        {
            var seriallized = "[{\"title\":\"title0\",\"text\":\"text0\"}, {\"title\":\"title1\",\"text\":\"text1\"}]";
            var document0 = new Document()
            {
                Title = "title0",
                Text = "text0"
            };
            var document1 = new Document()
            {
                Title = "title1",
                Text = "text1"
            };
            var documentList = new List<Document>(){ document0, document1 };

            var convertor = new JSONConvertor();
            var deseriallized = convertor.Deserialize<Document>(seriallized).ToList();

            for(int i = 0; i < documentList.Count; i++)
            {
                Assert.AreEqual(documentList.ElementAt(i).Title, deseriallized.ElementAt(i).Title);
                Assert.AreEqual(documentList.ElementAt(i).Text, deseriallized.ElementAt(i).Text);
            }
            Assert.AreEqual(documentList.Count, deseriallized.Count);
        }

        [TestMethod]
        public void JsonSerializeObject()
        {
            var seriallized = "{\"title\":\"title\",\"text\":\"text\"}";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new JSONConvertor();
            var deseriallized = convertor.Serialize<Document>(document);

            Assert.AreEqual(seriallized.ToLower().DeleteControls(), deseriallized.ToLower().DeleteControls());
        }

        [TestMethod]
        public void JsonSerializeList()
        {
            var seriallized = "[{\"title\":\"title0\",\"text\":\"text0\"},{\"title\":\"title1\",\"text\":\"text1\"}]";
            var document0 = new Document()
            {
                Title = "title0",
                Text = "text0"
            };
            var document1 = new Document()
            {
                Title = "title1",
                Text = "text1"
            };
            var documentList = new List<Document>() { document0, document1 };

            var convertor = new JSONConvertor();
            var deseriallized = convertor.Serialize<Document>(documentList);

            Assert.AreEqual(seriallized.ToLower().DeleteControls(), deseriallized.ToLower().DeleteControls());
        }

        [TestMethod]
        public void JsonDeserializeFromStream()
        {
            var seriallized = "{\"title\":\"title\",\"text\":\"text\"}";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new JSONConvertor();

            using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(seriallized)))
            {
                
                var deseriallized = convertor.Deserialize<Document>(new StreamReader(memStream)).ToList().ElementAt(0);

                Assert.AreEqual(document.Title, deseriallized.Title);
                Assert.AreEqual(document.Text, deseriallized.Text);
            }
        }

        [TestMethod]
        public void JsonSerializeToStream()
        {
            var seriallized = "{\"title\":\"title\",\"text\":\"text\"}";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new JSONConvertor();

            using (var memStream = new MemoryStream())
            {
                convertor.Serialize<Document>(document, new StreamWriter(memStream));
                string actual = Encoding.UTF8.GetString(memStream.ToArray());
                Assert.AreEqual(seriallized.ToLower().DeleteControls(), actual.ToLower().DeleteControls());
            }
        }

        [TestMethod]
        public void JsonSerializeNotSerializableException()
        {
            var anon = new { Text = "title", Title = "text" };
            var convertor = new JSONConvertor();
            
            Assert.ThrowsException<SerializationException>(() => convertor.Serialize<Document>(anon));
        }

        /////////////////
        [TestMethod]
        public void XmlDeserializeObject()
        {
            var seriallized = "<Document><Title>title</Title><Text>text</Text></Document>";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new XMLConvertor();
            var deseriallized = convertor.Deserialize<Document>(seriallized).ToList().ElementAt(0);

            Assert.AreEqual(document.Title, deseriallized.Title);
            Assert.AreEqual(document.Text, deseriallized.Text);
        }

        [TestMethod]
        public void XmlDeserializeList()
        {

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<ArrayOfDocument>");
            stringBuilder.Append(@"<Document><Title>title0</Title><Text>text0</Text></Document>");
            stringBuilder.Append(@"<Document><Title>title1</Title><Text>text1</Text></Document>");
            stringBuilder.Append(@"</ArrayOfDocument>");

            var seriallized = stringBuilder.ToString();
            var document0 = new Document()
            {
                Title = "title0",
                Text = "text0"
            };
            var document1 = new Document()
            {
                Title = "title1",
                Text = "text1"
            };
            var documentList = new List<Document>() { document0, document1 };

            var convertor = new XMLConvertor();
            var deseriallized = convertor.Deserialize<Document>(seriallized).ToList();

            for (int i = 0; i < documentList.Count; i++)
            {
                Assert.AreEqual(documentList.ElementAt(i).Title, deseriallized.ElementAt(i).Title);
                Assert.AreEqual(documentList.ElementAt(i).Text, deseriallized.ElementAt(i).Text);
            }
            Assert.AreEqual(documentList.Count, deseriallized.Count);
        }

        [TestMethod]
        public void XmlSerializeObject()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<?xml version=""1.0"" encoding=""utf-16""?>");
            stringBuilder.Append(@"<Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
            stringBuilder.Append(@"<Title>title</Title><Text>text</Text>");
            stringBuilder.Append(@"</Document>");

            var seriallized = stringBuilder.ToString();
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new XMLConvertor();
            var deseriallized = convertor.Serialize<Document>(document);

            Assert.AreEqual(seriallized.ToLower().DeleteControls(), deseriallized.ToLower().DeleteControls());
        }

        [TestMethod]
        public void XmlSerializeList()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<?xml version=""1.0"" encoding=""utf-16""?>");
            stringBuilder.Append(@"<ArrayOfDocument xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
            stringBuilder.Append(@"<Document><Title>title0</Title><Text>text0</Text></Document>");
            stringBuilder.Append(@"<Document><Title>title1</Title><Text>text1</Text></Document>");
            stringBuilder.Append(@"</ArrayOfDocument>");

            var seriallized = stringBuilder.ToString();


            var document0 = new Document()
            {
                Title = "title0",
                Text = "text0"
            };
            var document1 = new Document()
            {
                Title = "title1",
                Text = "text1"
            };
            var documentList = new List<Document>() { document0, document1 };

            var convertor = new XMLConvertor();
            var deseriallized = convertor.Serialize<Document>(documentList);

            Assert.AreEqual(seriallized.ToLower().DeleteControls(), deseriallized.ToLower().DeleteControls());
        }

        [TestMethod]
        public void XmlDeserializeFromStream()
        {
            var seriallized = "<Document><Title>title</Title><Text>text</Text></Document>";
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new XMLConvertor();

            using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(seriallized)))
            {

                var deseriallized = convertor.Deserialize<Document>(new StreamReader(memStream)).ToList().ElementAt(0);

                Assert.AreEqual(document.Title, deseriallized.Title);
                Assert.AreEqual(document.Text, deseriallized.Text);
            }
        }

        [TestMethod]
        public void XmlSerializeToStream()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            stringBuilder.Append(@"<Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
            stringBuilder.Append(@"<Title>title</Title><Text>text</Text>");
            stringBuilder.Append(@"</Document>");

            var seriallized = stringBuilder.ToString();
            var document = new Document()
            {
                Title = "title",
                Text = "text"
            };
            var convertor = new XMLConvertor();

            using (var memStream = new MemoryStream())
            {
                convertor.Serialize<Document>(document, new StreamWriter(memStream));
                string actual = Encoding.UTF8.GetString(memStream.ToArray());
                System.Console.WriteLine(actual.DeleteControls());
                System.Console.WriteLine(seriallized.DeleteControls());
                Assert.AreEqual(seriallized.ToLower().DeleteControls(), actual.ToLower().DeleteControls());
            }
        }

        [TestMethod]
        public void XmlSerializeNotSerializableException()
        {
            var anon = new { Text = "title", Title = "text" };
            var convertor = new XMLConvertor();

            Assert.ThrowsException<SerializationException>(() => convertor.Serialize<Document>(anon));
        }
    }
}
