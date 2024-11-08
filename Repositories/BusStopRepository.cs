using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class BusStopRepository(PublicTransportNavigatorContext context, IMapper mapper) : IBusStopRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ETagGenerator<BusStop> _etagGenerator = new(context);
        public async Task<BusStopDTO> Create(BusStopCreateDTO busStopDto)
        {
            var busStop = _mapper.Map<BusStop>(busStopDto);
            busStop.LastModified = DateTime.UtcNow;
            _context.BusStops.Add(busStop);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusStopDTO>(busStop);
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BusStopDTO?>> GetAll()
        {
            var result = await _context.BusStops
                .ToListAsync();
            return _mapper.Map<IEnumerable<BusStopDTO>>(result);
        }

        public async Task<BusStopDTO?> GetById(long id)
        {
            var result = await _context.BusStops
                .FindAsync(id);
            return _mapper.Map<BusStopDTO>(result);
        }

        public Task<BusStopDTO> Patch(long id, BusStopDTO bus)
        {
            throw new NotImplementedException();
        }

        public Task<BusStopDTO> Update(long id, BusStopCreateDTO bus)
        {
            throw new NotImplementedException();
        }

        public Task<BusStopDetailsDTO> GetDetails(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BusStopDTO>> GetByFragment(string fragment, long? id)
        {
            var result = await (
                    from bs in _context.BusStops
                    where bs.Name.ToLower().Contains(fragment.ToLower())
                    select new BusStopDTO
                    {
                        Id = bs.Id,
                        Name = bs.Name,
                    })
                .Union(
                    from fav in _context.UserFavouriteBusStops
                    join b in _context.BusStops on fav.BusStopId equals b.Id
                    where id != null && fav.UserId == id && b.Name.ToLower().Contains(fragment.ToLower())
                    select new BusStopDTO
                    {
                        Id = b.Id,
                        Name = fav.Name,
                    })
                .ToListAsync();
            return result.Count < 5 ? result : result[..5];
        }

        public async Task<IEnumerable<BusStopDTO>> GetFavourites(long id)
        {
            var result = await (
                    from f in _context.UserFavouriteBusStops
                    join b in _context.BusStops on f.BusStopId equals b.Id
                    where f.UserId == id
                    select new BusStopDTO
                    {
                        Id = b.Id,
                        Name = b.Name
                    })
                .ToListAsync();
            return result;

        }
    }
}
