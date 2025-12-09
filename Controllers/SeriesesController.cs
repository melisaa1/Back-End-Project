using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Series>>> GetSeries() // 3.1.2 async/await
        {
            if (_context.Serieses == null)
            {
                return NotFound(); 
            }
            return await _context.Serieses.ToListAsync();
        }

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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