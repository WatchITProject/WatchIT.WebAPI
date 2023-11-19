using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingSeries
{
    public long Id { get; set; }

    public int SeriesId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Series Series { get; set; } = null!;
}
