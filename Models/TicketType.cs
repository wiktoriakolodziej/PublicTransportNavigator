using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("ticket_types")]
    public class TicketType : BaseEntity
    {

        [Required]
        [Column("price")]
        public required float Price { get; set; }

        [Required]
        [Column("time_range")]
        public required TimeSpan Time { get; set; }
    }
}
