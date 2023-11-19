using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class MovieGenre
{
    public long Id { get; set; }

    public int MovieId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
