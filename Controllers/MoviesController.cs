using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")] // 3.1.1 Attribute Routing ve RESTful isimlendirme
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Dependency Injection ile DbContext alınıyor (3.1.6 DI)
        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. READ (GET) - Tüm Filmleri Listele (3.1.1 CRUD)
        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies() // 3.1.2 async/await
        {
            if (_context.Movies == null)
            {
                // 404 Not Found durum kodu (3.1.1)
                return NotFound(); 
            }
            return await _context.Movies.ToListAsync();
        }

        // 2. READ (GET) - Belirli Bir Filmi Getir (3.1.1 CRUD)
        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            // Filmi Ratings ve Reviews ile birlikte çekme (Opsiyonel)
            var movie = await _context.Movies
                .Include(m => m.Ratings)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                // 404 Not Found durum kodu (3.1.1)
                return NotFound();
            }

            return movie;
        }

        // 3. CREATE (POST) - Yeni Film Ekle (3.1.1 CRUD)
        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (_context.Movies == null)
            {
                // 500 Internal Server Error (3.1.1)
                return Problem("Entity set 'AppDbContext.Movies'  is null.");
            }
            
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(); // 3.1.2 async/await

            // 201 Created durum kodu ve yeni kaynağın yeri (3.1.1)
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // 4. UPDATE (PUT) - Filmi Tamamen Güncelle (3.1.1 CRUD)
        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                // 400 Bad Request durum kodu (3.1.1)
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // 3.1.5 Logging and Error Handling
                }
            }

            // 204 No Content durum kodu (3.1.1)
            return NoContent();
        }

        // 5. DELETE - Filmi Sil (3.1.1 CRUD)
        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            // 204 No Content durum kodu (3.1.1)
            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}