
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RateNowApi.Data;

#nullable disable

namespace RateNowApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("RateNow.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SeriesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("SeriesId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MovieId = 1,
                            Text = "This movie was amazing!, 10/10.",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            MovieId = 1,
                            Text = "I dont like it very much.",
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            MovieId = 2,
                            Text = "I like it.",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("RateNowApi.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Movies", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "The Git Abomination"
                        },
                        new
                        {
                            Id = 2,
                            Title = "The Last Commit"
                        });
                });

            modelBuilder.Entity("RateNowApi.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SeriesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("SeriesId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MovieId = 1,
                            UserId = 1,
                            Value = 5
                        },
                        new
                        {
                            Id = 2,
                            MovieId = 1,
                            UserId = 2,
                            Value = 3
                        });
                });

            modelBuilder.Entity("RateNowApi.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Seasons")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Serieses", (string)null);
                });

            modelBuilder.Entity("RateNowApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "melisa@example.com",
                            PasswordHash = "$2a$11$iTJRPFsz/EzKH5axKZcJYep1Cue64kze6U9D8CDv8jR/0RQ/UcUvq",
                            Role = "User",
                            UserName = "MelisaAdmin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "rahmah@example.com",
                            PasswordHash = "$2a$11$k7yvHz3c6FHRxoWt4ZfVROBQsdSm3FWoXjzfs.4uGPpSq4CkwwhWy",
                            Role = "User",
                            UserName = "RahmahUser"
                        });
                });

            modelBuilder.Entity("RateNowApi.Models.WatchlistItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SeriesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WatchlistItems", (string)null);
                });

            modelBuilder.Entity("UserFriends", b =>
                {
                    b.Property<int>("FriendId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FriendId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFriends", (string)null);

                    b.HasData(
                        new
                        {
                            FriendId = 2,
                            UserId = 1
                        },
                        new
                        {
                            FriendId = 1,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("RateNow.Models.Review", b =>
                {
                    b.HasOne("RateNowApi.Models.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RateNowApi.Models.Series", null)
                        .WithMany("Reviews")
                        .HasForeignKey("SeriesId");

                    b.HasOne("RateNowApi.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RateNowApi.Models.Rating", b =>
                {
                    b.HasOne("RateNowApi.Models.Movie", "Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateNowApi.Models.Series", "Series")
                        .WithMany("Ratings")
                        .HasForeignKey("SeriesId");

                    b.HasOne("RateNowApi.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Series");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RateNowApi.Models.WatchlistItem", b =>
                {
                    b.HasOne("RateNowApi.Models.User", "User")
                        .WithMany("WatchlistItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserFriends", b =>
                {
                    b.HasOne("RateNowApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RateNowApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RateNowApi.Models.Movie", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("RateNowApi.Models.Series", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("RateNowApi.Models.User", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Reviews");

                    b.Navigation("WatchlistItems");
                });
#pragma warning restore 612, 618
        }
    }
}
