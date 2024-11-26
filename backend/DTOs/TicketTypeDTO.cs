using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.DTOs
{
    public class TicketTypeDTO
    {
        public long Id { get; set; }
        public  float? Price { get; set; }
        public string? Time { get; set; }
    }
}
