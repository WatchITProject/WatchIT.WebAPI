using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingMediaSeriesSeason
{
    public long Id { get; set; }

    public long MediaSeriesSeasonId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual MediaSeriesSeason MediaSeriesSeason { get; set; } = null!;
}
