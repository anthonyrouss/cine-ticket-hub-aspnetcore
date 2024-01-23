
using System.ComponentModel.DataAnnotations;

namespace CineTicketHub.Models.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public int ScreeningId { get; set; }

    public string UserId { get; set; }

    [Display(Name = "Number of seats")]
    public int NumOfSeats { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;

    public virtual Screening Screening { get; set; } = null!;
}
