using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class TimetableRepository(PublicTransportNavigatorContext context, IMapper mapper, IPathFinderManager pathFinder) : ITimetableRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IPathFinderManager _pathFinder = pathFinder;
        public async Task<IEnumerable<TimetableDTO>> Create(TimetableCreateDTO dto)
        {
            List<TimetableDTO> result = [];
            foreach (var time in dto.Time)
            {
                var timetable = new Timetable
                {
                    BusId = dto.BusId,
                    BusStopId = dto.BusStopId,
                    Time = time,
                    LastModified = DateTime.UtcNow,
                };
                _context.Timetables.Add(timetable);
                result.Add(_mapper.Map<TimetableDTO>(timetable));
            }
            await _context.SaveChangesAsync();
            return result;
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TimetableDTO?>> GetAll()
        {
            var result = await _context.Timetables
                .ToListAsync();
            return _mapper.Map<IEnumerable<TimetableDTO>>(result);
        }

        public Task<IEnumerable<TimetableDTO>> GetByBusStopAndBus(int busStopId, int busId)
        {
            throw new NotImplementedException();
        }

        public async Task<TimetableDTO?> GetById(long id)
        {
            var result = await _context.Timetables
                .FindAsync(id);
            return _mapper.Map<TimetableDTO>(result);
        }

        public async Task<RoutePreview> GetPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime)
        {
            var result =  await _pathFinder.FindPath(sourceBusStopId, destinationBusStopId, departureTime);
            return GetPreview(result);
        }

        public async Task<TimetableDTO> Patch(long id, TimetableDTO dto)
        {
            var timetable = await _context.Buses.FindAsync(id);
            if (timetable == null) throw new KeyNotFoundException($"Entity {nameof(Timetable)} with id: {id} was not found");
            _mapper.Map(dto, timetable);
            await _context.SaveChangesAsync();
            return _mapper.Map<TimetableDTO>(timetable);
        }

        public Task<TimetableDTO> Update(long id, TimetableCreateDTO bus)
        {
            throw new NotImplementedException();
        }

        private static RoutePreview GetPreview(RouteDetails details) => new RoutePreview()
        {
            Id = 1,
            DepartureTime = details.DepartureTime,
            DestinationTime = details.DestinationTime,
            TravelTime = details.TravelTime,
            BusNumbers = details.Parts.Keys.Reverse().ToList(),
            Coordinates = details.Coordinates,
        };

    }
}
