using System;

namespace WebGUI.Dtos
{
    public class Sensor
    {

        public int Id { get; set; }
        public string Type { get; set; }
        public int EspId { get; set; }

        public Esp Esp { get; set; }
        public int RoomId { get; set; }

        public Room Room { get; set; }


    }
}
