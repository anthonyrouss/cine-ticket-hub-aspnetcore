﻿namespace CineTicketHub.Models.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public int ScreeningId { get; set; }

    public int UserId { get; set; }

    public int NumOfSeats { get; set; }

    public virtual Screening Screening { get; set; } = null!;
}