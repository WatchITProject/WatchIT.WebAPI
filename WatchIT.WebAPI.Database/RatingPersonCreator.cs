using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class RatingPersonCreator
{
    public long Id { get; set; }

    public long PersonCreatorId { get; set; }

    public int AccountId { get; set; }

    public byte Rating { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual PersonCreator PersonCreator { get; set; } = null!;
}
