using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Description { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

    public virtual ICollection<ActorSeries> ActorSeries { get; set; } = new List<ActorSeries>();

    public virtual ICollection<DirectorMovie> DirectorMovies { get; set; } = new List<DirectorMovie>();

    public virtual ICollection<DirectorSeries> DirectorSeries { get; set; } = new List<DirectorSeries>();
}
