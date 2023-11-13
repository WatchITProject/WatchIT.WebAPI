using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Genre
{
    public int GenId { get; set; }

    public string GenName { get; set; } = null!;

    public string? GenDescription { get; set; }

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
