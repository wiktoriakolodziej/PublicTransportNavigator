namespace PublicTransportNavigator.Dijkstra
{
    public class RoutePart
    {
        public string BusName { get; set; }
        public Dictionary<TimeSpan, string> Details { get; set; } = [];
    }
}
