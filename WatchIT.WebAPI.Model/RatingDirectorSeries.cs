using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingDirectorSeries
{
    public long Id { get; set; }

    public long DirectorSeriesId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual DirectorSeries DirectorSeries { get; set; } = null!;
}
