using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface ITimetableRepository
    {
        Task<IEnumerable<TimetableDTO?>> GetAll();
        Task<TimetableDTO?> GetById(long id);
        Task<IEnumerable<TimetableDTO>> GetByBusStopAndBus(int  busStopId, int busId);
        Task<RoutePreview?> GetPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime, int dayOfWeek);
        Task<RouteDetailsDTO> GetRouteDetails(string routeId);
        Task<IEnumerable<TimetableDTO>> Create(TimetableCreateDTO busDto);
        Task<TimetableDTO> Update(long id, TimetableCreateDTO bus);
        Task Delete(long id);
        Task<TimetableDTO> Patch(long id, TimetableDTO bus);
    }
}
