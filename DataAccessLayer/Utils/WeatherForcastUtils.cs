using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace DataAccessLayer.Utils
{
    public class WeatherForcastUtils
    {/*
        public static void WeatherForcas()
        {


            WebClient client = new WebClient();
            Stream stream = client.OpenRead("http://api.openweathermap.org/data/2.5/forecast?id=665004&appid=d0b0b53a22d32f2c0f32afcdbf36830b");
            StreamReader reader = new StreamReader(stream);
            //Here I had read from the OpenWeather API



            var json = JObject.Parse(reader.ReadLine());
            var weatherList = json["list"].ToList();

            //Here i store Data in Celsius grade
            IDictionary<DateTime, double> InterpolatedDictionaryInCelsius = new Dictionary<DateTime, double>();

            //I need this for calculate from time stamp to readeable Date 
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);


            for (int i = 0; i < weatherList.Count - 1; i++)
            {
                var item0 = weatherList[i];
                var item1 = weatherList[i + 1];

                var timestamp0 = Double.Parse(item0["dt"].ToString());
                var timestamp1 = Double.Parse(item1["dt"].ToString());

                InterpolatedDictionaryInCelsius.Add(time.AddSeconds(timestamp0), Double.Parse(item0["main"]["temp"].ToString()) - 272.15);
                // Console.WriteLine(time.AddSeconds(timestamp0) + "  -- --  "+ item0["main"]["temp"]);

                for (var j = timestamp0; j < timestamp1; j++)
                {
                    j += 59;
                    var result = LinearInterpolation(time.AddSeconds(timestamp0), Double.Parse(item0["main"]["temp"].ToString()), time.AddSeconds(timestamp1), Double.Parse(item1["main"]["temp"].ToString()), time.AddSeconds(j));


                    InterpolatedDictionaryInCelsius.Add(time.AddSeconds(j), result - 272.15);
                    //Console.WriteLine(time.AddSeconds(j)+"  -  "+ result.ToString());
                }
            }

            Thread.Sleep(500);
            using (var ctx= new HeatingContext())
            {
                Room r = new Room { Name = "Furdo" };
                Esp e = new Esp { ChargeType = "220", LastCharge = DateTime.Now, LastInteraction = DateTime.Now, InteractionsCounter = 0, AvgInteractions = 0, AvgBatteryDuration = DateTime.Now };
                Sensor s = new Sensor { Esp = e, EspId = e.Id, Room = r, RoomId = r.Id, Type = "DHT11" };
                List<SensorData> sd = new List<SensorData>
                {
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "Hello", TimeStamp = DateTime.Now }
                };

                ctx.Rooms.Add(r);
                ctx.Esps.Add(e);
                ctx.Sensors.Add(s);
                ctx.SensorDatas.AddRange(sd);

                ctx.SaveChanges();
            }
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(10000);
                using (var ctx = new HeatingContext())
                {

                    var s = (Sensor)from sen in ctx.Sensors
                                    where sen.Id == 2
                                    select sen;

                    List<SensorData> sd = new List<SensorData>
                {
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "Hello", TimeStamp = DateTime.Now }
                };

                    ctx.SensorDatas.AddRange(sd);

                    ctx.SaveChanges();
                }
            }
         

        }
        //ne felejtsd el kicserelni a dokumentacioba a fuggvenyt
        static public double LinearInterpolation(DateTime t0, double v0, DateTime t1, double v1, DateTime target)
        {
            var temp = (target - t0).TotalSeconds / (t1 - t0).TotalSeconds;
            return v0 * (1 - temp) + v1 * temp;
        }*/
    }
}
