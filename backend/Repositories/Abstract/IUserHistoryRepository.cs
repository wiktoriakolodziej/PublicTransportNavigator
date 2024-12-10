using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IUserHistoryRepository
    {
        Task<IEnumerable<RoutePreview>> GetHistoryByUserId(long id);
        Task<HistoryDetailsDTO> GetById(long id);
    }
}
