using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicTransportNavigator;
using PublicTransportNavigator.Dijkstra;
using PublicTransportNavigator.DTOs;
using PublicTransportNavigator.Models;
using PublicTransportNavigator.Repositories.Abstract;
using PublicTransportNavigator.Services;

namespace PublicTransportNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTravelsController(IUserHistoryRepository repository) : Controller
    {
        private readonly IUserHistoryRepository _repository = repository;
        [HttpGet("preview/{id}/{page}/{pageSize}")]
        public async Task<IActionResult> GetHistoryPreviewByUserId(long id, int page, int pageSize)
        {
            var result = await _repository.GetHistoryByUserId(id);
            var paginated = PaginatedList<RoutePreview>.Create(result, page, pageSize);
            return Ok(paginated);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _repository.GetById(id);
            return Ok(result);
        }

    }
}
