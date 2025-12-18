using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateNowApi.Models;
using RateNowApi.Services;
using RateNowApi.Services.Interfaces;   

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesesService _seriesService;
        private readonly ILogger<SeriesController> _logger;

        public SeriesController(ISeriesesService seriesService, ILogger<SeriesController> logger)
        {
            _seriesService = seriesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Series>>> GetSeries()
        {
            var series = await _seriesService.GetAllSeriesAsync();
            return Ok(series);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Series>> GetSeries(int id)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);

            if (series == null)
                return NotFound();

            return Ok(series);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Series>> PostSeries(Series series)
        {
            var newSeries = await _seriesService.AddSeriesAsync(series);

            return CreatedAtAction(nameof(GetSeries), new { id = newSeries.Id }, newSeries);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeries(int id, Series series)
        {
            var updated = await _seriesService.UpdateSeriesAsync(id, series);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            var deleted = await _seriesService.DeleteSeriesAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}