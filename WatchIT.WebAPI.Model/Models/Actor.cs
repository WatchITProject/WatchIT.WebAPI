using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Actor
{
    public long ActId { get; set; }

    public int ActMovId { get; set; }

    public int ActPerId { get; set; }

    public string ActRole { get; set; } = null!;

    public virtual Movie ActMov { get; set; } = null!;

    public virtual Person ActPer { get; set; } = null!;

    public virtual ICollection<RatingActor> RatingActors { get; set; } = new List<RatingActor>();
}
