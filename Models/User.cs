using PublicTransportNavigator.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("users")]
    public class User : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("surname")]
        public required string Surname { get; set; }

        [Required]
        [DefaultValue("User")]
        [Column("role")]
        public Role Role { get; set; } = Role.User;

        [Column("profile_picture")]
        public byte[]? ImageData { get; set; }
        public List<UserFavouriteBusStop>? Favourites { get; set; }
        public List<Discount>? Discounts { get; set; }
        public List<UserTravel>? TravelHistory { get; set; } 
    }
}
