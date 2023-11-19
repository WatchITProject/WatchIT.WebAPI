using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingSeriesEpisode
{
    public long Id { get; set; }

    public long SeriesEpisodeId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual SeriesEpisode SeriesEpisode { get; set; } = null!;
}
