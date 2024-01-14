using System;
using System.Collections.Generic;
using CineTicketHub.Models.Base;

namespace CineTicketHub.Models;

public partial class Genre : IEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
