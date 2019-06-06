using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface ISensorService
    {
        IEnumerable<Sensor> GetAllSensorsWithoutOfRooms();
        IEnumerable<Sensor> GetAllSensorsRoomId(int id);
        void RemoveEspFromRoom(int espId);

        IEnumerable<Sensor> AddEspToRoom(int roomId, int espId);
    }
}
