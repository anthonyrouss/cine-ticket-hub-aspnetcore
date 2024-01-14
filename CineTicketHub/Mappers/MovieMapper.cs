using CineTicketHub.Data.ViewModels;
using CineTicketHub.Models;

namespace CineTicketHub.Mappers;

public class MovieMapper
{
    public MovieVM MapToMovieVM(Movie movie)
    {
        if (movie == null) return null;

        return new MovieVM()
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Duration = movie.Duration,
            ReleaseDate = movie.ReleaseDate,
            PosterUrl = movie.PosterUrl,
            GenreIds = movie.Genres.Select(g => g.Id).ToList()
        };
    }
}