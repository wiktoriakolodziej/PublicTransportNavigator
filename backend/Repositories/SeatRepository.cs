using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class SeatRepository(PublicTransportNavigatorContext context, IMapper mapper) : ISeatRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<SeatDTO> Create(SeatCreateDTO dto)
        {
            var seat = _mapper.Map<Seat>(dto);
            seat.LastModified = DateTime.UtcNow;
            await _context.AddAsync(seat);
            await _context.SaveChangesAsync();
            return _mapper.Map<SeatDTO>(seat);
        }

        public async Task<IEnumerable<SeatDTO>> GetAll()
        {
            var seats = await _context.SeatTypes.ToListAsync();
            return _mapper.Map<IEnumerable<SeatDTO>>(seats);
        }

        public async Task<SeatDTO> GetById(long id)
        {
            var seat = await _context.SeatTypes.FindAsync(id);
            if (seat == null) throw new KeyNotFoundException($"{nameof(BusSeat)} with id {id} was not found in teh database");
            return _mapper.Map<SeatDTO>(seat);
        }
    }
}
