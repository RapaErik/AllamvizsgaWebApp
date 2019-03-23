using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Utils
{
    public class InitDatabase
    {
      /* public static void Init()
        {
            using (var ctx = new HeatingContext())
            {
                Room r = new Room { Name = "Szoba" };
                Esp e = new Esp { ChargeType = "220", LastCharge = DateTime.Now, LastInteraction = DateTime.Now, InteractionsCounter = 0,AvgInteractions=0,AvgBatteryDuration=DateTime.Now };
                Sensor s = new Sensor { Esp=e, EspId=e.Id, Room=r, RoomId=r.Id, Type="DHT11"};
                List<SensorData> sd = new List<SensorData>
                {
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now },
                    new SensorData { Sensor = s, SensorId = s.Id, Data = "szia", TimeStamp = DateTime.Now }
                };

                ctx.Rooms.Add(r);
                ctx.Esps.Add(e);
                ctx.Sensors.Add(s);
                ctx.SensorDatas.AddRange(sd);

                ctx.SaveChanges();
            }
        }*/
    }
}
