﻿using Microsoft.Build.Framework;
using PublicTransportNavigator.Models.Enums;

namespace PublicTransportNavigator.DTOs.old
{
    public class BusCreateDTO
    {
        [Required]
        public required BusType Type { get; set; }

        [Required]
        public required int Number { get; set; }

        [Required]
        public required long FirstBusStopId { get; set; }

        [Required]
        public required long LastBusStopId { get; set; }

    }
}