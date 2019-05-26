using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Sevices
{
    public interface IRoomService
    {
        void AddNewRoom();
        void AddNewRoom(string name);
    }
}
