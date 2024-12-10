using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Models.Enums;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class BusSeatRepository(PublicTransportNavigatorContext context, IMapper mapper) : IBusSeatRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<BusSeatDTO>> Create(BusSeatCreateDTO dto)
        {
            List<BusSeat> result = [];
            foreach (var coordinate in dto.Coordinate)
            {
                var busSeat = new BusSeat
                {
                    BusId = dto.BusId,
                    CoordX = coordinate.X,
                    CoordY = coordinate.Y,
                    SeatTypeId = dto.SeatType,
                    LastModified = DateTime.UtcNow
                };
                _context.BusSeats.Add(busSeat);
                result.Add(busSeat);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<IEnumerable<BusSeatDTO>>(result);
        }

        public async Task<IEnumerable<BusSeatDTO>> GetAll()
        {
            var result = await _context.BusSeats.ToListAsync();
            return _mapper.Map<IEnumerable<BusSeatDTO>>(result);
        }

        public async Task<BusSeatDTO> Patch(long id, BusSeatDTO? seatDTO)
        {
            var seat = await _context.BusSeats.FindAsync(id);
            seatDTO.BusId = seatDTO.BusId ??  seat.BusId;
            if (seat == null) throw new KeyNotFoundException($"Entity {nameof(BusSeat)} with id: {id} was not found");
            _mapper.Map(seatDTO, seat);
            if (!seatDTO.SeatType.IsNullOrEmpty())
                seat.SeatTypeId = await _context.SeatTypes.Where(st => st.SeatType.ToString() == seatDTO.SeatType)
                    .Select(st => st.Id).FirstAsync();
            await _context.SaveChangesAsync();
            return _mapper.Map<BusSeatDTO>(seat);
        }
    }
}
