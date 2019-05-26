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
          //  (float)control.Control();
            
            


            Console.ReadKey();

        }

    }
}
