using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingActorSeries
{
    public long Id { get; set; }

    public long ActorSeriesId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ActorSeries ActorSeries { get; set; } = null!;
}
