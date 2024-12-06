using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Dijkstra.AStar
{
  
    public class AStarPathFinder(IServiceScopeFactory serviceProvider) : DijkstraPathFinder(serviceProvider)
    {
        private const double R = 6371000;
        protected override void CalculatePath(Node parentNode, long currentBusStopId, long destinationBusStopId, long calendarId, TimeSpan firstDepartureTime)
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
    }
}
