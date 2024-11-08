using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IBusStopRepository
    {
        Task<IEnumerable<BusStopDTO?>> GetAll();
        Task<BusStopDTO?> GetById(long id);
        Task<BusStopDTO> Create(BusStopCreateDTO busDto);
        Task<BusStopDTO> Update(long id, BusStopCreateDTO bus);
        Task Delete(long id);
        Task<BusStopDTO> Patch(long id, BusStopDTO bus);
        Task<BusStopDetailsDTO> GetDetails(long id);
        Task<IEnumerable<BusStopDTO>> GetByFragment(string  fragment, long? id);
        Task<IEnumerable<BusStopDTO>> GetFavourites(long id);
    }
}
