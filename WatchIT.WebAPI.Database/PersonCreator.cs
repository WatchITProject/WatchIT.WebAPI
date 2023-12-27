using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class PersonCreator
{
    public long Id { get; set; }

    public long MediaId { get; set; }

    public int PersonId { get; set; }

    public short PersonCreatorTypeId { get; set; }

    public virtual Media Media { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual PersonCreatorType PersonCreatorType { get; set; } = null!;

    public virtual ICollection<RatingPersonCreator> RatingPersonCreator { get; set; } = new List<RatingPersonCreator>();
}
