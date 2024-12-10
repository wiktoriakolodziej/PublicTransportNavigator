using Microsoft.AspNetCore.Mvc;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatsController(ISeatRepository repository) : Controller
    {
        private readonly ISeatRepository _repository = repository;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var seat = await _repository.GetById(id);
                return Ok(seat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeatCreateDTO dto)
        {
            var seat = await _repository.Create(dto);
            return Ok(seat);

        }
    }
}
