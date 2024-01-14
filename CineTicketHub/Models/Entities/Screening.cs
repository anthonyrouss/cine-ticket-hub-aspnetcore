namespace CineTicketHub.Models.Entities;

public partial class Screening
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int RoomId { get; set; }

    public DateTime StartsAt { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Room Room { get; set; } = null!;
}
