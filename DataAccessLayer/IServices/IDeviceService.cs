﻿using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.IServices
{
    public interface IDeviceService
    {
        IEnumerable<Device> GetAllDevicesWithoutOfRooms();
        IEnumerable<Device> GetAllDevicesRoomId(int id);
        void RemoveEspFromRoom(int espId);
        void RemoveDevice(int id);
        IEnumerable<Device> AddEspToRoom(int roomId, int espId);
        IEnumerable<Device> GetListOfDevices(int? roomId);
        void InitDevice(Device device);
        Device GetDevice(int comunitid, string type);
    }
}
