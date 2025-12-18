using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateNowApi.Models;
using RateNowApi.Services;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMoviesService moviesService, ILogger<MoviesController> logger)
        {
            _moviesService = moviesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _moviesService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _moviesService.GetMovieByIdAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            var addedMovie = await _moviesService.AddMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = addedMovie.Id }, addedMovie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            var updated = await _moviesService.UpdateMovieAsync(id, movie);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var deleted = await _moviesService.DeleteMovieAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}