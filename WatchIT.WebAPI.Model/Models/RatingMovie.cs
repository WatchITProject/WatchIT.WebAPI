using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class RatingMovie
{
    public long RatmovId { get; set; }

    public int RatmovMovId { get; set; }

    public int RatmovAccId { get; set; }

    public byte RatmovRating { get; set; }

    public virtual Account RatmovAcc { get; set; } = null!;

    public virtual Movie RatmovMov { get; set; } = null!;
}
