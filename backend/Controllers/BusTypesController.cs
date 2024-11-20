using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusTypesController(IBusTypeRepository repository) : Controller
    {
        private readonly IBusTypeRepository _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _repository.GetById(id);
            return Ok(result);
        }

        [HttpGet("ByBus/{id}")]
        public async Task<IActionResult> GetByBusId(long id)
        {
            var result = await _repository.GetByBusId(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusTypeCreateDTO? busType)
        {
            if (busType == null) return BadRequest($"Parameter {nameof(busType)} was null");
            try
            {
                var result = await _repository.Create(busType);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error while creating {nameof(BusType)} entity", details = ex.Message });
            }
        }


    }
}

       
