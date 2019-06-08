using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Sevices
{
    public class LogService : ILogService
    {
        private readonly HeatingContext _ctx;

        public LogService(HeatingContext ctx)
        {
            _ctx = ctx;
        }



        public Log InsertLog(Log newData)
        {

            _ctx.Logs.Add(newData);
            _ctx.SaveChanges();
            var res = _ctx.Logs.Where(c => c.Id == newData.Id).Include(s => s.Device).FirstOrDefault();
            return res;
        }
        public void ClearLogTable()
        {
            var d = _ctx.Logs.ToList();
            _ctx.Logs.RemoveRange();
            _ctx.SaveChanges();
        }
        public void InitDatabase()
        {
            Room r = new Room { Name = "Szoba" };
            CommunicationUnit e = new CommunicationUnit {Code="espcode", IPAddress="0.0.0.159"};
            Device s = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "temperature" ,Name="DHT11" ,IO=false};
            Device ss = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "humidity", Name="DHT11" ,IO=false };
            Device sss = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "heater", Name = "lampa", IO = true };

            _ctx.Add(r);
            _ctx.Add(e);
            _ctx.Add(s);
            _ctx.Add(ss);
            _ctx.Add(sss);
            _ctx.SaveChanges();

        }


        public IEnumerable<Log> GetLastFiftyLogsExceptHeaters()
        {
            return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type != "heater").OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<Log> GetLastNLogsByDeviceType(string type = "", int? number = null, int? DeviceId = null)
        {
            if (number == null && type == "" && DeviceId == null) // nincs semmi megadva
            {
                return _ctx.Logs.Include(t => t.Device).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number == null && type == "" && DeviceId != null) // csak szenzor van megadva
            {
                return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Id == DeviceId).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number == null && type != "" && DeviceId == null) // csak szenzor tipus  van megadva
            {
                return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type == type).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (number != null && type == "" && DeviceId == null) // csak darabszam  van megadva
            {
                return _ctx.Logs.Include(t => t.Device).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }





            if (DeviceId == null) //megvan adva a tipus es a szam is 
            {
                return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type == type).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }

            if (number == null)//megvan adva a tipus es a az Id is 
            {
                return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Id == DeviceId).Where(w => w.Device.Type == type).OrderByDescending(c => c.TimeStamp).ToList();
            }
            if (type == "") //megvan adva a szam es a  Id is 
            {
                return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Id == DeviceId).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
            }




            //megvan minden parameter adva
            return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Id == DeviceId).Where(w => w.Device.Type == type).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
        }
    }
}
