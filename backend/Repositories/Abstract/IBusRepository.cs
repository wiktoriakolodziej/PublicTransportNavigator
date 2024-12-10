using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IBusRepository
    {
        Task<IEnumerable<BusDTO?>> GetAll();
        Task<BusDTO?> GetById(long id);
        Task<BusDTO> Create(BusCreateDTO busDto);
        Task<BusDTO> Update(long id, BusCreateDTO bus);
        Task Delete(long id);
        Task<BusDTO> Patch(long id, BusDTO bus);
        Task<IEnumerable<BusSeatDTO>> GetBusSeatsForBus(long busId, TimeSpan timeIn, TimeSpan timeOut, DateTime date);
    }
}