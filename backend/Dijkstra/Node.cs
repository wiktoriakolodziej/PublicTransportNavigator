namespace PublicTransportNavigator.Dijkstra
{
    public class Node
    {
        public bool Checked { get; set; } = false;
        public TimeSpan BestArrivalTime { get; set; }
        public long? PreviousBusId { get; set; }
        public long? PreviousNodeId { get; set; }
        public TimeSpan BestDepartureTime { get; set; }
        public List<Connection> Connections { get; set; } = [];
    }
}
