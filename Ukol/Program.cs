using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Notino.Homework.FormatConvertors;
using Notino.Homework.Storages;

namespace Notino.Homework
{
    [Serializable]
    public class Document : ISerializable
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public Document()
        {

        }
        public Document(SerializationInfo info, StreamingContext context)
        {
            Title = (string)info.GetValue("title", typeof(string));
            Text = (string)info.GetValue("text", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("title", Title, typeof(string));
            info.AddValue("text", Text, typeof(string));
        }

        public override string ToString()
        {
            return $"{Title}: {Text}";
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            var doc = new Document
            {
                Text = "text0",
                Title = "title0"
            };
            var doc1 = new Document
            {
                Text = "text1",
                Title = "title1"
            };

            var list = new List<Document>();
            list.Add(doc);
            list.Add(doc1);

            var targetFileNameJ = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Ttest.json");
            var targetFileNameX = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Ttest.xml");
            var sourceFileNameJ = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Stest.json");
            var sourceFileNameX = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Stest.xml");
            var sourceHttpNameJ = @"http://echo.jsontest.com/Title/two/Text/two";

            var JSONConv = ConvertorFactory.GetConverter(Format.JSON);
            var XMLConv = ConvertorFactory.GetConverter(Format.XML);

            var streamR = StreamFactory.GetStream(sourceFileNameX, Location.FileSystem);
            var streamW = StreamFactory.GetStream(targetFileNameX, Location.FileSystem, false);

            //var vals = JSONConv.Deserialize<Document>(new StreamReader(streamR));
            //foreach(var v in vals)
            //{
            //    Console.WriteLine(v);
            //}

            //JSONConv.Serialize<Document>(doc, new StreamWriter(streamW));
            //string s = JSONConv.Serialize<Document>(doc);

            //var vals2 = JSONConv.Deserialize<Document>(s);
            //foreach (var v in vals)
            //{
            //    Console.WriteLine("!" + v);
            //}

            var vals = XMLConv.Deserialize<Document>(new StreamReader(streamR));
            foreach(var v in vals)
            {
                Console.WriteLine(v);
            }
            
            XMLConv.Serialize<Document>(list, new StreamWriter(streamW));
            var serialized = XMLConv.Serialize<Document>(list);
            var obj = XMLConv.Serialize<Document>(doc);
            Console.WriteLine(obj);

            var des = XMLConv.Deserialize<Document>(obj);
            foreach(var d in des)
            {
                Console.WriteLine(d);
            }
            var desList = XMLConv.Deserialize<Document>(serialized);
            foreach (var d in desList)
            {
                Console.WriteLine(d);
            }
        }
    }
}