using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface IDeviceService
    {
        IEnumerable<Device> GetAllDevicesWithoutOfRooms();
        IEnumerable<Device> GetAllDevicesRoomId(int id);
        void RemoveEspFromRoom(int espId);

        IEnumerable<Device> AddEspToRoom(int roomId, int espId);
    }
}
