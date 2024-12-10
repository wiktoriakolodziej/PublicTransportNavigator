using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs.Create
{
    public class TicketTypeCreateDTO
    {
        public required float Price { get; set; }
        public required int Time { get; set; }
    }
}
