using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Homework
{
    public interface IFileConvert
    {
        IEnumerable<TModel> Deserialize<TModel>(string source) where TModel : ISerializable; // READ FILE, HTTP, CLOUD
        // DESERIALIZE FROM STREAM
        IEnumerable<TModel> Deserialize<TModel>(StreamReader reader) where TModel : ISerializable; // Read from Stream => Model

        string Serialize<TModel>(object source) where TModel : ISerializable; // CONVERT MODEL => Format
        // SERIALIZE TO WRITE
        void Serialize<TModel>(object source, StreamWriter writer) where TModel : ISerializable; // Model => Write to Stream
    }
}
