using CineTicketHub.Services.Base;

namespace CineTicketHub.Models.Entities;

public partial class Genre : IEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
