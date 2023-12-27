using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class PersonCreatorType
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PersonCreator> PersonCreator { get; set; } = new List<PersonCreator>();
}
