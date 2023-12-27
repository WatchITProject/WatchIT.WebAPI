using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class MediaSeriesEpisode
{
    public long Id { get; set; }

    public long MediaSeriesSeasonId { get; set; }

    public short? Number { get; set; }

    public string? Name { get; set; }

    public virtual MediaSeriesSeason MediaSeriesSeason { get; set; } = null!;

    public virtual ICollection<RatingMediaSeriesEpisode> RatingMediaSeriesEpisode { get; set; } = new List<RatingMediaSeriesEpisode>();
}
