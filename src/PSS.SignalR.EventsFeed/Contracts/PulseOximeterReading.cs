namespace PAREXEL.PSS.SignalR.EventsFeed.Contracts
{
    public class PulseOximeterReading
    {
        public int SpO2 { get; set; }
        public int PulseRate { get; set; }
        public string TrackingId { get; set; }
        public string TimeZone { get; set; }
    }
}