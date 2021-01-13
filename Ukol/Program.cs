﻿using System;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;


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
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document11.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            try
            {
                FileStream sourceStream = File.Open(sourceFileName, FileMode.Open);
                var reader = new StreamReader(sourceStream); // encoding of sourceFile is always represented as UTF-8 (sourceFile may be in UTF32, ANSI, etc) => possible loss of data
                string input = reader.ReadToEnd(); // possible out of memory exception on large files
                // streams are not closed => can cause multiple issues
            }
            catch (Exception ex) // general exception handling
            {
                throw new Exception(ex.Message);
            }

            var xdoc = XDocument.Parse(input); // input is unknown in this context (Compile-time error), possible exception on wrong format of xml file
            var doc = new Document
            {
                Title = xdoc.Root.Element("title").Value, // possible null reference exception if element is not found
                Text = xdoc.Root.Element("text").Value // possible null reference exception if element is not found
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write); // possible directory not found exception, unauthorize access exception, possible override of already existing file (bug or feature?)
            var sw = new StreamWriter(targetStream);
            sw.Write(serializedDoc);
            // streams are not closed => can cause multiple issues
        }
    }
}
}