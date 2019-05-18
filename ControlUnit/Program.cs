using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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

            
            client.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            Thread thr = new Thread(()=>
            {
                int counter = 0;
                while (true)
                {
                    
                    Thread.Sleep(500);
                    client.Publish("/home/heatspeed", Encoding.UTF8.GetBytes((counter % 100).ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    Console.WriteLine("/home/heatspeed"+ "      "+ Encoding.UTF8.GetBytes((counter % 100).ToString()));
                    counter++;
                }
            });
          //  thr.Start();



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
