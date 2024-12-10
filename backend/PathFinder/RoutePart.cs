namespace PublicTransportNavigator.Dijkstra
{
    public class RoutePart
    {
        public long BusId { get; set; }
        public string BusName { get; set; }
        public List<string> Details { get; set; } = [];
    }
}
