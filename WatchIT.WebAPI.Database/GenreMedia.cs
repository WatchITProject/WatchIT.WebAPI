using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class GenreMedia
{
    public long Id { get; set; }

    public long MediaId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Media Media { get; set; } = null!;
}
