using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Director
{
    public long DirId { get; set; }

    public int DirMovId { get; set; }

    public int DirPerId { get; set; }

    public virtual Movie DirMov { get; set; } = null!;

    public virtual Person DirPer { get; set; } = null!;
}
