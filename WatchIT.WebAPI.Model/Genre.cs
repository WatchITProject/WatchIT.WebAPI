using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Descripiton { get; set; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();
}
