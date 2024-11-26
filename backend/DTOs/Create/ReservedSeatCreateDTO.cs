using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs.Create
{
    public class ReservedSeatCreateDTO
    {
        public required long BusSeatId { get; set; } 

        public required string TimeIn { get; set; }

        public required string TimeOff { get; set; }

        public long? UserTravelId { get; set; }

        public required string Date { get; set; }
        public required long UserId { get; set; }
    }
}
