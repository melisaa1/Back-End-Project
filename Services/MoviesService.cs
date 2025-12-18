using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Services
{
    public class MoviesService: IMoviesService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MoviesService> _logger;

        public MoviesService(AppDbContext context, ILogger<MoviesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            _logger.LogInformation("Fetching all movies");

            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            _logger.LogInformation("Fetching movie with ID: {MovieId}", id);

            return await _context.Movies
                .Include(m => m.Ratings)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            _logger.LogInformation("Adding new movie: {MovieTitle}", movie.Title);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<bool> UpdateMovieAsync(int id, Movie movie)
        {
            _logger.LogInformation("Updating movie with ID: {MovieId}", id);

            if (id != movie.Id)
                return false;

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Movies.AnyAsync(m => m.Id == id))
                {
                    _logger.LogWarning("Movie not found for update with ID: {MovieId}", id);
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            _logger.LogInformation("Deleting movie with ID: {MovieId}", id);

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}