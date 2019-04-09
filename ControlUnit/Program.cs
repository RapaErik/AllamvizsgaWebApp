using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ControlUnit
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new MqttClient(IPAddress.Parse("127.0.0.1"));//brocker IP addres

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2
            client.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            /*for (int i = 0; i < 50; i++)
            {

                temp.TimeStamp = DateTime.Now;
                string strValue = "5.6";
                // publish a message on "/home/temperature" topic with QoS 2 
                client.Publish("/home/temperature", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                System.Threading.Thread.Sleep(500);
            }*/









            //WeatherForcast v = new WeatherForcast();
            //var forcast = v.GetWeatherForcast();

            //SensorData data = new SensorData {  SensorId = 1, /*TimeStamp = DateTime.Now.ToUniversalTime(), */Temperature = 15f, Humidity=33.5f};

            //var jsonSettings = new JsonSerializerSettings();
            //jsonSettings.DateFormatString = "dd/MM/yyy hh:mm:ss";

            //string json = JsonConvert.SerializeObject(data, jsonSettings);
            //json = json.ToLower();
            //Console.WriteLine(json);
            //Console.WriteLine("Press any key after asp.net is loaded");
            //Console.ReadKey();

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:62325/api/apisensordata/");
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    Console.WriteLine(result);
            //}


            Console.ReadKey();
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            
          
            if (Encoding.UTF8.GetString(e.Message) != "NaN")
            {
                string[] datas = Encoding.UTF8.GetString(e.Message).Split(' ');
                foreach (var item in datas)
                {
                    Console.WriteLine(item);
                }
                var temp = float.Parse(datas[0], CultureInfo.InvariantCulture.NumberFormat);
                var humi = float.Parse(datas[1], CultureInfo.InvariantCulture.NumberFormat);
                SensorData data = new SensorData { SensorId = 1, Temperature = temp, Humidity = humi };

                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd/MM/yyy hh:mm:ss";

                string json = JsonConvert.SerializeObject(data, jsonSettings);
                Console.WriteLine(json);
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
            }
            else
            {
                Console.WriteLine("Nan!!!!");
            }


        }
    }
}
