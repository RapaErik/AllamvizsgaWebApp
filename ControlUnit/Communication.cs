using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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
        void SendHttpPostToRestController(string contoller, dynamic data)
        {



            string json = JsonConvert.SerializeObject(data);
            Console.WriteLine(json);
            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/"+ contoller +"/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();



            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        public string SendHttpGetToRestController(string contoller)
        {
            string url = "http://localhost:8080/api/" + contoller ;
            var request = (HttpWebRequest)WebRequest.Create(url);

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        void IncommingHumidityData(string msg)
        {
            var humi = float.Parse(msg, CultureInfo.InvariantCulture.NumberFormat);
            SensorData data = new SensorData { SensorId = 2, Data = humi, TimeStamp = DateTime.Now };

            SendHttpPostToRestController("apisensordata/PostData", data);
        }

        public List<SensorData> DeserializeSensorDataJson(string json)
        {
            JArray jsonArray = JArray.Parse(json);
            return jsonArray.ToObject<List<SensorData>>();
        }


        void IncommingTemperatureData(string msg)
        {
            var temp = float.Parse(msg, CultureInfo.InvariantCulture.NumberFormat);
            SensorData data = new SensorData { SensorId = 1, Data = temp, TimeStamp = DateTime.Now };

           // string controller = "data/{temperature}/{9}";
           // List<SensorData> list = DeserializeSensorDataJson(SendHttpGetToRestController(controller));

          //  list.Add(data);

             

            SendHttpPostToRestController("apisensordata/PostData", data);

        }
        void IncommingDataErrorInTopic(string topic)
        {
            Console.WriteLine("Nan!!!! at topic:" + topic);
        }

        void MqttController(object sender, MqttMsgPublishEventArgs e)
        {

                string msg = Encoding.UTF8.GetString(e.Message);
                if (msg != "NaN")
                {
                    switch (e.Topic)
                    {
                        case "/home/temperature":
                            IncommingTemperatureData(msg);
                            
                            break;

                        case "/home/humidity":
                            IncommingHumidityData(msg);
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
        public void PublishDataToTopic(string topic, dynamic data)
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(Convert.ToString(data)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            return;
        }

        public void PublishHeatSpead(float value)
        {
            var s = new SensorData {SensorId=3,TimeStamp=DateTime.Now,Data=value };
            PublishDataToTopic("/home/heatspeed", value);
            SendHttpPostToRestController("apisensordata/PostData", s);

        }
    }
}

