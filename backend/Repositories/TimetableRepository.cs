using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs.old;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Models.Enums;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;
using StackExchange.Redis;

namespace PublicTransportNavigator.Repositories
{
    public class TimetableRepository(PublicTransportNavigatorContext context, IMapper mapper, IPathFinderManager pathFinder, RedisCacheService redisCacheService) : ITimetableRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IPathFinderManager _pathFinder = pathFinder;
        private readonly RedisCacheService _redisCacheService = redisCacheService;
        private const int DefaultExpiryTime = 5;
        private const int ExtendedExpiryTime = 10;
      
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
                    CalendarId = dto.CalendarId
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

        public async Task<RoutePreview> GetPath(long sourceBusStopId, long destinationBusStopId, TimeSpan departureTime, int dayOfWeek)
        {
            var dayPredicate = dayOfWeek switch
            {
                (int)DaysOfWeekEnum.Monday => (Expression<Func<Calendar, bool>>)(c => c.Monday),
                (int)DaysOfWeekEnum.Tuesday => c => c.Tuesday,
                (int)DaysOfWeekEnum.Wednesday => c => c.Wednesday,
                (int)DaysOfWeekEnum.Thursday => c => c.Thursday,
                (int)DaysOfWeekEnum.Friday => c => c.Friday,
                (int)DaysOfWeekEnum.Saturday => c => c.Saturday,
                (int)DaysOfWeekEnum.Sunday => c => c.Sunday,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Invalid day of the week.")
            };
            var calendarEntity = await _context.Calendar.FirstOrDefaultAsync(dayPredicate);
            var result =  await _pathFinder.FindPath(sourceBusStopId, destinationBusStopId, departureTime, calendarEntity!.Id);
            var routeDetailsJson = JsonConvert.SerializeObject(result);
            _redisCacheService.SetAsync(result.Id.ToString(), routeDetailsJson, TimeSpan.FromMinutes(DefaultExpiryTime));
            return GetPreview(result);
        }

        public async Task<RouteDetailsDTO> GetRouteDetails(string routeId)
        {
            var jsonString = await _redisCacheService.GetAsync(routeId);
            if (jsonString == RedisValue.Null) throw new KeyNotFoundException($"Entity of id {routeId} was not found in the database");
            var route = JsonConvert.DeserializeObject<RouteDetails>(jsonString);
            _redisCacheService.ProlongKeyLifetime(routeId, TimeSpan.FromMinutes(ExtendedExpiryTime));
            return _mapper.Map<RouteDetailsDTO>(route);
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
            Id = details.Id,
            DepartureTime = details.DepartureTime,
            DestinationTime = details.DestinationTime,
            TravelTime = details.TravelTime,
            BusNumbers = details.Parts.Values
                .Select(part => part.BusName)
                .ToList(),
            Coordinates = details.Coordinates,
        };

    }
}
