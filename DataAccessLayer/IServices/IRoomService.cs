using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface IRoomService
    {
        void AddNewRoom();
        void AddNewRoom(string name);

        List<Room> GetAllRooms(int? roomId=null);
        Room GetRoomById(int Id);

        void UpdateRoomName(int id, string data);
        void UpdateRoomDayliSetpoint(int id, float data);
        void UpdateRoomNightliSetpoint(int id, float data);
        void ToggleHeaterByRoomId(int roomId);
        void ToggleCoolerByRoomId(int roomId);
    }
}
