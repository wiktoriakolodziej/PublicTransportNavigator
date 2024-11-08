using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("discounts")]
    public class Discount : BaseEntity
    {

        [MaxLength(200)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("percentage")]
        public required int Percentage { get; set; }

        public List<User> DiscountUsers { get; set; } = [];
    }
}
