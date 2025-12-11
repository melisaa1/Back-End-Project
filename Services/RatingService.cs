using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;

namespace RateNowApi.Services
{
    public class RatingService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RatingService> _logger;

        public RatingService(AppDbContext context, ILogger<RatingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            _logger.LogInformation("Fetching all ratings");
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating?> GetRatingByIdAsync(int id)
        {
            _logger.LogInformation("Fetching rating with ID: {RatingId}", id);
            return await _context.Ratings.FindAsync(id);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            _logger.LogInformation(
                "Creating new rating for MovieId: {MovieId} by UserId: {UserId}", 
                rating.MovieId, 
                rating.UserId);

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> UpdateRatingAsync(int id, Rating rating)
        {
            _logger.LogInformation("Updating rating with ID: {RatingId}", id);

            if (id != rating.Id)
                return false;

            _context.Entry(rating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Ratings.AnyAsync(r => r.Id == id);
                if (!exists)
                {
                    _logger.LogWarning("Rating not found for update with ID: {RatingId}", id);
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteRatingAsync(int id)
        {
            _logger.LogInformation("Deleting rating with ID: {RatingId}", id);

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
                return false;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}