﻿using PublicTransportNavigator.DTOs;

namespace PublicTransportNavigator.Dijkstra
{
    public interface IPathFinderManager
    {
        Task<RouteDetails> FindPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime);
    }
}