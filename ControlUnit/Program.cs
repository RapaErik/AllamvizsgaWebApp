﻿using ControlUnit.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ControlUnit
{
    class Program
    {
        private static void ControlRoom(object room)
        {
            Communication com = new Communication();
            com.ConnectMqtt();
            string json;
            double error;
            double dt;
            double setpoint;
            HeatControl control = new HeatControl();

            Room item = room as Room;
            json = com.SendHttpGetToRestController("apiLog/GetListOfDevices?roomid=" + item.Id.ToString());
            List<Device> dataDevices = com.DeserializeDeviceJson(json);
            List<Device> outputDevices = dataDevices.Where(w => w.IO == false).ToList();
            List<Device> inputDevices = dataDevices.Where(w => w.IO == true).ToList();
            var heater = inputDevices.Where(w => w.Type == "heater").FirstOrDefault();
            var cooler = inputDevices.Where(w => w.Type == "cooler").FirstOrDefault();
            var temperatureDevice = outputDevices.Where(w => w.Type == "temperature").FirstOrDefault();
            if (heater == null && cooler == null)
            {
                return;
            }
            if (temperatureDevice == null)
            {
                return;
            }
            json = com.SendHttpGetToRestController("apiLog/GetLastNLogsByDeviceType?number=2&DeviceId=" + temperatureDevice.Id);
            List<Log> dataLog = com.DeserializeLogJson(json);
            if (dataLog.Count < 2)
            {
                return;
            }
            if (DateTime.Now.Hour > 22 || DateTime.Now.Hour < 7) //Esti ido kell
            {
                setpoint = item.NightlySetpoint;
            }
            else
            {
                setpoint = item.DailySetpoint;
            }

            error = setpoint - dataLog[0].Data;
            dt = dataLog[1].Data - dataLog[0].Data;
            double heatspeed = control.Control(error, dt);
            double coolspeed=0;

            if (!item.CoolingEnable)
            {
                heatspeed = heatspeed < 0 ? 0 : heatspeed;
            }
            if (!item.HeatingEnable)
            {
                heatspeed = heatspeed > 0 ? 0 : heatspeed;
            }
            
            if(heatspeed<=0)
            {
                coolspeed = heatspeed*(-1);
            }
            if(heatspeed>0)
            {
                coolspeed = 0;
            }

            Console.WriteLine("Control Error:" + error.ToString() + " Derivate:" + dt.ToString() + " Fuzzy:" + heatspeed.ToString());
            if (heater != null)
            {
                com.PublishHeatSpeed((float)heatspeed, heater.Id, heater.CommunicationUnit.Code);
            }
            if (cooler != null)
            {
                com.PublishCoolSpeed((float)coolspeed, cooler.Id, cooler.CommunicationUnit.Code);
               
            }


        }



        static void Main(string[] args)
        {

            Communication com = new Communication();
            com.ConnectMqtt();
            com.SubscribeToMqttTopic("/home/temperature");
            com.SubscribeToMqttTopic("/home/humidity");
            com.SubscribeToMqttTopic("/toserver/init/");
            string json;
        //    com.PublishHeatSpeed(50,70, "963486");
            while (true)
            {
                Thread.Sleep(5000);


                json = com.SendHttpGetToRestController("apiLog/GetListOfRooms");
                List<Room> dataRooms = com.DeserializeRoomJson(json);

                foreach (var item in dataRooms)
                {

                    ThreadPool.QueueUserWorkItem(ControlRoom, item);

                }

            }
            
        }



    }
}

