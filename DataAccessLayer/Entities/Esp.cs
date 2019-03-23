using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
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
