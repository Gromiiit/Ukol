using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Notino.Homework.FormatConvertors
{
    public class JSONConvertor : IFileConvert
    {
        public JsonSerializer Serializer { get; }
        
        public JSONConvertor()
        {
            Serializer = new JsonSerializer();
        }

        public IEnumerable<TModel> Deserialize<TModel> (string source) where TModel : ISerializable
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (source == string.Empty)
            {
                throw new ArgumentException("Argument empty string", source);
            }

            using (var jsonTextReader = new JsonTextReader(new StringReader(source)))
            {
                var json = (JToken)Serializer.Deserialize(jsonTextReader);
                return json is JObject ? new List<TModel> { json.ToObject<TModel>() } : json.ToObject<List<TModel>>(); 
            }
        }

        public IEnumerable<TModel> Deserialize<TModel>(StreamReader reader) where TModel : ISerializable
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            using (var jsonTextReader = new JsonTextReader(reader))
            {
                var json = (JToken)Serializer.Deserialize(jsonTextReader);
                return json is JObject ? new List<TModel> { json.ToObject<TModel>() } : json.ToObject<List<TModel>>();
            }
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

            return JsonConvert.SerializeObject(source);
        }

        public void Serialize<TModel>(object source, StreamWriter writer) where TModel : ISerializable
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            writer = writer ?? throw new ArgumentNullException(nameof(writer));

            using (writer)
            {
                Serializer.Serialize(writer, source);
            }
        }
    }
}
