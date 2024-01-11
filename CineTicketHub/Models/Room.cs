using System;
using System.Collections.Generic;

namespace CineTicketHub.Models;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
