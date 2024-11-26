using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PublicTransportNavigator;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.DTOs.Create;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketTypesController(ITicketTypeRepository repository) : Controller
    {
        private readonly ITicketTypeRepository _ticketTypeRepository = repository;
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketTypeCreateDTO dto)
        {
            var result = await _ticketTypeRepository.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _ticketTypeRepository.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
