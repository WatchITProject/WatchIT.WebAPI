using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class AccountBackgroundPicture
{
    public int Id { get; set; }

    public DateTime UploadDate { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual ICollection<Account> Account { get; set; } = new List<Account>();
}
