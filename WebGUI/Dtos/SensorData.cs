using System;


namespace WebGUI.Dtos
{
    public class SensorData
    {

        public int Id { get; set; }
        public int SensorId { get; set; }

        public Sensor Sensor { get; set; }
        public string Data { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
