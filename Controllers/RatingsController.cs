using Microsoft.AspNetCore.Mvc;
using RateNowApi.Models;
using RateNowApi.Services;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(IRatingService ratingService, ILogger<RatingsController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
        {
            var ratings = await _ratingService.GetAllRatingsAsync();
            return Ok(ratings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            var rating = await _ratingService.GetRatingByIdAsync(id);

            if (rating == null)
                return NotFound();

            return Ok(rating);
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            var newRating = await _ratingService.AddRatingAsync(rating);

            return CreatedAtAction(nameof(GetRating), 
                new { id = newRating.Id }, 
                newRating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            var updated = await _ratingService.UpdateRatingAsync(id, rating);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var deleted = await _ratingService.DeleteRatingAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}