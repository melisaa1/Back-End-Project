using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WatchlistController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/watchlist?userId=1 - Belirli bir kullanıcının listesini getir
        // [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchlistItem>>> GetWatchlist([FromQuery] int userId)
        {
            // Gerçek projede, userId token'dan alınmalıdır (Güvenlik)
            var watchlist = await _context.WatchlistItems
                .Where(item => item.UserId == userId)
                .ToListAsync();

            return Ok(watchlist); // 200 OK
        }

        // POST: api/watchlist - Listeye yeni film/dizi ekle
        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<WatchlistItem>> AddToWatchlist(WatchlistItem item)
        {
            // Kullanıcının daha önce ekleyip eklemediği kontrol edilebilir (İş Mantığı)
            _context.WatchlistItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWatchlist), new { userId = item.UserId }, item); // 201 Created
        }

        // PATCH: api/watchlist/5 - Listedeki öğenin durumunu güncelle (Planned, Watching, Completed)
        // PATCH yerine kolaylık için PUT kullanıyoruz, ancak PATCH daha doğrudur.
        // [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchlistItemStatus(int id, WatchlistItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            // Sadece 'Status' alanını güncelleyen mantık buraya yazılmalıdır.
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }


        // DELETE: api/watchlist/5 - Listeden öğeyi kaldır
        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.WatchlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}