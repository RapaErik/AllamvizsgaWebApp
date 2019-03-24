using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace ControlUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Press any key after asp.net is loaded");
            Console.ReadKey();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:62325/api/apisensordata/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"sensorid\":1," +
                              "\"data\":\"szia Erik\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
           

            Console.ReadKey();
        }
    }
}
