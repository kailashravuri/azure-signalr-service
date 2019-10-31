using System;
using System.Collections.Generic;

namespace PAREXEL.PSS.SignalR.EventsFeed.Contracts
{
    public class Nonin3230DayReadings
    {
        public string DataItemId { get; set; }
        public string MessageType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string DaySplit { get; set; }
        public SortedDictionary<DateTimeOffset, PulseOximeterReading> PulseOximeterReadings{ get; set; }
    }
}