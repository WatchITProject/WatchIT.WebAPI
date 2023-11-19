using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class SeriesSeason
{
    public long Id { get; set; }

    public int SeriesId { get; set; }

    public short? Number { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<RatingSeriesSeason> RatingSeriesSeasons { get; set; } = new List<RatingSeriesSeason>();

    public virtual Series Series { get; set; } = null!;

    public virtual ICollection<SeriesEpisode> SeriesEpisodes { get; set; } = new List<SeriesEpisode>();
}
