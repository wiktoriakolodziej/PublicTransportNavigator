using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Dijkstra.AStar
{
    public class AStarPathFinder(IServiceScopeFactory serviceProvider) : DijkstraPathFinder<NodeAs>(serviceProvider)
    {
        private const double R = 6371000;
        protected override void SyncNodes(long calendarId)
        {
            Available = Task.Run(async () =>
            {
                var nodes = new Dictionary<long, NodeAs>();
                List<List<Timetable>> timetables;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                    foreach (var siblings in context.BusStops.GroupBy(bs =>bs.Name)
                                 .ToDictionary(g => g.Key, g => g))
                    {
                        foreach (var busStop in siblings.Value)
                        {
                            var node = new NodeAs
                            {
                                BusStopId = busStop.Id,
                                BestArrivalTime = TimeSpan.MaxValue,
                                PreviousNodeId = -1,
                                PreviousBusId = -1,
                                Connections = [],
                                BestDepartureTime = TimeSpan.MaxValue,
                                BestWeight = double.MaxValue,
                                Checked = false,
                                Coordinate = new Coordinate
                                {
                                    X = busStop.CoordX,
                                    Y = busStop.CoordY,
                                }
                            };
                            foreach (var siblingBusStop in siblings.Value.Except([busStop]))
                            {
                                var connection = new Connection
                                {
                                    BusId = 0,
                                    ConnectionTime = 0,
                                    DepartureTime = TimeSpan.Zero,
                                    From = busStop.Id,
                                    To = siblingBusStop.Id,
                                };
                                var sortedSet = new SortedSet<Connection>(new ConnectionComparer())
                                {
                                    connection
                                };
                                node.Connections.Add(connection.To, sortedSet);
                            }
                            nodes.Add(busStop.Id, (NodeAs)node);
                        }
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
                    for (var i = 0; i < ttForBusId.Count - 1; i++)
                    {
                        var firstBusStopId = ttForBusId[i].BusStopId;
                        var nextBusStopId = ttForBusId[i + 1].BusStopId;
                        var connection = new Connection
                        {
                            BusId = busId,
                            DepartureTime = ttForBusId[i].Time,
                            ConnectionTime = (ttForBusId[i + 1].Time - ttForBusId[i].Time).Minutes,
                            From = firstBusStopId,
                            To = nextBusStopId,
                        };
                        nodes.TryGetValue(firstBusStopId, out var currentNode);
                        if (currentNode == null) continue;
                        if (currentNode!.Connections.ContainsKey(connection.To))
                        {
                            currentNode.Connections.TryGetValue(connection.To, out var sortedSet);
                            sortedSet?.Add(connection);
                        }
                        else
                        {
                            var sortedSet = new SortedSet<Connection>(new ConnectionComparer())
                            {
                                connection
                            };
                            currentNode?.Connections.Add(connection.To, sortedSet);
                        }
                    }
                }
                _graphs.Add(calendarId, nodes);
            });
        }
        protected override void Recursive(NodeAs parentNode, long currentBusStopId, long destinationBusStopId, long calendarId)
        {
            while (parentNode.BusStopId != destinationBusStopId)
            {
                //change status of the current node; it is checked from now on
                var departureTime = parentNode.BestArrivalTime;
                parentNode.Checked = true;

                //update all neighbours of the current node
                //foreach neighbour bus stop find the closest time when passenger can get there from this bus stop
                foreach (var connection in parentNode.Connections)
                {
                    _graphs[calendarId].TryGetValue(connection.Key, out var destinationStop);

                    var fastestConnection = FindFastestConnection(departureTime, connection.Value);
                    if (fastestConnection == null) continue;

                    //aggregate time = starting time + waiting at the bus stop time + bus connection time
                    var aggregateTime =
                        departureTime.Add(fastestConnection.DepartureTime - departureTime)
                            .Add(TimeSpan.FromMinutes(fastestConnection.ConnectionTime));

                    //weight = total time in minutes + distance from destination in meters
                    var weight = TimeSpanToDouble(aggregateTime);
                    try
                    {
                        weight += GetDistance(destinationStop.Coordinate, destinationBusStopId, calendarId);
                    }
                    catch (Exception ex)
                    {
                        //very unlikely
                    }

                    //update bus stop that connection leads to if user would be there faster than previous best time
                    if (destinationStop!.BestArrivalTime <= aggregateTime) continue;

                    destinationStop.BestArrivalTime = aggregateTime;
                    destinationStop.PreviousNodeId = currentBusStopId;
                    destinationStop.PreviousBusId = fastestConnection.BusId;
                    destinationStop.BestWeight = weight;

                    parentNode.BestDepartureTime = fastestConnection.DepartureTime;
                    _pQueue.Enqueue(destinationStop, weight);
                }

                //find node with the min best time that is not yet checked
                try
                {
                    //var nextNode = _graphs[calendarId] //!
                    //    .Where(pair => pair.Value.Checked == false)
                    //    .MinBy(pair => pair.Value.BestWeight);
                    parentNode = _pQueue.Dequeue();
                    while (parentNode.Checked)
                    {
                        parentNode = _pQueue.Dequeue();
                    }

                    currentBusStopId = parentNode.BusStopId;
                    //this means the algorithm has found the shortest path
                    //if (parentNode.BusStopId == destinationBusStopId) return;

                    //Recursive(nextNode, nextNode.BusStopId, destinationBusStopId, calendarId);
                }
                //that means that the algorithm has checked all the nodes and haven't found the destination node (unlikely)
                catch (Exception ex)
                {
                }
            }
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
            if (destinationNode == null) throw new ArgumentException();
            var c2 = destinationNode.Coordinate;
        
            if (c1 == null || c2 == null || !c1.X.HasValue || !c1.Y.HasValue || !c2.X.HasValue || !c2.Y.HasValue)
            {
                throw new ArgumentException("Both coordinates must have valid X and Y values.");
            }

            if (c1.X.Equals(c2.X) && c1.Y.Equals(c2.Y))
            {
                return 0;
            }
            var dLat = (c1.X - c2.X) * Math.PI / 180; 
            var dLon = (c1.Y - c2.Y) * Math.PI / 180;
            var a = Math.Sin(dLat!.Value / 2) * Math.Sin(dLat.Value / 2) +
                       Math.Cos(c1.X.Value * Math.PI / 180) * Math.Cos(c2.X.Value * Math.PI / 180) *
                       Math.Sin(dLon!.Value / 2) * Math.Sin(dLon.Value / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (R * c)/84; 
        }
        private double TimeSpanToDouble(TimeSpan time)
        {
            return time.TotalMinutes;
        }
    }
}
