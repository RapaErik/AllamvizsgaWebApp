﻿using ControlUnit.Dtos;
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
        string SendHttpPostToRestController(string contoller, dynamic data)
        {



            string json = JsonConvert.SerializeObject(data);
            Console.WriteLine(json);
            HttpWebRequest httpWebRequest;
            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8080/api/" + contoller);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();


            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        public string SendHttpGetToRestController(string contoller)
        {
            string url = "http://localhost:8080/api/" + contoller;
            var request = (HttpWebRequest)WebRequest.Create(url);

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        void IncommingHumidityData(string msg)
        {
            var humi = float.Parse(msg, CultureInfo.InvariantCulture.NumberFormat);
            Log data = new Log { DeviceId = 2, Data = humi, TimeStamp = DateTime.Now };

            SendHttpPostToRestController("apiLog/PostData", data);
        }
        public CommunicationUnit DeserializeLogCommunicationUnit(string json)
        {
            JObject j = JObject.Parse(json);
            return j.ToObject<CommunicationUnit>();
        }

        public List<Log> DeserializeLogJson(string json)
        {
            JArray jsonArray = JArray.Parse(json);
            return jsonArray.ToObject<List<Log>>();
        }
        public List<Device> DeserializeDeviceJson(string json)
        {
            JArray jsonArray = JArray.Parse(json);
            return jsonArray.ToObject<List<Device>>();
        }
        public Device DeserializeDevice(string json)
        {
            JObject j = JObject.Parse(json);
            return j.ToObject<Device>();
        }
        public List<Room> DeserializeRoomJson(string json)
        {
            JArray jsonArray = JArray.Parse(json);
            return jsonArray.ToObject<List<Room>>();
        }


        void IncommingData(string chipId, string topic, string msg)
        {
            try
            {
                var temp = float.Parse(msg, CultureInfo.InvariantCulture.NumberFormat);
                Log data = new Log { DeviceId = 1, Data = temp, TimeStamp = DateTime.Now };


                string json = SendHttpGetToRestController("apiLog/GetCommunicationUnit?chipId=" + chipId);
                CommunicationUnit c = DeserializeLogCommunicationUnit(json);

                json = SendHttpGetToRestController("apiLog/GetDevice?comunitid=" + c.Id.ToString() + "&type=" + topic);
                Device device = DeserializeDevice(json);
                data.DeviceId = device.Id;
                // string controller = "data/{temperature}/{9}";
                // List<Log> list = DeserializeLogJson(SendHttpGetToRestController(controller));

                //  list.Add(data);



                SendHttpPostToRestController("apiLog/PostData", data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
                if (e.Topic == "/toserver/init/")
                {
                    Console.WriteLine("incomming communication unit with mychipId:  " + msg);
                    IncommingNewCommunicationUnit(msg);
                }
                else
                {
                    var topic = e.Topic.Split('/');
                    switch (topic[1])
                    {
                        case "temperature":
                            IncommingData(topic[0], topic[1], msg);
                            break;

                        case "humidity":
                            IncommingData(topic[0], topic[1], msg);
                            break;
                        case "myIp":
                            AddIpAddress(topic[0], msg);
                            break;
                        case "devices":
                            AddNewDeviceToCommunicationUnit(topic[0], msg);
                            break;
                        default:
                            Console.WriteLine("--------------" + msg);
                            break;
                    }


                }

            }
            else
            {
                IncommingDataErrorInTopic(e.Topic);
            }

        }
        private void AddNewDeviceToCommunicationUnit(string chipId, string msg)
        {
            var device = msg.Split('/');

            string type = device[0];
            string name = device[1];
            bool io = bool.Parse(device[2]);



            string json = SendHttpGetToRestController("apiLog/GetCommunicationUnit?chipId=" + chipId);
            CommunicationUnit c = DeserializeLogCommunicationUnit(json);
            Device d = new Device { Name = name, IO = io, Type = type, CommunicationUnitId = c.Id };

            SendHttpPostToRestController("apiLog/InitDevice/ ", d);
            
            if(type=="temperature"|| type == "humidity")
            {
                SubscribeToMqttTopic(chipId + "/" + type);
                PublishDataToTopic(chipId + "/start", true);
            }
           
        }
        private void AddIpAddress(string chipId, string ipAddress)
        {
            string json = SendHttpGetToRestController("apiLog/GetCommunicationUnit?chipId=" + chipId);
            CommunicationUnit c = DeserializeLogCommunicationUnit(json);
            c.IPAddress = ipAddress;
            SendHttpPostToRestController("apiLog/InitNewCommunicationUnitAddIpAddress/ ", c);


        }
        private void IncommingNewCommunicationUnit(string data)
        {
            SendHttpPostToRestController("apiLog/InitNewCommunicationUnit/", data);
            SubscribeToMqttTopic(data + "/myIp");
            SubscribeToMqttTopic(data + "/devices");
        }

        public void PublishDataToTopic(string topic, dynamic data)
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(Convert.ToString(data)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            return;
        }

        public void PublishHeatSpeed(float value, int deviceId, string code)
        {
            var s = new Log { DeviceId = deviceId, TimeStamp = DateTime.Now, Data = value };
        //    PublishDataToTopic("/home/heatspeed", value);

            PublishDataToTopic(code + "/heatspeed", value);
            SendHttpPostToRestController("apiLog/PostData", s);

        }

        public void PublishCoolSpeed(float value, int deviceId, string code)
        {

            var s = new Log { DeviceId = deviceId, TimeStamp = DateTime.Now, Data = value };
   
                
             //   PublishDataToTopic("/home/coolspeed", value);
           
                

                PublishDataToTopic(code + "/coolspeed", value);
                SendHttpPostToRestController("apiLog/PostData", s);
            
        }
    }
}

