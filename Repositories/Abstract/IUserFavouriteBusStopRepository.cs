using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs;

namespace PublicTransportNavigator.Repositories.Abstract
{
    public interface IUserFavouriteBusStopRepository
    {
        Task<IEnumerable<UserFavouriteBusStopDTO?>> GetAll();
        Task<UserFavouriteBusStopDTO?> GetById(long id);
        Task<UserFavouriteBusStopDTO> Create(UserFavouriteBusStopCreateDTO busDto);
        Task<UserFavouriteBusStopDTO> Update(long id, UserFavouriteBusStopCreateDTO bus);
        Task Delete(long id);
        Task<UserFavouriteBusStopDTO> Patch(long id, UserFavouriteBusStopDTO bus);
    }
}
