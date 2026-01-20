using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateNowApi.DTOs.Series;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;
    
namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesesService _seriesService;
        private readonly ILogger<SeriesController> _logger;

        public SeriesController(
            ISeriesesService seriesService,
            ILogger<SeriesController> logger)
        {
            _seriesService = seriesService;
            _logger = logger;
        }

        // ----------------------------------------------------
        // GET ALL SERIES
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeriesDto>>> GetSeries()
        {
            var seriesList = await _seriesService.GetAllSeriesAsync();

            var result = seriesList.Select(s => new SeriesDto
            {
                Id = s.Id,
                Title = s.Title,
                Seasons = s.Seasons
            });

            return Ok(result);
        }

        // ----------------------------------------------------
        // GET SERIES BY ID
        // ----------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<SeriesDto>> GetSeries(int id)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);

            if (series == null)
                return NotFound();

            var result = new SeriesDto
            {
                Id = series.Id,
                Title = series.Title,
                Seasons = series.Seasons
            };

            return Ok(result);
        }

        // ----------------------------------------------------
        // CREATE SERIES (ADMIN)
        // ----------------------------------------------------
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<SeriesDto>> PostSeries(SeriesCreateDto dto)
        {
            var series = new Series
            {
                Title = dto.Title,
                Seasons = dto.Seasons
            };

            var newSeries = await _seriesService.AddSeriesAsync(series);

            var result = new SeriesDto
            {
                Id = newSeries.Id,
                Title = newSeries.Title,
                Seasons = newSeries.Seasons
            };

            return CreatedAtAction(nameof(GetSeries),
                new { id = result.Id },
                result);
        }


        // UPDATE SERIES (ADMIN)
        
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeries(int id, SeriesUpdateDto dto)
        {
            var series = new Series
            {
                Id = id,
                Title = dto.Title,
                Seasons = dto.Seasons
            };

            var updated = await _seriesService.UpdateSeriesAsync(id, series);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE SERIES (ADMIN)
        [Authorize(Roles = "admin")]
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
