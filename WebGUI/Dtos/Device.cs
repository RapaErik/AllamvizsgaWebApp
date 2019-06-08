using System;

namespace WebGUI.Dtos
{
    public class Device
    {

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IO { get; set; }
        public int? CommunicationUnitId { get; set; }

        public CommunicationUnit CommunicationUnit { get; set; }
        public int? RoomId { get; set; }

        public Room Room { get; set; }


    }
}
