using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs.Create
{
    public class TimetableCreateDTO
    {
        public required long BusId { get; set; }

        public required long BusStopId { get; set; }

        public required List<TimeSpan> Time { get; set; }
    }
}
