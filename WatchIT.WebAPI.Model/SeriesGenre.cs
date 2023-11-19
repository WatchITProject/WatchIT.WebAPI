using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class SeriesGenre
{
    public long Id { get; set; }

    public int SeriesId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Series Series { get; set; } = null!;
}
