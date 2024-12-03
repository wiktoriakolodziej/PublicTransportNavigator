using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("bus_stops")]
    public class BusStop
    {
        [Column("id")]
        public long Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Column("name")]
        public required string Name { get; set; }

        [DefaultValue(false)]
        [Column("on_request")]
        public bool OnRequest { get; set; } = false;

        [Required]
        public float? CoordX { get; set; }

        [Required]
        public float? CoordY { get; set; }

        public List<Timetable> Timetables { get; set; } = [];

        [Required]
        public DateTime LastModified { get; set; }

        //public long SiblingId { get; set; }
    }
}
