using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using CineTicketHub.Services.Base;

namespace CineTicketHub.Services;

public class GenresService : EntityBaseRepository<Genre>, IGenresService
{
    public GenresService(CineTicketHubContext context) : base(context)
    {
    }
}