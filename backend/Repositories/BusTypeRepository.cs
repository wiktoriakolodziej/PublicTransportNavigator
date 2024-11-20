using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Models.Enums;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Repositories
{
    public class BusTypeRepository(IMapper mapper, PublicTransportNavigatorContext context) : IBusTypeRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PublicTransportNavigatorContext _context = context;
        public async Task<BusTypeDTO> Create(BusTypeCreateDTO busTypeDto)
        {
            var busType = new BusType
            {
                ImagePath = busTypeDto.ImagePath,
                Type = (BusTypeEnum)busTypeDto.Type,
                LastModified = DateTime.UtcNow,
            };
            _context.BusType.Add(busType);
            await _context.SaveChangesAsync();
            return _mapper.Map<BusTypeDTO>(busType);
        }

        public async Task<IEnumerable<BusTypeDTO?>> GetAll()
        {
            var result = await _context.BusType
                .ToListAsync();
            return _mapper.Map<IEnumerable<BusTypeDTO>>(result);
        }

        public async Task<BusTypeDTO?> GetById(long id)
        {
            var result = await _context.BusType.FindAsync(id);
            return _mapper.Map<BusTypeDTO>(result);
        }

        public async Task<BusTypeDTO?> GetByBusId(long id)
        {
            var typeId = _context.Buses.FindAsync(id).Result?.TypeId;
            if (typeId == null)
                throw new KeyNotFoundException($"{nameof(BusType)} with id {id} was not found in the database");
            var result = await _context.BusType.FindAsync(typeId);
            return _mapper.Map<BusTypeDTO>(result);
        }
    }
}
