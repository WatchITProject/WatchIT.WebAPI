using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingDirectorMovie
{
    public long Id { get; set; }

    public long DirectorMovieId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual DirectorMovie DirectorMovie { get; set; } = null!;
}
