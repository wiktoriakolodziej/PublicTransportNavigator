using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Repositories
{
    public class BusStopRepository(PublicTransportNavigatorContext context, IMapper mapper) : IBusStopRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        //private readonly ETagGenerator<BusStop> _etagGenerator = new(context);
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

        public async Task<BusStopDetailsDTO> GetDetails(long id)
        {
            var result = await (
                from busStop in _context.BusStops
                where busStop.Id == id
                select new BusStopDetailsDTO
                {
                    Id = id,
                    Name = busStop.Name,
                    OnRequest = busStop.OnRequest,
                    BusList = (
                        from timetable in _context.Timetables
                        where timetable.BusStopId == id
                        select new BusOnBusStopDTO
                        {
                            Id = timetable.BusId,
                            Number = (from bus in _context.Buses
                                      where bus.Id == timetable.BusId
                                      select bus.Number).First(),
                            Time = (from t in _context.Timetables
                                    where t.BusStopId == id && t.BusId == timetable.BusId
                                    select
                                        t.Time.ToString()
                                ).ToList()
                        }).GroupBy(b => b.Id)  // Group by BusId
                        .Select(g => g.First()).ToList()
                })
                .FirstAsync();
            return result;
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
                .OrderBy(bs=> bs.Name.Length)
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
