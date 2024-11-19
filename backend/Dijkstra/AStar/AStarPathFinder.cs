using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.Dijkstra.AStar
{
    public class AStarPathFinder(IServiceScopeFactory serviceProvider) : DijkstraPathFinder<NodeAs>(serviceProvider)
    {
        public override void SyncNodes()
        {
            Available = Task.Run(async () =>
            {
                List<List<Timetable>> timetables;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                    _nodes.Clear();
                    foreach (var busStop in context.BusStops)
                    {
                        var node = new NodeAs
                        {
                            BestArrivalTime = TimeSpan.MaxValue,
                            BestDepartureTime = TimeSpan.MaxValue,
                            PreviousNodeId = -1,
                            PreviousBusId = -1,
                            Connections = [],
                            BestWeight = double.MaxValue,
                            Coordinate = new Coordinate
                            {
                                X = busStop.CoordX,
                                Y = busStop.CoordY,
                            }
                        };
                        _nodes.Add(busStop.Id, (NodeAs)node);
                    }

                    timetables = await context.Timetables
                        .GroupBy(t => t.BusId)
                        .Select(tim => tim.OrderBy(time => time.Time).ToList())
                        .ToListAsync();

                }

                foreach (var ttForBusId in timetables)
                {

                    var busId = ttForBusId[0].BusId;
                    var busStopIds = ttForBusId.Select(t => t.BusStopId).Distinct().ToList();
                    var bestTimes = new Dictionary<TimeSpan, Connection>();
                    foreach (var busStopId in busStopIds)
                    {
                        var times = ttForBusId.Where(t => t.BusStopId == busStopId && t.BusId == busId)
                            .Select(t => t.Time).ToHashSet();

                        var connection = new Connection
                        {
                            DepartureTimes = times,
                            From = busStopId,
                            BusId = busId,
                        };
                        bestTimes.Add(times.Min(), connection);
                    }

                    while (bestTimes.Count > 1)
                    {
                        var first = bestTimes.Keys.Min();
                        bestTimes.TryGetValue(first, out var firstConnection);
                        bestTimes.Remove(first);

                        var second = bestTimes.Keys.Min();
                        bestTimes.TryGetValue(second, out var secondConnection);

                        firstConnection.ConnectionTime = (second - first).Minutes;
                        firstConnection.To = secondConnection.From;

                        _nodes.TryGetValue(firstConnection.From, out var fromNode);
                        fromNode.Connections.Add(firstConnection);

                        _nodes.TryGetValue(firstConnection.To, out var toNode);
                        toNode.Connections.Add(firstConnection);
                    }
                }
            });
        }

        protected override void Recursive(NodeAs parentNode, long currentBusStopId, long destinationBusStopId)
        {
            //change status of the current node; it is checked from now on
            var departureTime = parentNode.BestArrivalTime;
            parentNode.Checked = true;

            //update all neighbours of the current node
            foreach (var connection in parentNode.Connections.Where(c => c.From == currentBusStopId))
            {
                _nodes.TryGetValue(connection.To, out var destinationStop);
                var closestDepartureTime = FindClosestAfter(departureTime, connection.DepartureTimes);
                if (closestDepartureTime == null) continue;
                var aggregateTime =
                    departureTime.Add(closestDepartureTime.Value - departureTime).Add(TimeSpan.FromMinutes(connection.ConnectionTime));
                var weight = TimeSpanToFloat(aggregateTime) + GetDistance(destinationStop.Coordinate, destinationBusStopId);

                if (destinationStop!.BestArrivalTime <= aggregateTime) continue;

                destinationStop.BestArrivalTime = aggregateTime;
                destinationStop.PreviousNodeId = currentBusStopId;
                destinationStop.PreviousBusId = connection.BusId;
                destinationStop.BestWeight = weight;
                parentNode.BestDepartureTime = closestDepartureTime.Value;

            }

            //find node with the min best time that is not yet checked
            try
            {
                var nextNode = _nodes
                    .Where(pair => pair.Value.Checked == false)
                    .MinBy(pair => pair.Value.BestWeight);

                //this means the algorithm has found the shortest path
                if (nextNode.Key == destinationBusStopId) return;

                Recursive(nextNode.Value, nextNode.Key, destinationBusStopId);
            }
            //that means that the algorithm has checked all the nodes and haven't found the destination node (unlikely)
            catch (Exception ex) { }
        }

        public override void CleanUpNodes()
        {
            Available = Task.Run(() =>
                {
                    foreach (var node in _nodes)
                    {
                        node.Value.BestArrivalTime = TimeSpan.MaxValue;
                        node.Value.BestDepartureTime = TimeSpan.MaxValue;
                        node.Value.PreviousBusId = -1;
                        node.Value.PreviousNodeId = -1;
                        node.Value.Checked = false;
                        node.Value.BestWeight = double.MaxValue;
                    }
                }
            );
        }

        private double GetDistance(Coordinate c1, long destinationBusStopId)
        {
            _nodes.TryGetValue(destinationBusStopId, out var destinationNode);
            var c2 = destinationNode.Coordinate;
            // Check if either coordinate is null or has null values
            if (c1 == null || c2 == null || !c1.X.HasValue || !c1.Y.HasValue || !c2.X.HasValue || !c2.Y.HasValue)
            {
                throw new ArgumentException("Both coordinates must have valid X and Y values.");
            }

            // Calculate Euclidean distance
            var deltaX = c1.X.Value - c2.X.Value;
            var deltaY = c1.Y.Value - c2.Y.Value;

            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        private static int TimeSpanToFloat(TimeSpan time)
        {
            return time.Minutes;
        }
    }
}
