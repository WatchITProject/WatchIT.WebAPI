using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingActorMovie
{
    public long Id { get; set; }

    public long ActorMovieId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ActorMovie ActorMovie { get; set; } = null!;
}
