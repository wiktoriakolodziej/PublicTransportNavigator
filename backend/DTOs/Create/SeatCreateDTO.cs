using PublicTransportNavigator.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs.Create
{
    public class SeatCreateDTO
    {
        public required int SeatType { get; set; }

        public required string ImagePath { get; set; }
    }
}
