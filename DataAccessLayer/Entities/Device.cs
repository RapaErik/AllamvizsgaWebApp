using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public bool IO { get; set; }
        public int? CommunicationUnitId { get; set; }
        [ForeignKey("CommunicationUnitId")]
        public CommunicationUnit CommunicationUnit { get; set; }
        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }


    }
}
