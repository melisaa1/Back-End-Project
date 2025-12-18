using RateNow.Models;

namespace RateNowApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // Role-based authorization için
        public string Role { get; set; } = "User";

        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<WatchlistItem>? WatchlistItems { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        // Many-to-many friendship
        public ICollection<User> Friends { get; set; } = new List<User>();
        public ICollection<User> FriendOf { get; set; } = new List<User>();
    }
}
