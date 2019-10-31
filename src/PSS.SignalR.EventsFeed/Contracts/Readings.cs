using System;

namespace PAREXEL.PSS.SignalR.EventsFeed.Contracts
{
    [Serializable]
    public class Readings
    {
        public DateTime MeasurementTime { get; set; }
        public int PulseRate { get; set; }
        public int SpO2 { get; set; }
        public string Id { get; set; }
    }
}
