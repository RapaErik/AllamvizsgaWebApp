using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public class RoomService : IRoomService
    {
        private readonly HeatingContext _ctx;
        public RoomService(HeatingContext ctx)
        {
            _ctx = ctx;
        }

        public Room AddNewRoom()
        {
            Room r = new Room { Name = "Room" };
            _ctx.Rooms.Add(r);
            _ctx.SaveChanges();
            return r;
        }

        public void AddNewRoom(string name)
        {
            Room r = new Room { Name = name };
            _ctx.Add(r);
            _ctx.SaveChangesAsync();
        }

        public void DeleteRoomById(int roomId)
        {
            var element = _ctx.Rooms.Where(w => w.Id == roomId).FirstOrDefault();
            var devices=_ctx.Devices.Where(w => w.RoomId == roomId).ToList();
            foreach (var item in devices)
            {
                item.RoomId = null;
                item.Room = null;
            }
            _ctx.Rooms.Remove(element);
            _ctx.SaveChanges();
        }

        public List<Room> GetAllRooms(int? roomId)
        {
            if(roomId==null)
            {
                return _ctx.Rooms.ToList();
            }
            return _ctx.Rooms.Where(w=>w.Id==roomId).ToList();
        }

        public Room GetRoomById(int id)
        {
            return _ctx.Rooms.Where(w=>w.Id==id).FirstOrDefault();
        }

        public Room GetRoomWhereDeviceIs(int deviceId)
        {
            Device d = _ctx.Devices.Where(w => w.Id == deviceId).FirstOrDefault();
            return _ctx.Rooms.Where(w => w.Id == d.RoomId).FirstOrDefault();
        }

        public void ToggleCoolerByRoomId(int roomId)
        {
            var temp= _ctx.Rooms.Where(w => w.Id == roomId).FirstOrDefault();
            temp.CoolingEnable = !temp.CoolingEnable;
            _ctx.SaveChanges();
        }

        public void ToggleHeaterByRoomId(int roomId)
        {
            var temp = _ctx.Rooms.Where(w => w.Id == roomId).FirstOrDefault();
            temp.HeatingEnable = !temp.HeatingEnable;
            _ctx.SaveChanges();
        }

        public void UpdateRoomDayliSetpoint(int id, float data)
        {
            Room r = _ctx.Rooms.Where(w => w.Id == id).FirstOrDefault();
            r.DailySetpoint = data;
            _ctx.SaveChanges();
        }

        public void UpdateRoomName(int id, string data)
        {
            Room r = _ctx.Rooms.Where(w => w.Id == id).FirstOrDefault();
            r.Name = data;
            _ctx.SaveChanges();
        }

        public void UpdateRoomNightliSetpoint(int id, float data)
        {
            Room r = _ctx.Rooms.Where(w => w.Id == id).FirstOrDefault();
            r.NightlySetpoint = data;
            _ctx.SaveChanges();
        }
    }

}

