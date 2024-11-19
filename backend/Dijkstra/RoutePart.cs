namespace PublicTransportNavigator.Dijkstra
{
    public class RoutePart
    {
        public int BusName { get; set; }
        public Dictionary<TimeSpan, string> Details { get; set; } = [];
    }
}
