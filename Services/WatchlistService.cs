using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;

namespace RateNowApi.Services
{
    public class WatchlistService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WatchlistService> _logger;

        public WatchlistService(AppDbContext context, ILogger<WatchlistService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<WatchlistItem>> GetWatchlistAsync(int userId)
        {
            _logger.LogInformation("Service: Getting watchlist for UserId: {UserId}", userId);

            return await _context.WatchlistItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message, WatchlistItem? Item)>
            AddToWatchlistAsync(WatchlistItem item)
        {
            _logger.LogInformation("Service: Adding item to watchlist — UserId={UserId}", item.UserId);

            bool movieExists = await _context.Movies.AnyAsync(m => m.Id == item.MovieId);
            if (!movieExists)
                return (false, "Movie does not exist.", null);

            bool alreadyExists = await _context.WatchlistItems
                .AnyAsync(w => w.UserId == item.UserId && w.MovieId == item.MovieId);

            if (alreadyExists)
                return (false, "This movie already exists in the watchlist.", null);

            _context.WatchlistItems.Add(item);
            await _context.SaveChangesAsync();
            return (true, "Added successfully", item);
        }

        public async Task<bool> UpdateWatchlistItemStatusAsync(int id, WatchlistItem item)
        {
            if (id != item.Id)
                return false;

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromWatchlistAsync(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null)
                return false;

            _context.WatchlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}