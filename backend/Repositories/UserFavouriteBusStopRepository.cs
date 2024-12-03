using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Repositories
{
    public class UserFavouriteBusStopRepository(PublicTransportNavigatorContext context, IMapper mapper) : IUserFavouriteBusStopRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        //private readonly ETagGenerator<BusStop> _etagGenerator = new(context);
        public async Task<UserFavouriteBusStopDTO> Create(UserFavouriteBusStopCreateDTO busDto)
        {
            var busStop = _mapper.Map<UserFavouriteBusStop>(busDto);
            busStop.LastModified = DateTime.UtcNow;
            _context.UserFavouriteBusStops.Add(busStop);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserFavouriteBusStopDTO>(busStop);
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserFavouriteBusStopDTO?>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserFavouriteBusStopDTO?> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<UserFavouriteBusStopDTO> Patch(long id, UserFavouriteBusStopDTO bus)
        {
            throw new NotImplementedException();
        }

        public Task<UserFavouriteBusStopDTO> Update(long id, UserFavouriteBusStopCreateDTO bus)
        {
            throw new NotImplementedException();
        }
    }
}
