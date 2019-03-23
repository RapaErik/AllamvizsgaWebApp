using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }
        public int SensorId { get; set; }
        [ForeignKey("SensorId")]
        public Sensor Sensor { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
