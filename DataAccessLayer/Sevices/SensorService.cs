using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Sevices
{
    public class SensorService : ISensorService
    {
        private readonly HeatingContext _ctx;
        public SensorService(HeatingContext ctx)
        {
            _ctx = ctx;
        }
        void InsertIntoSensor()
        {
            _ctx.Sensors.Add(new Sensor { Type = "valami", EspId = 2 });
            _ctx.SaveChanges();
        }
        public IEnumerable<Sensor> GetAllSensorsWithoutOfRooms()
        {
            return _ctx.Sensors.Include(i => i.Esp).Where(w => w.Room == null).ToList();
        }

        public IEnumerable<Sensor> GetAllSensorsRoomId(int id)
        {
            return _ctx.Sensors.Include(i => i.Esp).Where(w => w.RoomId == id).ToList();
        }

        public void RemoveEspFromRoom(int espId)
        {
            var sensorsWithEsp = _ctx.Sensors.Where(w => w.EspId == espId).ToList();
            foreach (var item in sensorsWithEsp)
            {
                item.RoomId = null;
                item.Room = null;
            }
            _ctx.SaveChanges();
        }
    }
}
