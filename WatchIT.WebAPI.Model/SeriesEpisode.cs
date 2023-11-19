using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class SeriesEpisode
{
    public long Id { get; set; }

    public long SeasonId { get; set; }

    public short? Number { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<RatingSeriesEpisode> RatingSeriesEpisodes { get; set; } = new List<RatingSeriesEpisode>();

    public virtual SeriesSeason Season { get; set; } = null!;
}
