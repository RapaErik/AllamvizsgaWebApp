using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Sevices
{
    public class SensorDataService : ISensorDataService
    {
        private readonly HeatingContext _ctx; 

        public SensorDataService(HeatingContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatas()
        {
            return _ctx.SensorDatas.Include(t=>t.Sensor).OrderByDescending(c=>c.Id).Take(50).ToList();
        }
        public string GetSensorDatasTypeOfId(int id)
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Select(c=>c.Sensor.Type).FirstOrDefault();
            
        }

        public IEnumerable<SensorData> GetSensorDatas()
        {
            return _ctx.SensorDatas.ToList();
        }

        public SensorData InsertSensorData(SensorData newData)
        {
            
            _ctx.SensorDatas.Add(newData);
            _ctx.SaveChanges();
            var res = _ctx.SensorDatas.Where(c => c.Id == newData.Id).Include(s => s.Sensor).FirstOrDefault() ;
            return res;
        }
        public void ClearSensorDataTable()
        {
            var d = _ctx.SensorDatas.ToList();
            _ctx.SensorDatas.RemoveRange();
            _ctx.SaveChanges();
        }
        public void InitDatabase()
        {
            Room r = new Room { Name = "Szoba" };
            Esp e = new Esp { ChargeType = "220V", LastCharge = DateTime.Now, LastInteraction = DateTime.Now, InteractionsCounter = 0, AvgInteractions = 0, AvgBatteryDuration = DateTime.Now };
            Sensor s = new Sensor { Esp = e, EspId = e.Id, Room = r, RoomId = r.Id, Type = "temperature" };
            Sensor ss = new Sensor { Esp = e, EspId = e.Id, Room = r, RoomId = r.Id, Type = "humidity" };

            _ctx.Add(r);
            _ctx.Add(e);
            _ctx.Add(s);
            _ctx.Add(ss);
            _ctx.SaveChanges();

        }
    }
}
