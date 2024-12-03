using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportNavigator.Models
{
    [Table("calendar")]
    public class Calendar : BaseEntity
    {
        [Required]
        [Column("monday")]
        public required bool Monday { get; set; }

        [Required]
        [Column("tuesday")]
        public required bool Tuesday { get; set; }

        [Required]
        [Column("wednesday")]
        public required bool Wednesday { get; set; }

        [Required]
        [Column("thursday")]
        public required bool Thursday { get; set; }

        [Required]
        [Column("friday")]
        public required bool Friday { get; set; }

        [Required]
        [Column("saturday")]
        public required bool Saturday { get; set; }

        [Required]
        [Column("sunday")]
        public required bool Sunday { get; set; }

    }
}
