using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class BusRepository(PublicTransportNavigatorContext context, IMapper mapper) : IBusRepository
    {

        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<BusDTO?>> GetAll()
        {
            var buses = await _context.Buses
                .Include(b => b.FirstBusStop)
                .Include(b => b.LastBusStop)
                .Include(b => b.Type)
                .ToListAsync();
            return _mapper.Map<List<BusDTO>>(buses);

        }
        public async Task<BusDTO?> GetById(long id)
        {
            var bus = await _context.Buses
                .Include(b => b.FirstBusStop)
                .Include(b => b.LastBusStop)
                .Include(b=> b.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<BusDTO>(bus);
        }
        public async Task<BusDTO> Create(BusCreateDTO busDto)
        {
            var bus = _mapper.Map<Bus>(busDto);
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusDTO>(bus);
        }
        public async Task<BusDTO> Patch(long id, BusDTO busDto)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) throw new KeyNotFoundException($"Entity {nameof(Bus)} with id: {id} was not found");
            _mapper.Map(busDto, bus);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusDTO>(bus);
        }

        public async Task<BusDTO> Update(long id, BusCreateDTO busDto)
        {
            var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == id);
            if (bus == null) throw new KeyNotFoundException($"{nameof(Bus)} with id: {id} was not found");
            _mapper.Map(busDto, bus);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusDTO>(bus);
        }
        public async Task Delete(long id)
        {
            var bus = await _context.Buses
                .FindAsync(id);
            if (bus == null) 
                throw new KeyNotFoundException($"{nameof(Bus)} not found with id {id}");
            _context.Remove(bus);
            await _context.SaveChangesAsync();
        }

        public async  Task<IEnumerable<BusSeatDTO>> GetBusSeatsForBus(long busId, TimeSpan timeIn, TimeSpan timeOut, DateTime date)
        {
            var newDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

            //var result = await _context.BusSeats.Where(bs => bs.BusId == busId).ToListAsync();
            var result = await (
                    from bs in _context.BusSeats
                    where bs.BusId == busId
                    select new BusSeatDTO
                    {
                        Id = bs.Id,
                        BusId = busId,
                        Coordinate = new Coordinate
                        {
                            X = bs.CoordX,
                            Y = bs.CoordY,
                        },
                        SeatType = bs.SeatType.SeatType.ToString(),
                        Available = !_context.ReservedSeats.Any(rs =>
                        rs.BusSeatId == bs.Id &&
                        rs.Date.Day == newDate.Day && rs.Date.Month == newDate.Month && rs.Date.Year == newDate.Year &&
                        (
                        (timeIn >= rs.TimeIn.Time && timeIn < rs.TimeOff.Time) ||
                        (timeOut > rs.TimeIn.Time && timeOut <= rs.TimeOff.Time) ||
                        (timeIn <= rs.TimeIn.Time && timeOut >= rs.TimeOff.Time)
                        )),
                        ImagePath = _context.SeatTypes.Where(st => st.Id == bs.SeatTypeId).Select(st => st.ImagePath).First()
                    })
                .ToListAsync();

            if (result.IsNullOrEmpty()) throw new KeyNotFoundException($"{nameof(Bus)} of id: {busId} doesn't exist or has no seats");
            return _mapper.Map<IEnumerable<BusSeatDTO>>(result);
        }
    }
}
