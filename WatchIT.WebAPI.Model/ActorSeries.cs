using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class ActorSeries
{
    public long Id { get; set; }

    public int SeriesId { get; set; }

    public int PersonId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RatingActorSeries> RatingActorSeries { get; set; } = new List<RatingActorSeries>();

    public virtual Series Series { get; set; } = null!;
}
