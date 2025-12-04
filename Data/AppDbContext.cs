using Microsoft.EntityFrameworkCore;
using RateNowApi.Models;
using BCrypt.Net; // Parola hashleme için gerekli

namespace RateNowApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DB SETS
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Series> Serieses { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<WatchlistItem> WatchlistItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // **********************************************
            // 1. ZORUNLU İLİŞKİLER (3.1.2 - Bire-Çok & Çoka-Çok)
            // **********************************************

            // User-Rating (Bire-Çok)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Movie-Rating (Bire-Çok)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-Review (ZORUNLU Bire-Çok İlişkisi)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Movie-Review (Bire-Çok)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-WatchlistItem (Bire-Çok)
            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WatchlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // User <-> User (ZORUNLU Çoka-Çok İlişkisi - Arkadaşlık/Takip)
            // Kendi kendine referans veren Many-to-Many ilişkisi
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.FriendOf)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriends", 
                        j => j.HasOne<User>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.Cascade),
                         j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade)
                );
            
            // Parola Hash'i Zorunlu Yapma (Güvenlik için)
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired(); 


            // **********************************************
            // 2. DATABASE SEEDING (3.1.2)
            // **********************************************
            
            // Kullanıcı Verileri
            var user1 = new User 
            { 
                Id = 1, 
                UserName = "MelisaAdmin", 
                Email = "melisa@example.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") 
            };
            var user2 = new User 
            { 
                Id = 2, 
                UserName = "RahmahUser", 
                Email = "rahmah@example.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") 
            };

            modelBuilder.Entity<User>().HasData(user1, user2);

            // Film Verileri
            var movie1 = new Movie { Id = 1, Title = "The Git Abomination" };
            var movie2 = new Movie { Id = 2, Title = "The Last Commit" };
            modelBuilder.Entity<Movie>().HasData(movie1, movie2);

            // Yorum Verileri (Bire-Çok Testi)
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, UserId = 1, MovieId = 1, Text = "Muhteşem bir filmdi, 10/10." },
                new Review { Id = 2, UserId = 2, MovieId = 1, Text = "Ortalama bir yapım, beklentiyi karşılamadı." },
                new Review { Id = 3, UserId = 1, MovieId = 2, Text = "Git öğrenme sürecimi özetliyor." }
            );

            // Derecelendirme Verileri
            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, UserId = 1, MovieId = 1, Value = 5 },
                new Rating { Id = 2, UserId = 2, MovieId = 1, Value = 3 }
            );
            
            // Arkadaşlık Verileri (Çoka-Çok Testi)
            // UserFriends ara tablosuna veri ekleniyor
            modelBuilder.Entity<Dictionary<string, object>>("UserFriends").HasData(
                new { UserId = 1, FriendId = 2 }, 
                new { UserId = 2, FriendId = 1 } 
            );
        }
    }
}