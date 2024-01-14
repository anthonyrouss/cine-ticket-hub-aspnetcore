﻿// <auto-generated />
using System;
using CineTicketHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CineTicketHub.Migrations
{
    [DbContext(typeof(CineTicketHubContext))]
    partial class CineTicketHubContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("CineTicketHub.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("genres", (string)null);
                });

            modelBuilder.Entity("CineTicketHub.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.Property<int>("Duration")
                        .HasColumnType("int")
                        .HasColumnName("duration");

                    b.Property<string>("PosterUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("poster_url");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date")
                        .HasColumnName("release_date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("movies", (string)null);
                });

            modelBuilder.Entity("CineTicketHub.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("NumOfSeats")
                        .HasColumnType("int")
                        .HasColumnName("num_of_seats");

                    b.Property<int>("ScreeningId")
                        .HasColumnType("int")
                        .HasColumnName("screening_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ScreeningId" }, "screening_id");

                    b.ToTable("reservations", (string)null);
                });

            modelBuilder.Entity("CineTicketHub.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("Capacity")
                        .HasColumnType("int")
                        .HasColumnName("capacity");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("rooms", (string)null);
                });

            modelBuilder.Entity("CineTicketHub.Models.Screening", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("room_id");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("datetime")
                        .HasColumnName("starts_at");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "MovieId" }, "movie_id");

                    b.HasIndex(new[] { "RoomId" }, "room_id");

                    b.ToTable("screenings", (string)null);
                });

            modelBuilder.Entity("MovieHasGenre", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int")
                        .HasColumnName("movie_id");

                    b.Property<int>("GenreId")
                        .HasColumnType("int")
                        .HasColumnName("genre_id");

                    b.HasKey("MovieId", "GenreId")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "GenreId" }, "genre_id");

                    b.ToTable("movie_has_genre", (string)null);
                });

            modelBuilder.Entity("CineTicketHub.Models.Reservation", b =>
                {
                    b.HasOne("CineTicketHub.Models.Screening", "Screening")
                        .WithMany("Reservations")
                        .HasForeignKey("ScreeningId")
                        .IsRequired()
                        .HasConstraintName("reservations_ibfk_1");

                    b.Navigation("Screening");
                });

            modelBuilder.Entity("CineTicketHub.Models.Screening", b =>
                {
                    b.HasOne("CineTicketHub.Models.Movie", "Movie")
                        .WithMany("Screenings")
                        .HasForeignKey("MovieId")
                        .IsRequired()
                        .HasConstraintName("screenings_ibfk_1");

                    b.HasOne("CineTicketHub.Models.Room", "Room")
                        .WithMany("Screenings")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("screenings_ibfk_2");

                    b.Navigation("Movie");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("MovieHasGenre", b =>
                {
                    b.HasOne("CineTicketHub.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("movie_has_genre_ibfk_2");

                    b.HasOne("CineTicketHub.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("movie_has_genre_ibfk_1");
                });

            modelBuilder.Entity("CineTicketHub.Models.Movie", b =>
                {
                    b.Navigation("Screenings");
                });

            modelBuilder.Entity("CineTicketHub.Models.Room", b =>
                {
                    b.Navigation("Screenings");
                });

            modelBuilder.Entity("CineTicketHub.Models.Screening", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
