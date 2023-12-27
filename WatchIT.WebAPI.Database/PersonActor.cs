using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class PersonActor
{
    public long Id { get; set; }

    public long MediaId { get; set; }

    public int PersonId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Media Media { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RatingPersonActor> RatingPersonActor { get; set; } = new List<RatingPersonActor>();
}
