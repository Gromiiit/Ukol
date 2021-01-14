using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Notino.Homework.FormatConvertors
{
    public class XMLConvertor : IFileConvert
    {
        public XmlSerializer Serializer {get;}
        public XMLConvertor()
        {
        }

        public IEnumerable<TModel> Deserialize<TModel>(string source) where TModel : ISerializable
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (source == string.Empty)
            {
                throw new ArgumentException("Argument empty string", source);
            }

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
                else if
                {
                    throw new Exception($"Source cannot be deserialized to provided type {typeof(TModel)}");
                }
            }
            return des.GetType() == typeof(TModel) ? new List<TModel> { (TModel)des } : (List<TModel>)des;            
        }

        public IEnumerable<TModel> Deserialize<TModel>(StreamReader reader) where TModel : ISerializable
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

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
            else
            {
                throw new Exception($"Source cannot be deserialized to provided type {typeof(TModel)}");
            }

            return des.GetType() == typeof(TModel) ? new List<TModel> { (TModel)des } : (List<TModel>)des;
        }

        public string Serialize<TModel>(object source) where TModel : ISerializable
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (source is ISerializable)
            {
                throw new SerializationException("Object is not serializable");
            }

            XmlSerializer serializer = new XmlSerializer(source.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, source);
                return textWriter.ToString();
            }
        }

        public void Serialize<TModel>(object source, StreamWriter writer) where TModel : ISerializable
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            writer = writer ?? throw new ArgumentNullException(nameof(writer));

            XmlSerializer serializer = new XmlSerializer(source.GetType());

            using (writer)
            {
                serializer.Serialize(writer, source);
            }
        }
    }
}
