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

        public void AddNewRoom()
        {
            Room r = new Room { Name = "Room" };
            _ctx.Rooms.Add(r);
            _ctx.SaveChanges();

        }

        public void AddNewRoom(string name)
        {
            Room r = new Room { Name = name };
            _ctx.Add(r);
            _ctx.SaveChangesAsync();
        }

        public void DeleteRoomById(int roomId)
        {
            throw new NotImplementedException();
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

