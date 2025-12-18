using Microsoft.AspNetCore.Mvc;
using RateNowApi.Models;
using RateNowApi.Services;
using RateNowApi.Services.Interfaces;


namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _service;
        private readonly ILogger<WatchlistController> _logger;

        public WatchlistController(IWatchlistService service, ILogger<WatchlistController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistItem>>> GetWatchlist([FromQuery] int userId)
        {
            var list = await _service.GetWatchlistAsync(userId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> AddToWatchlist(WatchlistItem item)
        {
            var result = await _service.AddToWatchlistAsync(item);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetWatchlist),
                new { userId = item.UserId }, result.Item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchlistItemStatus(int id, WatchlistItem item)
        {
            bool ok = await _service.UpdateWatchlistItemStatusAsync(id, item);

            if (!ok)
                return BadRequest("Invalid item or ID mismatch");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            bool ok = await _service.RemoveFromWatchlistAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}