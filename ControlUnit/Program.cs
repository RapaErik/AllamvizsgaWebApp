using Newtonsoft.Json;
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


            SensorData data = new SensorData {  SensorId = 1, /*TimeStamp = DateTime.Now.ToUniversalTime(), */Data = "szia" };
      
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyy hh:mm:ss";

            string json = JsonConvert.SerializeObject(data, jsonSettings);
            json = json.ToLower();
            Console.WriteLine(json);
            Console.WriteLine("Press any key after asp.net is loaded");
            Console.ReadKey();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:62325/api/apisensordata/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
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
