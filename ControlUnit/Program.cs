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


            
            /*
            Thread thr = new Thread(() =>
            {
                int counter = 0;
                while (true)
                {

                    Thread.Sleep(500);
                    client.Publish("/home/heatspeed", Encoding.UTF8.GetBytes((counter % 100).ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    Console.WriteLine("/home/heatspeed" + "      " + Encoding.UTF8.GetBytes((counter % 100).ToString()));
                    counter++;
                }
            });
            // thr.Start();
            */


            Console.ReadKey();

        }

    }
}
