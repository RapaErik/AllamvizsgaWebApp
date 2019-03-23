using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public int EspId { get; set; }
        [ForeignKey("EspId")]
        public Esp Esp { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }


    }
}
