using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Series
{
    public int Id { get; set; }

    public virtual ICollection<ActorSeries> ActorSeries { get; set; } = new List<ActorSeries>();

    public virtual ICollection<DirectorSeries> DirectorSeries { get; set; } = new List<DirectorSeries>();

    public virtual ICollection<RatingSeries> RatingSeries { get; set; } = new List<RatingSeries>();

    public virtual ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();

    public virtual ICollection<SeriesSeason> SeriesSeasons { get; set; } = new List<SeriesSeason>();
}
