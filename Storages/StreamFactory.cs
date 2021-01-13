using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Notino.Homework.Storages.Clouds;

namespace Notino.Homework.Storages
{
    public enum Location
    {
        FileSystem,
        Http,
        Cloud
    }

    public enum CloudService
    {
        Dropbox,
        OneDrive
    }

    public static class StreamFactory
    {
        public static Stream GetStream(string path, Location type, CloudService cloud = CloudService.Dropbox, bool read = true)
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
                    var cloudInstance = GetCloud(cloud);
                    return read ? cloudInstance.Download() : cloudInstance.Upload();
                default:
                    throw new NotImplementedException();
            }
        }

        private static ICloud GetCloud(CloudService type)
        {
            switch (type)
            {
                case CloudService.Dropbox:
                    return new Clouds.Dropbox();
                case CloudService.OneDrive:
                    return new Clouds.OneDrive();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
