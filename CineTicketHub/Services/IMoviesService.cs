using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using CineTicketHub.Models.ViewModels;
using CineTicketHub.Services.Base;

namespace CineTicketHub.Services;

public interface IMoviesService : IEntityBaseRepository<Movie>
{
    Task AddMovieAsync(MovieVM movieVM);
    Task UpdateMovieAsync(MovieVM movieVM);
}