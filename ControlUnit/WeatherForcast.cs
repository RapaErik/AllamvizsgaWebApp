using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ControlUnit
{
    public class WeatherForcast
    {
        public IDictionary<DateTime, double> GetWeatherForcast()
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


            foreach (var item in InterpolatedDictionaryInCelsius)
            {
                Console.WriteLine(item.Key.ToString() + "  :  " + item.Value.ToString());
            }



            return InterpolatedDictionaryInCelsius;
        }
        //ne felejtsd el kicserelni a dokumentacioba a fuggvenyt
        private double LinearInterpolation(DateTime t0, double v0, DateTime t1, double v1, DateTime target)
        {
            var temp = (target - t0).TotalSeconds / (t1 - t0).TotalSeconds;
            return v0 * (1 - temp) + v1 * temp;
        }
    }
}
