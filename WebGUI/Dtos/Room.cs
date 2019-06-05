using System;

namespace WebGUI.Dtos
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float DailySetpoint { get; set; }
        public float NightlySetpoint { get; set; }
    }
}
