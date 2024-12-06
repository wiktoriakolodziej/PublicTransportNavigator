using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservedSeatsController(IReservedSeatRepository repository) : Controller
    {
        private readonly IReservedSeatRepository _repository = repository;
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
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ReservedSeatCreateDTO dto)
        {
            try
            {
                var result = await _repository.Create(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("confirm/{id}")]
        [Authorize]
        public async Task<IActionResult> Confirm(long id)
        {
            try
            {
                var result = await _repository.Confirm(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
