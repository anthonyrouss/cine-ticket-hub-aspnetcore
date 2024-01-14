using System.ComponentModel.DataAnnotations;

namespace CineTicketHub.Models.ViewModels;

public class MovieVM
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int Duration { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string? PosterUrl { get; set; }
    
    // Relationships
    
    [Display(Name = "Select Genre(s)")]
    [Required(ErrorMessage = "Movie Genre(s) is required")]
    public List<int> GenreIds { get; set; }
    
    
}