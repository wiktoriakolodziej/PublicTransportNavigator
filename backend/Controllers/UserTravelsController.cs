using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicTransportNavigator.Dijkstra;
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
        [Authorize]
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
