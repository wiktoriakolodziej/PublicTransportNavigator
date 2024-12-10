using PublicTransportNavigator.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs
{
    public class SeatDTO
    {
        public long Id { get; set; }
        public  string? SeatType { get; set; }

        public string? ImagePath { get; set; }
    }
}
