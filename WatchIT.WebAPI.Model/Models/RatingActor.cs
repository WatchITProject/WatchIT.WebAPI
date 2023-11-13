using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class RatingActor
{
    public long RatactId { get; set; }

    public long RatactActId { get; set; }

    public int RatactAccId { get; set; }

    public byte RatactRating { get; set; }

    public virtual Account RatactAcc { get; set; } = null!;

    public virtual Actor RatactAct { get; set; } = null!;
}
