using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Description { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<PersonActor> PersonActor { get; set; } = new List<PersonActor>();

    public virtual ICollection<PersonCreator> PersonCreator { get; set; } = new List<PersonCreator>();
}
