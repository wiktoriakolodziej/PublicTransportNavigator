using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface ITicketTypeRepository
    {
        Task<TicketTypeDTO> Create(TicketTypeCreateDTO dto);
        Task<TicketTypeDTO> GetById(long id);
    }
}
