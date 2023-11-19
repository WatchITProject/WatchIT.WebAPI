using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingMovie
{
    public long Id { get; set; }

    public int MovieId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
