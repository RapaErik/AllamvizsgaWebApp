using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.IServices;
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
            CommunicationUnit e = new CommunicationUnit { Code = "espcode", IPAddress = "0.0.0.159" };
            Device s = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "temperature", Name = "DHT11", IO = false };
            Device ss = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "humidity", Name = "DHT11", IO = false };
            Device sss = new Device { CommunicationUnit = e, CommunicationUnitId = e.Id, Room = r, RoomId = r.Id, Type = "heater", Name = "lampa", IO = true };

            _ctx.Add(r);
            _ctx.Add(e);
            _ctx.Add(s);
            _ctx.Add(ss);
            _ctx.Add(sss);
            _ctx.SaveChanges();

        }


        public IEnumerable<Log> GetLastFiftyLogsExceptHeatersAndCooler()
        {
            return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type != "heater" && w.Device.Type != "cooler").OrderByDescending(c => c.Id).Take(50).ToList();


            return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type != "heater").OrderByDescending(c => c.Id).ToList();
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
                return _ctx.Logs.Include(t => t.Device).Include(t => t.Device.Room).OrderByDescending(c => c.TimeStamp).Take(number ?? 0).ToList();
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

        public List<Log> GetLogsOfTempAndHumi()
        {
            var devices = _ctx.Devices.Where(w => w.Type == "humidity" || w.Type == "temperature").ToList();
            List<Log> logs = new List<Log>();
            foreach (var item in devices)
            {
                logs.Add(_ctx.Logs.Include(t => t.Device).Where(w => w.Device.Id == item.Id).OrderByDescending(c => c.TimeStamp).Take(1).FirstOrDefault());
            }
            return logs;
        }

        public IEnumerable<Log> GetLastFiftyLogsExceptHeatersAndCooler(int roomId)
        {
            return _ctx.Logs.Include(t => t.Device).Where(w => w.Device.Type != "heater" && w.Device.Type != "cooler" && w.Device.RoomId == roomId).OrderByDescending(c => c.Id).Take(50).ToList();
        }

        public IEnumerable<Log> Filter(DateTime startDate, DateTime endDate, int page = 1, string type = "", string roomName = "")
        {
            string[] types = type.Split('-', StringSplitOptions.RemoveEmptyEntries);

            List<Log> list = new List<Log>();

            var startIndex = (page - 1) * 50/ (types.Count()>0? types.Count():1);
            var endIndex = Math.Min(startIndex + 50 / (types.Count() > 0 ? types.Count() : 1) - 1, GetFiltredDbSize(startDate, endDate, type, roomName) - 1);

            startIndex = startIndex < 0 ? 0 : startIndex;
            endIndex = endIndex < 0 ? 50 : endIndex;

            if (roomName==null)
            {
                if (types.Count() != 0)
                {
                    foreach (var item in types)
                    {
                        var q = _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.Device.Type == item && w.TimeStamp <= endDate && w.TimeStamp >= startDate).OrderBy(o => o.TimeStamp).Skip(startIndex).Take(endIndex).ToList();
                        list.AddRange(q);
                    }
                }
                else
                {
                    var q = _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.TimeStamp <= endDate && w.TimeStamp >= startDate).OrderBy(o => o.TimeStamp).OrderBy(o => o.TimeStamp).Skip(startIndex).Take(endIndex).ToList();
                    list.AddRange(q);
                }
            }
            else
            {
                if (types.Count() != 0)
                {
                    foreach (var item in types)
                    {
                        var q = _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.Device.Type == item && w.TimeStamp <= endDate && w.TimeStamp >= startDate && w.Device.Room.Name.ToLower().Contains(roomName.ToLower())).OrderBy(o => o.TimeStamp).Skip(startIndex).Take(endIndex).ToList();
                        list.AddRange(q);
                    }
                }
                else
                {
                    var q = _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.TimeStamp <= endDate && w.TimeStamp >= startDate && w.Device.Room.Name.ToLower().Contains(roomName.ToLower())).OrderBy(o => o.TimeStamp).Skip(startIndex).Take(endIndex).ToList();
                    list.AddRange(q);
                }
              
            }
            

            return list;
        }

        public int GetDbSize()
        {
            return _ctx.Logs.Count();
        }

        public void ReduceLogs()
        {
            throw new NotImplementedException();
        }

        public int GetFiltredDbSize(DateTime startDate, DateTime endDate,  string type, string roomName)
        {
            string[] types = type.Split('-', StringSplitOptions.RemoveEmptyEntries);

            int pageNum = 0;



            if (roomName == null)
            {
                if (types.Count() != 0)
                {
                    foreach (var item in types)
                    {
                        pageNum+= _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.Device.Type == item && w.TimeStamp <= endDate && w.TimeStamp >= startDate).Count();
                       
                    }
                }
                else
                {
                    pageNum += _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.TimeStamp <= endDate && w.TimeStamp >= startDate).OrderBy(o => o.TimeStamp).Count();
                    
                }
            }
            else
            {
                if (types.Count() != 0)
                {
                    foreach (var item in types)
                    {
                        pageNum += _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.Device.Type == item && w.TimeStamp <= endDate && w.TimeStamp >= startDate && w.Device.Room.Name.ToLower().Contains(roomName.ToLower())).Count();
                        
                    }
                }
                else
                {
                    pageNum += _ctx.Logs.Include(i => i.Device).Include(i => i.Device.Room).Where(w => w.TimeStamp <= endDate && w.TimeStamp >= startDate && w.Device.Room.Name.ToLower().Contains(roomName.ToLower())).Count();
                   
                }

            }


            return pageNum ;
        }
    }
}
