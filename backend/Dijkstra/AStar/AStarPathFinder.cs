using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.Dijkstra.AStar
{
    public class AStarPathFinder(IServiceScopeFactory serviceProvider) : DijkstraPathFinder<NodeAs>(serviceProvider)
    {
        protected override void SyncNodes(long calendarId)
        {
            Available = Task.Run(async () =>
            {
                var nodes = new Dictionary<long, NodeAs>();
                List<List<Timetable>> timetables;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
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
                        nodes.TryAdd(busStop.Id, (NodeAs)node);
                    }

                    timetables = await context.Timetables
                        .Where(t => t.CalendarId == calendarId)
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
                        bestTimes.TryAdd(times.Min(), connection);
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

                        nodes.TryGetValue(firstConnection.From, out var fromNode);
                        fromNode.Connections.Add(firstConnection);

                        nodes.TryGetValue(firstConnection.To, out var toNode);
                        toNode.Connections.Add(firstConnection);
                    }
                }
                _graphs.Add(calendarId, nodes);
            });
        }

        protected override void Recursive(NodeAs parentNode, long currentBusStopId, long destinationBusStopId, long calendarId)
        {
            var nodes = _graphs[calendarId];
            //change status of the current node; it is checked from now on
            var departureTime = parentNode.BestArrivalTime;
            parentNode.Checked = true;

            //update all neighbours of the current node
            foreach (var connection in parentNode.Connections.Where(c => c.From == currentBusStopId))
            {
                nodes.TryGetValue(connection.To, out var destinationStop);
                var closestDepartureTime = FindClosestAfter(departureTime, connection.DepartureTimes);
                if (closestDepartureTime == null) continue;
                var aggregateTime =
                    departureTime.Add(closestDepartureTime.Value - departureTime).Add(TimeSpan.FromMinutes(connection.ConnectionTime));
                var weight = TimeSpanToFloat(aggregateTime) + GetDistance(destinationStop.Coordinate, destinationBusStopId, calendarId);

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
                var nextNode = nodes
                    .Where(pair => pair.Value.Checked == false)
                    .MinBy(pair => pair.Value.BestWeight);

                //this means the algorithm has found the shortest path
                if (nextNode.Key == destinationBusStopId) return;

                Recursive(nextNode.Value, nextNode.Key, destinationBusStopId, calendarId);
            }
            //that means that the algorithm has checked all the nodes and haven't found the destination node (unlikely)
            catch (Exception ex) { }
        }

        public override void CleanUpNodes(long calendarId)
        {
            Available = Task.Run(() =>
                {
                    foreach (var node in _graphs[calendarId])
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

        private double GetDistance(Coordinate c1, long destinationBusStopId, long calendarId)
        {
            var nodes = _graphs[calendarId];
            nodes.TryGetValue(destinationBusStopId, out var destinationNode);
            var c2 = destinationNode.Coordinate;
            // Check if either coordinate is null or has null values
            if (c1 == null || c2 == null || !c1.X.HasValue || !c1.Y.HasValue || !c2.X.HasValue || !c2.Y.HasValue)
            {
                throw new ArgumentException("Both coordinates must have valid X and Y values.");
            }

            if (c1.X.Equals(c2.X) && c1.Y.Equals(c2.Y))
            {
                return 0;
            }

            //// Calculate Euclidean distance
            //var deltaX = c1.X.Value - c2.X.Value;
            //var deltaY = c1.Y.Value - c2.Y.Value;

            //return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            const double R = 6371000; 
            var dLat = (c1.X - c2.X) * Math.PI / 180; 
            var dLon = (c1.Y - c2.Y) * Math.PI / 180;
            var a = Math.Sin(dLat.Value / 2) * Math.Sin(dLat.Value / 2) +
                       Math.Cos(c1.X.Value * Math.PI / 180) * Math.Cos(c2.X.Value * Math.PI / 180) *
                       Math.Sin(dLon.Value / 2) * Math.Sin(dLon.Value / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; 
        }

        private static int TimeSpanToFloat(TimeSpan time)
        {
            return time.Minutes;
        }
    }
}
