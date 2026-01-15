using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RateNowApi.DTOs.Movies;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            IMoviesService moviesService,
            ILogger<MoviesController> logger)
        {
            _moviesService = moviesService;
            _logger = logger;
        }

        // ----------------------------------------------------
        // GET ALL MOVIES
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _moviesService.GetAllMoviesAsync();

            var result = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title
            });

            return Ok(result);
        }

        // ----------------------------------------------------
        // GET MOVIE BY ID
        // ----------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _moviesService.GetMovieByIdAsync(id);

            if (movie == null)
                return NotFound();

            var result = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title
            };

            return Ok(result);
        }

        // ----------------------------------------------------
        // CREATE MOVIE (ADMIN)
        // ----------------------------------------------------
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title
            };

            var addedMovie = await _moviesService.AddMovieAsync(movie);

            var result = new MovieDto
            {
                Id = addedMovie.Id,
                Title = addedMovie.Title
            };

            return CreatedAtAction(nameof(GetMovie), new { id = result.Id }, result);
        }


        // ----------------------------------------------------
        // UPDATE MOVIE (ADMIN)
        // ----------------------------------------------------
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
        {
            var movie = new Movie
            {
                Id = id,
                Title = dto.Title
            };

            var updated = await _moviesService.UpdateMovieAsync(id, movie);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // ----------------------------------------------------
        // DELETE MOVIE (ADMIN)
        // ----------------------------------------------------
        [Authorize(Roles = "admin")]
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
