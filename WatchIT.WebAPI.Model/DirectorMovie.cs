using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class DirectorMovie
{
    public long Id { get; set; }

    public int MovieId { get; set; }

    public int PersonId { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RatingDirectorMovie> RatingDirectorMovies { get; set; } = new List<RatingDirectorMovie>();
}
