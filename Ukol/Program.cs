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

        public Document() { }
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
        static string sourceFileNameXML = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\StestLists.xml");
        static string targetFileNameJSON = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Output.json");

        // static string sourceFileNameJSON = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\StestList.json");
        // static string sourceFileNameXML = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\StestObject.xml");
        // static string sourceFileNameJSON = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\StestObject.json");
        // static string targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Test.txt");
        // static string targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Test.txt");
        static void Main(string[] args) // sourcePath, targetPath, sourceFormat, targetFormat, location, cloud
        {
            //string[] args2 = { sourceFileNameXML, targetFileNameJSON, "xml", "json", "filesystem", "cloud" }; // example
            string[] args2 = args;
            Location location;
            CloudService cloud;
            Format rFormat, wFormat;

            if (args2.Length < 4)
            {
                Console.WriteLine("Lack of parameters");
                return;
            }
            if (!Enum.TryParse(args2[2], true, out rFormat))
            {
                Console.WriteLine("Incorrect source format");
                return;
            }
            if (!Enum.TryParse(args2[3], true, out wFormat))
            {
                Console.WriteLine("Incorrect target format");
                return;
            }
            if (Enum.TryParse(args2[4], true, out location))
            {
                if (location == Location.Cloud && args2.Length > 2)
                {
                    if (!Enum.TryParse(args2[5], true, out cloud))
                    {
                        Console.WriteLine("Incorrect cloud");
                        return;
                    }
                    return;
                }
            }
            else
            {
                Console.WriteLine("Incorrect location");
                return;
            }

            Stream rStream, wStream;
            try
            {
                rStream = StreamFactory.GetStream(args2[0], location);
                wStream = StreamFactory.GetStream(args2[1], location, read: false);
            }
            catch (ArgumentException argumentEx)
            {
                Console.WriteLine("Incorrect path");
                Console.WriteLine(argumentEx.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            IFileConvert rConvert, wConvert;
            try
            {
                rConvert = ConvertorFactory.GetConvertor(rFormat);
                wConvert = ConvertorFactory.GetConvertor(wFormat);
            }
            catch(NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            object data = rConvert.Deserialize<Document>(new StreamReader(rStream));
            wConvert.Serialize<Document>(data, new StreamWriter(wStream));
        }
    }
}