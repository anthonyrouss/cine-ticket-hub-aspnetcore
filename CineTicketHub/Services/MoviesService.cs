using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using CineTicketHub.Models.ViewModels;
using CineTicketHub.Services.Base;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace CineTicketHub.Services;

public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
{
    private readonly CineTicketHubContext _context;
    
    public MoviesService(CineTicketHubContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddMovieAsync(MovieVM movieVM)
    {
        // Create a new movie based on the newMovieVM
        var newMovie = new Movie()
        {
            Title = movieVM.Title,
            Description = movieVM.Description,
            Duration = movieVM.Duration,
            ReleaseDate = movieVM.ReleaseDate,
            PosterUrl = movieVM.PosterUrl
        };
        
        // Retrieve selected genres from the database
        var selectedGenres = _context.Genres.Where(g => movieVM.GenreIds.Contains(g.Id)).ToList();

        // Associate the genres with the movie
        newMovie.Genres = selectedGenres;
        
        // Add the movie to the context
        await _context.Movies.AddAsync(newMovie);
        
        // Save changes to the database
        await _context.SaveChangesAsync();

    }

    public async Task UpdateMovieAsync(MovieVM movieVM)
    {
        // Find the existing movie by ID
        var existingMovie = await _context.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == movieVM.Id);

        if (existingMovie == null)
        {
            // TODO: Implement
            return;
        }
        
        // Update the movie properties
        existingMovie.Title = movieVM.Title;
        existingMovie.Description = movieVM.Description;
        existingMovie.Duration = movieVM.Duration;
        existingMovie.ReleaseDate = movieVM.ReleaseDate;
        existingMovie.PosterUrl = movieVM.PosterUrl;
        
        // Retrieve selected genres from the database
        var selectedGenres = await _context.Genres
            .Where(g => movieVM.GenreIds.Contains(g.Id)).ToListAsync();
        
        // Update the associated genres with the movie
        existingMovie.Genres.Clear();
        existingMovie.Genres.AddRange(selectedGenres);
        
        // Save changes to the database
        await _context.SaveChangesAsync();

    }

    public async Task<Movie> GetByIdAsync(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id);
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        IEnumerable<Movie> movies = await _context.Movies
            .Include(m => m.Genres)
            .ToListAsync();
        return movies;
    }
}