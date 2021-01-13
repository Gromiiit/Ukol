using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Notino.Homework
{
    public enum Location
    {
        FileSystem,
        Http,
        Cloud
    }

    static class StreamFactory
    {
        public static Stream GetStream(string path, Location type, bool read = true)
        {
            switch (type)
            {
                case Location.FileSystem:
                    return read ? File.Open(path, FileMode.Open, FileAccess.Read) : File.Open(path, FileMode.Create, FileAccess.Write);
                case Location.Http:
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path); // http://echo.jsontest.com/Title/two/Text/two
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;

                        if (String.IsNullOrWhiteSpace(response.CharacterSet))
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                        //string data = readStream.ReadToEnd();
                        //Console.WriteLine(data);

                        //response.Close();
                        //readStream.Close();
                        if (read)
                            return receiveStream;
                        else
                            throw new NotImplementedException();
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString());
                    }
                case Location.Cloud:
                    return null;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
