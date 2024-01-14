using CineTicketHub.Services.Base;

namespace CineTicketHub.Models.Entities;

public partial class Movie : IEntityBase
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Duration { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public string? PosterUrl { get; set; }

    // Relationships
    
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
