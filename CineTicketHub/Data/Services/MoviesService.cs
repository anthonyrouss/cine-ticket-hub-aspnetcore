using CineTicketHub.Data.ViewModels;
using CineTicketHub.Models.Base;

namespace CineTicketHub.Models.Services;

public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
{
    private readonly CineTicketHubContext _context;
    
    public MoviesService(CineTicketHubContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddMovieVMAsync(MovieVM movieVM)
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
}