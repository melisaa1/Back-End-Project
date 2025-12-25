using Microsoft.AspNetCore.Mvc;
using RateNowApi.DTOs.WatchList;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListService _service;
        private readonly ILogger<WatchListController> _logger;

        public WatchListController(
            IWatchListService service,
            ILogger<WatchListController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ----------------------------------------------------
        // GET WATCHLIST BY USER
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchListItemDto>>> GetWatchList(
            [FromQuery] int userId)
        {
            var list = await _service.GetWatchListAsync(userId);

            var result = list.Select(w => new WatchListItemDto
            {
                Id = w.Id,
                UserId = w.UserId,
                MovieId = w.MovieId,
                Status = w.Status
            });

            return Ok(result);
        }

        // ----------------------------------------------------
        // ADD TO WATCHLIST
        // ----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<WatchListItemDto>> AddToWatchList(
            WatchListCreateDto dto)
        {
            var item = new WatchListItem
            {
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                SeriesId = dto.SeriesId,
                Status = dto.Status
            };

            var result = await _service.AddToWatchListAsync(item);

            if (!result.Success)
                return BadRequest(result.Message);

            var response = new WatchListItemDto
            {
                Id = result.Item!.Id,
                UserId = result.Item.UserId,
                MovieId = result.Item.MovieId,
                SeriesId = result.Item.SeriesId,
                Status = result.Item.Status
            };

            return CreatedAtAction(
                nameof(GetWatchList),
                new { userId = response.UserId },
                response);
        }

        // ----------------------------------------------------
        // UPDATE WATCHLIST ITEM
        // ----------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchListitemStatus(
            int id,
            WatchListUpdateDto dto)
        {
            var item = new WatchListItem
            {
                Id = id,
                Status = dto.Status,
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                SeriesId = dto.SeriesId
            };

            bool ok = await _service.UpdateWatchListItemStatusAsync(id, item);

            if (!ok)
                return BadRequest("Invalid item or ID mismatch");

            return NoContent();
        }

        // ----------------------------------------------------
        // DELETE FROM WATCHLIST
        // ----------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchList(int id)
        {
            bool ok = await _service.RemoveFromWatchListAsync(id);

            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
