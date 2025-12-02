using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;
using System.Security.Claims;

namespace RateNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users/5 - Kullanıcı profilini getir
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            // Parola hash'ini ifşa etmemek için seçici kolon çekimi yapılmalıdır (DTO gereksinimi)
            var user = await _context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    // Diğer ilgili koleksiyonlar (Ratings, Reviews vb.)
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PATCH: api/users/5 - Profil Bilgisini Güncelle (3.1.1 PATCH kullanımı)
        // [Authorize] (Sadece kullanıcı kendi profilini güncelleyebilir)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, User userChanges)
        {
            // DTO kullanılmadığı için sadece bir alanın güncellendiğini varsayalım.
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Sadece kullanıcı adı güncellensin (Örnek)
            if (!string.IsNullOrEmpty(userChanges.UserName))
            {
                user.UserName = userChanges.UserName;
            }

            await _context.SaveChangesAsync();
            return NoContent(); // 204 No Content
        }

        // POST: api/users/5/friends/10 - Arkadaş Ekle/Takip Et (Çoka-Çok İlişkisi Yönetimi)
        // [Authorize]
        [HttpPost("{userId}/friends/{friendId}")]
        public async Task<IActionResult> AddFriend(int userId, int friendId)
        {
             // İş mantığı ve Çoka-Çok ilişkisi burada yönetilir
             // User.Friends koleksiyonunu güncelleyen kod burada olmalıdır.

             return Ok(new { Message = "Arkadaşlık isteği gönderildi/Kullanıcı takip edildi." });
        }
    }
}