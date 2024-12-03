using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
   
    public class UserHistoryRepository(PublicTransportNavigatorContext context) : IUserHistoryRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;

        public async Task<HistoryDetailsDTO> GetById(long id)
        {

            var result = await (from h in _context.UserTravels
                          where h.Id == id
                          select new HistoryDetailsDTO
                          {
                              DepartureTime = (from rs in _context.ReservedSeats
                                               join t in _context.Timetables on rs.TimeInId equals t.Id
                                               where rs.UserTravelId == h.Id
                                               select t.Time).Min(),
                              DestinationTime = (from rs in _context.ReservedSeats
                                                 join t in _context.Timetables on rs.TimeOffId equals t.Id
                                                 where rs.UserTravelId == h.Id
                                                 select t.Time).Max(),
                              Coordinates = (from rs in _context.ReservedSeats
                                      .Include(r => r.TimeIn)
                                      .Include(r => r.TimeOff)
                                      .Include(r => r.BusSeat)
                                  where rs.UserTravelId == h.Id
                                  select (
                                      from t in _context.Timetables.Include(t => t.BusStop)
                                      where t.BusId == rs.BusSeat.BusId && t.Time >= rs.TimeIn.Time && t.Time <= rs.TimeOff.Time
                                      select new Coordinate
                                      {
                                          X = t.BusStop.CoordX,
                                          Y = t.BusStop.CoordY

                                      }).Distinct().ToList()).SelectMany(coords => coords).Distinct().ToList(),
                              DepartureStopName = (from rs in _context.ReservedSeats
                                  join t in _context.Timetables.Include(t => t.BusStop) on rs.TimeInId equals t.Id
                                  where rs.UserTravelId == h.Id
                                  select t.BusStop.Name).First(),
                              DestinationStopName = (from rs in _context.ReservedSeats
                                  join t in _context.Timetables.Include(t => t.BusStop) on rs.TimeOffId equals t.Id
                                  where rs.UserTravelId == h.Id
                                  select t.BusStop.Name).First(),
                              Id = h.Id.ToString(),
                              TravelTime = (from rs in _context.ReservedSeats
                                  join t in _context.Timetables on rs.TimeOffId equals t.Id
                                  where rs.UserTravelId == h.Id
                                  select t.Time).Max().TotalMinutes - (from rs in _context.ReservedSeats
                                  join t in _context.Timetables on rs.TimeInId equals t.Id
                                  where rs.UserTravelId == h.Id
                                  select t.Time).Min().TotalMinutes
                          }).FirstAsync();
            return result;
        }

        public async Task<IEnumerable<RoutePreview>> GetHistoryByUserId(long id)
        {

            var result = await (
                from history in _context.UserTravels
                join rs in _context.ReservedSeats
                        .Include(rs => rs.TimeIn).Include(rs => rs.TimeOff).Include(rs => rs.BusSeat).ThenInclude(bs => bs!.Bus)
                    on history.Id equals rs.UserTravelId
                where history.UserId == id
                group rs by rs.UserTravelId into g
                select new RoutePreview
                {
                    DepartureTime = g.AsQueryable().Min(seat => seat.TimeIn.Time),
                    DestinationTime = g.AsQueryable().Max(seat => seat.TimeOff.Time),
                    BusNumbers = (from bs in g
                                  select bs.BusSeat.Bus.Number).Distinct().ToList(),
                    TravelTime = (g.AsQueryable().Max(seat => seat.TimeOff.Time) - g.AsQueryable().Min(seat => seat.TimeIn.Time)).Minutes,
                    Id = g.Key.ToString()
                }).ToListAsync();
            return result;
        }
    }
}
