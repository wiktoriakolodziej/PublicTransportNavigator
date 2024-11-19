namespace PublicTransportNavigator.DTOs
{
    public class RoutePreview
    {
        public long Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public int TravelTime { get; set; }
        public List<long> BusNumbers { get; set; } = [];
        public List<Coordinate> Coordinates { get; set; } = [];
    }
}
