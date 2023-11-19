using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class DirectorSeries
{
    public long Id { get; set; }

    public int SeriesId { get; set; }

    public int PersonId { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RatingDirectorSeries> RatingDirectorSeries { get; set; } = new List<RatingDirectorSeries>();

    public virtual Series Series { get; set; } = null!;
}
