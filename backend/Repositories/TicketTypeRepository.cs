using AutoMapper;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class TicketTypeRepository(PublicTransportNavigatorContext context, IMapper mapper): ITicketTypeRepository
    {
        private readonly PublicTransportNavigatorContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<TicketTypeDTO> Create(TicketTypeCreateDTO dto)
        {
            var ticketType = new TicketType
            {
                LastModified = DateTime.UtcNow,
                Price = dto.Price,
                Time = TimeSpan.FromMinutes(dto.Time)
            };
            await _context.TicketTypes.AddAsync(ticketType);
            await _context.SaveChangesAsync();
            return _mapper.Map<TicketTypeDTO>(ticketType);
        }

        public async Task<TicketTypeDTO> GetById(long id)
        {
            var result = await _context.TicketTypes.FindAsync(id);
            return _mapper.Map<TicketTypeDTO>(result);
        }
    }
}
