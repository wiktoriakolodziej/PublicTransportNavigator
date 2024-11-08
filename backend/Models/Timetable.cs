using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("timetables")]
    public class Timetable : BaseEntity
    {

        [Column("bus_id")]
        public required long BusId { get; set; }
        public Bus? Bus { get; set; }

        [Column("bus_stop_id")]
        public required long BusStopId { get; set; }
        public BusStop? BusStop { get; set; }

        [Required]
        [Column("bus_arrival_time")]
        public required DateTime Time { get; set; }
    }
}
