using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notino.Homework.FormatConvertors;
using System.Collections.Generic;
using System.Linq;

namespace Notino.Homework.ConvertorsTest
{
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

            Assert.AreEqual(seriallized.ToLower().Replace(" ", ""), deseriallized.ToLower().Replace(" ", ""));
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

            Assert.AreEqual(seriallized.ToLower().Replace(" ", ""), deseriallized.ToLower().Replace(" ", ""));
        }
    }
}
