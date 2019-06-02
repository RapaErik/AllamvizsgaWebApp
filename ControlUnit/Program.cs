using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            Communication com = new Communication();

            com.ConnectMqtt();
            com.SubscribeToMqttTopic("/home/temperature");
            com.SubscribeToMqttTopic("/home/humidity");

            HeatControl control ;


            Thread thr = new Thread(() =>
                {
                    string json;
                    double error;
                    double dt;
                    double setpoint = 30;
                    control = new HeatControl();
                    while (true)
                    {

                        
                        Thread.Sleep(5000);
                       
                        json = com.SendHttpGetToRestController("apisensordata/GetLastNSensorDatasBySensorType?number=2&sensorId=1");
                        List<SensorData> data= com.DeserializeSensorDataJson(json);
                        error = setpoint - data[0].Data;
                        dt = data[0].Data - data[1].Data;
                        

                        double heatspeed = control.Control(error, dt);
                        Console.WriteLine("Control Error:" + error.ToString() + " Derivate:" + dt.ToString() + " Fuzzy:" + heatspeed.ToString());
                         com.PublishHeatSpead((float)heatspeed);

                    }
                });
            thr.Start();






            Console.ReadKey();

        }

    }
}
