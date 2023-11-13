using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class MovieGenre
{
    public long MovgenId { get; set; }

    public int MovgenMovId { get; set; }

    public int MovgenGenId { get; set; }

    public virtual Genre MovgenGen { get; set; } = null!;

    public virtual Movie MovgenMov { get; set; } = null!;
}
