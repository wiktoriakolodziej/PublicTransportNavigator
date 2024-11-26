using Microsoft.AspNetCore.Mvc;
using PublicTransportNavigator.DTOs.old;
using PublicTransportNavigator.Migrations;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusesController(IBusRepository busRepository) : Controller
    {
        
        private readonly IBusRepository _busRepository = busRepository;

        // GET: Buses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buses = await busRepository.GetAll();
            return Ok(buses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var bus = await busRepository.GetById(id);
                if (bus == null) return NotFound();
                return Ok(bus);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }  
        }

        [HttpGet("seat/{busId}")]
        public async Task<IActionResult> GetBusSeatsForBus(long busId, [FromQuery] TimeSpan timeIn, [FromQuery] TimeSpan timeOut, [FromQuery] DateTime date)
        {
            try
            {
                var result = await _busRepository.GetBusSeatsForBus(busId,  timeIn, timeOut, date);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            { 
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusCreateDTO? busDto)
        {
            if (busDto == null) return BadRequest($"Parameter {busDto} was null");
            try
            {
                var result = await _busRepository.Create(busDto);
                return CreatedAtAction(nameof(GetById), new {id = result.Id}, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while creating {nameof(Bus)} entity", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long? id, [FromBody] BusCreateDTO? busDto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (busDto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _busRepository.Update(id.Value, busDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while updating {nameof(Bus)} entity of id: {id}", details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long? id, [FromBody] BusDTO? busDto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (busDto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _busRepository.Patch(id.Value, busDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while updating {nameof(Bus)} entity of id: {id}", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await busRepository.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while creating {nameof(Bus)} entity", details = ex.Message });
            }
        }

    }
}
