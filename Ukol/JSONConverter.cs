using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Notino.Homework
{
    public class JSONConverter : IFileConvert
    {
        public JsonSerializer Serializer { get; }
        
        public JSONConverter()
        {
            Serializer = new JsonSerializer();
        }

        public IEnumerable<TModel> Deserialize<TModel> (string source) where TModel : ISerializable
        {
            using (var jsonTextReader = new JsonTextReader(new StringReader(source)))
            {
                var json = (JToken)Serializer.Deserialize(jsonTextReader);
                return json is JObject ? new List<TModel> { json.ToObject<TModel>() } : json.ToObject<List<TModel>>(); // json is JArray
            }
        }

        public IEnumerable<TModel> Deserialize<TModel>(StreamReader reader) where TModel : ISerializable
        {
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                var json = (JToken)Serializer.Deserialize(jsonTextReader);
                return json is JObject ? new List<TModel> { json.ToObject<TModel>() } : json.ToObject<List<TModel>>(); // json is JArray
            }
        }

        public string Serialize<TModel>(object source) where TModel : ISerializable
        {
            return JsonConvert.SerializeObject(source);
        }

        public void Serialize<TModel>(object source, StreamWriter writer) where TModel : ISerializable
        {
            //using (var jsonTextReader = new JsonTextWriter(writer))
            //{
            //    string s = JsonConvert.SerializeObject(source);
            //    writer.Write(s);
            //}
            using (writer)
            {
                Serializer.Serialize(writer, source);
            }
        }
    }
}
