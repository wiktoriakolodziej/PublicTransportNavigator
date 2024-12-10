using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TimetablesController(ITimetableRepository repository) : Controller
    {
        private readonly ITimetableRepository _repository = repository;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var result = await repository.GetById(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{busStopId}/{busId}")]
        public async Task<IActionResult> GetByBusStopAndBus(int busStopId, int busId)
        {
            try
            {
                var result = await repository.GetByBusStopAndBus(busStopId, busId);
                if (result.IsNullOrEmpty()) return NotFound();
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("path")]
        public async Task<IActionResult> GetPath([FromQuery] long sourceBusStopId,
            [FromQuery] long destinationBusStopId, [FromQuery] TimeSpan departureTime, [FromQuery] int day)
        {
            try
            {
                var result = await repository.GetPath(sourceBusStopId, destinationBusStopId, departureTime, day);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("path/details/{pathId}")]
        public async Task<IActionResult> GetPathDetails(string pathId)
        {
            try
            {
                var result = await _repository.GetRouteDetails(pathId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimetableCreateDTO? dto)
        {
            if (dto == null) return BadRequest($"Parameter {dto} was null");
            try
            {
                var result = await repository.Create(dto);
                return CreatedAtAction(nameof(GetByBusStopAndBus), new { busStopId = result.First().BusStopId, busId = result.First().BusId}, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        message = $"Unexpected error while creating {nameof(Timetable)} entity", details = ex.Message
                    });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long? id, [FromBody] TimetableCreateDTO? dto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (dto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await repository.Update(id.Value, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        message = $"Unexpected error while updating {nameof(Timetable)} entity of id: {id}",
                        details = ex.Message
                    });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long? id, [FromBody] TimetableDTO? dto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (dto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await repository.Patch(id.Value, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        message = $"Unexpected error while updating {nameof(Timetable)} entity of id: {id}",
                        details = ex.Message
                    });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await repository.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        message = $"Unexpected error while creating {nameof(Timetable)} entity", details = ex.Message
                    });
            }
        }


    }
}
