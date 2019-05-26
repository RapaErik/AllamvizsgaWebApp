using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
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
    }

}

