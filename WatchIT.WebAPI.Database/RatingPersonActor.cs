using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingPersonActor
{
    public long Id { get; set; }

    public long PersonActorId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual PersonActor PersonActor { get; set; } = null!;
}
