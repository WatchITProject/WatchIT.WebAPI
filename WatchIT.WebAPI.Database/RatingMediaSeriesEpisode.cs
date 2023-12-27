using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingMediaSeriesEpisode
{
    public long Id { get; set; }

    public long MediaSeriesEpisodeId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual MediaSeriesEpisode MediaSeriesEpisode { get; set; } = null!;
}
