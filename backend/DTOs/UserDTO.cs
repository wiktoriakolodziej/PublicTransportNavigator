using PublicTransportNavigator.Models.Enums;
using PublicTransportNavigator.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PublicTransportNavigator.DTOs
{
    public class UserDTO
    {
        public long? Id { get; set; }
        public  string? Name { get; set; }

        public  string? Surname { get; set; }

        public Role Role { get; set; } = Role.User;

        public byte[]? ImageData { get; set; }
        public List<UserFavouriteBusStop>? Favourites { get; set; }
        public List<Discount>? Discounts { get; set; }
        public List<UserTravel>? TravelHistory { get; set; }
    }
}
