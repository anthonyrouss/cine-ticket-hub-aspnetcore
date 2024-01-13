using CineTicketHub.Data.ViewModels;
using CineTicketHub.Models.Base;

namespace CineTicketHub.Models.Services;

public interface IMoviesService : IEntityBaseRepository<Movie>
{
    Task AddMovieVMAsync(MovieVM movieVM);
}