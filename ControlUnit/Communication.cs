using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ControlUnit
{
    public class Communication
    {

        private MqttClient client;
        


        public Communication()
        {
            string mqttBorkerIpAddres = "127.0.0.1";
            client = new MqttClient(mqttBorkerIpAddres);


            
        }
        public void ConnectMqtt()
        {
            client.MqttMsgPublishReceived += MqttController;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
        }

        public void SubscribeToMqttTopic(string topic)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }


        void IncommingTemperatureData(string msg)
        {
            string[] datas = msg.Split(' ');
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
            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:62325/api/apisensordata/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        void IncommingDataErrorInTopic(string topic)
        {
            Console.WriteLine("Nan!!!! at topic:"+topic);
        }
        void MqttController(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);
            if(msg!="NaN")
            {
                switch (e.Topic)
                {
                    case "/home/temperature":
                        IncommingTemperatureData(msg);
                        break;

                    case "/home/humidity":
                        break;

                    case "/home/heatspead":
                        break;
                }
            }
            else
            {
                IncommingDataErrorInTopic(e.Topic);
            }
            
        }


    }
}

