using Microsoft.AspNetCore.Identity;

namespace CineTicketHub.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Reservation> Reservations { get; set; }
}