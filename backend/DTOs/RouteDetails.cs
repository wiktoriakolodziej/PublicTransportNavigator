namespace PublicTransportNavigator.DTOs
{
    public class RouteDetails
    {
        public long Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public int TravelTime { get; set; }
        public Dictionary<long, RoutePart> Parts { get; set; } = [];
        public List<Coordinate> Coordinates { get; set; } = [];
    }
}
