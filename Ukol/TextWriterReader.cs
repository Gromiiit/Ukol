using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Notino.Homework
{
    public class TextWriterReader
    {
        public void Write(StreamReader writer)
        {

        }

        public IEnumerable<string> Read(StreamReader reader)
        {

            using (var r = new JsonTextReader(reader))
            {
                r.SupportMultipleContent = true;

                var serializer = new JsonSerializer();
                while (r.Read())
                {
                    if (r.TokenType == JsonToken.StartObject)
                    {
                        yield return serializer.Deserialize(r).ToString();
                    }
                }
            }
        }
    }
}
