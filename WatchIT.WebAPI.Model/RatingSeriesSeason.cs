using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingSeriesSeason
{
    public long Id { get; set; }

    public long SeriesSeasonId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual SeriesSeason SeriesSeason { get; set; } = null!;
}
