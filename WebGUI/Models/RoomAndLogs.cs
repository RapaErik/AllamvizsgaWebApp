using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGUI.Dtos;

namespace WebGUI.Models
{
    public class RoomAndLogs
    {
        public Room Room { get; set; }
        public List<Log> Logs { get; set; }
    }
}
