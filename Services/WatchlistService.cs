using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;   

namespace RateNowApi.Services
{
    public class WatchListService: IWatchListService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WatchListService> _logger;

        public WatchListService(AppDbContext context, ILogger<WatchListService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<WatchListItem>> GetWatchListAsync(int userId)
        {
            _logger.LogInformation("Service: Getting watchList for UserId: {UserId}", userId);

            return await _context.WatchListItems
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message, WatchListItem? Item)>
            AddToWatchListAsync(WatchListItem item)
        {
            _logger.LogInformation("Service: Adding item to watchList — UserId={UserId}", item.UserId);

            bool movieExists = await _context.Movies.AnyAsync(m => m.Id == item.MovieId);
            if (!movieExists)
                return (false, "Movie does not exist.", null);

            bool alreadyExists = await _context.WatchListItems
                .AnyAsync(w => w.UserId == item.UserId && w.MovieId == item.MovieId);

            if (alreadyExists)
                return (false, "This movie already exists in the watchList.", null);

            _context.WatchListItems.Add(item);
            await _context.SaveChangesAsync();
            return (true, "Added successfully", item);
        }

        public async Task<bool> UpdateWatchListItemStatusAsync(int id, WatchListItem item)
        {
            if (id != item.Id)
                return false;

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromWatchListAsync(int id)
        {
            var item = await _context.WatchListItems.FindAsync(id);
            if (item == null)
                return false;

            _context.WatchListItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}