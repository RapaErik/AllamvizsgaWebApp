using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public float Data { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool IsNan { get; set; }
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
        
        
    }
}
