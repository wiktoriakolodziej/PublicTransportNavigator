using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface ISeatRepository
    {
        Task<IEnumerable<SeatDTO>> GetAll();
        Task<SeatDTO> GetById(long id);
        Task<SeatDTO> Create(SeatCreateDTO dto);
    }
}
