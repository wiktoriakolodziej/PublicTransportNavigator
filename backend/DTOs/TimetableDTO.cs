using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs
{
    public class TimetableDTO
    {
        public long Id { get; set; }
        public long? BusId { get; set; }
        public Bus? Bus { get; set; }
        public long? BusStopId { get; set; }
        public BusStop? BusStop { get; set; }
        public string? Time { get; set; }
    }
}
