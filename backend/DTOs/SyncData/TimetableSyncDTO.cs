using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs.SyncData
{
    public class TimetableSyncDTO
    {
        public long Id { get; set; }
        public required long BusId { get; set; }
        public required long BusStopId { get; set; }
        public required TimeSpan Time { get; set; }
        public required long CalendarId { get; set; }
        public DateTime LastModified { get; set; }
        public int Sequence { get; set; } 
    }
}
