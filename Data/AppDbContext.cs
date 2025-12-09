using Microsoft.EntityFrameworkCore;
using RateNowApi.Models; // Doğru Model namespace'i
using BCrypt.Net;
using System.Collections.Generic;
using RateNow.Models; // Dictionary için gerekli

namespace RateNowApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

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
            // 1. ZORUNLU İLİŞKİLER (3.1.2 - Fluent API)
            // **********************************************

            // Bire-Çok İlişkileri (Rating, Review, Watchlist)
            modelBuilder.Entity<Rating>().HasOne(r => r.User).WithMany(u => u.Ratings).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Rating>().HasOne(r => r.Movie).WithMany(m => m.Ratings).HasForeignKey(r => r.MovieId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Review>().HasOne(r => r.Movie).WithMany(m => m.Reviews).HasForeignKey(r => r.MovieId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WatchlistItem>().HasOne(w => w.User).WithMany(u => u.WatchlistItems).HasForeignKey(w => w.UserId).OnDelete(DeleteBehavior.Cascade);
            
            // Parola Hash'i Zorunlu Yapma (3.1.2)
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            // **********************************************
            // 2. DATABASE SEEDING (3.1.2)
            // **********************************************

            // Kullanıcı Verileri
            var user1 = new User { Id = 1, UserName = "MelisaAdmin", Email = "melisa@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") };
            var user2 = new User { Id = 2, UserName = "RahmahUser", Email = "rahmah@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456") };
            modelBuilder.Entity<User>().HasData(user1, user2);
            
            // Film Verileri
            var movie1 = new Movie { Id = 1, Title = "The Git Abomination" };
            var movie2 = new Movie { Id = 2, Title = "The Last Commit" };
            modelBuilder.Entity<Movie>().HasData(movie1, movie2);

            // Yorum Verileri (Review)
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, UserId = 1, MovieId = 1, Text = "This movie was amazing!, 10/10." },
                new Review { Id = 2, UserId = 2, MovieId = 1, Text = "I dont like it very much." },
                new Review { Id = 3, UserId = 1, MovieId = 2, Text = "I like it." }
            );

            // Derecelendirme Verileri (Rating)
            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, UserId = 1, MovieId = 1, Value = 5 },
                new Rating { Id = 2, UserId = 2, MovieId = 1, Value = 3 }
            );

            // User <-> User (ZORUNLU Çoka-Çok İlişkisi & Seeding - SADECE TEK TANIM)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.FriendOf)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriends", 
                        j => j.HasOne<User>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade)
                )
                .HasData(
                    new Dictionary<string, object> { { "UserId", 1 }, { "FriendId", 2 } }, 
                    new Dictionary<string, object> { { "UserId", 2 }, { "FriendId", 1 } }
                );
        }
    }
}