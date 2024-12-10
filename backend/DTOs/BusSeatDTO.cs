using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs
{
    public class BusSeatDTO
    {
        public long Id { get; set; }
        public long? BusId { get; set; }

        public string? SeatType { get; set; }

        public Coordinate? Coordinate { get; set; } = new();

        public bool? Available { get; set; }

        public string? ImagePath { get; set; }
    }
}
