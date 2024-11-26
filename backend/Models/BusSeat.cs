using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("bus_seats")]
    public class BusSeat : BaseEntity
    {

        [Required]
        [Column("bus_id")]
        public required long BusId { get; set; }
        public Bus? Bus { get; set; }

        [Required]
        [Column("seat_type_id")]
        public required long SeatTypeId { get; set; }
        public Seat? SeatType { get; set; }

        //[MaxLength(10)]
        //[DefaultValue("standing")]
        //[Column("seat_position")]
        //public string Position { get; set; } = "standing";

        [Required]
        [Column("x_offset")]
        public float? CoordX { get; set; }

        [Required]
        [Column("y_offset")]
        public float? CoordY { get; set; }
    }
}
