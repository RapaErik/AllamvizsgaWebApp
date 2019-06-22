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
    public class DeviceService : IDeviceService
    {
        private readonly HeatingContext _ctx;
        public DeviceService(HeatingContext ctx)
        {
            _ctx = ctx;
        }
        void InsertIntoDevice()
        {
            _ctx.Devices.Add(new Device { Type = "valami", CommunicationUnitId = 2 });
            _ctx.SaveChanges();
        }
        public IEnumerable<Device> GetAllDevicesWithoutOfRooms()
        {
            return _ctx.Devices.Include(i => i.CommunicationUnit).Where(w => w.Room == null).ToList();
        }

        public IEnumerable<Device> GetAllDevicesRoomId(int id)
        {
            return _ctx.Devices.Include(i => i.CommunicationUnit).Where(w => w.RoomId == id).ToList();
        }

        public void RemoveEspFromRoom(int espId)
        {
            var DevicesWithEsp = _ctx.Devices.Where(w => w.CommunicationUnitId == espId).ToList();
            foreach (var item in DevicesWithEsp)
            {
                item.RoomId = null;
                item.Room = null;
            }
            _ctx.SaveChanges();
        }

        public IEnumerable<Device> AddEspToRoom(int roomId, int espId)
        {
            var DevicesWithEsp = _ctx.Devices.Where(w => w.CommunicationUnitId == espId).ToList();
            foreach (var item in DevicesWithEsp)
            {
                item.RoomId = roomId;
            }
            _ctx.SaveChanges();

            return DevicesWithEsp;

        }

        public IEnumerable<Device> GetListOfDevices(int? roomId=null)
        {
            if(roomId == null)
            {
                return _ctx.Devices.ToList();
            }
            return _ctx.Devices.Include(i=>i.CommunicationUnit).Where(w=>w.RoomId==roomId).ToList();
        }

        public void InitDevice(Device device)
        {
            Device d= (from q in _ctx.Devices
                       where q.CommunicationUnitId == device.CommunicationUnitId && q.Type == device.Type
                       select q).FirstOrDefault();
            if (d==null)
            {
                _ctx.Add(device);
                _ctx.SaveChanges();
            }
        }

        public Device GetDevice(int comunitid, string type)
        {
            return (from q in _ctx.Devices
                    where q.CommunicationUnitId == comunitid && q.Type == type
                    select q).FirstOrDefault();
        }
    }
}
