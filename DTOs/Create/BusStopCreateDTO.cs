using Microsoft.Build.Framework;

namespace PublicTransportNavigator.DTOs.Create
{
    public class BusStopCreateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float CoordX { get; set; }
        [Required]
        public float CoordY { get; set; }

        public bool OnRequest { get; set; } = false;
    }
}
