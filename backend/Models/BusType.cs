using PublicTransportNavigator.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("bus_types")]
    public class BusType : BaseEntity
    {
        [Required]
        [Column("type")]
        public required BusTypeEnum Type { get; set; }

        [Required]
        [Column("image")]
        public required string ImagePath { get; set; }

    }
}
