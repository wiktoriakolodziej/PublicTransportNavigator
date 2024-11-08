using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace PublicTransportNavigator.Models
{
    [Table("user_favourite_bus_stops")]
    public class UserFavouriteBusStop : BaseEntity
    {
        [Column("user_id")]
        public required long UserId { get; set; }
        public User? User { get; set; }
        [Column("bus_stop_id")]
        public required long BusStopId { get; set; }
        public BusStop? BusStop { get; set; }

        [MaxLength(100)]
        [Column("name")]
        public string? Name { get; set; }
    }
}
