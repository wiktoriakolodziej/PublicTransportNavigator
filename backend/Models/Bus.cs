using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("buses")]
    public class Bus : BaseEntity
    {

        [Required]
        [Column("bus_type_id")]
        public required long TypeId { get; set; }

        [Required]
        [Column("bus_number")]
        public required int Number { get; set; }

        [Required]
        [Column("first_bus_stop_id")]
        public required long FirstBusStopId { get; set; }

        [Required]
        [Column("last_bus_stop_id")]
        public required long LastBusStopId { get; set; }

        public List<Timetable> Timetables { get; set; } = []; 
        public List<BusSeat> BusSeats { get; set; } = [];
        public BusStop? FirstBusStop { get; set; }
        public BusStop? LastBusStop { get; set; }
        public BusType? Type { get; set; }

    }
}
