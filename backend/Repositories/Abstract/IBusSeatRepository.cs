using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IBusSeatRepository
    {
        Task<IEnumerable<BusSeatDTO>> Create(BusSeatCreateDTO dto);
        Task<IEnumerable<BusSeatDTO>> GetAll();
        Task<BusSeatDTO> Patch(long id, BusSeatDTO seatDTO);
    }
}
