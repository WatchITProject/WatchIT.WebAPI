using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Movie
{
    public int MovId { get; set; }

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<RatingMovie> RatingMovies { get; set; } = new List<RatingMovie>();
}
