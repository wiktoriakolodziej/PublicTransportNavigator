using Microsoft.AspNetCore.Mvc;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusStopsController(IBusStopRepository repository, PublicTransportNavigatorContext context) : Controller
    {
        private readonly IBusStopRepository _busStopRepository = repository;
        //private readonly ETagGenerator<BusStop> _etagGenerator = new(context);

        // GET: BusStops
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var busStops = await _busStopRepository.GetAll();
            return Ok(busStops);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                //var etag = _etagGenerator.GenerateEtag(id);
                //Response.Headers["ETag"] = etag;
                //if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"] == etag)
                //    return StatusCode(304);
                var busStop = await _busStopRepository.GetById(id);
                if (busStop == null) return NotFound();
                return Ok(busStop);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(long id)
        {
            var details = await _busStopRepository.GetDetails(id);
            return Ok(details);
        }

        [HttpGet("fragment/{fragment}")]
        public async Task<IActionResult> GetByFragment(string fragment, [FromQuery] long? userId)
        {
            var result = await _busStopRepository.GetByFragment(fragment, userId);
            return Ok(result);
        }

        [HttpGet("favourites/{userId}")]
        public async Task<IActionResult> GetFavourites(long userId)
        {
            var result = await _busStopRepository.GetFavourites(userId);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusStopCreateDTO? busStopDTO)
        {
            if (busStopDTO == null) return BadRequest($"Parameter {busStopDTO} was null");
            try
            {
                var result = await _busStopRepository.Create(busStopDTO);
                //Response.Headers["ETag"] = _etagGenerator.GenerateEtag(result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while creating {nameof(BusStop)} entity", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long? id, [FromBody] BusStopCreateDTO? busStopCreateDTO)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (busStopCreateDTO == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _busStopRepository.Update(id.Value, busStopCreateDTO);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while updating {nameof(BusStop)} entity of id: {id}", details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long? id, [FromBody] BusStopDTO? busStopDTO)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (busStopDTO == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _busStopRepository.Patch(id.Value, busStopDTO);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while updating {nameof(BusStop)} entity of id: {id}", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _busStopRepository.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while creating {nameof(BusStop)} entity", details = ex.Message });
            }
        }
    }

}
