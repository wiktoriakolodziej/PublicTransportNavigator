namespace PublicTransportNavigator.DTOs.Create
{
    public class BusSeatCreateDTO
    {
        public required long BusId { get; set; }

        public required int SeatType { get; set; }

        public required List<Coordinate> Coordinate { get; set; }
    }
}
