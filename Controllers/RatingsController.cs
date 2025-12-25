using Microsoft.AspNetCore.Mvc;
using RateNowApi.DTOs.Ratings;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(
            IRatingService ratingService,
            ILogger<RatingsController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        // ----------------------------------------------------
        // GET ALL RATINGS
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings()
        {
            var ratings = await _ratingService.GetAllRatingsAsync();

            var result = ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                Score = r.Score,
                MovieId = r.MovieId,
                SeriesId = r.SeriesId,
                UserId = r.UserId
            });

            return Ok(result);
        }

        // ----------------------------------------------------
        // GET RATING BY ID
        // ----------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDto>> GetRating(int id)
        {
            var rating = await _ratingService.GetRatingByIdAsync(id);

            if (rating == null)
                return NotFound();

            var result = new RatingDto
            {
                Id = rating.Id,
                Score = rating.Score,
                MovieId = rating.MovieId,
                SeriesId = rating.SeriesId,
                UserId = rating.UserId
            };

            return Ok(result);
        }

        // ----------------------------------------------------
        // CREATE RATING
        // ----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<RatingDto>> PostRating(RatingCreateDto dto)
        {
            var rating = new Rating
            {
                Score = dto.Score,
                MovieId = dto.MovieId,
                SeriesId = dto.SeriesId,
                UserId = dto.UserId
            };

            var newRating = await _ratingService.AddRatingAsync(rating);

            var result = new RatingDto
            {
                Id = newRating.Id,
                Score = newRating.Score,
                MovieId = newRating.MovieId,
                SeriesId = newRating.SeriesId,
                UserId = newRating.UserId
            };

            return CreatedAtAction(nameof(GetRating),
                new { id = result.Id },
                result);
        }

        // ----------------------------------------------------
        // UPDATE RATING
        // ----------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, RatingUpdateDto dto)
        {
            var rating = new Rating
            {
                Id = id,
                Score = dto.Score,
                MovieId = dto.MovieId,
                SeriesId = dto.SeriesId,
                UserId = dto.UserId
            };

            var updated = await _ratingService.UpdateRatingAsync(id, rating);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ----------------------------------------------------
        // DELETE RATING
        // ----------------------------------------------------
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
