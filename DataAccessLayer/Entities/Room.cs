using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public float DailySetpoint { get; set; }
        public float NightlySetpoint { get; set; }

        public bool CoolingEnable { get; set; }
        public bool HeatingEnable { get; set; }



    }
}
