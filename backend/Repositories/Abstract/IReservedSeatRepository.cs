using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IReservedSeatRepository
    {
        Task<IEnumerable<ReservedSeatDTO>> GetAll();
        Task<ReservedSeatDTO> GetById(long id);
        Task<ReservedSeatDTO> Create(ReservedSeatCreateDTO dto);
        Task Delete (long id);
        Task<int> Confirm (long id);
    }
}
