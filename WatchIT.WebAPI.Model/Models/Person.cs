using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Model.Models;

public partial class Person
{
    public int PerId { get; set; }

    public string PerName { get; set; } = null!;

    public string? PerFullName { get; set; }

    public string? PerDescription { get; set; }

    public DateTime? PerBirthday { get; set; }

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
}
