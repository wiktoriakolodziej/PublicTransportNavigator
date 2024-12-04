namespace PublicTransportNavigator.Dijkstra
{
    public class Node
    {
        public long BusStopId { get; set; }
        public bool Checked { get; set; } = false;
        public TimeSpan BestArrivalTime { get; set; }
        public long? PreviousBusId { get; set; }
        public long? PreviousNodeId { get; set; }
        public TimeSpan BestDepartureTime { get; set; }
        public Dictionary<long, SortedSet<Connection>> Connections { get; set; } = []; //key - connection.to
    }
}
