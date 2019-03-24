using System;

namespace WebGUI.Dtos
{
    public class Esp
    {
        public int Id { get; set; }
        public string ChargeType { get; set; }
        public DateTime LastCharge { get; set; }
        public DateTime LastInteraction { get; set; }
        public int InteractionsCounter { get; set; }
        public int AvgInteractions { get; set; }
        public DateTime AvgBatteryDuration { get; set; }
    }
}
