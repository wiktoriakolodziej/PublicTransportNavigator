using PublicTransportNavigator.DTOs;

namespace PublicTransportNavigator.Dijkstra
{
    public class RoutePreview
    {
        public string Id { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan DestinationTime { get; set; }
        public int TravelTime { get; set; }
        public List<int> BusNumbers { get; set; } = [];
        public List<Coordinate> Coordinates { get; set; } = [];
    }
}
