using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace PublicTransportNavigator.Models
{
    [Table("reserved_seats")]
    public class ReservedSeat : BaseEntity
    {

        [Column("bus_seat_id")]
        public required long BusSeatId { get; set; } 
        public BusSeat? BusSeat { get; set; }

        [Column("timetable_in_id")]
        public long TimeInId { get; set; }
        public Timetable? TimeIn { get; set; }

        [Column("timetable_off_id")]
        public long TimeOffId { get; set; }
        public Timetable? TimeOff { get; set; }

        [Column("user_travel_id")]
        public required long UserTravelId { get; set; }
        public UserTravel? UserTravel { get; set; }

        [Column("reservation_date")]
        [Required]
        public required DateTime Date { get; set; }

        [Column("valid_until")]
        public DateTime? ValidUntil { get; set; }

    }
}
