using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? OriginalTitle { get; set; }

    public string? Description { get; set; }

    public DateTime ReleaseDate { get; set; }

    public short Length { get; set; }

    public virtual ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

    public virtual ICollection<DirectorMovie> DirectorMovies { get; set; } = new List<DirectorMovie>();

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<RatingMovie> RatingMovies { get; set; } = new List<RatingMovie>();
}
