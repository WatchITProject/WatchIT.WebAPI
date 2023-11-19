using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class ActorMovie
{
    public long Id { get; set; }

    public int MovieId { get; set; }

    public int PersonId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RatingActorMovie> RatingActorMovies { get; set; } = new List<RatingActorMovie>();
}
