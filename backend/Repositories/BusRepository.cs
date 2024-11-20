using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs.old;
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
    }
}
