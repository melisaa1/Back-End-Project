using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")] // RESTful isimlendirme: /api/series
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Dependency Injection (DI)
        public SeriesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. READ (GET) - Tüm Dizileri Listele (3.1.1 CRUD)
        // GET: api/series
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Series>>> GetSeries() // 3.1.2 async/await
        {
            if (_context.Serieses == null)
            {
                return NotFound(); 
            }
            return await _context.Serieses.ToListAsync();
        }

        // 2. READ (GET) - Belirli Bir Diziyi Getir (3.1.1 CRUD)
        // GET: api/series/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Series>> GetSeries(int id)
        {
            if (_context.Serieses == null)
            {
                return NotFound();
            }
            
            var series = await _context.Serieses
                .Include(s => s.Ratings) // Rating/Review koleksiyonları modele eklenmelidir.
                .FirstOrDefaultAsync(s => s.Id == id);

            if (series == null)
            {
                return NotFound(); // 404 Not Found
            }

            return series;
        }

        // 3. CREATE (POST) - Yeni Dizi Ekle (3.1.1 CRUD)
        // POST: api/series
        // [Authorize(Roles = "Admin")] // Admin yetkilendirmesi (Daha sonra eklenebilir)
        [HttpPost]
        public async Task<ActionResult<Series>> PostSeries(Series series)
        {
            if (_context.Serieses == null)
            {
                return Problem("Entity set 'AppDbContext.Series' is null.");
            }
            
            _context.Serieses.Add(series);
            await _context.SaveChangesAsync();

            // 201 Created durum kodu (3.1.1)
            return CreatedAtAction(nameof(GetSeries), new { id = series.Id }, series);
        }

        // 4. UPDATE (PUT) - Diziyi Tamamen Güncelle (3.1.1 CRUD)
        // PUT: api/series/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeries(int id, Series series)
        {
            if (id != series.Id)
            {
                return BadRequest(); // 400 Bad Request
            }

            _context.Entry(series).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeriesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content
        }

        // 5. DELETE - Diziyi Sil (3.1.1 CRUD)
        // DELETE: api/series/1
        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            if (_context.Serieses == null)
            {
                return NotFound();
            }
            var series = await _context.Serieses.FindAsync(id);
            if (series == null)
            {
                return NotFound();
            }

            _context.Serieses.Remove(series);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        private bool SeriesExists(int id)
        {
            return (_context.Serieses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}