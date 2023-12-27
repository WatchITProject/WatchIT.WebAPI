using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class MediaSeries
{
    public int Id { get; set; }

    public long MediaId { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Media Media { get; set; } = null!;

    public virtual ICollection<MediaSeriesSeason> MediaSeriesSeason { get; set; } = new List<MediaSeriesSeason>();
}
