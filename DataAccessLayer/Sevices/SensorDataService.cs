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


        public IEnumerable<SensorData> GetSensorDatas()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).ToList();
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

        public IEnumerable<SensorData> GetSensorDatasOfTemperature()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "temperature").ToList();
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatasOfTemperature()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "temperature").OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<SensorData> GetSensorDatasOfHumidity()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "humidity").ToList();
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatasOfHumidity()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "humidity").OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<SensorData> GetSensorDatasExceptHeaters()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type != "heater").ToList();
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatasExceptHeaters()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w=>w.Sensor.Type!="heater").OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<SensorData> GetSensorDatasOfHeater()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "heater").ToList();
        }

        public IEnumerable<SensorData> GetLastFiftySensorDatasOfHeater()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == "heater").OrderByDescending(c => c.Id).Take(50).ToList();
        }
    }
}
