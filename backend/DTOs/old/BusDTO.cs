using PublicTransportNavigator.Models.Enums;
using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PublicTransportNavigator.DTOs.old
{
    public class BusDTO
    {
        public long Id { get; set; }
        public required BusType Type { get; set; }
        public required int Number { get; set; }
        public required long FirstBusStopId { get; set; }
        public required long LastBusStopId { get; set; }
        public List<Timetable> Timetables { get; } = [];
        public List<BusSeat> BusSeats { get; } = [];
        public BusStop? FirstBusStop { get; set; }
        public BusStop? LastBusStop { get; set; }
    }
}
