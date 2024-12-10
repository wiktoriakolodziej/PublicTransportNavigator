using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransportNavigator.Models.Enums;

namespace PublicTransportNavigator.Models
{
    [Table("timetables")]
    public class Timetable 
    {
        [Column("id")]
        public long Id { get; set; }

        [Required]
        public DateTime LastModified { get; set; }

        [Column("bus_id")]
        public required long BusId { get; set; }
        public Bus? Bus { get; set; }

        [Column("bus_stop_id")]
        public required long BusStopId { get; set; }
        public BusStop? BusStop { get; set; }

        [Required]
        [Column("bus_arrival_time")]
        public required TimeSpan Time { get; set; }

        [Required]
        [Column("calendar_id")]
        public required long CalendarId { get; set; }
        public Calendar? Calendar { get; set; }
    }
}
