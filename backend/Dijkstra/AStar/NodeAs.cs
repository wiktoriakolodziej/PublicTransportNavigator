using PublicTransportNavigator.DTOs;

namespace PublicTransportNavigator.Dijkstra.AStar
{
    public class NodeAs : Node
    {
        public double BestWeight { get; set; }
        public Coordinate? Coordinate { get; set; }
    }
}
