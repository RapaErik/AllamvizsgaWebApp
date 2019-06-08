using System;


namespace WebGUI.Dtos
{
    public class Log
    {

        public int Id { get; set; }
        public float Data { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool IsNan { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
