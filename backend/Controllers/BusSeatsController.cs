using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusSeatsController(IBusSeatRepository repository) : Controller
    {
        private readonly IBusSeatRepository _repository = repository;
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusSeatCreateDTO? dto)
        {
            try
            {
                var result = await _repository.Create(dto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long? id, [FromBody] BusSeatDTO? busStopDTO)
        {
            if (id == null) return BadRequest($"Parameter {nameof(id)} was null");
            if (busStopDTO == null)
                return BadRequest("Body was null");
            try
            {
                var result = await _repository.Patch(id.Value, busStopDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
