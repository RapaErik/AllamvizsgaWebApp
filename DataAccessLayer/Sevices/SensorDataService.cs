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



        public SensorData InsertSensorData(SensorData newData)
        {

            _ctx.SensorDatas.Add(newData);
            _ctx.SaveChanges();
            var res = _ctx.SensorDatas.Where(c => c.Id == newData.Id).Include(s => s.Sensor).FirstOrDefault();
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


        public IEnumerable<SensorData> GetLastFiftySensorDatasExceptHeaters()
        {
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type != "heater").OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<SensorData> GetLastNSensorDatasBySensorType(string type = "", int? number = null, int? sensorId = null)
        {
            if (number == null && type == "" && sensorId == null) // nincs semmi megadva
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number == null && type == "" && sensorId != null) // csak szenzor van megadva
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Id == sensorId).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number == null && type != "" && sensorId == null) // csak szenzor tipus  van megadva
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == type).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number != null && type == "" && sensorId == null) // csak darabszam  van megadva
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }





            if (sensorId == null) //megvan adva a tipus es a szam is 
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Type == type).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }

            if (number == null)//megvan adva a tipus es a az Id is 
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Id == sensorId).Where(w => w.Sensor.Type == type).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (type == "") //megvan adva a szam es a  Id is 
            {
                return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Id == sensorId).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }




            //megvan minden parameter adva
            return _ctx.SensorDatas.Include(t => t.Sensor).Where(w => w.Sensor.Id == sensorId).Where(w => w.Sensor.Type == type).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
        }
    }
}
