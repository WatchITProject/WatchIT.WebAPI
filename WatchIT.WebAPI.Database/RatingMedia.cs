using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingMedia
{
    public long Id { get; set; }

    public long MediaId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Media Media { get; set; } = null!;
}
