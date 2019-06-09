using System;
using System.Collections.Generic;
using System.Text;

namespace ControlUnit.Dtos
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float DailySetpoint { get; set; }
        public float NightlySetpoint { get; set; }


        public bool CoolingEnable { get; set; }
        public bool HeatingEnable { get; set; }
    }
}
