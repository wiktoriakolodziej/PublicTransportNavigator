using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Xml.Linq;
using PublicTransportNavigator.Dijkstra.AStar;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Dijkstra
{
    public class DijkstraPathFinder<TNode> (IServiceScopeFactory serviceProvider) where TNode : Node
    {
        protected readonly Dictionary<long, Dictionary<long, TNode>> _graphs = [];
        protected readonly IServiceScopeFactory _serviceProvider = serviceProvider;
        protected PriorityQueue<TNode, double> _pQueue = new PriorityQueue<TNode, double>();
        public Task? Available { get; protected set; }
        public void PrepareGraphs()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
            var calendar = context.Calendar;
            foreach (var calendarEntry in calendar)
            {
                SyncNodes(calendarEntry.Id);
            }
            //for (var day = 0; day < 7; day++)
            //{
            //   _graphs.Add(new Dictionary<long, TNode>());
            //}
            //for (var day = 0; day < 7; day++)
            //{
            //    SyncNodes(day);
            //}
        }
        protected virtual void SyncNodes(long calendarId)
        {
           
            Available = Task.Run(async () =>
            {
                var nodes = new Dictionary<long, TNode>();
                List<List<Timetable>> timetables;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                    foreach (var busStop in context.BusStops)
                    {
                        var node = new Node
                        {
                            BusStopId = busStop.Id,
                            BestArrivalTime = TimeSpan.MaxValue,
                            PreviousNodeId = -1,
                            PreviousBusId = -1,
                            Connections = [],
                            BestDepartureTime = TimeSpan.MaxValue
                        };
                        nodes.Add(busStop.Id, (TNode)node);
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
                        var firstBusStopId = ttForBusId[i].BusId;
                        var nextBusStopId = ttForBusId[i + 1].BusId;
                        var connection = new Connection
                        {
                            BusId = busId,
                            DepartureTime = ttForBusId[i].Time,
                            ConnectionTime = (ttForBusId[i + 1].Time - ttForBusId[i].Time).Minutes,
                            From = firstBusStopId,
                            To = nextBusStopId,
                        };
                        nodes.TryGetValue(firstBusStopId, out var currentNode);
                        if(currentNode == null) continue;
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
                //foreach (var ttForBusId in timetables)
                //{
                //    var busId = ttForBusId[0].BusId;
                //    var busStopIds = ttForBusId.Select(t => t.BusStopId).Distinct().ToList();
                //    var bestTimes = new Dictionary<TimeSpan, Connection>();
                //    foreach (var busStopId in busStopIds)
                //    {
                //        var times = ttForBusId.Where(t => t.BusStopId == busStopId && t.BusId == busId)
                //            .Select(t => t.Time).ToHashSet();
                        
                //        var connection = new Connection
                //        {
                //            DepartureTimes = times,
                //            From = busStopId,
                //            BusId = busId,
                //        };
                //        bestTimes.TryAdd(times.Min(), connection);
                //    }

                //    while (bestTimes.Count > 1)
                //    {
                //        var first = bestTimes.Keys.Min();
                //        bestTimes.TryGetValue(first, out var firstConnection);
                //        bestTimes.Remove(first);

                //        var second = bestTimes.Keys.Min();
                //        bestTimes.TryGetValue(second, out var secondConnection);

                //        firstConnection.ConnectionTime = (second - first).Minutes;
                //        firstConnection.To = secondConnection.From;

                //        nodes.TryGetValue(firstConnection.From, out var fromNode);
                //        if(!fromNode.Connections.ContainsKey(firstConnection.From))
                //        {
                //            fromNode.Connections.TryAdd(firstConnection.From, new List<Connection>{ firstConnection});
                //        }
                //        else
                //        {
                //            from VAR in COLLECTION 
                //        }
                        

                //        nodes.TryGetValue(firstConnection.To, out var toNode);
                //        toNode.Connections.Add(firstConnection.From, firstConnection);
                //    }
                //}
                _graphs.Add(calendarId, nodes);
            });
        }
        public RouteDetails FindPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime, long calendarId)
        {
            _pQueue.Clear();
            var nodes = _graphs[calendarId];
            //get the first node and set up its best time
            nodes.TryGetValue(sourceBusStopId, out var firstNode);
            nodes.TryGetValue(sourceBusStopId, out var lastNode);
            if (firstNode == null || lastNode == null) throw new ArgumentException($"Missing bus stop of id {sourceBusStopId} or {destinationBusStopId}");
            firstNode.BestArrivalTime = departureTime;

            Recursive(firstNode, sourceBusStopId, destinationBusStopId, calendarId);
            var result = GetBestPath(destinationBusStopId, calendarId);

            return result;

        }
        public virtual void CleanUpNodes(long calendarId)
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
                    }
                }
            );
        }
        protected Connection? FindFastestConnection(TimeSpan targetTime, SortedSet<Connection> connections)
        {
            var x = new Connection
            {
                DepartureTime = targetTime,
                ConnectionTime = 0,
            };
            if (connections!.FirstOrDefault()!.DepartureTime == TimeSpan.Zero) return new Connection
            {
                DepartureTime = targetTime,
                ConnectionTime = 1
            };
            var view = connections.GetViewBetween(x, new Connection
            {
                DepartureTime = TimeSpan.MaxValue,
                ConnectionTime = 0
            });
            return view.FirstOrDefault();
        }
        protected virtual void Recursive(TNode parentNode, long currentBusStopId, long destinationBusStopId, long calendarId)
        {
            ////change status of the current node; it is checked from now on
            //var departureTime = parentNode.BestArrivalTime;
            //parentNode.Checked = true;

            ////update all neighbours of the current node
            //foreach (var connection in parentNode.Connections.Where(c => c.From == currentBusStopId))
            //{
            //    _graphs[calendarId].TryGetValue(connection.To, out var destinationStop);
            //    var closestDepartureTime = FindClosestAfter(departureTime, connection.DepartureTimes);
            //    if (closestDepartureTime == null) continue;
            //    var aggregateTime =
            //        departureTime.Add(closestDepartureTime.Value - departureTime).Add(TimeSpan.FromMinutes(connection.ConnectionTime));

            //    if (destinationStop!.BestArrivalTime <= aggregateTime) continue;

            //    destinationStop.BestArrivalTime = aggregateTime;
            //    destinationStop.PreviousNodeId = currentBusStopId;
            //    destinationStop.PreviousBusId = connection.BusId;
            //    parentNode.BestDepartureTime = closestDepartureTime.Value;

            //}

            ////find node with the min best time that is not yet checked
            //try
            //{
            //    var nextNode = _graphs[calendarId]
            //        .Where(pair => pair.Value.Checked == false)
            //        .MinBy(pair => pair.Value.BestArrivalTime);

            //    if (nextNode.Value.BestArrivalTime == TimeSpan.MaxValue) return;

            //    //this means the algorithm has found the shortest path
            //    if (nextNode.Key == destinationBusStopId) return;

            //    Recursive(nextNode.Value, nextNode.Key, destinationBusStopId, calendarId);
            //}
            ////that means that the algorithm has checked all the nodes and haven't found the destination node (unlikely)
            //catch (Exception ex) { }

        }
        private RouteDetails GetBestPath(long destinationBusStopId, long calendarId)
        {
            var nodes = _graphs[calendarId];
            nodes.TryGetValue(destinationBusStopId, out var node);
            RouteDetails result = new()
            {
                DestinationTime = node.BestArrivalTime,
                Id = Guid.NewGuid().ToString(),
            };
            
            var stopNumber = destinationBusStopId;
            long previousBusNumber = -1;

            while (node!.PreviousNodeId != -1)
            {
                Console.WriteLine($"bus stop id: {node.BusStopId}, bus id: {node.PreviousBusId}, time: {node.BestArrivalTime}");
                nodes.TryGetValue(node.PreviousNodeId.Value, out node);
                //var busNumber = node.PreviousBusId;
                //if (busNumber == 0) continue;
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                //    var busStopData = (context.BusStops.FirstOrDefault(bs => bs.Id == stopNumber));
                //    result.Coordinates.Add(new Coordinate
                //    {
                //        X = busStopData.CoordX,
                //        Y = busStopData.CoordY,
                //    });
                //    if (!result.Parts.ContainsKey(busNumber.Value))
                //    {
                //        var part = new RoutePart
                //        {
                //            BusName = busNumber != 0 ? (context.Buses.FirstOrDefault(b => b.Id == busNumber)).Number : "by foot",
                //        };
                //        part.Details.Add(node.BestArrivalTime, busStopData.Name);
                //        result.Parts.Add(busNumber.Value, part);
                //        if (previousBusNumber != -1)
                //        {
                //            result.Parts.TryGetValue(previousBusNumber, out part);
                //            part.Details.Add(node.BestDepartureTime, busStopData.Name);
                //            part.Details = part.Details
                //                .Reverse()
                //                .ToDictionary(pair => pair.Key, pair => pair.Value);
                //        }
                //    }
                //    else
                //    {
                //        result.Parts.TryGetValue(busNumber.Value, out var part);
                //        part.Details.Add(node.BestArrivalTime, busStopData.Name);

                //    }
                //}
                //stopNumber = node.PreviousNodeId.Value;
                //previousBusNumber = busNumber.Value;
                //nodes.TryGetValue(node.PreviousNodeId.Value, out node);

                //if (node!.PreviousNodeId != -1) continue;

                //result.DepartureTime = node.BestArrivalTime;
                //result.TravelTime = (result.DestinationTime - result.DepartureTime).Minutes;
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    var context = scope.ServiceProvider.GetRequiredService<PublicTransportNavigatorContext>();
                //    result.Parts.TryGetValue(busNumber.Value, out var firstPart);
                //    var firstBusStop = context.BusStops.FirstOrDefault(bs => bs.Id == stopNumber);
                //    firstPart.Details.Add(node.BestDepartureTime, firstBusStop.Name);
                //    result.Coordinates.Add(new Coordinate
                //    {
                //        X = firstBusStop.CoordX,
                //        Y = firstBusStop.CoordY,
                //    });
                //    firstPart.Details = firstPart.Details
                //        .Reverse()
                //        .ToDictionary(pair => pair.Key, pair => pair.Value);
                //}
            }
            result.Parts = result.Parts
                .Reverse()
                .ToDictionary(pair => pair.Key, pair => pair.Value); 
            return result;
        }
    }
}
