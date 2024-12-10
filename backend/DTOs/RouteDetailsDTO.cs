using PublicTransportNavigator.Dijkstra;

namespace PublicTransportNavigator.DTOs
{
    public class RouteDetailsDTO
    {
        public string Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public long TravelTime { get; set; }
        public List<RoutePartDTO> Parts { get; set; } = [];
        public List<Coordinate> Coordinates { get; set; } = [];
    }
}
