using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PublicTransportNavigator.DTOs
{
    public class UserFavouriteBusStopDTO
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public long? BusStopId { get; set; }
        public BusStop? BusStop { get; set; }
        public string? Name { get; set; }
    }
}
