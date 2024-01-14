using CineTicketHub.Models.Base;

namespace CineTicketHub.Models.Services;

public class GenresService : EntityBaseRepository<Genre>, IGenresService
{
    public GenresService(CineTicketHubContext context) : base(context)
    {
    }
}