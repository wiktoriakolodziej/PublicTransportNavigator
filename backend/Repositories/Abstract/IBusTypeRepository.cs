using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IBusTypeRepository
    {
        Task<IEnumerable<BusTypeDTO?>> GetAll();
        Task<BusTypeDTO> Create(BusTypeCreateDTO busTypeDto);
        Task<BusTypeDTO?> GetById(long id);
        Task<BusTypeDTO?> GetByBusId(long id);
    }
}
