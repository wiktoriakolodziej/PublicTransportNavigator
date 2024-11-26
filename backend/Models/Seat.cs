using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransportNavigator.Models.Enums;

namespace PublicTransportNavigator.Models
{
    [Table("seat_types")]
    public class Seat : BaseEntity
    {

        [Required]
        [Column("type")]
        public required SeatType SeatType { get; set; }

        [Column("icon_path")]
        public string ImagePath { get; set; }
    }
}
