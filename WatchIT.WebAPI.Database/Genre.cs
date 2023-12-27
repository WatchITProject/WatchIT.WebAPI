using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<GenreMedia> GenreMedia { get; set; } = new List<GenreMedia>();
}
