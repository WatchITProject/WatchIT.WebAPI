using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class MediaMovie
{
    public int Id { get; set; }

    public long MediaId { get; set; }

    public virtual Media Media { get; set; } = null!;
}
