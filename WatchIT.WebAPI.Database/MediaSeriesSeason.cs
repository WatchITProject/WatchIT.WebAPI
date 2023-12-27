using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class MediaSeriesSeason
{
    public long Id { get; set; }

    public int MediaSeriesId { get; set; }

    public short? Number { get; set; }

    public string? Name { get; set; }

    public virtual MediaSeries MediaSeries { get; set; } = null!;

    public virtual ICollection<MediaSeriesEpisode> MediaSeriesEpisode { get; set; } = new List<MediaSeriesEpisode>();

    public virtual ICollection<RatingMediaSeriesSeason> RatingMediaSeriesSeason { get; set; } = new List<RatingMediaSeriesSeason>();
}
