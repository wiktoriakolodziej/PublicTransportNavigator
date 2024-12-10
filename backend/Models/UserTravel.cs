using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace PublicTransportNavigator.Models
{
    [Table("user_travels")]
    public class UserTravel : BaseEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }
        public User User { get; set; }
        [Column("ticket_id")]
        public long? TicketId { get; set; }
        public TicketType TicketType { get; set; }
        public List<ReservedSeat> ReservedSeats { get; set; }
    }
}
