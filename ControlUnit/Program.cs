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

            Communication com = new Communication() ;

            com.ConnectMqtt();
            com.SubscribeToMqttTopic("/home/temperature");
            com.SubscribeToMqttTopic("/home/humidity");




            Thread thr = new Thread(() =>
                {
                    int counter = 0;
                    while (true)
                    {
                        Thread.Sleep(500);
                        com.PublishHeatSpead(counter % 25);

                        counter++;
                    }
                });
        //    thr.Start();
            HeatControl control = new HeatControl();
            var error = 11;
            var dt = -0.65;
            Console.WriteLine("Control:"+  control.Control(error, dt).ToString());




            Console.ReadKey();

        }

    }
}
