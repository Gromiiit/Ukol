using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Notino.Homework
{
    public class XMLConverter : IFileConvert
    {
        public XmlSerializer Serializer {get;}
        public XMLConverter()
        {
        }

        public IEnumerable<TModel> Deserialize<TModel>(string source) where TModel : ISerializable // HODINA
        {

            var serializerList = new XmlSerializer(typeof(List<TModel>));
            var serializerObject = new XmlSerializer(typeof(TModel));
            object des = null;
            using (var reader = new StringReader(source))
            using (var xmlReader = XmlReader.Create(reader))
            {
                if (serializerList.CanDeserialize(xmlReader))
                {
                    des = serializerList.Deserialize(xmlReader);
                }
                else if (serializerObject.CanDeserialize(xmlReader))
                {
                    des = serializerObject.Deserialize(xmlReader);
                }
                else if (des == null)
                {
                    throw new Exception();
                }
            }
            return des.GetType() == typeof(TModel) ? new List<TModel> { (TModel)des } : (List<TModel>)des;            
        }

        public IEnumerable<TModel> Deserialize<TModel>(StreamReader reader) where TModel : ISerializable // HODINA
        {
            var serializerList = new XmlSerializer(typeof(List<TModel>));
            var serializerObject = new XmlSerializer(typeof(TModel));
            var rr = XmlReader.Create(reader);
            object des = null;
            if (serializerList.CanDeserialize(rr))
            {
                des = serializerList.Deserialize(rr);
            }
            else if (serializerObject.CanDeserialize(rr))
            {
                des = serializerObject.Deserialize(rr);
            }
            else if(des == null)
            {
                throw new Exception();
            }

            return des.GetType() == typeof(TModel) ? new List<TModel> { (TModel)des } : (List<TModel>)des;
        }

        public string Serialize<TModel>(object source) where TModel : ISerializable
        {
            XmlSerializer serializer = new XmlSerializer(source.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, source);
                return textWriter.ToString();
            }
        }

        public void Serialize<TModel>(object source, StreamWriter writer) where TModel : ISerializable
        {
            XmlSerializer serializer = new XmlSerializer(source.GetType());

            using (writer)
            {
                serializer.Serialize(writer, source);
            }
        }
    }
}
