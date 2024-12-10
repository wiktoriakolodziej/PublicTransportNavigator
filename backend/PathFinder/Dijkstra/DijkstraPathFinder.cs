using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.PathFinder.Dijkstra
{
    public class DijkstraPathFinder(IServiceScopeFactory serviceProvider)
    {
        protected readonly Dictionary<long, Dictionary<long, Node>> _graphs = [];
        protected readonly IServiceScopeFactory _serviceProvider = serviceProvider;
        protected PriorityQueue<Node, double> _pQueue = new PriorityQueue<Node, double>();
        public Task? Available { get; protected set; }
        public void PrepareGraphs()
        {
            using var scope = _serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
            var calendar = context.Calendar;
            foreach (var calendarEntry in calendar)
            {
                SyncNodes(calendarEntry.Id);
            }
        }
        protected void SyncNodes(long calendarId)
        {
            Available = Task.Run(async () =>
            {
                var nodes = new Dictionary<long, Node>();
                List<List<Timetable>> timetables;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                    foreach (var siblings in context.BusStops.GroupBy(bs => bs.Name)
                                 .ToDictionary(g => g.Key, g => g))
                    {
                        if (siblings.Key.Contains("granica") || siblings.Key.Contains("[tech]")) continue;
                        foreach (var busStop in siblings.Value)
                        {
                            var node = new Node
                            {
                                BusStopId = busStop.Id,
                                BestArrivalTime = TimeSpan.MaxValue,
                                PreviousNodeId = -1,
                                PreviousBusId = -1,
                                Connections = [],
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
                                    DepartureTime = TimeSpan.MaxValue,
                                    From = busStop.Id,
                                    To = siblingBusStop.Id,
                                };
                                var sortedSet = new SortedSet<Connection>(new ConnectionComparer())
                                {
                                    connection
                                };
                                node.Connections.Add(connection.To, sortedSet);
                            }
                            nodes.Add(busStop.Id, node);
                        }
                    }

                    timetables = await context.Timetables
                        .Join(
                            context.BusStops,
                            timetable => timetable.BusStopId,
                            busStop => busStop.Id,
                            (timetable, busStop) => new { timetable, busStop }
                        )
                        .Where(joined =>
                                joined.timetable.CalendarId == calendarId &&
                                !joined.busStop.Name.Contains("granica") &&
                                !joined.busStop.Name.Contains("[tech]")
                        )
                        .GroupBy(joined => joined.timetable.BusId)
                        .Select(group => group
                            .OrderBy(item => item.timetable.Time)
                            .Select(item => item.timetable)
                            .ToList()
                        )
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
        public async Task<RouteDetails> FindPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime, long calendarId)
        {
            _pQueue.Clear();
            var nodes = _graphs[calendarId];
            //get the first node and set up its best time
            nodes.TryGetValue(sourceBusStopId, out var firstNode);
            nodes.TryGetValue(sourceBusStopId, out var lastNode);
            if (firstNode == null || lastNode == null) throw new ArgumentException($"Missing bus stop of id {sourceBusStopId} or {destinationBusStopId}");
            firstNode.BestArrivalTime = departureTime;

            CalculatePath(firstNode, sourceBusStopId, destinationBusStopId, calendarId, departureTime);
            var result = await GetBestPath(destinationBusStopId, calendarId);

            return result;
        }
        public void CleanUpNodes(long calendarId)
        {
            Available = Task.Run(() =>
                {
                    foreach (var node in _graphs[calendarId])
                    {
                        node.Value.BestArrivalTime = TimeSpan.MaxValue;
                        //node.Value.BestDepartureTime = TimeSpan.MaxValue;
                        node.Value.PreviousBusId = -1;
                        node.Value.PreviousNodeId = -1;
                        node.Value.Checked = false;
                        node.Value.BestWeight = double.MaxValue;
                    }
                }
            );
        }
        protected Connection? FindFastestConnection(TimeSpan targetTime, SortedSet<Connection> connections)
        {
            var x = new Connection
            {
                DepartureTime = targetTime.Subtract(TimeSpan.FromDays(targetTime.Days)),
                ConnectionTime = 0,
            };
            if (connections!.LastOrDefault()!.DepartureTime == TimeSpan.MaxValue) return new Connection
            {
                DepartureTime = targetTime.Subtract(TimeSpan.FromDays(targetTime.Days)),
                ConnectionTime = 1
            };
            var view = connections.GetViewBetween(x, new Connection
            {
                DepartureTime = TimeSpan.MaxValue,
                ConnectionTime = 0
            });
            return view.IsNullOrEmpty() ? connections.First() : view.FirstOrDefault();
        }
        protected virtual void CalculatePath(Node parentNode, long currentBusStopId, long destinationBusStopId, long calendarId, TimeSpan firstDepartureTime)
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
                    if (destinationStop!.Checked) continue;

                    var fastestConnection = FindFastestConnection(departureTime, connection.Value);
                    if (fastestConnection == null) continue;

                    //aggregate time = starting time + waiting at the bus stop time + bus connection time
                    var aggregateTime =
                        departureTime.Add(GetTimeSpanBetweenTimes(departureTime, fastestConnection.DepartureTime))
                            .Add(TimeSpan.FromMinutes(fastestConnection.ConnectionTime));

                    //weight = difference between the departure time and calculated time in minutes
                    var weight = TimeSpanToDouble(firstDepartureTime, aggregateTime);

                    //update bus stop that connection leads to if user would be there faster than previous best time
                    if (destinationStop!.BestArrivalTime <= aggregateTime) continue;

                    destinationStop.BestArrivalTime = aggregateTime;
                    destinationStop.PreviousNodeId = currentBusStopId;
                    destinationStop.PreviousBusId = fastestConnection.BusId;
                    destinationStop.BestWeight = weight;
                    destinationStop.TravelTime = fastestConnection.ConnectionTime;

                    _pQueue.Enqueue(destinationStop, weight);
                }

                //find node with the min best time that is not yet checked
                try
                {
                    var newParentNode = _pQueue.Dequeue();
                    while (newParentNode.Checked)
                    {
                        newParentNode = _pQueue.Dequeue();
                    }
                    //parentNode.BestDepartureTime = newParentNode.BestArrivalTime - newParentNode.BestConnectionTime;
                    parentNode = newParentNode;
                    currentBusStopId = parentNode.BusStopId;
                }
                //that means that the algorithm has checked all the nodes and haven't found the destination node (unlikely)
                catch (Exception ex)
                {
                }
            }

        }
        private async Task<RouteDetails> GetBestPath(long destinationBusStopId, long calendarId)
        {
            var nodes = _graphs[calendarId];
            nodes.TryGetValue(destinationBusStopId, out var node);
            RouteDetails result = new()
            {
                DestinationTime = new TimeSpan(0, node.BestArrivalTime.Hours, node.BestArrivalTime.Minutes, 0),
                Id = Guid.NewGuid().ToString(),
            };
            var destinationTimeWithDays = node.BestArrivalTime;

            while (node.PreviousBusId == 0)
            {
                nodes.TryGetValue(node.PreviousNodeId!.Value, out node);
            }

            Dictionary<long, string> busesWithNames, busStopsWithNames;
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                busesWithNames = await context.Buses.ToDictionaryAsync(b => b.Id, b => b.Number);
                busStopsWithNames = await context.BusStops.ToDictionaryAsync(bs => bs.Id, bs => bs.Name);
            }

            var previousTravelTime = 0;
            var timeAtPreviousBusStop = TimeSpan.Zero;

            while (node!.PreviousNodeId != -1)
            {
                Console.WriteLine(
                    $"bus stop id: {node.BusStopId}, bus id: {node.PreviousBusId}, time: {node.BestArrivalTime}");
                var currentBusId = node.PreviousBusId;
                result.Coordinates.Add(node.Coordinate!);
                busesWithNames.TryGetValue(node.PreviousBusId!.Value, out var busName);
                busStopsWithNames.TryGetValue(node.BusStopId, out var busStopName);
                var part = new RoutePart
                {
                    BusId = currentBusId!.Value,
                    BusName = busName ?? "undefined bus name",
                    Details =
                    {
                         new TimeSpan(0, node.BestArrivalTime.Hours, node.BestArrivalTime.Minutes, 0) + " " + (busStopName ?? "undefined bus stop name")
                    }
                };
                previousTravelTime = node.TravelTime;
                timeAtPreviousBusStop = node.BestArrivalTime;


                nodes.TryGetValue(node.PreviousNodeId!.Value, out node);
                while (node!.PreviousBusId == currentBusId || node.PreviousBusId == 0)
                {
                    if (node.PreviousBusId != 0 && currentBusId != 0)
                    {
                        busStopsWithNames.TryGetValue(node.BusStopId, out busStopName);
                        part.Details.Add(new TimeSpan(0, node.BestArrivalTime.Hours, node.BestArrivalTime.Minutes, 0) + " " + (busStopName ?? "undefined bus stop name"));
                        previousTravelTime = node.TravelTime;
                        timeAtPreviousBusStop = node.BestArrivalTime;
                    }
                    nodes.TryGetValue(node.PreviousNodeId!.Value, out node);
                }

                //add the first bus stop (also the last bus stop for another connection)
                busStopsWithNames.TryGetValue(node.BusStopId, out busStopName);
                var x = timeAtPreviousBusStop.Subtract(TimeSpan.FromMinutes(previousTravelTime));
                part.Details.Add(new TimeSpan(0, x.Hours, x.Minutes, 0) + " " + (busStopName ?? "undefined bus stop name"));


                part.Details.Reverse();
                result.Parts.Add(part);
            }
            result.DepartureTime = timeAtPreviousBusStop.Subtract(TimeSpan.FromMinutes(previousTravelTime));
            result.TravelTime = destinationTimeWithDays.TotalMinutes - result.DepartureTime.TotalMinutes;
            result.Coordinates.Add(node!.Coordinate!);

            result.Parts.Reverse();
            return result;
        }
        protected double TimeSpanToDouble(TimeSpan departureTime, TimeSpan time)
        {
            return (time - departureTime).TotalMinutes;
        }
        protected TimeSpan GetTimeSpanBetweenTimes(TimeSpan first, TimeSpan second)
        {
            if (first <= second) return second - first;

            TimeSpan days;
            var firstTimeInMinutes = first.Hours * 60 + first.Minutes;
            var secondTimeInMinutes = second.Hours * 60 + second.Minutes;

            if (first.Days < 1 || firstTimeInMinutes > secondTimeInMinutes) days = TimeSpan.FromDays(first.Days + 1);
            else days = TimeSpan.FromDays(first.Days);

            return days.Subtract(first - second);
        }
    }
}
