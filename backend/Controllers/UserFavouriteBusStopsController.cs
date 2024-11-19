using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserFavouriteBusStopsController(IUserFavouriteBusStopRepository repository, PublicTransportNavigatorContext context) : Controller
    {
        private readonly IUserFavouriteBusStopRepository _userFavouriteBusStopRepository = repository;
        private readonly ETagGenerator<UserFavouriteBusStop> _etagGenerator = new(context);

        // GET: BusStops
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var busStops = await _userFavouriteBusStopRepository.GetAll();
            return Ok(busStops);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var etag = _etagGenerator.GenerateEtag(id);
                Response.Headers["ETag"] = etag;
                if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"] == etag)
                    return StatusCode(304);
                var busStop = await _userFavouriteBusStopRepository.GetById(id);
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


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserFavouriteBusStopCreateDTO? userFavouriteBusStopDto)
        {
            if (userFavouriteBusStopDto == null) return BadRequest($"Parameter {userFavouriteBusStopDto} was null");
            try
            {
                var result = await _userFavouriteBusStopRepository.Create(userFavouriteBusStopDto);
                Response.Headers["ETag"] = _etagGenerator.GenerateEtag(result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        message = $"Unexpected error while creating {nameof(UserFavouriteBusStop)} entity", details = ex.Message
                    });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long? id, [FromBody] UserFavouriteBusStopCreateDTO? userFavouriteBusStopDto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (userFavouriteBusStopDto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _userFavouriteBusStopRepository.Update(id.Value, userFavouriteBusStopDto);
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
                        message = $"Unexpected error while updating {nameof(UserFavouriteBusStop)} entity of id: {id}",
                        details = ex.Message
                    });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long? id, [FromBody] UserFavouriteBusStopDTO? userFavouriteBusStopDto)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (userFavouriteBusStopDto == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _userFavouriteBusStopRepository.Patch(id.Value, userFavouriteBusStopDto);
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
                        message = $"Unexpected error while updating {nameof(UserFavouriteBusStop)} entity of id: {id}",
                        details = ex.Message
                    });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _userFavouriteBusStopRepository.Delete(id);
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
                        message = $"Unexpected error while creating {nameof(UserFavouriteBusStop)} entity", details = ex.Message
                    });
            }
        }
    }
}
