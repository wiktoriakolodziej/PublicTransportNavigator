using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class ReservedSeatRepository(PublicTransportNavigatorContext context, IMapper mapper) : IReservedSeatRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<int> Confirm(long id)
        {
            var result = await _context.ReservedSeats
                .Where(rs => rs.UserTravelId == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(rs => rs.ValidUntil, (DateTime?)null));
            if (result == 0) throw new KeyNotFoundException("No rows were affected by the call");
            return result;
        }

        public async Task<ReservedSeatDTO> Create(ReservedSeatCreateDTO dto)
        {
            UserTravel travel;
            if (dto.UserTravelId == null)
            {
                travel = new UserTravel
                {
                    LastModified = DateTime.UtcNow,
                    UserId = dto.UserId,
                };
                await _context.UserTravels.AddAsync(travel);
                await _context.SaveChangesAsync();
            }
            else
            {
                travel = await _context.UserTravels.FindAsync(dto.UserTravelId.Value);
                if (travel == null)
                    throw new KeyNotFoundException($"{nameof(UserTravel)} of id: {dto.UserTravelId} was not found");
            }

            var date = DateTime.ParseExact(dto.Date, "yyyy-MM-dd", CultureInfo.InstalledUICulture);
            date = date.ToUniversalTime();
            var reservedSeat = new ReservedSeat
            {
                LastModified = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow + TimeSpan.FromMinutes(5),
                UserTravelId = travel.Id,
                BusSeatId = dto.BusSeatId,
                Date = date,
                TimeInId = await(
                    from t in _context.Timetables join bs in _context.BusSeats on t.BusId equals bs.BusId
                    where bs.Id == dto.BusSeatId && t.Time.ToString() == dto.TimeIn
                    select t.Id)
                    .FirstAsync(),
                TimeOffId = await(
                    from t in _context.Timetables join bs in _context.BusSeats on t.BusId equals bs.BusId
                    where bs.Id == dto.BusSeatId && t.Time.ToString() == dto.TimeOff
                    select t.Id)
                    .FirstAsync()
            };
            
            await _context.ReservedSeats.AddAsync(reservedSeat);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReservedSeatDTO>(reservedSeat);
        }

        public async Task Delete(long id)
        {
            var seat = await _context.ReservedSeats.FindAsync(id);
            if (seat == null) throw new KeyNotFoundException($"{nameof(ReservedSeat)} of id: {id} was not found"); 
            _context.ReservedSeats.Remove(seat);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReservedSeatDTO>> GetAll()
        {
            var result = await _context.ReservedSeats.ToListAsync();
            return _mapper.Map<IEnumerable<ReservedSeatDTO>>(result);
        }

        public async Task<ReservedSeatDTO> GetById(long id)
        {
            var seat = await _context.ReservedSeats.FindAsync(id);
            return _mapper.Map<ReservedSeatDTO>(seat);
        }
    }
}
