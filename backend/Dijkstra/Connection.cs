namespace PublicTransportNavigator.Dijkstra
{
    public class Connection
    {
        public int ConnectionTime { get; set; }
        public HashSet<TimeSpan> DepartureTimes { get; set; } = [];
        public long From { get; set; }
        public long To { get; set; }
        public long BusId { get; set; }
    }
}
